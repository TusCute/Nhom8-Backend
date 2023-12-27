namespace MovieStoreAPI.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? ReleaseYear { get; set; }

        public string? ReviewImage { get; set; }

        public string Cast { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Director { get; set; } = null!;

    }
}
