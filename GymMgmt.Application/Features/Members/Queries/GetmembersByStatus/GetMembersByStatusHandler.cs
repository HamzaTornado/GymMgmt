using Dapper;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
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

            // 1. Dynamic WHERE Clause
            // Note: We alias Subscriptions as 's' and Members as 'm' later
            string whereClause = request.StatusFilter switch
            {
                MemberStatusFilter.All => "", // No filter

                MemberStatusFilter.Active =>
                    "WHERE s.Status = 'Active' AND s.EndDate >= @Now",

                MemberStatusFilter.GracePeriod =>
                    "WHERE s.Status = 'Active' AND s.EndDate < @Now AND s.EndDate >= DATEADD(day, -@GraceDays, @Now)",

                MemberStatusFilter.Expired =>
                    "WHERE (s.Status = 'Active' AND s.EndDate < DATEADD(day, -@GraceDays, @Now)) OR s.Status = 'Expired'",

                MemberStatusFilter.Cancelled =>
                    "WHERE s.Status = 'Cancelled'",

                _ => "WHERE 1=0"
            };

            var sql = $@"
            DECLARE @GraceDays INT = (SELECT TOP 1 SubscriptionGracePeriodInDays FROM ClubSettings);
            DECLARE @Now DATETIME2 = SYSDATETIME();

            -- Count Total (Matching the filter)
            SELECT COUNT(*) 
            FROM Members m
            LEFT JOIN Subscriptions s ON m.Id = s.MemberId
            {whereClause};

            -- Get Data
            SELECT 
                m.Id as MemberId,
                m.FirstName + ' ' + m.LastName as FullName,
                ISNULL(s.PlanName, 'No Plan') as PlanName, -- Handle NULL for prospects
                ISNULL(s.EndDate, '1900-01-01') as EndDate, -- Handle NULL dates
                m.PhoneNumber,
                CASE 
                    WHEN s.EndDate IS NULL THEN 0 
                    ELSE DATEDIFF(day, s.EndDate, @Now) 
                END as DaysOverdue
            FROM Members m
            LEFT JOIN Subscriptions s ON m.Id = s.MemberId
            {whereClause}
            ORDER BY 
                CASE WHEN s.EndDate IS NULL THEN 1 ELSE 0 END, -- Put 'No Plan' people at the bottom
                s.EndDate ASC
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            using var multi = await connection.QueryMultipleAsync(sql, new
            {
                Skip = (request.PageNumber - 1) * request.PageSize,
                Take = request.PageSize
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
