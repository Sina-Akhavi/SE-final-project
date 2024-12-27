using Paradaim.BaseGateway;
using Paradaim.Gateway.GYM.Interfaces;
using Paradaim.Gateway.Models;

namespace Paradaim.Gateway.GYM
{
    public class GymProgramGateway : BaseGateway<GymProgram>, IGymProgramGateway
    {
        public GymProgramGateway(ParadaimDbContext paradaimDbContext) : base(paradaimDbContext)
        {
        }
    }
}
