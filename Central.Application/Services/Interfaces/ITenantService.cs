using Central.Application.Common;
using Central.Application.DTOs;
using Central.Domain.Entities;

namespace Central.Application.Services.Interfaces;

public interface ITenantService : IGenericService<Tenant, TenantDto, CreateTenantRequest, UpdateTenantRequest, TenantListRequest>
{
    Task<Result<TenantDto>> GetByEmailAsync(string email);
    Task<Result<TenantDto>> GetByPhoneNumberAsync(string phoneNumber);
    Task<Result> ActivateAsync(Guid id);
    Task<Result> DeactivateAsync(Guid id);
}