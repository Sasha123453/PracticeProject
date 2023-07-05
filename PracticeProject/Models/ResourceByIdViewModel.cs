namespace PracticeProject.Models
{
    public class ResourceByIdViewModel<T>
    {
        public List<T> ResourceCommentsWithNickname;
        public ResourceModel Resource { get; set; }
    }
}
