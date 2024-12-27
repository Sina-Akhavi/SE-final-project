namespace Paradaim.BaseGateway.Boundary.Requests;
public class PagingRequest
{
    
    public int? Draw { get; set; } // we can remove it
    public int page { get; set; }
    public int pageSize { get; set; }
}

