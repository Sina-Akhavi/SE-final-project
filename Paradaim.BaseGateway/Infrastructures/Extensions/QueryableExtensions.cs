

using Paradaim.BaseGateway.Boundary.Requests;
using Paradaim.BaseGateway.Boundary.Response;

namespace Paradaim.BaseGateway.Infrastructures.Extensions
{
    public static class QueryableExtensions
    {
        public static ApiResponse<T> ToApiResponse<T>(this IQueryable<T> data, PaginationRequest? paginationRequest = null)
        {
            return new ApiResponse<T>(data, paginationRequest);
        }
        public static async Task<ApiResponse<T>> ToApiResponseAsync<T>(this IQueryable<T> data, PaginationRequest? paginationRequest = null)
        {
            var result = new ApiResponse<T>(data, paginationRequest);
            return await Task.FromResult(result).ConfigureAwait(false);
        }
        public static ApiResponse<T> ToApiResponse<T>(this IQueryable<T> data, PaginationRequest paginationRequest, params SortRequest[] sortRequests)
        {
            return new ApiResponse<T>(data, paginationRequest, sortRequests);
        }
        public static async Task<ApiResponse<T>> ToApiResponseAsync<T>(this IQueryable<T> data, PaginationRequest paginationRequest, params SortRequest[] sortRequests)
        {
            var result = new ApiResponse<T>(data, paginationRequest, sortRequests);
            return await Task.FromResult(result).ConfigureAwait(false);
        }
        public static ApiResponse<T> ToApiResponse<T>(this IQueryable<T> data, params SortRequest[] sortRequests)
        {
            return new ApiResponse<T>(data, sortRequests);
        }
        public static async Task<ApiResponse<T>> ToApiResponseAsync<T>(this IQueryable<T> data, params SortRequest[] sortRequests)
        {
            var result = new ApiResponse<T>(data, sortRequests);
            return await Task.FromResult(result).ConfigureAwait(false);
        }
    }
}
