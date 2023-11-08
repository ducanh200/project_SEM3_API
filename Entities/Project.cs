using System;
using System.Collections.Generic;

namespace SEM3_API.Entities;

public partial class Project
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Thumbnail1 { get; set; }

    public string? Thumbnail2 { get; set; }

    public decimal? Fund { get; set; }

    public string? Description { get; set; }

    public string? City { get; set; }

    public string? Address { get; set; }

    public int CountryId { get; set; }

    public int TopicId { get; set; }

    public DateTime? Begin { get; set; }

    public DateTime? Finish { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual Topic Topic { get; set; } = null!;
}
