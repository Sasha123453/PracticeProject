namespace PracticeProject.Models
{
    public class ResourceRequestWithNicknameModel
    {
        public ResourceRequestModel Request { get; set; }
        public string Nickname { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
