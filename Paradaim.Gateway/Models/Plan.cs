using System;
using System.Collections.Generic;

namespace Paradaim.Gateway.Models;

public partial class Plan
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Feature> Features { get; set; } = new List<Feature>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
