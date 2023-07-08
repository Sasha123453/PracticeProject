using Microsoft.AspNetCore.SignalR;

namespace PracticeProject.Models
{
    public class CommentHub: Hub
    {
        public async Task SendCommentNotification(ResourceCommentModel comment)
        {
            await Clients.All.SendAsync("NewComment", comment);
        }
    }
}
