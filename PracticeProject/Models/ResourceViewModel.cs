namespace PracticeProject.Models
{
    public class ResourceViewModel<T>
    {
        public IEnumerable<T> Resources { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
