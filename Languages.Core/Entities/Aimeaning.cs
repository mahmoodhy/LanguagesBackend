using System;
using System.Collections.Generic;

namespace Core.Entities;

public partial class Aimeaning
{
    public int Id { get; set; }

    public int BoxId { get; set; }

    public string? Persian { get; set; }

    public string? Meaning { get; set; }

    public string? Example { get; set; }
}
