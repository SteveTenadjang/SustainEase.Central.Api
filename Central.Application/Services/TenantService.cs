using AutoMapper;
using FluentValidation;
using Central.Domain.Entities;
using Central.Application.DTOs;
using Central.Domain.Interfaces;
using Central.Application.Common;
using Central.Application.Events;
using Central.Domain.Events.Tenant;
using Central.Application.Services.Interfaces;

namespace Central.Application.Services;

public class TenantService(
    ITenantRepository tenantRepository,
    ITenantDomainRepository tenantDomainRepository,
    IMapper mapper,
    IEventDispatcher eventDispatcher,
    IValidator<CreateTenantRequest>? createValidator = null,
    IValidator<UpdateTenantRequest>? updateValidator = null
)
    : GenericService<Tenant, TenantDto, CreateTenantRequest, UpdateTenantRequest, TenantListRequest>(tenantRepository,
        mapper, createValidator, updateValidator), ITenantService
{
    protected override Guid GetEntityIdFromRequest(UpdateTenantRequest request)
        => request.Id;

    public async Task<Result<TenantDto>> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result<TenantDto>.Failure("Email cannot be empty.");

        var tenant = await tenantRepository.GetByEmailAsync(email);
        if (tenant == null)
            return Result<TenantDto>.Failure("Tenant not found.");

        var tenantDto = Mapper.Map<TenantDto>(tenant);
        return Result<TenantDto>.Success(tenantDto);
    }

    public async Task<Result<TenantDto>> GetByPhoneNumberAsync(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return Result<TenantDto>.Failure("Phone number cannot be empty.");

        var tenant = await tenantRepository.GetByPhoneNumberAsync(phoneNumber);
        if (tenant == null)
            return Result<TenantDto>.Failure("Tenant not found.");

        var tenantDto = Mapper.Map<TenantDto>(tenant);
        return Result<TenantDto>.Success(tenantDto);
    }

    public async Task<Result> ActivateAsync(Guid id)
    {
        var tenant = await tenantRepository.GetByIdAsync(id);
        if (tenant == null)
            return Result.Failure("Tenant not found.");

        if (tenant.IsActive)
            return Result.Failure("Tenant is already active.");

        tenant.IsActive = true;
        await tenantRepository.UpdateAsync(tenant);

        return Result.Success();
    }

    public async Task<Result> DeactivateAsync(Guid id)
    {
        var tenant = await tenantRepository.GetByIdAsync(id);
        if (tenant == null)
            return Result.Failure("Tenant not found.");

        if (!tenant.IsActive)
            return Result.Failure("Tenant is already inactive.");

        tenant.IsActive = false;
        await tenantRepository.UpdateAsync(tenant);

        return Result.Success();
    }

    public override async Task<Result<TenantDto>> CreateAsync(CreateTenantRequest request)
    {
        // Validate request
        if (CreateValidator != null)
        {
            var validationResult = await CreateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return Result<TenantDto>.Failure(errors);
            }
        }

        // Create tenant
        var tenant = Mapper.Map<Tenant>(request);
        var createdTenant = await Repository.AddAsync(tenant);

        // Create tenant domains if provided
        if (request.DomainNames.Count != 0)
        {
            foreach (var domain in request.DomainNames.Select(domainName => new TenantDomain
                     {
                         Id = Guid.NewGuid(),
                         TenantId = createdTenant.Id,
                         Name = domainName
                     }))
            {
                await tenantDomainRepository.AddAsync(domain);
            }
        }

        // Dispatch TenantCreated event
        var tenantCreatedEvent = new TenantCreatedEvent(createdTenant.Id, createdTenant.Name, createdTenant.Email);
        await eventDispatcher.DispatchAsync(tenantCreatedEvent);

        var dto = Mapper.Map<TenantDto>(createdTenant);
        return Result<TenantDto>.Success(dto);
    }

    public override async Task<Result<PaginatedResponse<TenantDto>>> GetAllAsync(TenantListRequest request)
    {
        var entities = await Repository.GetAllAsync();

        var query = entities.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(t =>
                t.Name.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                t.Email.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                (t.PhoneNumber != null && t.PhoneNumber.Contains(request.Search, StringComparison.OrdinalIgnoreCase)));
        }

        // Apply Tenant-specific filters
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(t => t.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.Email))
            query = query.Where(t => t.Email.Contains(request.Email, StringComparison.OrdinalIgnoreCase));

        if (request.IsActive.HasValue)
            query = query.Where(t => t.IsActive == request.IsActive.Value);

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "name" => request.SortDescending ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name),
                "email" => request.SortDescending ? query.OrderByDescending(t => t.Email) : query.OrderBy(t => t.Email),
                "isactive" => request.SortDescending
                    ? query.OrderByDescending(t => t.IsActive)
                    : query.OrderBy(t => t.IsActive),
                "createdat" => request.SortDescending
                    ? query.OrderByDescending(t => t.CreatedAt)
                    : query.OrderBy(t => t.CreatedAt),
                _ => query.OrderBy(t => t.Name)
            };
        }
        else
            query = query.OrderBy(t => t.Name);

        var totalCount = query.Count();
        var paginatedEntities = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = Mapper.Map<List<TenantDto>>(paginatedEntities);

        var response = new PaginatedResponse<TenantDto>(
            dtos,
            totalCount,
            request.Page,
            request.PageSize
        );

        return Result<PaginatedResponse<TenantDto>>.Success(response);
    }
}