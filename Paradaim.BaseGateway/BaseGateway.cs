using System.Linq.Expressions;
using Paradaim.BaseGateway.Interface;
using Microsoft.EntityFrameworkCore;
using Paradaim.BaseGateway.Infrastructures;
using Paradaim.BaseGateway.Boundary.Requests;
using Paradaim.BaseGateway.Infrastructures.Extensions;
using System.Transactions;


namespace Paradaim.BaseGateway
{
    public class BaseGateway<T> : IBaseGateway<T>
            where T : class
    {
        private DbContext? _context = null;


        public BaseGateway(DbContext context)
        {

            _context = context;
        }

        public void SetDb(DbContext context)
        {
            _context = context;
        }


        public string ExportToSpreadSheet(string fileName)
        {
            var dir = AppConfig.Get("Upload:AbsolutePath");
            var file = $"{Environment.CurrentDirectory}\\{dir}\\{fileName}";
            var context = GetDbContext();

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            var data = context.Set<T>().ToList();
            ExportToExcel.Export(data, file);
            return file;
        }

        private string ExportToSpreadSheet(List<T> data, string fileName)
        {
            var dir = AppConfig.Get("Upload:AbsolutePath");
            var file = $"{Environment.CurrentDirectory}\\{dir}\\{fileName}";
            var context = GetDbContext();

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            ExportToExcel.Export(data, file);
            return file;
        }

        private string ExportToSpreadSheet(List<object> data, string fileName)
        {
            var dir = AppConfig.Get("Upload:AbsolutePath");
            var file = $"{Environment.CurrentDirectory}\\{dir}\\{fileName}";
            var context = GetDbContext();

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            ExportToExcel.Export(data.ToDataTable<object>(), file);
            return file;
        }


        public string ExportToSpreadSheet(Expression<Func<T, bool>> whereExpression, string fileName)
        {
            var dir = AppConfig.Get("Upload:AbsolutePath");
            var file = $"{Environment.CurrentDirectory}\\{dir}\\{fileName}";
            var context = GetDbContext();
            var result = context.Set<T>().Where(whereExpression).ToList();
            return ExportToSpreadSheet(result, fileName);
        }

        public string ExportToSpreadSheet(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T>> selectExpression, string fileName)
        {

            var dir = AppConfig.Get("Upload:AbsolutePath");
            var file = $"{Environment.CurrentDirectory}\\{dir}\\{fileName}";
            var context = GetDbContext();
            var result = context.Set<T>()
                .Where(whereExpression)
                .Select(selectExpression)
                .ToList();
            return ExportToSpreadSheet(result, fileName);
        }

        public string ExportToSpreadSheet(Expression<Func<T, object>> selector, string fileName)
        {
            var context = GetDbContext();
            var result = context.Set<T>()
                .Select(selector.Compile())
                .ToList();
            return ExportToSpreadSheet(result, fileName);
        }

        public string ExportToSpreadSheet(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> selector, string fileName)
        {
            var context = GetDbContext();
            var result = context.Set<T>()
                .Where(whereExpression).AsEnumerable()
                .Select(selector.Compile())
                .ToList();
            return ExportToSpreadSheet(result, fileName);
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            var context = GetDbContext();
            return context.Set<T>().Any(expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Parent id</param>
        /// <returns></returns>
        public async Task<T?> Get(int id)
        {
            var context = GetDbContext();
            return await context.FindAsync<T>(id).ConfigureAwait(false);
        }

        public async Task<T?> Get(Guid id)
        {
            var context = GetDbContext();
            return await context.FindAsync<T>(id).ConfigureAwait(false);
        }

        public async Task<T?> Get(string id)
        {
            var context = GetDbContext();
            return await context.FindAsync<T>(id).ConfigureAwait(false);
        }

        public async Task<T?> Get(long id)
        {
            var context = GetDbContext();
            return await context.FindAsync<T>(id).ConfigureAwait(false);
        }

        public async Task<T?> Get(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var context = GetDbContext();
            var prop = entity.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == "id");
            int id = 0;
            if (prop != null)
                id = int.Parse(prop.GetValue(entity)?.ToString() ?? string.Empty);

            return await context.FindAsync<T>(id).ConfigureAwait(false);
        }

        public virtual T? Get(Expression<Func<T, bool>> predicate)
        {
            var context = GetDbContext();
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var data = context.Set<T>().Where(predicate);
            if (data.Any())
            {
                return data.FirstOrDefault();

            }
            else
            {
                return null;

            }
        }
        public virtual T? Get(
    Expression<Func<T, bool>> predicate,
    Expression<Func<T, object>> include = null,
    Expression<Func<object, object>> thenInclude = null)
        {
            var context = GetDbContext();
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            IQueryable<T> query = context.Set<T>();

            // Apply the Include for the first level (if provided)
            if (include != null)
            {
                var includedQuery = query.Include(include);

                // Apply ThenInclude for the second level (if provided)
                if (thenInclude != null)
                {
                    query = includedQuery.ThenInclude(thenInclude);
                }
                else
                {
                    query = includedQuery;
                }
            }

            // Apply the predicate filter
            var data = query.Where(predicate);

            // Return the first matching result or null
            return data.FirstOrDefault();
        }

        public virtual async Task<ApiResponse<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            var context = GetDbContext();
            var data = context.Set<T>()
                .Where(predicate);
            return await data.ToApiResponseAsync().ConfigureAwait(false);
        }

        public virtual async Task<ApiResponse<T>> GetAllWithNoLock(Expression<Func<T, bool>> predicate)
        {
            ApiResponse<T>? result = default;

            var context = GetDbContext();
            var data = context.Set<T>().AsNoTracking()
                .Where(predicate);
            result = await data.ToApiResponseAsync().ConfigureAwait(false);
            return result;
        }

        public virtual async Task<ApiResponse<T>> GetAllWithNoLockSQL(Expression<Func<T, bool>> predicate)
        {
            ApiResponse<T>? result = default;

            var context = GetDbContext();

            // Use NOLOCK hint in a raw SQL query
            var data = context.Set<T>()
                .FromSqlRaw($"SELECT * FROM {typeof(T).Name} WITH (NOLOCK)")
                .Where(predicate);

            result = await data.ToApiResponseAsync().ConfigureAwait(false);
            return result;
        }
        public TransactionScope CreateTransaction()
        {

            return new TransactionScope(TransactionScopeOption.Required,
                                        new TransactionOptions()
                                        {
                                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                                        },
                                    TransactionScopeAsyncFlowOption.Enabled);
        }
        public virtual async Task<ApiResponse<T>> GetAll(Expression<Func<T, bool>> predicate, PaginationRequest? paginationRequest)
        {
            var context = GetDbContext();
            var data = context.Set<T>()
                .Where(predicate);
            return await data.ToApiResponseAsync(paginationRequest).ConfigureAwait(false);
        }

        public virtual async Task<ApiResponse<T>> GetAll(PaginationRequest? paginationRequest)
        {
            var context = GetDbContext();
            return await context.Set<T>().ToApiResponseAsync(paginationRequest).ConfigureAwait(false);
        }

        public virtual async Task<ApiResponse<T>> GetAll()
        {
            var context = GetDbContext();
            return await context.Set<T>().ToApiResponseAsync().ConfigureAwait(false);

        }
        public async Task<ApiResponse<Object>> GetAll(Expression<Func<T, object>> selector)
        {
            var context = GetDbContext();
            return await context.Set<T>()
            .Select(selector)
            .ToApiResponseAsync().ConfigureAwait(false);
        }

        public async Task<ApiResponse<TResult>> GetAll<TResult>(
                    Expression<Func<T, TResult>> selector,
                    PaginationRequest? paginationRequest = null,
                    params SortRequest[] sortRequests)
        {
            var context = GetDbContext();
            var query = context.Set<T>().Select(selector);
            var result = await query.ToListAsync().ConfigureAwait(false);

            if (paginationRequest == null && (sortRequests == null || sortRequests.Length == 0))
            {
                return new ApiResponse<TResult>(result);
            }

            return new ApiResponse<TResult>(query, paginationRequest, sortRequests);
        }
        public async Task<ApiResponse<TResult>> GetAll<TResult>(
                   Expression<Func<T, bool>> predicate,
                    Expression<Func<T, TResult>> selector,
                   PaginationRequest? paginationRequest = null,
                   params SortRequest[]? sortRequests)
        {
            var context = GetDbContext();
            var query = context.Set<T>().Where(predicate).Select(selector);
            var result = await query.ToListAsync().ConfigureAwait(false);

            if (paginationRequest == null && (sortRequests == null || sortRequests.Length == 0))
            {
                return new ApiResponse<TResult>(result);
            }

            return new ApiResponse<TResult>(query, paginationRequest, sortRequests);
        }
        public async Task<ApiResponse<TResult>> GetAll<TResult>(
                    Expression<Func<T, bool>> predicate,
                     Expression<Func<T, TResult>> selector)
        {
            var context = GetDbContext();
            var query = context.Set<T>().Where(predicate).Select(selector);
            var result = await query.ToListAsync().ConfigureAwait(false);
            return new ApiResponse<TResult>(result);
        }
        public async Task<ApiResponse<Object>> GetAll(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> selector)
        {
            var context = GetDbContext();
            return await context.Set<T>()
            .Where(predicate)
            .Select(selector)
            .ToApiResponseAsync().ConfigureAwait(false);
        }
        public async Task<ApiResponse<Object>> GetAll(Expression<Func<T, object>> selector, PaginationRequest paginationRequest)
        {
            var context = GetDbContext();
            return await context.Set<T>()
            .Select(selector)
            .ToApiResponseAsync(paginationRequest).ConfigureAwait(false);
        }
        public async Task<ApiResponse<Object>> GetAll(Expression<Func<T, object>> selector, PaginationRequest paginationRequest, params SortRequest[] sortRequests)
        {
            var context = GetDbContext();
            return await context.Set<T>()
            .Select(selector)
            .ToApiResponseAsync<Object>(paginationRequest, sortRequests).ConfigureAwait(false);
        }
        public virtual int CountAll(Expression<Func<T, bool>> predicate)
        {
            var context = GetDbContext();
            return context.Set<T>().Count(predicate);

        }

        public virtual double? Sum(Func<T, double?> selector)
        {
            var context = GetDbContext();
            return context.Set<T>().Sum(selector);
        }
        public virtual double? Sum(Expression<Func<T, bool>> predicate, Func<T, double?> selector)
        {
            var context = GetDbContext();
            return context.Set<T>().Where(predicate).Sum(selector);
        }
        public virtual int? Sum(Func<T, int?> selector)
        {
            var context = GetDbContext();
            return context.Set<T>().Sum(selector);
        }
        public virtual int? Sum(Expression<Func<T, bool>> predicate, Func<T, int?> selector)
        {
            var context = GetDbContext();
            return context.Set<T>().Where(predicate).Sum(selector);
        }

        public async Task<T> Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var context = GetDbContext();

            await context.AddAsync(entity).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task<List<T>> AddRange(List<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (entities.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(entities));

            var context = GetDbContext();
            await context.AddRangeAsync(entities).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entities;
        }

        public async Task<T> Remove(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var result = await Get(entity).ConfigureAwait(false);
            if (result != null)
            {
                var context = GetDbContext();
                context.Remove(result);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
            return entity;
        }

        public async Task<T> Remove(int id)
        {
            var result = await Get(id).ConfigureAwait(false);
            if (result != null)
                await Remove(result).ConfigureAwait(false);
            return result;
        }

        public async Task<T> Remove(Guid id)
        {
            var result = await Get(id).ConfigureAwait(false);
            if (result != null)
                await Remove(result).ConfigureAwait(false);
            return result;
        }

        public async Task<T> Remove(string id)
        {
            var result = await Get(id).ConfigureAwait(false);
            if (result != null)
                await Remove(result).ConfigureAwait(false);
            return result;
        }

        public async Task<List<T>?> RemoveRange(List<T> entities)
        {
            if (entities.Count > 0)
            {
                var context = GetDbContext();
                context.RemoveRange(entities);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
            return entities;
        }

        public async Task<T> Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var context = GetDbContext();
            /*context.Entry(entity).State = EntityState.Detached;*/
            context.Update(entity);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task<List<T>> UpdateRange(List<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (entities.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(entities));

            var context = GetDbContext();
            context.UpdateRange(entities);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return entities;
        }

        public async Task<T> Save(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var result = await Get(entity).ConfigureAwait(false);
            if (result == null)
            {
                await Add(entity).ConfigureAwait(false);
            }
            else
            {
                await Update(entity).ConfigureAwait(false);
            }
            return entity;
        }

        public async Task<List<T>> SaveRange(List<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            if (entities.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(entities));

            foreach (var entity in entities)
            {
                await Save(entity).ConfigureAwait(false);
            }

            return entities;
        }

        public DbContext GetDbContext()
        {
            return _context;
        }

        public async Task<T?> Remove(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var result = Get(predicate);
            if (result != null)
            {
                var context = GetDbContext();
                context.Remove(result);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
            return result;
        }


    }
}
