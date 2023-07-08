namespace PracticeProject.Models
{
    public class CommentWithNicknameModel
    {
        public string CommentText { get; set; }
        public string Nickname { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
