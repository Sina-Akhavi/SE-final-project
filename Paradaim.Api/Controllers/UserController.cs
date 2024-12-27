using Microsoft.AspNetCore.Mvc;
using Paradaim.Gateway.GYM.Interfaces;
using Paradaim.Api.Models.ViewModel.User.Request;
using Paradaim.Gateway.Models;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserGateway _userGateway;

    public UserController(IUserGateway userGateway)
    {
        _userGateway = userGateway;
    }
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _userGateway.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult Getuser(int id)
    {
        var user = _userGateway.Get(x=>x.UserId == id);
         if (id < 0 || user is null)
            return NotFound("Item not found");
        return Ok(user);
    }
    

    [HttpPost("AddUser")]
    public async Task<IActionResult> AddUser (UserViewModel user){
       
       var result = await _userGateway.Add(new User{
            Email=user.Email,
            PasswordHash=user.Password
            });
        return Ok(result);
    }

   [HttpPut("EditUser")]
public async Task<IActionResult> EditUser(UserEditModel userEdit)
{
    if (userEdit == null || userEdit.UserId <= 0)
    {
        return BadRequest("Invalid user data.");
    }

    var existingUser = _userGateway.Get(x => x.UserId == userEdit.UserId);
    
    if (existingUser == null)
    {
        return NotFound("User not found.");
    }

    existingUser.Email = userEdit.Email ?? existingUser.Email;
    existingUser.Name = userEdit.Name ?? existingUser.Name;
    existingUser.Job = userEdit.Job ?? existingUser.Job;
    existingUser.Age = userEdit.Age ?? existingUser.Age;
    existingUser.Weight = userEdit.Weight ?? existingUser.Weight;
    existingUser.Height = userEdit.Height ?? existingUser.Height;
    existingUser.ProfilePicture = userEdit.ProfilePicture ?? existingUser.ProfilePicture;
    existingUser.PasswordHash = userEdit.Password ?? existingUser.PasswordHash;

    
    var updatedUser = await _userGateway.Update(existingUser);

    if (updatedUser != null)
    {
        return Ok(updatedUser);
    }
    else
    {
        return StatusCode(500, "Failed to update user.");
    }
}

    

}
