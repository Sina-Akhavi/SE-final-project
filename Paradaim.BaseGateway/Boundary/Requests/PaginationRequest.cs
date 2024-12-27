namespace Paradaim.BaseGateway.Boundary.Requests
{
    public class PaginationRequest
    {
        public PaginationRequest(PagingRequest? pagingRequest = null)
        {
            if (pagingRequest != null)
            {
                this.PageSize = pagingRequest.pageSize;
                this.PageNumber = pagingRequest.page + 1;
            }
        }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
