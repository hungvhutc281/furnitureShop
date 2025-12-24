namespace ProjectApi.Dto
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateCommentDTO
    {
        public int UserId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
    }

}
