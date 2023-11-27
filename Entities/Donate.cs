using System;
using System.Collections.Generic;

namespace SEM3_API.Entities;

public partial class Donate
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public int UserId { get; set; }

    public int ProjectId { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
