using System;
using System.Collections.Generic;

namespace SEM3_API.Entities;

public partial class Donate
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Amount { get; set; }
}
