using Microsoft.AspNetCore.Mvc;
using Paradaim.Gateway.GYM.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ValueController : ControllerBase
{
    private readonly IValueGateway _valueGateway;

    public ValueController(IValueGateway valueGateway)
    {
        _valueGateway = valueGateway;
    }
    [HttpGet]
    public IActionResult GetAllValues()
    {
        var values = _valueGateway.GetAll();
        return Ok(values);
    }

    [HttpGet("{id}")]
    public IActionResult GetValue(int id)
    {
        var value = _valueGateway.Get(x=>x.Id == id);
         if (id < 0 || value is null)
            return NotFound("Item not found");
        return Ok(value);
    }
}
