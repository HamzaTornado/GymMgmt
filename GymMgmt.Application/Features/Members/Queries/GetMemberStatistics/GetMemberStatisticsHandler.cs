using Dapper;
using GymMgmt.Application.Common.Interfaces;
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

            // We fetch the GracePeriod days from settings dynamically
            // Then we use CASE WHEN to categorize every subscription efficiently
            var sql = @"
            DECLARE @GraceDays INT = (SELECT TOP 1 SubscriptionGracePeriodInDays FROM ClubSettings);
            DECLARE @Now DATETIME2 = SYSDATETIME();

            SELECT 
                COUNT(CASE 
                    WHEN s.Status = 'Active' AND s.EndDate >= @Now 
                    THEN 1 END) as ActiveCount,

                COUNT(CASE 
                    WHEN s.Status = 'Active' AND s.EndDate < @Now AND s.EndDate >= DATEADD(day, -@GraceDays, @Now) 
                    THEN 1 END) as GracePeriodCount,

                COUNT(CASE 
                    WHEN (s.Status = 'Active' AND s.EndDate < DATEADD(day, -@GraceDays, @Now)) OR s.Status = 'Expired'
                    THEN 1 END) as ExpiredCount,

                COUNT(CASE 
                    WHEN s.Status = 'Cancelled' 
                    THEN 1 END) as CancelledCount

            FROM Subscriptions s
            -- Ensure we only count the latest/relevant subscription per member if needed
            -- For now, this counts ALL subscriptions in the system
            WHERE s.WillNotRenew = 0 OR s.Status = 'Cancelled'";

            return await connection.QuerySingleAsync<MemberStatisticsDto>(sql);
        }
    }
}
