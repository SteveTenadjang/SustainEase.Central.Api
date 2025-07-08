using System.Linq.Expressions;
using Central.Domain.Entities;
using Central.Domain.Interfaces;
using Central.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Central.Persistence.Repositories;

public class GenericRepository<T>(CentralDbContext context) : IGenericRepository<T>
    where T : BaseEntity
{
    protected readonly CentralDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public virtual async Task<IQueryable> QueryAsync()
    {
        return await Task.FromResult(DbSet.AsQueryable());
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.SingleOrDefaultAsync(predicate);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<List<T>> AddRangeAsync(List<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
        await Context.SaveChangesAsync();
        return entities;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity == null) return false;

        // Soft delete
        entity.DeletedAt = DateTime.UtcNow;
        await Context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await DbSet.AnyAsync(e => e.Id == id);
    }

    public virtual async Task<int> CountAsync()
    {
        return await DbSet.CountAsync();
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.CountAsync(predicate);
    }
}