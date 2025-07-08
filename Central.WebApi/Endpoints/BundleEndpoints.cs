using Central.Application.DTOs;
using Central.Application.Services.Interfaces;
using Central.WebApi.Extensions;
using Central.WebApi.Filters;
using Central.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Central.WebApi.Endpoints;

public static class BundleEndpoints
{
    public static RouteGroupBuilder MapBundleEndpoints(this RouteGroupBuilder group)
    {
        var bundleGroup = group.MapGroup("/bundles")
            .WithTags("Bundles")
            .WithDescription("Bundle management endpoints");

        // GET /api/bundles
        bundleGroup.MapGet("/", GetAllBundles)
            .WithName("GetAllBundles")
            .WithSummary("Get all bundles with pagination")
            .WithDescription("Retrieve a paginated list of bundles with optional filtering and sorting")
            .Produces<PaginatedResponse<BundleDto>>(200)
            .Produces<ApiErrorResponse>(400);

        // GET /api/bundles/{id}
        bundleGroup.MapGet("/{id:guid}", GetBundleById)
            .WithName("GetBundleById")
            .WithSummary("Get bundle by ID")
            .WithDescription("Retrieve a specific bundle by its unique identifier")
            .Produces<BundleDto>(200)
            .Produces<ApiErrorResponse>(404);

        // GET /api/bundles/key/{key}
        bundleGroup.MapGet("/key/{key}", GetBundleByKey)
            .WithName("GetBundleByKey")
            .WithSummary("Get bundle by key")
            .WithDescription("Retrieve a specific bundle by its unique key")
            .Produces<BundleDto>(200)
            .Produces<ApiErrorResponse>(404);

        // POST /api/bundles
        bundleGroup.MapPost("/", CreateBundle)
            .WithName("CreateBundle")
            .WithSummary("Create a new bundle")
            .WithDescription("Create a new bundle with the provided information")
            .AddEndpointFilter<ValidationFilter<CreateBundleRequest>>()
            .Produces<BundleDto>(201)
            .Produces<ValidationErrorResponse>(400);

        // PUT /api/bundles/{id}
        bundleGroup.MapPut("/{id:guid}", UpdateBundle)
            .WithName("UpdateBundle")
            .WithSummary("Update an existing bundle")
            .WithDescription("Update an existing bundle with the provided information")
            .AddEndpointFilter<ValidationFilter<UpdateBundleRequest>>()
            .Produces<BundleDto>(200)
            .Produces<ApiErrorResponse>(404)
            .Produces<ValidationErrorResponse>(400);

        // DELETE /api/bundles/{id}
        bundleGroup.MapDelete("/{id:guid}", DeleteBundle)
            .WithName("DeleteBundle")
            .WithSummary("Delete a bundle")
            .WithDescription("Delete a bundle by its unique identifier")
            .Produces(204)
            .Produces<ApiErrorResponse>(404);

        // GET /api/bundles/key-exists/{key}
        bundleGroup.MapGet("/key-exists/{key}", CheckKeyExists)
            .WithName("CheckBundleKeyExists")
            .WithSummary("Check if bundle key exists")
            .WithDescription("Check if a bundle key is already in use")
            .Produces<bool>(200);

        return group;
    }

    private static async Task<IResult> GetAllBundles(
        [FromServices] IBundleService bundleService,
        [AsParameters] BundleListRequest request)
    {
        var result = await bundleService.GetAllAsync(request);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> GetBundleById(
        Guid id,
        [FromServices] IBundleService bundleService)
    {
        var result = await bundleService.GetByIdAsync(id);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> GetBundleByKey(
        string key,
        [FromServices] IBundleService bundleService)
    {
        var result = await bundleService.GetByKeyAsync(key);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> CreateBundle(
        [FromBody] CreateBundleRequest request,
        [FromServices] IBundleService bundleService)
    {
        var result = await bundleService.CreateAsync(request);
        return result.ToCreatedResponse($"/api/bundles/{result.Data?.Id}");
    }

    private static async Task<IResult> UpdateBundle(
        Guid id,
        [FromBody] UpdateBundleRequest request,
        [FromServices] IBundleService bundleService)
    {
        if (id != request.Id)
            return Results.BadRequest(new ApiErrorResponse("ID mismatch", 400));

        var result = await bundleService.UpdateAsync(request);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> DeleteBundle(
        Guid id,
        [FromServices] IBundleService bundleService)
    {
        var result = await bundleService.DeleteAsync(id);
        return result.ToHttpResponse();
    }

    private static async Task<IResult> CheckKeyExists(
        string key,
        [FromServices] IBundleService bundleService)
    {
        var result = await bundleService.KeyExistsAsync(key);
        return result.ToHttpResponse();
    }
}