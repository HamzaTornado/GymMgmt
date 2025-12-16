using Dapper;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using GymMgmt.Domain.Common.Enums;
using MediatR;


namespace GymMgmt.Application.Features.Members.Queries.GetmembersByStatus
{
    public class GetMembersByStatusHandler : IRequestHandler<GetMembersByStatusQuery, PaginatedList<MemberListDto>>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetMembersByStatusHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PaginatedList<MemberListDto>> Handle(GetMembersByStatusQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateNewConnection();

            // 1. Refactor-Proof Enum Names
            // We use the actual C# Enum to generate the string values ('Insurance', 'Paid')
            // If you rename the Enum member, this SQL updates automatically.
            var insuranceType = PaymentType.Insurance.ToString();
            var paidStatus = PaymentStatus.Paid.ToString();
            var activeStatus = SubscriptionStatus.Active.ToString(); // Assuming you have this Enum too
            var cancelledStatus = SubscriptionStatus.Cancelled.ToString();

            // 2. Filter Logic 
            string whereClause = request.StatusFilter switch
            {
                MemberStatusFilter.All => "",
                MemberStatusFilter.Active => $"WHERE s.Status = '{activeStatus}' AND s.EndDate >= @Now",

                MemberStatusFilter.GracePeriod =>
                    $"WHERE s.Status = '{activeStatus}' AND s.EndDate < @Now AND s.EndDate >= DATEADD(day, -@GraceDays, @Now)",

                MemberStatusFilter.Expired =>
                    $"WHERE (s.Status = '{activeStatus}' AND s.EndDate < DATEADD(day, -@GraceDays, @Now)) OR s.Status = '{nameof(SubscriptionStatus.Expired)}'",

                MemberStatusFilter.Cancelled => $"WHERE s.Status = '{cancelledStatus}'",

                _ => "WHERE 1=0"
            };

            var sql = $@"
                DECLARE @GraceDays INT = (SELECT TOP 1 SubscriptionGracePeriodInDays FROM ClubSettings);
    
                -- Use Parameter for Time to keep Application Server as the Source of Truth
                -- (We pass @Now from C# below)

                -- 1. Count Total
                SELECT COUNT(*) 
                FROM Members m
                LEFT JOIN Subscriptions s ON m.Id = s.MemberId
                {whereClause};

                -- 2. Get Data
                SELECT 
                    m.Id as MemberId,
                    m.FirstName + ' ' + m.LastName as FullName,
                    ISNULL(s.PlanName, 'No Plan') as PlanName,
                    ISNULL(s.EndDate, '1900-01-01') as EndDate,
                    ISNULL(s.Status, 'New') as SubscriptionStatus,
                    m.PhoneNumber,
        
                    CASE 
                        WHEN s.EndDate IS NULL THEN 0 
                        ELSE DATEDIFF(day, s.EndDate, @Now) 
                    END as DaysOverdue,

                    -- HAS ACTIVE SUBSCRIPTION?
                    CASE WHEN EXISTS (
                        SELECT 1 
                        FROM Subscriptions sub 
                        WHERE sub.MemberId = m.Id 
                          AND sub.Status = '{activeStatus}' 
                          AND sub.EndDate >= @Now
                    ) THEN 1 ELSE 0 END as HasActiveSubscription,

                    -- HAS VALID INSURANCE?
                    CASE WHEN EXISTS (
                        SELECT 1 
                        FROM Payments p 
                        WHERE p.MemberId = m.Id 
                          AND p.Type = '{insuranceType}'  -- Inject 'Insurance'
                          AND p.Status = '{paidStatus}'   -- Inject 'Paid'
                          AND @Now >= p.PeriodStart 
                          AND @Now <= p.PeriodEnd
                    ) THEN 1 ELSE 0 END as PaidInsurance

                FROM Members m
                LEFT JOIN Subscriptions s ON m.Id = s.MemberId
                {whereClause}
                ORDER BY 
                    CASE WHEN s.EndDate IS NULL THEN 1 ELSE 0 END,
                    s.EndDate ASC
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            using var multi = await connection.QueryMultipleAsync(sql, new
            {
                Skip = (request.PageNumber - 1) * request.PageSize,
                Take = request.PageSize,
                Now = DateTime.Now // Pass time from C#
            });

            var totalCount = await multi.ReadFirstAsync<int>();
            var items = (await multi.ReadAsync<MemberListDto>()).ToList();

            return new PaginatedList<MemberListDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
