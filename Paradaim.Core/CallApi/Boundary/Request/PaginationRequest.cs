namespace Paradaim.Core.CallApi.Boundary.Boundary.Request
{
    public class PaginationRequest
    {
        public int PageSize { get; set; } = 50;
        public int PageNumber { get; set; } = 1;
    }
}
