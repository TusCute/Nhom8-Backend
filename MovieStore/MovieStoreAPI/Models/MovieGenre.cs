using System;
using System.Collections.Generic;

namespace MovieStoreAPI.Models;

public partial class MovieGenre
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public int GenreId { get; set; }

    public int ReviewId { get; set; }
}
