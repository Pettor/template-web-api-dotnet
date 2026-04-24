namespace Backend.Application.Dashboard.Entities;

public class DashboardStatsDto
{
    public decimal TotalRevenue { get; set; }
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public decimal ConversionRate { get; set; }
}

public class DashboardChartPointDto
{
    public string Label { get; set; } = default!;
    public decimal Revenue { get; set; }
    public decimal Expenses { get; set; }
}

public class DashboardTransactionDto
{
    public string Id { get; set; } = default!;
    public string Customer { get; set; } = default!;
    public string Email { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Status { get; set; } = default!;
    public string Date { get; set; } = default!;
}

public class DashboardKpiDto
{
    public string Id { get; set; } = default!;
    public decimal DeltaPct { get; set; }
    public decimal DeltaAbs { get; set; }
    public List<decimal> Spark { get; set; } = default!;
}

public class DashboardCohortDto
{
    public string Key { get; set; } = default!;
    public decimal Value { get; set; }
    public decimal Total { get; set; }
}

public class DashboardChannelDto
{
    public string Key { get; set; } = default!;
    public decimal Pct { get; set; }
}

public class DashboardActivityDto
{
    public string Key { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Detail { get; set; } = default!;
    public string RelativeTime { get; set; } = default!;
    public string Color { get; set; } = default!;
}

public class DashboardDataDto
{
    public DashboardStatsDto Stats { get; set; } = default!;
    public List<DashboardChartPointDto> ChartData { get; set; } = default!;
    public List<DashboardTransactionDto> RecentTransactions { get; set; } = default!;
    public List<DashboardKpiDto> Kpis { get; set; } = default!;
    public List<DashboardCohortDto> Cohorts { get; set; } = default!;
    public List<DashboardChannelDto> Channels { get; set; } = default!;
    public List<DashboardActivityDto> Activity { get; set; } = default!;
}
