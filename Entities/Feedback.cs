using System;
using System.Collections.Generic;

namespace SEM3_API.Entities;

public partial class Feedback
{
    public int Id { get; set; }

    public string? Message { get; set; }

    public int UserId { get; set; }

    public int ProjectId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
