using Central.Application.Common;
using Central.Application.DTOs;
using Central.Domain.Entities;

namespace Central.Application.Services.Interfaces;

public interface IBundleService : IGenericService<Bundle, BundleDto, CreateBundleRequest, UpdateBundleRequest, BundleListRequest>
{
    Task<Result<BundleDto>> GetByKeyAsync(string key);
    Task<Result<bool>> KeyExistsAsync(string key);
}