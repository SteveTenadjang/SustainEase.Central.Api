using AutoMapper;
using Central.Application.DTOs;
using Central.Domain.Entities;

namespace Central.Application.Mappings;

public class TenantProfile : Profile
{
    public TenantProfile()
    {
        // Entity to DTO mappings
        CreateMap<Tenant, TenantDto>();

        CreateMap<TenantDomain, TenantDomainDto>();
        CreateMap<TenantSubscription, TenantSubscriptionDto>();

        // Request to Entity mappings
        CreateMap<CreateTenantRequest, Tenant>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.ConnectionString, opt => opt.Ignore())
            .ForMember(dest => dest.Domains, opt => opt.Ignore())
            .ForMember(dest => dest.Subscriptions, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());

        CreateMap<UpdateTenantRequest, Tenant>()
            .ForMember(dest => dest.ConnectionString, opt => opt.Ignore())
            .ForMember(dest => dest.Domains, opt => opt.Ignore())
            .ForMember(dest => dest.Subscriptions, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedBy, opt => opt.Ignore());
    }
}