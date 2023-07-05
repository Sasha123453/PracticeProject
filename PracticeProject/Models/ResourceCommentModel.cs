using PracticeProject.Areas.Identity.Data;

namespace PracticeProject.Models
{
    public class ResourceCommentModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string Text { get; set; }
        public virtual ResourceModel Resource { get; set; }
        public int ResourceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ResourceCommentModel(string text, string userId, int resourceId) 
        {
            Text = text;
            UserId = userId;
            ResourceId = resourceId;
            CreatedAt = DateTime.Now;
        }
    }
}
