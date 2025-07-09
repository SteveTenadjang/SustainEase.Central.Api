using Central.Application.DTOs;
using Central.Application.Services.Interfaces;
using Central.WebApi.Extensions;
using Central.WebApi.Filters;
using Central.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Central.WebApi.Endpoints;

public static class TenantSubscriptionEndpoints
{
    public static RouteGroupBuilder MapTenantSubscriptionEndpoints(this RouteGroupBuilder group)
    {
        var subscriptionGroup = group.MapGroup("/tenant-subscriptions")
            .WithTags("Tenant Subscriptions")
            .WithDescription("Tenant subscription management endpoints");

        // GET /api/tenant-subscriptions
        subscriptionGroup.MapGet("/", GetAllSubscriptions)
            .WithName("GetAllTenantSubscriptions")
            .WithSummary("Get all tenant subscriptions with pagination")
            .WithDescription("Retrieve a paginated list of tenant subscriptions with optional filtering and sorting")
            .Produces<PaginatedResponse<TenantSubscriptionDto>>();

        // GET /api/tenant-subscriptions/{id}
        subscriptionGroup.MapGet("/{id:guid}", GetSubscriptionById)
            .WithName("GetTenantSubscriptionById")
            .WithSummary("Get tenant subscription by ID")
            .WithDescription("Retrieve a specific tenant subscription by its unique identifier")
            .Produces<TenantSubscriptionDto>()
            .Produces<ApiErrorResponse>(404);

        // GET /api/tenant-subscriptions/tenant/{tenantId}
        subscriptionGroup.MapGet("/tenant/{tenantId:guid}", GetSubscriptionsByTenantId)
            .WithName("GetSubscriptionsByTenantId")
            .WithSummary("Get subscriptions by tenant ID")
            .WithDescription("Retrieve all subscriptions associated with a specific tenant")
            .Produces<List<TenantSubscriptionDto>>();

        // GET /api/tenant-subscriptions/bundle/{bundleId}
        subscriptionGroup.MapGet("/bundle/{bundleId:guid}", GetSubscriptionsByBundleId)
            .WithName("GetSubscriptionsByBundleId")
            .WithSummary("Get subscriptions by bundle ID")
            .WithDescription("Retrieve all subscriptions associated with a specific bundle")
            .Produces<List<TenantSubscriptionDto>>();

        // GET /api/tenant-subscriptions/active
        subscriptionGroup.MapGet("/active", GetActiveSubscriptions)
            .WithName("GetActiveSubscriptions")
            .WithSummary("Get all active subscriptions")
            .WithDescription("Retrieve all currently active subscriptions across all tenants")
            .Produces<List<TenantSubscriptionDto>>();

        // POST /api/tenant-subscriptions
        subscriptionGroup.MapPost("/", CreateSubscription)
            .WithName("CreateTenantSubscription")
            .WithSummary("Create a new tenant subscription")
            .WithDescription("Create a new tenant subscription with the provided information")
            .AddEndpointFilter<ValidationFilter<CreateTenantSubscriptionRequest>>()
            .Produces<TenantSubscriptionDto>(201)
            .Produces<ValidationErrorResponse>(400);

        // PUT /api/tenant-subscriptions/{id}
        subscriptionGroup.MapPut("/{id:guid}", UpdateSubscription)
            .WithName("UpdateTenantSubscription")
            .WithSummary("Update an existing tenant subscription")
            .WithDescription("Update an existing tenant subscription with the provided information")
            .AddEndpointFilter<ValidationFilter<UpdateTenantSubscriptionRequest>>()
            .Produces<TenantSubscriptionDto>()
            .Produces<ApiErrorResponse>(404)
            .Produces<ValidationErrorResponse>(400);

        // DELETE /api/tenant-subscriptions/{id}
        subscriptionGroup.MapDelete("/{id:guid}", DeleteSubscription)
            .WithName("DeleteTenantSubscription")
            .WithSummary("Delete a tenant subscription")
            .WithDescription("Delete a tenant subscription by its unique identifier")
            .Produces(204)
            .Produces<ApiErrorResponse>(404);

        // GET /api/tenant-subscriptions/check-active/{tenantId}/{bundleId}
        subscriptionGroup.MapGet("/check-active/{tenantId:guid}/{bundleId:guid}", CheckActiveSubscription)
            .WithName("CheckActiveSubscription")
            .WithSummary("Check if tenant has active subscription")
            .WithDescription("Check if a tenant has an active subscription for a specific bundle")
            .Produces<bool>();

        // GET /api/tenant-subscriptions/{id}/is-active
        subscriptionGroup.MapGet("/{id:guid}/is-active", CheckSubscriptionActive)
            .WithName("CheckSubscriptionActive")
            .WithSummary("Check if subscription is active")
            .WithDescription("Check if a specific subscription is currently active")
            .Produces<bool>();

        return group;
    }

    private static async Task<IResult> GetAllSubscriptions(
        [FromServices] ITenantSubscriptionService subscriptionService,
        [AsParameters] PaginatedRequest request)
    {
        var result = await subscriptionService.GetAllAsync(request);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> GetSubscriptionById(
        Guid id,
        [FromServices] ITenantSubscriptionService subscriptionService)
    {
        var result = await subscriptionService.GetByIdAsync(id);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> GetSubscriptionsByTenantId(
        Guid tenantId,
        [FromServices] ITenantSubscriptionService subscriptionService)
    {
        var result = await subscriptionService.GetByTenantIdAsync(tenantId);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> GetSubscriptionsByBundleId(
        Guid bundleId,
        [FromServices] ITenantSubscriptionService subscriptionService)
    {
        var result = await subscriptionService.GetByBundleIdAsync(bundleId);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> GetActiveSubscriptions(
        [FromServices] ITenantSubscriptionService subscriptionService)
    {
        var result = await subscriptionService.GetActiveSubscriptionsAsync();
        return result.ToHttpResponse();
    }

    private static async Task<IResult> CreateSubscription(
        [FromBody] CreateTenantSubscriptionRequest request,
        [FromServices] ITenantSubscriptionService subscriptionService)
    {
        var result = await subscriptionService.CreateAsync(request);
        return result.ToCreatedResponse($"/api/tenant-subscriptions/{result.Data?.Id}");
    }

    private static async Task<IResult> UpdateSubscription(
        Guid id,
        [FromBody] UpdateTenantSubscriptionRequest request,
        [FromServices] ITenantSubscriptionService subscriptionService)
    {
        if (id != request.Id)
            return Results.BadRequest(new ApiErrorResponse("ID mismatch", 400));

        var result = await subscriptionService.UpdateAsync(request);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> DeleteSubscription(
        Guid id,
        [FromServices] ITenantSubscriptionService subscriptionService)
    {
        var result = await subscriptionService.DeleteAsync(id);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> CheckActiveSubscription(
        Guid tenantId,
        Guid bundleId,
        [FromServices] ITenantSubscriptionService subscriptionService)
    {
        var result = await subscriptionService.HasActiveSubscriptionAsync(tenantId, bundleId);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> CheckSubscriptionActive(
        Guid id,
        [FromServices] ITenantSubscriptionService subscriptionService)
    {
        var result = await subscriptionService.IsSubscriptionActiveAsync(id);
        return result.ToHttpResponse();
    }
}