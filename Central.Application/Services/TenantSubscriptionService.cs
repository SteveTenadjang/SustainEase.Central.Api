using AutoMapper;
using FluentValidation;
using Central.Domain.Entities;
using Central.Application.DTOs;
using Central.Domain.Interfaces;
using Central.Application.Common;
using Central.Application.Services.Interfaces;

namespace Central.Application.Services;

public class TenantSubscriptionService(
    ITenantSubscription tenantSubscriptionRepository,
    IMapper mapper,
    IValidator<CreateTenantSubscriptionRequest>? createValidator = null,
    IValidator<UpdateTenantSubscriptionRequest>? updateValidator = null)
    : GenericService<TenantSubscription, TenantSubscriptionDto, CreateTenantSubscriptionRequest,
        UpdateTenantSubscriptionRequest, PaginatedRequest>(tenantSubscriptionRepository, mapper, createValidator,
        updateValidator), ITenantSubscriptionService
{
    protected override Guid GetEntityIdFromRequest(UpdateTenantSubscriptionRequest request)
        => request.Id;

    public async Task<Result<List<TenantSubscriptionDto>>> GetByTenantIdAsync(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            return Result<List<TenantSubscriptionDto>>.Failure("Tenant ID cannot be empty.");

        var subscriptions = await Repository.FindAsync(s => s.TenantId == tenantId);
        var subscriptionDtos = Mapper.Map<List<TenantSubscriptionDto>>(subscriptions);

        // Calculate derived properties
        var enhancedDtos = subscriptionDtos.Select(dto =>
        {
            var subscription = subscriptions.First(s => s.Id == dto.Id);
            return dto with
            {
                EndDate = subscription.StartDate.AddDays(subscription.Duration),
                IsActive = IsSubscriptionCurrentlyActive(subscription.StartDate, subscription.Duration)
            };
        }).ToList();

        return Result<List<TenantSubscriptionDto>>.Success(enhancedDtos);
    }

    public async Task<Result<List<TenantSubscriptionDto>>> GetActiveSubscriptionsAsync()
    {
        var allSubscriptions = await tenantSubscriptionRepository.GetActiveSubscriptionsAsync();
        var activeDtos = (from subscription in allSubscriptions
                where IsSubscriptionCurrentlyActive(subscription.StartDate, subscription.Duration)
                let dto = Mapper.Map<TenantSubscriptionDto>(subscription)
                select dto with { EndDate = subscription.StartDate.AddDays(subscription.Duration), IsActive = true })
            .ToList();

        return Result<List<TenantSubscriptionDto>>.Success(activeDtos);
    }

    public async Task<Result<List<TenantSubscriptionDto>>> GetByBundleIdAsync(Guid bundleId)
    {
        if (bundleId == Guid.Empty)
            return Result<List<TenantSubscriptionDto>>.Failure("Bundle ID cannot be empty.");

        var subscriptions = await Repository.FindAsync(s => s.BundleId == bundleId);
        var subscriptionDtos = Mapper.Map<List<TenantSubscriptionDto>>(subscriptions);

        // Calculate derived properties
        var enhancedDtos = subscriptionDtos.Select(dto =>
        {
            var subscription = subscriptions.First(s => s.Id == dto.Id);
            return dto with
            {
                EndDate = subscription.StartDate.AddDays(subscription.Duration),
                IsActive = IsSubscriptionCurrentlyActive(subscription.StartDate, subscription.Duration)
            };
        }).ToList();

        return Result<List<TenantSubscriptionDto>>.Success(enhancedDtos);
    }

    public async Task<Result<bool>> HasActiveSubscriptionAsync(Guid tenantId, Guid bundleId)
    {
        if (tenantId == Guid.Empty)
            return Result<bool>.Failure("Tenant ID cannot be empty.");

        if (bundleId == Guid.Empty)
            return Result<bool>.Failure("Bundle ID cannot be empty.");

        var subscriptions = await Repository.FindAsync(s =>
            s.TenantId == tenantId &&
            s.BundleId == bundleId);

        var hasActiveSubscription = subscriptions.Any(s =>
            IsSubscriptionCurrentlyActive(s.StartDate, s.Duration));

        return Result<bool>.Success(hasActiveSubscription);
    }

    public async Task<Result<bool>> IsSubscriptionActiveAsync(Guid subscriptionId)
    {
        if (subscriptionId == Guid.Empty)
            return Result<bool>.Failure("Subscription ID cannot be empty.");

        var subscription = await Repository.GetByIdAsync(subscriptionId);
        if (subscription == null)
            return Result<bool>.Failure("Subscription not found.");

        var isActive = IsSubscriptionCurrentlyActive(subscription.StartDate, subscription.Duration);
        return Result<bool>.Success(isActive);
    }

    public override async Task<Result<TenantSubscriptionDto>> CreateAsync(CreateTenantSubscriptionRequest request)
    {
        // Use base validation and creation
        var result = await base.CreateAsync(request);

        if (!result.IsSuccess || result.Data == null) return result;

        // Calculate derived properties for the response
        var dto = result.Data;
        var endDate = request.StartDate.AddDays(request.Duration);
        var isActive = IsSubscriptionCurrentlyActive(request.StartDate, request.Duration);

        var enhancedDto = dto with
        {
            EndDate = endDate,
            IsActive = isActive
        };

        return Result<TenantSubscriptionDto>.Success(enhancedDto);
    }

    public override async Task<Result<PaginatedResponse<TenantSubscriptionDto>>> GetAllAsync(PaginatedRequest request)
    {
        var entities = await Repository.GetAllAsync();
        var query = entities.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            // Search by tenant or bundle relationship would require joins
            // For now, search by duration or date ranges
            if (int.TryParse(request.Search, out var duration))
                query = query.Where(s => s.Duration == duration);
        }

        // Apply sorting
        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "startdate" => request.SortDescending
                    ? query.OrderByDescending(s => s.StartDate)
                    : query.OrderBy(s => s.StartDate),
                "duration" => request.SortDescending
                    ? query.OrderByDescending(s => s.Duration)
                    : query.OrderBy(s => s.Duration),
                "tenantid" => request.SortDescending
                    ? query.OrderByDescending(s => s.TenantId)
                    : query.OrderBy(s => s.TenantId),
                "bundleid" => request.SortDescending
                    ? query.OrderByDescending(s => s.BundleId)
                    : query.OrderBy(s => s.BundleId),
                "createdat" => request.SortDescending
                    ? query.OrderByDescending(s => s.CreatedAt)
                    : query.OrderBy(s => s.CreatedAt),
                _ => query.OrderByDescending(s => s.StartDate)
            };
        }
        else
            query = query.OrderByDescending(s => s.StartDate);

        var totalCount = query.Count();
        var paginatedEntities = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var dtos = Mapper.Map<List<TenantSubscriptionDto>>(paginatedEntities);

        // Calculate derived properties for each DTO
        var enhancedDtos = dtos.Select(dto =>
        {
            var subscription = paginatedEntities.First(s => s.Id == dto.Id);
            return dto with
            {
                EndDate = subscription.StartDate.AddDays(subscription.Duration),
                IsActive = IsSubscriptionCurrentlyActive(subscription.StartDate, subscription.Duration)
            };
        }).ToList();

        var response = new PaginatedResponse<TenantSubscriptionDto>(
            enhancedDtos,
            totalCount,
            request.Page,
            request.PageSize
        );

        return Result<PaginatedResponse<TenantSubscriptionDto>>.Success(response);
    }

    // Helper method to determine if a subscription is currently active
    private static bool IsSubscriptionCurrentlyActive(DateTime startDate, int duration)
    {
        var endDate = startDate.AddDays(duration);
        var currentDate = DateTime.UtcNow;
        return currentDate >= startDate && currentDate <= endDate;
    }
}