using Paradaim.BaseGateway;
using Paradaim.Gateway.GYM.Interfaces;
using Paradaim.Gateway.Models;

namespace Paradaim.Gateway.GYM
{
    public class PlanGateway : BaseGateway<Plan>, IPlanGateway
    {
        public PlanGateway(ParadaimDbContext paradaimDbContext) : base(paradaimDbContext)
        {
        }
    }
}
