using AutoMapper;
using FluentValidation;
using Central.Domain.Entities;
using Central.Application.DTOs;
using Central.Domain.Interfaces;
using Central.Application.Common;
using Central.Application.Services.Interfaces;

namespace Central.Application.Services;

public class TenantDomainService(
    ITenantDomainRepository tenantDomainRepository,
    IMapper mapper,
    IValidator<CreateTenantDomainRequest>? createValidator = null,
    IValidator<UpdateTenantDomainRequest>? updateValidator = null)
    : GenericService<TenantDomain, TenantDomainDto, CreateTenantDomainRequest, UpdateTenantDomainRequest,
        PaginatedRequest>(tenantDomainRepository, mapper, createValidator, updateValidator), ITenantDomainService
{
    protected override Guid GetEntityIdFromRequest(UpdateTenantDomainRequest request) 
        => request.Id;

    public async Task<Result<TenantDomainDto>> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<TenantDomainDto>.Failure("Domain name cannot be empty.");

        var domain = await tenantDomainRepository.GetByNameAsync(name);
        if (domain == null)
            return Result<TenantDomainDto>.Failure("Domain not found.");

        var domainDto = Mapper.Map<TenantDomainDto>(domain);
        return Result<TenantDomainDto>.Success(domainDto);
    }

    public async Task<Result<List<TenantDomainDto>>> GetByTenantIdAsync(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            return Result<List<TenantDomainDto>>.Failure("Tenant ID cannot be empty.");

        var domains = await tenantDomainRepository.GetByTenantIdAsync(tenantId);
        var domainDtos = Mapper.Map<List<TenantDomainDto>>(domains);
        return Result<List<TenantDomainDto>>.Success(domainDtos);
    }

    public async Task<Result<bool>> DomainExistsAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<bool>.Failure("Domain name cannot be empty.");

        var exists = await tenantDomainRepository.DomainExistsAsync(name);
        return Result<bool>.Success(exists);
    }

    public override async Task<Result<PaginatedResponse<TenantDomainDto>>> GetAllAsync(PaginatedRequest request)
    {
        var entities = await Repository.GetAllAsync();
        
        var query = entities.AsQueryable();
        
        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(d => 
                d.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase));
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "name" => request.SortDescending ? 
                    query.OrderByDescending(d => d.Name) : 
                    query.OrderBy(d => d.Name),
                "tenantid" => request.SortDescending ? 
                    query.OrderByDescending(d => d.TenantId) : 
                    query.OrderBy(d => d.TenantId),
                "createdat" => request.SortDescending ? 
                    query.OrderByDescending(d => d.CreatedAt) : 
                    query.OrderBy(d => d.CreatedAt),
                _ => query.OrderBy(d => d.Name)
            };
        }
        else
        {
            query = query.OrderBy(d => d.Name);
        }

        var totalCount = query.Count();
        var paginatedEntities = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = Mapper.Map<List<TenantDomainDto>>(paginatedEntities);
        
        var response = new PaginatedResponse<TenantDomainDto>(
            dtos,
            totalCount,
            request.Page,
            request.PageSize
        );

        return Result<PaginatedResponse<TenantDomainDto>>.Success(response);
    }
}