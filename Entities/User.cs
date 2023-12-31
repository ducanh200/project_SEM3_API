﻿using System;
using System.Collections.Generic;

namespace SEM3_API.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public string? City { get; set; }

    public string? Address { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Donate> Donates { get; set; } = new List<Donate>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
