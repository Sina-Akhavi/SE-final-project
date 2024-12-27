using Paradaim.BaseGateway;
using Paradaim.Gateway.GYM.Interfaces;
using Paradaim.Gateway.Models;

namespace Paradaim.Gateway.GYM
{
    public class ValueGateway : BaseGateway<Value>, IValueGateway
    {
        public ValueGateway(ParadaimDbContext paradaimDbContext) : base(paradaimDbContext)
        {
        }
    }
}
