using Central.Application.DTOs;
using Central.Application.Services.Interfaces;
using Central.WebApi.Extensions;
using Central.WebApi.Filters;
using Central.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Central.WebApi.Endpoints;

public static class TenantEndpoints
{
    public static RouteGroupBuilder MapTenantEndpoints(this RouteGroupBuilder group)
    {
        var tenantGroup = group.MapGroup("/tenants")
            .WithTags("Tenants")
            .WithDescription("Tenant management endpoints");

        // GET /api/tenants
        tenantGroup.MapGet("/", GetAllTenants)
            .WithName("GetAllTenants")
            .WithSummary("Get all tenants with pagination")
            .WithDescription("Retrieve a paginated list of tenants with optional filtering and sorting")
            .Produces<PaginatedResponse<TenantDto>>();

        // GET /api/tenants/{id}
        tenantGroup.MapGet("/{id:guid}", GetTenantById)
            .WithName("GetTenantById")
            .WithSummary("Get tenant by ID")
            .WithDescription("Retrieve a specific tenant by its unique identifier")
            .Produces<TenantDto>()
            .Produces<ApiErrorResponse>(404);

        // GET /api/tenants/email/{email}
        tenantGroup.MapGet("/email/{email}", GetTenantByEmail)
            .WithName("GetTenantByEmail")
            .WithSummary("Get tenant by email")
            .WithDescription("Retrieve a specific tenant by its email address")
            .Produces<TenantDto>()
            .Produces<ApiErrorResponse>(404);

        // POST /api/tenants
        tenantGroup.MapPost("/", CreateTenant)
            .WithName("CreateTenant")
            .WithSummary("Create a new tenant")
            .WithDescription(
                "Create a new tenant with the provided information. This will trigger a TenantCreated event.")
            .AddEndpointFilter<ValidationFilter<CreateTenantRequest>>()
            .Produces<TenantDto>(201)
            .Produces<ValidationErrorResponse>(400);

        // PUT /api/tenants/{id}
        tenantGroup.MapPut("/{id:guid}", UpdateTenant)
            .WithName("UpdateTenant")
            .WithSummary("Update an existing tenant")
            .WithDescription("Update an existing tenant with the provided information")
            .AddEndpointFilter<ValidationFilter<UpdateTenantRequest>>()
            .Produces<TenantDto>()
            .Produces<ApiErrorResponse>(404)
            .Produces<ValidationErrorResponse>(400);

        // DELETE /api/tenants/{id}
        tenantGroup.MapDelete("/{id:guid}", DeleteTenant)
            .WithName("DeleteTenant")
            .WithSummary("Delete a tenant")
            .WithDescription("Delete a tenant by its unique identifier")
            .Produces(204)
            .Produces<ApiErrorResponse>(404);

        // POST /api/tenants/{id}/activate
        tenantGroup.MapPost("/{id:guid}/activate", ActivateTenant)
            .WithName("ActivateTenant")
            .WithSummary("Activate a tenant")
            .WithDescription("Activate a tenant by its unique identifier")
            .Produces(204)
            .Produces<ApiErrorResponse>(404);

        // POST /api/tenants/{id}/deactivate
        tenantGroup.MapPost("/{id:guid}/deactivate", DeactivateTenant)
            .WithName("DeactivateTenant")
            .WithSummary("Deactivate a tenant")
            .WithDescription("Deactivate a tenant by its unique identifier")
            .Produces(204)
            .Produces<ApiErrorResponse>(404);

        return group;
    }

    private static async Task<IResult> GetAllTenants(
        [FromServices] ITenantService tenantService,
        [AsParameters] TenantListRequest request)
    {
        var result = await tenantService.GetAllAsync(request);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> GetTenantById(
        Guid id,
        [FromServices] ITenantService tenantService)
    {
        var result = await tenantService.GetByIdAsync(id);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> GetTenantByEmail(
        string email,
        [FromServices] ITenantService tenantService)
    {
        var result = await tenantService.GetByEmailAsync(email);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> CreateTenant(
        [FromBody] CreateTenantRequest request,
        [FromServices] ITenantService tenantService)
    {
        var result = await tenantService.CreateAsync(request);
        return result.ToCreatedResponse($"/api/tenants/{result.Data?.Id}");
    }

    private static async Task<IResult> UpdateTenant(
        Guid id,
        [FromBody] UpdateTenantRequest request,
        [FromServices] ITenantService tenantService)
    {
        if (id != request.Id)
            return Results.BadRequest(new ApiErrorResponse("ID mismatch", 400));

        var result = await tenantService.UpdateAsync(request);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> DeleteTenant(
        Guid id,
        [FromServices] ITenantService tenantService)
    {
        var result = await tenantService.DeleteAsync(id);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> ActivateTenant(
        Guid id,
        [FromServices] ITenantService tenantService)
    {
        var result = await tenantService.ActivateAsync(id);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> DeactivateTenant(
        Guid id,
        [FromServices] ITenantService tenantService)
    {
        var result = await tenantService.DeactivateAsync(id);
        return result.ToHttpResponse();
    }
}