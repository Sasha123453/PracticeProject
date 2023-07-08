using Microsoft.AspNetCore.SignalR;

namespace PracticeProject.Models
{
    public class CommentHub: Hub
    {
        public async Task SendCommentNotification(CommentWithNicknameModel comment)
        {
            await Clients.All.SendAsync("NewComment", comment);
        }
    }
}
