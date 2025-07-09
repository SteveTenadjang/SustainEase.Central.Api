using AutoMapper;
using Central.Application.DTOs;
using Central.Domain.Entities;

namespace Central.Application.Mappings;

public class TenantSubscriptionProfile : Profile
{
    public TenantSubscriptionProfile()
    {
        // Entity to DTO mappings
        CreateMap<TenantSubscription, TenantSubscriptionDto>()
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.StartDate.AddMonths(src.Duration)))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src =>
                DateTime.UtcNow >= src.StartDate &&
                DateTime.UtcNow <= src.StartDate.AddMonths(src.Duration)))
            .ForMember(dest => dest.TenantName, opt => opt.MapFrom(src => src.Tenant != null ? src.Tenant.Name : null))
            .ForMember(dest => dest.BundleName, opt => opt.MapFrom(src => src.Bundle != null ? src.Bundle.Name : null));

        // Request to Entity mappings
        CreateMap<CreateTenantSubscriptionRequest, TenantSubscription>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Tenant, opt => opt.Ignore())
            .ForMember(dest => dest.Bundle, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

        CreateMap<UpdateTenantSubscriptionRequest, TenantSubscription>()
            .ForMember(dest => dest.Tenant, opt => opt.Ignore())
            .ForMember(dest => dest.Bundle, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());
    }
}