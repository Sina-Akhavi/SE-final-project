using System;
using System.Collections.Generic;

namespace Paradaim.Gateway.Models;

public partial class Trainer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Job { get; set; } = null!;

    public string ImagePath { get; set; } = null!;

    public virtual ICollection<Social> Socials { get; set; } = new List<Social>();
}
