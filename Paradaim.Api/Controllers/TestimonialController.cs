using Microsoft.AspNetCore.Mvc;
using Paradaim.Gateway.GYM.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class TestimonialController : ControllerBase
{
    private readonly ITestimonialGateway _testimonialGateway;

    public TestimonialController(ITestimonialGateway testimonialGateway)
    {
        _testimonialGateway = testimonialGateway;
    }
    [HttpGet]
    public IActionResult GetAlltestimonial()
    {
        var testimonials = _testimonialGateway.GetAll();
        return Ok(testimonials);
    }

    [HttpGet("{id}")]
    public IActionResult GetTestimonial(int id)
    {
        var testimonials = _testimonialGateway.Get(x=>x.Id == id);
         if (id < 0 || testimonials is null)
            return NotFound("Item not found");
        return Ok(testimonials);
    }
}
