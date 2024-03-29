﻿namespace Backend.Application.Multitenancy.Queries.Upgrade;

public class UpgradeSubscriptionRequest : IRequest<string>
{
    public string TenantId { get; set; } = default!;
    public DateTime ExtendedExpiryDate { get; set; }
}
