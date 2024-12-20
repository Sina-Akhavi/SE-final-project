
using Paradaim.BaseGateway.Boundary.Response;
namespace Paradaim.BaseGateway.Infrastructures.Extensions
{
    public static class ApiResponseExtensions
    {

        public static DataTableResponse<T> ToDataTableResponse<T>(this ApiResponse<T> response, int? draw)
        {
            return new DataTableResponse<T>()
            {
                Data = response.OrderedData?.ToArray() ?? response.Data.ToArray(),
                Draw = draw,
                RecordsFiltered = response.TotalNumber,
                RecordsTotal = response.TotalNumber,
                Error = String.Empty
            };
        }
        public static Task<DataTableResponse<T>> ToDataTableResponse<T>(this Task<ApiResponse<T>> response, int? draw)
        {
            return Task.Run(() => new DataTableResponse<T>()
            {
                Data = response.Result.OrderedData?.ToArray() ?? response.Result.Data.ToArray(),
                Draw = draw,
                RecordsFiltered = response.Result.TotalNumber,
                RecordsTotal = response.Result.TotalNumber,
                Error = String.Empty
            });
        }
        
    }
}
