using System;
using System.Collections.Generic;

namespace Paradaim.Gateway.Models;

public partial class Social
{
    public int Id { get; set; }

    public int TrainerId { get; set; }

    public string Platform { get; set; } = null!;

    public string Url { get; set; } = null!;

    public virtual Trainer Trainer { get; set; } = null!;
}
