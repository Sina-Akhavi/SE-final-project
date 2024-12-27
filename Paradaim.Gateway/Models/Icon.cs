using System;
using System.Collections.Generic;

namespace Paradaim.Gateway.Models;

public partial class Icon
{
    public int Id { get; set; }

    public string IconName { get; set; } = null!;

    public virtual ICollection<GymProgram> GymPrograms { get; set; } = new List<GymProgram>();

    public virtual ICollection<Value> Values { get; set; } = new List<Value>();
}
