using System;
using System.Collections.Generic;

namespace Paradaim.Api.Models.ViewModel.User.Request;

public class UserEditModel
{
   
   public int UserId { get; set; }


    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? Name { get; set; }

    public string? ProfilePicture { get; set; }

    public string? Job { get; set; }

    public int? Age { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Height { get; set; }

}
