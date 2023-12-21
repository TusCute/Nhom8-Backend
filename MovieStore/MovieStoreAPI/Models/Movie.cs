using System;
using System.Collections.Generic;

namespace MovieStoreAPI.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? ReleaseYear { get; set; }

    public string? MovieImage { get; set; }

    public string Cast { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Director { get; set; } = null!;
}
