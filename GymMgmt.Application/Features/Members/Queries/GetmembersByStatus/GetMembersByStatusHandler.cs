using Dapper;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // 1. Determine the WHERE clause based on the Enum
            string whereClause = request.StatusFilter switch
            {
                MemberStatusFilter.All =>"",
                MemberStatusFilter.Active =>
                    "WHERE s.Status = 'Active' AND s.EndDate >= @Now",

                MemberStatusFilter.GracePeriod =>
                    "WHERE s.Status = 'Active' AND s.EndDate < @Now AND s.EndDate >= DATEADD(day, -@GraceDays, @Now)",

                MemberStatusFilter.Expired =>
                    "WHERE (s.Status = 'Active' AND s.EndDate < DATEADD(day, -@GraceDays, @Now)) OR s.Status = 'Expired'",

                MemberStatusFilter.Cancelled =>
                    "WHERE s.Status = 'Cancelled'",

                _ => "WHERE 1=0" // Should not happen
            };

            var sql = $@"
            DECLARE @GraceDays INT = (SELECT TOP 1 SubscriptionGracePeriodInDays FROM ClubSettings);
            DECLARE @Now DATETIME2 = SYSDATETIME();

            -- Count Total
            SELECT COUNT(*) 
            FROM Subscriptions s 
            {whereClause};

            -- Get Data
            SELECT 
                m.Id as MemberId,
                m.FirstName + ' ' + m.LastName as FullName,
                s.PlanName,
                s.EndDate,
                m.PhoneNumber,
                DATEDIFF(day, s.EndDate, @Now) as DaysOverdue
            FROM Subscriptions s
            INNER JOIN Members m ON s.MemberId = m.Id
            {whereClause}
            ORDER BY s.EndDate ASC -- Prioritize those expiring soonest
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
