using System;
using System.Collections.Generic;

namespace Paradaim.Gateway.Models;

public partial class Testimonial
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Quote { get; set; } = null!;

    public string Job { get; set; } = null!;

    public string Avatar { get; set; } = null!;
}
