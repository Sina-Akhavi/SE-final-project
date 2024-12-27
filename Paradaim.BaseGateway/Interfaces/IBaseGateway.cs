using System.Linq.Expressions;
using Paradaim.BaseGateway.Boundary.Requests;
using Paradaim.BaseGateway.Boundary.Response;
using Microsoft.EntityFrameworkCore;


namespace Paradaim.BaseGateway.Interface
{
    public interface IBaseGateway<T> 
        where T : class
    {
        public Task<T> Add(T entity);
        public Task<List<T>> AddRange(List<T> entities);
        public Task<T> Remove(T entity);
        public Task<T> Remove(int id);
        public Task<T> Remove(Guid id);
        public Task<T> Remove(string id);
        public Task<T?> Remove(Expression<Func<T, bool>> predicate);
        public Task<List<T>?> RemoveRange(List<T> entities);
        public Task<T> Update(T entity);
        public Task<List<T>> UpdateRange(List<T> entities);
        public Task<T> Save(T entity);
        public Task<List<T>> SaveRange(List<T> entities);
        public Task<T?> Get(int id);
        public Task<T?> Get(Guid id);
        public Task<T?> Get(string id);
        public Task<T?> Get(long id);
        public Task<T?> Get(T entity);
        public abstract T? Get(Expression<Func<T, bool>> predicate);
        public abstract T? Get(Expression<Func<T, bool>> predicate,Expression<Func<T, object>> include = null,Expression<Func<object, object>> thenInclude = null);
        public abstract Task<ApiResponse<Object>> GetAll(Expression<Func<T, object>> selector, PaginationRequest paginationRequest);
        public abstract Task<ApiResponse<Object>> GetAll(Expression<Func<T, object>> selector);
        public abstract Task<ApiResponse<TResult>> GetAll<TResult>(Expression<Func<T, TResult>> selector,
                    PaginationRequest? paginationRequest = null,
                    params SortRequest[] sortRequests);
        public abstract Task<ApiResponse<TResult>> GetAll<TResult>(
            Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector,
                   PaginationRequest? paginationRequest = null,
                   params SortRequest[] sortRequests);
        public abstract Task<ApiResponse<TResult>> GetAll<TResult>(
             Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector);
        public abstract Task<ApiResponse<Object>> GetAll(Expression<Func<T, bool>> predicate,Expression<Func<T, object>> selector);
        public abstract Task<ApiResponse<Object>> GetAll(Expression<Func<T, object>> selector, PaginationRequest paginationRequest, params SortRequest[] sortRequests);
        public abstract Task<ApiResponse<T>> GetAll(Expression<Func<T, bool>> predicate, PaginationRequest paginationRequest);
        public abstract Task<ApiResponse<T>> GetAll(Expression<Func<T, bool>> predicate);
        public abstract Task<ApiResponse<T>> GetAllWithNoLock(Expression<Func<T, bool>> predicate);
        public abstract Task<ApiResponse<T>> GetAllWithNoLockSQL(Expression<Func<T, bool>> predicate);

        public abstract Task<ApiResponse<T>> GetAll(PaginationRequest paginationRequest);
        public abstract Task<ApiResponse<T>> GetAll();
        public abstract int CountAll(Expression<Func<T, bool>> predicate);
        public abstract double? Sum(Func<T, double?> selector);
        public abstract double? Sum(Expression<Func<T, bool>> predicate, Func<T, double?> selector); 
        public abstract int? Sum(Func<T, int?> selector);
        public abstract int? Sum(Expression<Func<T, bool>> predicate, Func<T, int?> selector);
        public void SetDb(DbContext context);
        public abstract string ExportToSpreadSheet(string fileName);
        public abstract string ExportToSpreadSheet(Expression<Func<T, bool>> whereExpression, string fileName);
        public abstract string ExportToSpreadSheet(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T>> selectExpression, string fileName);
        public abstract string ExportToSpreadSheet(Expression<Func<T, object>> selector, string fileName);
        public abstract string ExportToSpreadSheet(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> selector, string fileName);
        public bool Any(Expression<Func<T, bool>> expression);
    }
}