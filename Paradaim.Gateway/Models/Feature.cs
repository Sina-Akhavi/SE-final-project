using System;
using System.Collections.Generic;

namespace Paradaim.Gateway.Models;

public partial class Feature
{
    public int Id { get; set; }

    public int PlanId { get; set; }

    public string FeatureDescription { get; set; } = null!;

    public bool Available { get; set; }

    public virtual Plan Plan { get; set; } = null!;
}
