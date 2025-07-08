using Central.Application.DTOs;
using Central.Application.Services.Interfaces;
using Central.WebApi.Extensions;
using Central.WebApi.Filters;
using Central.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Central.WebApi.Endpoints;

public static class TenantDomainEndpoints
{
    public static RouteGroupBuilder MapTenantDomainEndpoints(this RouteGroupBuilder group)
    {
        var domainGroup = group.MapGroup("/tenant-domains")
            .WithTags("Tenant Domains")
            .WithDescription("Tenant domain management endpoints");

        // GET /api/tenant-domains
        domainGroup.MapGet("/", GetAllDomains)
            .WithName("GetAllTenantDomains")
            .WithSummary("Get all tenant domains with pagination")
            .WithDescription("Retrieve a paginated list of tenant domains with optional filtering and sorting")
            .Produces<PaginatedResponse<TenantDomainDto>>(200);

        // GET /api/tenant-domains/{id}
        domainGroup.MapGet("/{id:guid}", GetDomainById)
            .WithName("GetTenantDomainById")
            .WithSummary("Get tenant domain by ID")
            .WithDescription("Retrieve a specific tenant domain by its unique identifier")
            .Produces<TenantDomainDto>(200)
            .Produces<ApiErrorResponse>(404);

        // GET /api/tenant-domains/name/{name}
        domainGroup.MapGet("/name/{name}", GetDomainByName)
            .WithName("GetTenantDomainByName")
            .WithSummary("Get tenant domain by name")
            .WithDescription("Retrieve a specific tenant domain by its name")
            .Produces<TenantDomainDto>(200)
            .Produces<ApiErrorResponse>(404);

        // GET /api/tenant-domains/tenant/{tenantId}
        domainGroup.MapGet("/tenant/{tenantId:guid}", GetDomainsByTenantId)
            .WithName("GetDomainsByTenantId")
            .WithSummary("Get domains by tenant ID")
            .WithDescription("Retrieve all domains associated with a specific tenant")
            .Produces<List<TenantDomainDto>>(200);

        // POST /api/tenant-domains
        domainGroup.MapPost("/", CreateDomain)
            .WithName("CreateTenantDomain")
            .WithSummary("Create a new tenant domain")
            .WithDescription("Create a new tenant domain with the provided information")
            .AddEndpointFilter<ValidationFilter<CreateTenantDomainRequest>>()
            .Produces<TenantDomainDto>(201)
            .Produces<ValidationErrorResponse>(400);

        // PUT /api/tenant-domains/{id}
        domainGroup.MapPut("/{id:guid}", UpdateDomain)
            .WithName("UpdateTenantDomain")
            .WithSummary("Update an existing tenant domain")
            .WithDescription("Update an existing tenant domain with the provided information")
            .AddEndpointFilter<ValidationFilter<CreateTenantDomainRequest>>()
            .Produces<TenantDomainDto>(200)
            .Produces<ApiErrorResponse>(404)
            .Produces<ValidationErrorResponse>(400);

        // DELETE /api/tenant-domains/{id}
        domainGroup.MapDelete("/{id:guid}", DeleteDomain)
            .WithName("DeleteTenantDomain")
            .WithSummary("Delete a tenant domain")
            .WithDescription("Delete a tenant domain by its unique identifier")
            .Produces(204)
            .Produces<ApiErrorResponse>(404);

        // GET /api/tenant-domains/name-exists/{name}
        domainGroup.MapGet("/name-exists/{name}", CheckDomainExists)
            .WithName("CheckDomainNameExists")
            .WithSummary("Check if domain name exists")
            .WithDescription("Check if a domain name is already in use")
            .Produces<bool>(200);

        return group;
    }

    private static async Task<IResult> GetAllDomains(
        [FromServices] ITenantDomainService domainService,
        [AsParameters] PaginatedRequest request)
    {
        var result = await domainService.GetAllAsync(request);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> GetDomainById(
        Guid id,
        [FromServices] ITenantDomainService domainService)
    {
        var result = await domainService.GetByIdAsync(id);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> GetDomainByName(
        string name,
        [FromServices] ITenantDomainService domainService)
    {
        var result = await domainService.GetByNameAsync(name);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> GetDomainsByTenantId(
        Guid tenantId,
        [FromServices] ITenantDomainService domainService)
    {
        var result = await domainService.GetByTenantIdAsync(tenantId);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> CreateDomain(
        [FromBody] CreateTenantDomainRequest request,
        [FromServices] ITenantDomainService domainService)
    {
        var result = await domainService.CreateAsync(request);
        return result.ToCreatedResponse($"/api/tenant-domains/{result.Data?.Id}");
    }

    private static async Task<IResult> UpdateDomain(
        Guid id,
        [FromBody] UpdateTenantDomainRequest request,
        [FromServices] ITenantDomainService domainService)
    {
        if (id != request.Id)
            return Results.BadRequest(new ApiErrorResponse("ID mismatch", 400));

        var result = await domainService.UpdateAsync(request);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> DeleteDomain(
        Guid id,
        [FromServices] ITenantDomainService domainService)
    {
        var result = await domainService.DeleteAsync(id);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> CheckDomainExists(
        string name,
        [FromServices] ITenantDomainService domainService)
    {
        var result = await domainService.DomainExistsAsync(name);
        return result.ToHttpResponse();
    }
}
