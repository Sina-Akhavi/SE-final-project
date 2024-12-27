using Microsoft.AspNetCore.Mvc;
using Paradaim.Gateway.GYM.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class GymProgramController : ControllerBase
{
    private readonly IGymProgramGateway _gymProgramGateway;

    public GymProgramController(IGymProgramGateway gymProgramGateway)
    {
        _gymProgramGateway = gymProgramGateway;
    }
    [HttpGet]
    public IActionResult GetAllGymProgram()
    {
        var programs = _gymProgramGateway.GetAll();
        return Ok(programs);
    }

    [HttpGet("{id}")]
    public IActionResult GetGymProgram(int id)
    {
       
        var program = _gymProgramGateway.Get(x=>x.Id == id);
         if (id < 0 || program is null)
            return NotFound("Item not found");
            
        return Ok(program);
    }
}
