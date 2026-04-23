using Backend.Application.Dashboard.Entities;

namespace Backend.Application.Dashboard.Queries.Get;

public class GetDashboardDataRequestHandler
    : IRequestHandler<GetDashboardDataRequest, DashboardDataDto>
{
    public Task<DashboardDataDto> Handle(
        GetDashboardDataRequest request,
        CancellationToken cancellationToken
    )
    {
        var data = new DashboardDataDto
        {
            Stats = new DashboardStatsDto
            {
                TotalRevenue = 45231.89m,
                TotalUsers = 2350,
                ActiveUsers = 1280,
                ConversionRate = 12.5m,
            },
            ChartData =
            [
                new()
                {
                    Label = "Jan",
                    Revenue = 28400,
                    Expenses = 18200,
                },
                new()
                {
                    Label = "Feb",
                    Revenue = 21900,
                    Expenses = 16400,
                },
                new()
                {
                    Label = "Mar",
                    Revenue = 34800,
                    Expenses = 20100,
                },
                new()
                {
                    Label = "Apr",
                    Revenue = 31200,
                    Expenses = 19800,
                },
                new()
                {
                    Label = "May",
                    Revenue = 42100,
                    Expenses = 23400,
                },
                new()
                {
                    Label = "Jun",
                    Revenue = 45231,
                    Expenses = 24100,
                },
                new()
                {
                    Label = "Jul",
                    Revenue = 38900,
                    Expenses = 22500,
                },
                new()
                {
                    Label = "Aug",
                    Revenue = 47800,
                    Expenses = 25200,
                },
                new()
                {
                    Label = "Sep",
                    Revenue = 52100,
                    Expenses = 26800,
                },
                new()
                {
                    Label = "Oct",
                    Revenue = 49300,
                    Expenses = 26100,
                },
                new()
                {
                    Label = "Nov",
                    Revenue = 54800,
                    Expenses = 27900,
                },
                new()
                {
                    Label = "Dec",
                    Revenue = 61200,
                    Expenses = 29400,
                },
            ],
            RecentTransactions =
            [
                new()
                {
                    Id = "tx-001",
                    Customer = "Olivia Martin",
                    Email = "olivia.martin@email.com",
                    Amount = 1999.0m,
                    Status = "completed",
                    Date = "2026-04-01",
                },
                new()
                {
                    Id = "tx-002",
                    Customer = "Jackson Lee",
                    Email = "jackson.lee@email.com",
                    Amount = 39.0m,
                    Status = "completed",
                    Date = "2026-04-02",
                },
                new()
                {
                    Id = "tx-003",
                    Customer = "Isabella Nguyen",
                    Email = "isabella.nguyen@email.com",
                    Amount = 299.0m,
                    Status = "pending",
                    Date = "2026-04-03",
                },
                new()
                {
                    Id = "tx-004",
                    Customer = "William Kim",
                    Email = "will@email.com",
                    Amount = 99.0m,
                    Status = "completed",
                    Date = "2026-04-04",
                },
                new()
                {
                    Id = "tx-005",
                    Customer = "Sofia Davis",
                    Email = "sofia.davis@email.com",
                    Amount = 450.0m,
                    Status = "pending",
                    Date = "2026-04-05",
                },
                new()
                {
                    Id = "tx-006",
                    Customer = "Ethan Walker",
                    Email = "ethan.walker@email.com",
                    Amount = 129.0m,
                    Status = "refunded",
                    Date = "2026-04-05",
                },
                new()
                {
                    Id = "tx-007",
                    Customer = "Maya Patel",
                    Email = "maya.patel@email.com",
                    Amount = 880.0m,
                    Status = "completed",
                    Date = "2026-04-06",
                },
            ],
            Kpis =
            [
                new()
                {
                    Id = "revenue",
                    DeltaPct = 20.1m,
                    DeltaAbs = 7582m,
                    Spark = [22, 24, 23, 28, 26, 31, 30, 34, 33, 38, 36, 45],
                },
                new()
                {
                    Id = "users",
                    DeltaPct = 18.0m,
                    DeltaAbs = 180m,
                    Spark = [12, 15, 14, 17, 16, 19, 18, 20, 22, 24, 23, 24],
                },
                new()
                {
                    Id = "active",
                    DeltaPct = 19.0m,
                    DeltaAbs = 201m,
                    Spark = [8, 9, 9, 11, 12, 11, 13, 14, 14, 15, 14, 16],
                },
                new()
                {
                    Id = "conversion",
                    DeltaPct = -2.1m,
                    DeltaAbs = -0.3m,
                    Spark =
                    [
                        14,
                        13.5m,
                        14.2m,
                        13,
                        13.5m,
                        12.8m,
                        13.1m,
                        12.9m,
                        12.6m,
                        13.0m,
                        12.8m,
                        12.5m,
                    ],
                },
            ],
            Cohorts =
            [
                new()
                {
                    Key = "dau",
                    Value = 1280,
                    Total = 2350,
                },
                new()
                {
                    Key = "wau",
                    Value = 1842,
                    Total = 2350,
                },
                new()
                {
                    Key = "mau",
                    Value = 2180,
                    Total = 2350,
                },
            ],
            Channels =
            [
                new() { Key = "direct", Pct = 38 },
                new() { Key = "organic", Pct = 26 },
                new() { Key = "referral", Pct = 18 },
                new() { Key = "paid", Pct = 12 },
                new() { Key = "other", Pct = 6 },
            ],
            Activity =
            [
                new()
                {
                    Key = "deploy",
                    Title = "Deployment succeeded",
                    Detail = "api-gateway · v4.12.3",
                    RelativeTime = "just now",
                    Color = "success",
                },
                new()
                {
                    Key = "signup",
                    Title = "New enterprise signup",
                    Detail = "Northwind Analytics · 340 seats",
                    RelativeTime = "2m",
                    Color = "accent",
                },
                new()
                {
                    Key = "errors",
                    Title = "Spike in 4xx errors",
                    Detail = "/v1/checkout · 3.2% error rate",
                    RelativeTime = "14m",
                    Color = "warning",
                },
                new()
                {
                    Key = "invoice",
                    Title = "Invoice paid",
                    Detail = "Acme Inc · $18,400",
                    RelativeTime = "1h",
                    Color = "success",
                },
                new()
                {
                    Key = "policy",
                    Title = "Password policy updated",
                    Detail = "admin@company.com",
                    RelativeTime = "3h",
                    Color = "default",
                },
            ],
        };

        return Task.FromResult(data);
    }
}
