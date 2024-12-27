using Microsoft.AspNetCore.Mvc;
using Paradaim.Gateway.GYM.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class TrainerController : ControllerBase
{
    private readonly ITrainerGateway _trainerGateway;

    public TrainerController(ITrainerGateway trainerGateway)
    {
        _trainerGateway = trainerGateway;
    }
    [HttpGet]
    public IActionResult GetAllTrainers()
    {
        var trainers = _trainerGateway.GetAll();
        return Ok(trainers);
    }

    [HttpGet("{id}")]
    public IActionResult GetTrainer(int id)
    {
        var trainer = _trainerGateway.Get(x=>x.Id == id);
         if (id < 0 || trainer is null)
            return NotFound("Item not found");
        return Ok(trainer);
    }
}
