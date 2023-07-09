namespace PracticeProject.Models
{
    public class ResourceRequestsWithTotalPagesModel
    {
        public IQueryable<ResourceRequestWithNicknameModel> Requests;
        public int TotalPages { get; set; }
    }
}
