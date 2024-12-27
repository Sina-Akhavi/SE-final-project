using Microsoft.AspNetCore.Mvc;
using Paradaim.Gateway.GYM.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class LinkController : ControllerBase
{
    private readonly ILinkGateway _linkGateway;

    public LinkController(ILinkGateway linkGateway)
    {
        _linkGateway = linkGateway;
    }
    [HttpGet]
    public IActionResult GetAllLink()
    {
        var links = _linkGateway.GetAll();
        return Ok(links);
    }

    [HttpGet("{id}")]
    public IActionResult GetLink(int id)
    {
       
        var link = _linkGateway.Get(x=>x.Id == id);
         if (id < 0 || link is null)
            return NotFound("Item not found");
            
        return Ok(link);
    }
}
