using Central.Application.Common;
using Central.Application.DTOs;
using Central.Domain.Entities;

namespace Central.Application.Services.Interfaces;

public interface ITenantDomainService : IGenericService<TenantDomain, TenantDomainDto, CreateTenantDomainRequest, UpdateTenantDomainRequest, PaginatedRequest>
{
    Task<Result<TenantDomainDto>> GetByNameAsync(string name);
    Task<Result<List<TenantDomainDto>>> GetByTenantIdAsync(Guid tenantId);
    Task<Result<bool>> DomainExistsAsync(string name);
}