using Microsoft.AspNetCore.Mvc;
using Paradaim.Gateway.GYM.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class IconController : ControllerBase
{
    private readonly IIconGateway _iconGateway;

    public IconController(IIconGateway iconGateway)
    {
        _iconGateway = iconGateway;
    }
    [HttpGet]
    public IActionResult GetAllicon()
    {
        var icons = _iconGateway.GetAll();
        return Ok(icons);
    }

    [HttpGet("{id}")]
    public IActionResult Geticon(int id)
    {
       
        var icon = _iconGateway.Get(x=>x.Id == id);
         if (id < 0 || icon is null)
            return NotFound("Item not found");
            
        return Ok(icon);
    }
}
