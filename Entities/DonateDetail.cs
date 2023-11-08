using System;
using System.Collections.Generic;

namespace SEM3_API.Entities;

public partial class DonateDetail
{
    public int DonateId { get; set; }

    public int ProjectId { get; set; }

    public decimal? Amount { get; set; }

    public virtual Donate Donate { get; set; } = null!;

    public virtual Project Project { get; set; } = null!;
}
