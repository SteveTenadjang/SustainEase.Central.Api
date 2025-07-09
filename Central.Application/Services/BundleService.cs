using AutoMapper;
using Central.Application.Common;
using Central.Application.DTOs;
using Central.Application.Services.Interfaces;
using Central.Domain.Entities;
using Central.Domain.Interfaces;
using FluentValidation;

namespace Central.Application.Services;

public class BundleService(
    IBundleRepository bundleRepository,
    IMapper mapper,
    IValidator<CreateBundleRequest>? createValidator = null,
    IValidator<UpdateBundleRequest>? updateValidator = null)
    : GenericService<Bundle, BundleDto, CreateBundleRequest, UpdateBundleRequest, BundleListRequest>(bundleRepository,
        mapper, createValidator, updateValidator), IBundleService
{
    public async Task<Result<BundleDto>> GetByKeyAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return Result<BundleDto>.Failure("Bundle key cannot be empty.");

        var bundle = await bundleRepository.GetByKeyAsync(key);
        if (bundle == null)
            return Result<BundleDto>.Failure("Bundle not found.");

        var bundleDto = Mapper.Map<BundleDto>(bundle);
        return Result<BundleDto>.Success(bundleDto);
    }

    public async Task<Result<bool>> KeyExistsAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return Result<bool>.Failure("Bundle key cannot be empty.");

        var exists = await bundleRepository.KeyExistsAsync(key);
        return Result<bool>.Success(exists);
    }

    public override async Task<Result<PaginatedResponse<BundleDto>>> GetAllAsync(BundleListRequest request)
    {
        var entities = await Repository.GetAllAsync();

        var query = entities.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(b =>
                b.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                b.Key.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                (b.Description != null && b.Description.Contains(request.Search, StringComparison.OrdinalIgnoreCase)));

        // Apply Bundle-specific filters
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(b => b.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Key))
            query = query.Where(b => b.Key.Contains(request.Key, StringComparison.OrdinalIgnoreCase));

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(request.SortBy))
            query = request.SortBy.ToLower() switch
            {
                "name" => request.SortDescending ? query.OrderByDescending(b => b.Name) : query.OrderBy(b => b.Name),
                "key" => request.SortDescending ? query.OrderByDescending(b => b.Key) : query.OrderBy(b => b.Key),
                "createdat" => request.SortDescending
                    ? query.OrderByDescending(b => b.CreatedAt)
                    : query.OrderBy(b => b.CreatedAt),
                _ => query.OrderBy(b => b.Name)
            };
        else
            query = query.OrderBy(b => b.Name);

        var totalCount = query.Count();
        var paginatedEntities = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = Mapper.Map<List<BundleDto>>(paginatedEntities);

        var response = new PaginatedResponse<BundleDto>(
            dtos,
            totalCount,
            request.Page,
            request.PageSize
        );

        return Result<PaginatedResponse<BundleDto>>.Success(response);
    }

    protected override Guid GetEntityIdFromRequest(UpdateBundleRequest request)
    {
        return request.Id;
    }
}