using AutoMapper;
using Central.Application.Common;
using Central.Application.DTOs;
using Central.Application.Services.Interfaces;
using Central.Domain.Interfaces;
using FluentValidation;

namespace Central.Application.Services;

public abstract class GenericService<TEntity, TDto, TCreateRequest, TUpdateRequest, TListRequest>(
    IGenericRepository<TEntity> repository,
    IMapper mapper,
    IValidator<TCreateRequest>? createValidator = null,
    IValidator<TUpdateRequest>? updateValidator = null)
    : IGenericService<TEntity, TDto, TCreateRequest, TUpdateRequest, TListRequest>
    where TEntity : class
    where TDto : class
    where TCreateRequest : class
    where TUpdateRequest : class
    where TListRequest : PaginatedRequest
{
    protected readonly IValidator<TCreateRequest>? CreateValidator = createValidator;
    protected readonly IMapper Mapper = mapper;
    protected readonly IGenericRepository<TEntity> Repository = repository;
    protected readonly IValidator<TUpdateRequest>? UpdateValidator = updateValidator;

    public virtual async Task<Result<TDto>> GetByIdAsync(Guid id)
    {
        var entity = await Repository.GetByIdAsync(id);
        if (entity == null)
            return Result<TDto>.Failure($"{typeof(TEntity).Name} not found.");

        var dto = Mapper.Map<TDto>(entity);
        return Result<TDto>.Success(dto);
    }

    public virtual async Task<Result<List<TDto>>> GetAllAsync()
    {
        var entities = await Repository.GetAllAsync();
        var dtos = Mapper.Map<List<TDto>>(entities);
        return Result<List<TDto>>.Success(dtos);
    }

    public virtual async Task<Result<PaginatedResponse<TDto>>> GetAllAsync(TListRequest request)
    {
        var entities = await Repository.GetAllAsync();

        var query = entities.AsQueryable();

        var totalCount = query.Count();
        var paginatedEntities = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = Mapper.Map<List<TDto>>(paginatedEntities);

        var response = new PaginatedResponse<TDto>(
            dtos,
            totalCount,
            request.Page,
            request.PageSize
        );

        return Result<PaginatedResponse<TDto>>.Success(response);
    }

    public virtual async Task<Result<TDto>> CreateAsync(TCreateRequest request)
    {
        if (CreateValidator != null)
        {
            var validationResult = await CreateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<TDto>.Failure(errors);
            }
        }

        var entity = Mapper.Map<TEntity>(request);
        var createdEntity = await Repository.AddAsync(entity);
        var dto = Mapper.Map<TDto>(createdEntity);

        return Result<TDto>.Success(dto);
    }

    public virtual async Task<Result<TDto>> UpdateAsync(TUpdateRequest request)
    {
        if (UpdateValidator != null)
        {
            var validationResult = await UpdateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<TDto>.Failure(errors);
            }
        }

        var entityId = GetEntityIdFromRequest(request);
        var existingEntity = await Repository.GetByIdAsync(entityId);
        if (existingEntity == null)
            return Result<TDto>.Failure($"{typeof(TEntity).Name} not found.");

        Mapper.Map(request, existingEntity);
        var updatedEntity = await Repository.UpdateAsync(existingEntity);
        var dto = Mapper.Map<TDto>(updatedEntity);

        return Result<TDto>.Success(dto);
    }

    public virtual async Task<Result> DeleteAsync(Guid id)
    {
        var exists = await Repository.ExistsAsync(id);
        if (!exists)
            return Result.Failure($"{typeof(TEntity).Name} not found.");

        var deleted = await Repository.DeleteAsync(id);
        return !deleted
            ? Result.Failure($"Failed to delete {typeof(TEntity).Name}.")
            : Result.Success();
    }

    public virtual async Task<Result<bool>> ExistsAsync(Guid id)
    {
        var exists = await Repository.ExistsAsync(id);
        return Result<bool>.Success(exists);
    }

    public virtual async Task<Result<int>> CountAsync()
    {
        var count = await Repository.CountAsync();
        return Result<int>.Success(count);
    }

    protected abstract Guid GetEntityIdFromRequest(TUpdateRequest request);
}