using Central.Application.Common;
using Central.Application.DTOs;

namespace Central.Application.Services.Interfaces;

public interface IGenericService<TEntity, TDto, TCreateRequest, TUpdateRequest, TListRequest>
    where TEntity : class
    where TDto : class
    where TCreateRequest : class
    where TUpdateRequest : class
    where TListRequest : PaginatedRequest
{
    Task<Result<TDto>> GetByIdAsync(Guid id);
    Task<Result<List<TDto>>> GetAllAsync();
    Task<Result<PaginatedResponse<TDto>>> GetAllAsync(TListRequest request);
    Task<Result<TDto>> CreateAsync(TCreateRequest request);
    Task<Result<TDto>> UpdateAsync(TUpdateRequest request);
    Task<Result> DeleteAsync(Guid id);
    Task<Result<bool>> ExistsAsync(Guid id);
    Task<Result<int>> CountAsync();
}