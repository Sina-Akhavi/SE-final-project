using Microsoft.AspNetCore.Mvc;
using Paradaim.Gateway.GYM.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class FaqController : ControllerBase
{
    private readonly IFaqGateway _faqGateway;

    public FaqController(IFaqGateway faqGateway)
    {
        _faqGateway = faqGateway;
    }
    [HttpGet]
    public IActionResult GetAllFaq()
    {
        var faqs = _faqGateway.GetAll();
        return Ok(faqs);
    }

[HttpGet("{id}")]
    public IActionResult GetFaq(int id)
    {
       
        var faq = _faqGateway.Get(x=>x.Id == id);
         if (id < 0 || faq is null)
            return NotFound("Item not found");
            
        return Ok(faq);
    }
}
