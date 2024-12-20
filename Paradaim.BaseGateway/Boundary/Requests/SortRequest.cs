using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Paradaim.BaseGateway.Boundary.Requests
{
    public class SortRequest
    {
        public string? ColName { get; set; }
        public ESortType SortType { get; set; }

        public SortRequest()
        {
            
        }
        public SortRequest(string colName, ESortType sortType)
        {
            ColName = colName;
            SortType = sortType;    
        }
    }
}
