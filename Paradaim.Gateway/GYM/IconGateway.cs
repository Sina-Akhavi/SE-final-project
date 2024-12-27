using Paradaim.BaseGateway;
using Paradaim.Gateway.GYM.Interfaces;
using Paradaim.Gateway.Models;

namespace Paradaim.Gateway.GYM
{
    public class IconGateway : BaseGateway<Icon>, IIconGateway
    {
        public IconGateway(ParadaimDbContext paradaimDbContext) : base(paradaimDbContext)
        {
        }
    }
}
