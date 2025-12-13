using System.Linq.Expressions;

namespace MyGrades.Application.Contracts.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<Result<ICollection<T>>> GetAllAsync();
        public Task<Result> AddAsync(T entity);
        public Task<Result<T>> GetByIdAsync(int id);
        public Task<Result<ICollection<T>>> AddRangeAsync(List<T> values);
        public Task<Result<ICollection<T>>> FindAllAsync(Expression<Func<T, bool>> expression);
        public Task<Result<T>> FindAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        public Task<Result> UpdateAsync(T entity);
        public Task<Result> DeleteAsync(T entity);
        public Task<Result<bool>> AnyAsync(Expression<Func<T, bool>> expression);
        public Task<Result<bool>> ClearAsync(Expression<Func<T, bool>> expression);
        public Task<Result<T>> GetWithIncludeAsync(Expression<Func<T, bool>> expression,
                                    params Expression<Func<T, object>>[] includes);
        public Task<Result<ICollection<T>>> FindAllWithIncludeAsync(Expression<Func<T, bool>> expression,
                            params Expression<Func<T, object>>[] includes);

    }
}
