using System;
using System.Collections.Generic;

namespace Paradaim.Gateway.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Name { get; set; }

    public string? ProfilePicture { get; set; }

    public string? Job { get; set; }

    public int? Age { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Height { get; set; }

    public virtual ICollection<Plan> Plans { get; set; } = new List<Plan>();
}
