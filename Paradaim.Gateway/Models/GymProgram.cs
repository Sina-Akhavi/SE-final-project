using System;
using System.Collections.Generic;

namespace Paradaim.Gateway.Models;

public partial class GymProgram
{
    public int Id { get; set; }

    public int IconId { get; set; }

    public string Title { get; set; } = null!;

    public string Info { get; set; } = null!;

    public string Path { get; set; } = null!;

    public virtual Icon Icon { get; set; } = null!;
}
