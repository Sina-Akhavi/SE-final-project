
using Paradaim.Core.CallApi.Boundary.Boundary.Request;

namespace Paradaim.Core.CallApi.Boundary.Response
{
    public class ApiResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalNumber { get; set; }
        public PaginationRequest? PaginationRequest { get; set; }

        public ApiResponse(IQueryable<T> data, PaginationRequest? paginationRequest = null)
        {
            PaginationRequest = paginationRequest;
            TotalNumber = data.Count();
            if (paginationRequest != null)
                Data = data
                    .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                    .Take(paginationRequest.PageSize).ToList();
            else
                Data = data.ToList();
        }

        public ApiResponse(string error)
        {
            Error = error;
        }
        public string Error { get; set; }
        public IOrderedEnumerable<T>? OrderedData { get; set; } = null;

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
}

