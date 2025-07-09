using Central.Application.Events;
using Central.Application.Events.Handlers.Tenant;
using Central.Application.Mappings;
using Central.Application.Services;
using Central.Application.Services.Interfaces;
using Central.Domain.Events.Tenant;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Central.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        // Add AutoMapper
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<BundleProfile>();
            cfg.AddProfile<TenantProfile>();
            cfg.AddProfile<TenantDomainProfile>();
            cfg.AddProfile<TenantSubscriptionProfile>();
        });

        // Add FluentValidation
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        // Add Event Infrastructure
        services.AddEventInfrastructure();

        // Add Application Services
        services.AddApplicationServices();
    }

    private static void AddEventInfrastructure(this IServiceCollection services)
    {
        // Register event dispatcher
        services.AddScoped<IEventDispatcher, EventDispatcher>();

        // Register event handlers
        services.AddScoped<IEventHandler<TenantCreatedEvent>, TenantCreatedEventHandler>();
        services.AddScoped<IEventHandler<TenantDeletedEvent>, TenantDeletedEventHandler>();
    }

    private static void AddApplicationServices(this IServiceCollection services)
    {
        // Register application services
        // services.AddScoped(typeof(IGenericService<,,,,>), typeof(GenericService<,,,,>));
        services.AddScoped<IBundleService, BundleService>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<ITenantDomainService, TenantDomainService>();
        services.AddScoped<ITenantSubscriptionService, TenantSubscriptionService>();
    }
}