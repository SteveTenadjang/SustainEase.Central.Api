namespace Central.Domain.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } =  true;
    public string? LogoUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? ConnectionString { get; set; }

    public List<TenantDomain> Domains { get; set; } = [];
    public List<TenantSubscription> Subscriptions { get; set; } = [];
}