using Microsoft.AspNetCore.Mvc;
using Paradaim.Gateway.GYM.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class SocialController : ControllerBase
{
    private readonly ISocialGateway _socialGateway;

    public SocialController(ISocialGateway socialGateway)
    {
        _socialGateway = socialGateway;
    }
    [HttpGet]
    public IActionResult GetAllSocials()
    {
        var socials = _socialGateway.GetAll();
        return Ok(socials);
    }

    [HttpGet("{id}")]
    public IActionResult GetSocial(int id)
    {
        var social = _socialGateway.Get(x=>x.Id == id);
         if (id < 0 || social is null)
            return NotFound("Item not found");
        return Ok(social);
    }
}
