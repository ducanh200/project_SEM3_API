using System;
using System.Collections.Generic;

namespace SEM3_API.Entities;

public partial class News
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? City { get; set; }

    public string? Thumbnail { get; set; }

    public string? Description { get; set; }

    public int TopicId { get; set; }

    public int CountryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual Topic Topic { get; set; } = null!;
}
