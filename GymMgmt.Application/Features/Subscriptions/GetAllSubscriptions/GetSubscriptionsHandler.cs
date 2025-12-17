using Dapper;
using GymMgmt.Application.Common.Interfaces;
using GymMgmt.Application.Common.Models;
using MediatR;

using System.Text;


namespace GymMgmt.Application.Features.Subscriptions.GetAllSubscriptions
{
    public class GetSubscriptionsHandler : IRequestHandler<GetSubscriptionsQuery, PaginatedList<SubscriptionDto>>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetSubscriptionsHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<PaginatedList<SubscriptionDto>> Handle(GetSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateNewConnection();

            // 1. Build the "Base" Query (JOINS + WHERE)
            // We build this once so we can reuse it for both counting and fetching data.
            var sqlBuilder = new StringBuilder(@"
                FROM Subscriptions s
                INNER JOIN Members m ON s.MemberId = m.Id
                WHERE 1=1");

            var parameters = new DynamicParameters();

            // --- Filter: Search Term ---
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                // Search against Member Name OR Plan Name
                sqlBuilder.Append(" AND (m.FirstName LIKE @Search OR m.LastName LIKE @Search OR s.PlanName LIKE @Search)");
                parameters.Add("Search", $"%{request.SearchTerm}%");
            }

            // --- Filter: Statuses ---
            if (request.Statuses is not null && request.Statuses.Any())
            {
                sqlBuilder.Append(" AND s.Status IN @Statuses");
                // Dapper handles IEnumerable<string> for IN clauses automatically
                parameters.Add("Statuses", request.Statuses.Select(s => s.ToString()));
            }

            // --- Sort Logic ---
            var validColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["CustomerName"] = "m.FirstName", // Sort by Member Name
                ["Plan"] = "s.PlanName",
                ["Price"] = "s.Price",
                ["Date"] = "s.StartDate",
                ["Status"] = "s.Status"
            };

            var sortColumn = validColumns.TryGetValue(request.SortColumn ?? "", out var col) ? col : "s.StartDate";
            var sortDirection = (request.SortDirection?.ToUpper() == "ASC") ? "ASC" : "DESC";

            var orderBySql = $"ORDER BY {sortColumn} {sortDirection}";

            // --- Pagination Params ---
            parameters.Add("Skip", (request.PageNumber - 1) * request.PageSize);
            parameters.Add("Take", request.PageSize);

            // 2. Construct the Final Batch Query
            var baseSql = sqlBuilder.ToString();

            var fullSql = $@"
                -- Query 1: Total Count
                SELECT COUNT(*) 
                {baseSql};

                -- Query 2: Fetch Data with Member Name
                SELECT 
                    s.Id, 
                    s.MemberId, 
                    s.PlanName, 
                    s.Status, 
                    s.StartDate, 
                    s.EndDate, 
                    s.Price,
                    (m.FirstName + ' ' + m.LastName) as MemberFullName,
                    s.Created,
                    s.CreatedBy
                {baseSql}
                {orderBySql}
                OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            // 3. Execute
            using var multi = await connection.QueryMultipleAsync(fullSql, parameters);

            var totalCount = await multi.ReadFirstAsync<int>();
            var items = (await multi.ReadAsync<SubscriptionDto>()).ToList();

            // 4. Return
            return new PaginatedList<SubscriptionDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }
    }
}
