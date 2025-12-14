using Dapper;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Domain.Common.Enums;
using MediatR;


namespace GymMgmt.Application.Features.Members.Queries.GetMemberStatistics
{
    public class GetMemberStatisticsHandler : IRequestHandler<GetMemberStatisticsQuery, MemberStatisticsDto>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetMemberStatisticsHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<MemberStatisticsDto> Handle(GetMemberStatisticsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateNewConnection();

            // 1. Refactor-Proof Enum Names
            // We prepare these strings so we don't hardcode 'Insurance' or 'Paid'
            var insuranceType = PaymentType.Insurance.ToString();
            var paidStatus = PaymentStatus.Paid.ToString();
            var activeSubStatus = SubscriptionStatus.Active.ToString();
            var expiredSubStatus = SubscriptionStatus.Expired.ToString();
            var cancelledSubStatus = SubscriptionStatus.Cancelled.ToString();

            // 2. The Query
            var sql = $@"
                DECLARE @GraceDays INT = (SELECT TOP 1 SubscriptionGracePeriodInDays FROM ClubSettings);
                DECLARE @Now DATETIME2 = SYSDATETIME();

                SELECT 
                    -- 1. Subscription Counts (From Subscriptions Table)
                    COUNT(CASE 
                        WHEN s.Status = '{activeSubStatus}' AND s.EndDate >= @Now 
                        THEN 1 END) as ActiveCount,

                    COUNT(CASE 
                        WHEN s.Status = '{activeSubStatus}' AND s.EndDate < @Now AND s.EndDate >= DATEADD(day, -@GraceDays, @Now) 
                        THEN 1 END) as GracePeriodCount,

                    COUNT(CASE 
                        WHEN (s.Status = '{activeSubStatus}' AND s.EndDate < DATEADD(day, -@GraceDays, @Now)) OR s.Status = '{expiredSubStatus}'
                        THEN 1 END) as ExpiredCount,

                    COUNT(CASE 
                        WHEN s.Status = '{cancelledSubStatus}' 
                        THEN 1 END) as CancelledCount,

                    -- 2. Insurance Count (Scalar Subquery from Payments Table)
                    -- We count DISTINCT MemberId to ensure we don't double-count if a member has overlapping records (edge case)
                    (
                        SELECT COUNT(DISTINCT MemberId) 
                        FROM Payments p 
                        WHERE p.Type = '{insuranceType}' 
                          AND p.Status = '{paidStatus}' 
                          AND @Now >= p.PeriodStart 
                          AND @Now <= p.PeriodEnd
                    ) as ActivePaidInsuranceCount

                FROM Subscriptions s
                WHERE s.WillNotRenew = 0 OR s.Status = '{cancelledSubStatus}'";

            return await connection.QuerySingleAsync<MemberStatisticsDto>(sql);
        }
    }
}
