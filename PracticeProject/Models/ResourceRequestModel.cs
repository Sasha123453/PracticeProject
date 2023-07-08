using PracticeProject.Areas.Identity.Data;

namespace PracticeProject.Models
{
    public class ResourceRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsRejected { get; set; }
        public bool IsBeingWatched { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual User User { get; set; }
        public string UserId { get; set; }
        public ResourceRequestModel(string name, string description, string link, string userId) 
        {
            Name = name;
            Description = description;
            Link = link;
            UserId = userId;
            IsCompleted = false;
            IsRejected = false;
            IsBeingWatched = false;
            CreatedAt = DateTime.Now;
        }
    }
}
