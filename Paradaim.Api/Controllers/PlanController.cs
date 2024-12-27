using Microsoft.AspNetCore.Mvc;
using Paradaim.Gateway.GYM.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class PlanController : ControllerBase
{
    private readonly IPlanGateway _planGateway;

    public PlanController(IPlanGateway planGateway)
    {
        _planGateway = planGateway;
    }
    [HttpGet]
    public IActionResult GetAllPlans()
    {
        var plans = _planGateway.GetAll();
        return Ok(plans);
    }

    [HttpGet("{id}")]
    public IActionResult GetPlans(int id)
    {
       
        var plan = _planGateway.Get(x=>x.Id == id);
         if (id < 0 || plan is null)
            return NotFound("Item not found");
            
        return Ok(plan);
    }
}
