using Microsoft.EntityFrameworkCore;
using MyGrades.Application.Contracts;
using MyGrades.Application.Contracts.Repositories;
using System.Linq.Expressions;

namespace MyGrades.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return Result.Success();
        }

        public async Task<Result<ICollection<T>>> AddRangeAsync(List<T> values)
        {
            await _context.Set<T>().AddRangeAsync(values);
            return Result<ICollection<T>>.Success(values);
        }

        public async Task<Result<bool>> AnyAsync(Expression<Func<T, bool>> expression)
        {
            var result = await _context.Set<T>().AnyAsync(expression);
            if (!result)
            {
                return  Result<bool>.Failure(" Not found",404);
            }
            return Result<bool>.Success(result);
        }

        public async Task<Result<bool>> ClearAsync(Expression<Func<T, bool>> expression)
        {
            var entities = await _context.Set<T>().Where(expression).ToListAsync();
            if (entities.Count == 0)
            {
                return Result<bool>.Failure("No entities found to delete");
            }
            _context.Set<T>().RemoveRange(entities);
            return Result<bool>.Success(true);
        }

        public async Task<Result> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Result.Success();
        }

        public async Task<Result<ICollection<T>>> FindAllAsync(Expression<Func<T, bool>> expression)
        {
            var entities = await _context.Set<T>().Where(expression).ToListAsync();
            if (entities.Count == 0) {
                return Result<ICollection<T>>.Failure("No entities found");
            }
            return Result<ICollection<T>>.Success(entities);
        }

        public async Task<Result<ICollection<T>>> FindAllWithIncludeAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(expression);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var entities = await query.ToListAsync();
            if (entities.Count == 0)
            {
                return Result<ICollection<T>>.Failure("No entities found");
            }
            return Result<ICollection<T>>.Success(entities);
        }

        public async Task<Result<T>> FindAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(expression);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var entity = await query.FirstOrDefaultAsync();
            if (entity == null)
            {
                return Result<T>.Failure("Entity not found");
            }
            return Result<T>.Success(entity);
        }

        public async Task<Result<ICollection<T>>> GetAllAsync()
        {
            var entities = await _context.Set<T>().ToListAsync();
            if (entities.Count == 0)
            {
                return Result<ICollection<T>>.Failure("No entities found");
            }
            return Result<ICollection<T>>.Success(entities);
        }

        public async Task<Result<T>> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return Result<T>.Failure("Entity not found");
            }
            return Result<T>.Success(entity);
        }

        public async Task<Result<T>> GetWithIncludeAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(expression);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var entity = await query.FirstOrDefaultAsync();
            if (entity == null)
            {
                return Result<T>.Failure("Entity not found");
            }
            return Result<T>.Success(entity);
        }

        public async Task<Result> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return Result.Success();
        }
    }
}
