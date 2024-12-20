

using Paradaim.BaseGateway.Boundary.Requests;
using System.Linq.Expressions;

namespace Paradaim.BaseGateway.Boundary.Response
{
    public class ApiResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalNumber { get; set; }
        public PaginationRequest PaginationRequest { get; set; }
        public IOrderedEnumerable<T>? OrderedData { get; set; } = null;

        public ApiResponse(IQueryable<T> data, params SortRequest[] sortRequests)
        {
            TotalNumber = data.Count();
            Data = Sort(data.AsEnumerable(), sortRequests).ToList();
        }
        public ApiResponse(IQueryable<T> data, PaginationRequest paginationRequest, params SortRequest[] sortRequests)
        {
            this.PaginationRequest = paginationRequest;
            TotalNumber = data.Count();
            if (paginationRequest != null && (paginationRequest?.PageSize ?? 0) != 0)
            {
                var tmpData = Sort(data.AsEnumerable(), sortRequests);
                Data = tmpData.Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                    .Take(paginationRequest.PageSize).ToList();
            }
            else
                Data = Sort(data, sortRequests).ToList();
        }

        private IEnumerable<T> Sort(IEnumerable<T> data, params SortRequest[] sortList)
        {
            if (sortList != null && sortList.Length > 0)
            {
                IOrderedEnumerable<T> orderedData = null;

                foreach (var sortRequest in sortList)
                {
                    if (sortRequest?.ColName != null)
                    {
                        if (orderedData == null)
                        {
                            orderedData = sortRequest.SortType == ESortType.Desc
                                ? data.OrderByDescending(p => p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null))
                                : data.OrderBy(p => p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null));
                        }
                        else
                        {
                            orderedData = sortRequest.SortType == ESortType.Desc
                                ? orderedData.ThenByDescending(p => p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null))
                                : orderedData.ThenBy(p => p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null));
                        }
                        /*if (orderedData == null)
                        {
                            orderedData = sortRequest.SortType == ESortType.Desc
                                ? data.OrderByDescending(p => StringComparer.OrdinalIgnoreCase.Compare(p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null)?.ToString(), null))
                                : data.OrderBy(p => StringComparer.OrdinalIgnoreCase.Compare(p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null)?.ToString(), null));
                        }
                        else
                        {
                            orderedData = sortRequest.SortType == ESortType.Desc
                                ? orderedData.ThenByDescending(p => StringComparer.OrdinalIgnoreCase.Compare(p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null)?.ToString(), null))
                                : orderedData.ThenBy(p => StringComparer.OrdinalIgnoreCase.Compare(p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null)?.ToString(), null));
                        }*/
                    }
                }

                if (orderedData != null)
                {
                    return orderedData.AsEnumerable();
                }
            }

            return data.AsEnumerable();
        }
        public ApiResponse<T> OrderBy<TKey>(params Func<T, TKey>[] expressions)
        {
            if (expressions.Length > 0)
            {
                var query = Data.AsEnumerable().OrderBy(expressions[0]);
                for (int i = 1; i < expressions.Length; i++)
                {
                    query = query.ThenBy(expressions[i]);
                }

                OrderedData = query;
                Data = OrderedData.ToList();
            }
            return this;
        }

        public ApiResponse<T> ThenBy<TKey>(Func<T, TKey> expression)
        {
            OrderedData = OrderedData.ThenBy(expression);
            Data = OrderedData.ToList();
            return this;
        }
    }
    public class ApiResponse<T, TSearchModel>
    {
        public TSearchModel SearchModel { get; set; }
        public List<T> Data { get; set; }
        public int TotalNumber { get; set; }
        public PaginationRequest PaginationRequest { get; set; }
    }
}


public class ApiResponse<TResult>
{
    public List<TResult> Data { get; set; }
    public int TotalNumber { get; set; }
    public PaginationRequest PaginationRequest { get; set; }
    public IOrderedEnumerable<TResult>? OrderedData { get; set; } = null;

    public ApiResponse(IQueryable<TResult> data, params SortRequest[] sortRequests)
    {
        TotalNumber = data.Count();
        Data = Sort(data.AsEnumerable(), sortRequests).ToList();
    }    
    public ApiResponse(IQueryable<TResult> data, PaginationRequest paginationRequest, params SortRequest[] sortRequests)
    {
        this.PaginationRequest = paginationRequest;
        TotalNumber = data.Count();

        var sortedData = Order(data, sortRequests);

        if (paginationRequest != null && paginationRequest.PageSize != 0)
        {
            Data = sortedData
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToList();
        }
        else
        {
            Data = sortedData.ToList();
        }
    }    

    public ApiResponse(IEnumerable<TResult> data)
    {
        Data = data.ToList();
        TotalNumber = data.Count();
    }

    private IEnumerable<TResult> Sort(IEnumerable<TResult> data, params SortRequest[] sortList)
    {
        if (sortList != null && sortList.Length > 0)
        {
            IOrderedEnumerable<TResult> orderedData = null;

            foreach (var sortRequest in sortList)
            {
                if (sortRequest?.ColName != null)
                {
                    if (orderedData == null)
                    {
                        orderedData = sortRequest.SortType == ESortType.Desc
                            ? data.OrderByDescending(p => p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null))
                            : data.OrderBy(p => p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null));
                    }
                    else
                    {
                        orderedData = sortRequest.SortType == ESortType.Desc
                            ? orderedData.ThenByDescending(p => p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null))
                            : orderedData.ThenBy(p => p.GetType().GetProperty(sortRequest.ColName)?.GetValue(p, null));
                    }
                }
            }

            if (orderedData != null)
            {
                return orderedData.AsEnumerable();
            }
        }

        return data.AsEnumerable();
    }
    public ApiResponse<TResult> OrderBy<TKey>(params Func<TResult, TKey>[] expressions)
    {
        if (expressions.Length > 0)
        {
            var query = Data.AsEnumerable().OrderBy(expressions[0]);
            for (int i = 1; i < expressions.Length; i++)
            {
                query = query.ThenBy(expressions[i]);
            }

            OrderedData = query;
            Data = OrderedData.ToList();
        }
        return this;
    }

    public ApiResponse<TResult> ThenBy<TKey>(Func<TResult, TKey> expression)
    {
        OrderedData = OrderedData.ThenBy(expression);
        Data = OrderedData.ToList();
        return this;
    }
    private IQueryable<TResult> Order(IQueryable<TResult> data, params SortRequest[] sortRequests)
    {
        if (sortRequests == null || !sortRequests.Any())
            return data; // No sorting requested

        // Dynamically apply sorting
        IOrderedQueryable<TResult> orderedData = null;
        foreach (var sortRequest in sortRequests)
        {
            // Use reflection to get the property by name
            var parameter = Expression.Parameter(typeof(TResult), "x");
            var property = Expression.Property(parameter, sortRequest.ColName);
            var keySelector = Expression.Lambda(property, parameter);

            // Determine whether to use OrderBy or OrderByDescending
            if (orderedData == null)
            {
                orderedData = sortRequest.SortType == ESortType.Asc
                    ? Queryable.OrderBy(data, (dynamic)keySelector)
                    : Queryable.OrderByDescending(data, (dynamic)keySelector);
            }
            else
            {
                orderedData = sortRequest.SortType == ESortType.Asc
                    ? Queryable.ThenBy(orderedData, (dynamic)keySelector)
                    : Queryable.ThenByDescending(orderedData, (dynamic)keySelector);
            }
        }

        return orderedData ?? data;
    }
}