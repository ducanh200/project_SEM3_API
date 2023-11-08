using System;
using System.Collections.Generic;

namespace SEM3_API.Entities;

public partial class Topic
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
