using System;
using System.Collections.Generic;

namespace Paradaim.Gateway.Models;

public partial class Value
{
    public int Id { get; set; }

    public int IconId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual Icon Icon { get; set; } = null!;
}
