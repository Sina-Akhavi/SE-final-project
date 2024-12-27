using Paradaim.BaseGateway;
using Paradaim.Gateway.GYM.Interfaces;
using Paradaim.Gateway.Models;

namespace Paradaim.Gateway.GYM
{
    public class LinkGateway : BaseGateway<Link>, ILinkGateway
    {
        public LinkGateway(ParadaimDbContext paradaimDbContext) : base(paradaimDbContext)
        {
        }
    }
}
