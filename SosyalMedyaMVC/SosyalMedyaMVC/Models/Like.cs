namespace SosyalMedyaMVC.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int? CommentId { get; set; } // Nullable yaparak esnekliği artırabiliriz
    }
}
