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
            // 1. USE CreateNewConnection: 
            // We use this so we can dispose it immediately after this query finishes 
            // without affecting the rest of the HTTP request scope.
            using var connection = _connectionFactory.CreateNewConnection();

            // 2. Build Dynamic SQL parts
            var (whereSql, parameters) = BuildWhereClause(request);
            var sortSql = BuildSortClause(request.SortColumn, request.SortDirection);

            // 3. Calculate Pagination
            parameters.Add("Skip", (request.PageNumber - 1) * request.PageSize);
            parameters.Add("Take", request.PageSize);

            // 4. Construct the Batch Query
            // We use string interpolation carefully here. 
            // {whereSql} and {sortSql} are safe because they are built internally, not direct user input.
            var sql = $@"
            SELECT COUNT(*) 
            FROM Subscriptions s
            {whereSql};

            SELECT s.* FROM Subscriptions s
            {whereSql}
            {sortSql}
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;";

            // 5. Execute Single Round-Trip
            using var multi = await connection.QueryMultipleAsync(sql, parameters);

            var totalCount = await multi.ReadFirstAsync<int>();
            var items = (await multi.ReadAsync<SubscriptionDto>()).ToList();

            // 6. Return Result
            return new PaginatedList<SubscriptionDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        // --- Helper Methods ---

        private static (string Sql, DynamicParameters Parameters) BuildWhereClause(GetSubscriptionsQuery request)
        {
            var sb = new StringBuilder("WHERE 1=1");
            var p = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                sb.Append(" AND (s.CustomerName LIKE @Search OR s.PlanName LIKE @Search)");
                p.Add("Search", $"%{request.SearchTerm}%");
            }

            if (request.Statuses is not null && request.Statuses.Any())
            {
                sb.Append(" AND s.status IN @Statuses");

                // --- FIX IS HERE ---
                // Convert the Enum (e.g. 3) to its Name (e.g. "Cancelled")
                // This ensures SQL receives: WHERE status IN ('Cancelled', 'Active')
                var statusNames = request.Statuses.Select(s => s.ToString());

                p.Add("Statuses", statusNames);
            }

            return (sb.ToString(), p);
        }

        private static string BuildSortClause(string? column, string? direction)
        {
            // Security: Whitelist allowed columns to prevent SQL Injection
            var validColumns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["CustomerName"] = "s.CustomerName",
                ["Plan"] = "s.PlanName",
                ["Price"] = "s.Price",
                ["Date"] = "s.StartDate",
                ["Status"] = "s.Status"
            };

            // Default to StartDate if invalid column provided
            var selectedColumn = validColumns.TryGetValue(column ?? "", out var col) ? col : "s.StartDate";

            // Default to DESC (newest first)
            var selectedDirection = (direction?.ToUpper() == "ASC") ? "ASC" : "DESC";

            return $"ORDER BY {selectedColumn} {selectedDirection}";
        }
    }
}
