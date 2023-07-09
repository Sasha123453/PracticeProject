using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PracticeProject.Areas.Identity.Data;
using PracticeProject.Models;
using System.Drawing.Printing;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileSystemGlobbing;

namespace PracticeProject.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly GoogleCaptchaService _googleCaptchaService;
        private readonly IHubContext<CommentHub> _commentHubContext;
        const int pageSizeComments = 8;
        const int pageSizeResources = 10;
        const int pageSizeRequests = 4;
        public ResourcesController(UserManager<User> userManager, ApplicationContext context, GoogleCaptchaService googleCaptchaService, IHubContext<CommentHub> commenthubContext)
        {
            _context = context;
            _userManager = userManager;
            _googleCaptchaService = googleCaptchaService;
            _commentHubContext = commenthubContext;
        }
        [Authorize]
        public IActionResult ResourceRequest()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> SendRequest(string name, string link, string description, string token)
        {
            try
            {
                var captchaResult = await _googleCaptchaService.VerifyToken(token);
                if (!captchaResult)
                {
                    return Json(new { success = false, error = "Проблемы с каптчей" });
                }
                string userId = _userManager.GetUserId(User);
                ResourceRequestModel resourceRequest = new ResourceRequestModel(name, description, link, userId);
                await _context.ResourceRequests.AddAsync(resourceRequest);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex) 
            {
                string errorMessage = "An error occurred while processing the chat request. Details: " + ex.Message;
                return Json(new { success = false, error = errorMessage });
            }
        }
        public async Task<IActionResult> ShowResourcesPage(int page = 1)
        {
            List<ResourceModel> resources = await GetResourcesFromDataSource(page);
            double resourcesAmount = await GetResourcesAmountFromDataSource();
            int totalPages = (int)Math.Ceiling(resourcesAmount / pageSizeResources);

            var viewModel = new ResourceViewModel<ResourceModel>
            {
                Resources = resources,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(viewModel);
        }
        public async Task<IActionResult> ResourceById(int id)
        {
            var comments = await GetResourceCommentsFromDataSource(id);
            var resource = await _context.Resources.FindAsync(id);
            var viewModel = new ResourceByIdViewModel<CommentWithNicknameModel>
            {
                ResourceCommentsWithNickname = comments,
                Resource = resource
            };
            return View(viewModel);
        }
        public async Task<ResourceRequestsWithTotalPagesModel> GetRequestsFromDataSource(int page, bool watched = false, bool completed = false, bool rejected = false, bool nothing = false)
        {
            var result = from request in _context.ResourceRequests
                        join user in _context.Users
                        on request.UserId equals user.Id
                        select new ResourceRequestWithNicknameModel
                        {
                            Request = request,
                            Nickname = user.Nickname,
                            CreatedAt = request.CreatedAt
                        };

            if (!User.IsInRole("Admin"))
            {
                string userId = _userManager.GetUserId(User);
                result = result.Where(request => request.Request.UserId == userId);
            }
            if (watched)
                result = result.Where(x => x.Request.IsBeingWatched);
            if (completed)
                result = result.Where(x => x.Request.IsCompleted);
            if (rejected)
                result = result.Where(x => x.Request.IsRejected);
            if (nothing)
                result = result.Where(x => !x.Request.IsBeingWatched && !x.Request.IsCompleted && !x.Request.IsRejected);
            
            double amount = await result.CountAsync();
            int totalPages = (int)Math.Ceiling(amount / pageSizeRequests);
            if (totalPages < 1) totalPages = 1;
            result = result.Skip((page - 1) * pageSizeRequests).Take(pageSizeRequests);

            ResourceRequestsWithTotalPagesModel newResult = new ResourceRequestsWithTotalPagesModel()
            {
                Requests = result,
                TotalPages = totalPages
            };
            return newResult;
        }
        public async Task<IActionResult> ShowRequestsPage(int page = 1)
        {
            bool watched = Request.Query.ContainsKey("watched");
            bool completed = Request.Query.ContainsKey("completed");
            bool rejected = Request.Query.ContainsKey("rejected");
            bool nothing = Request.Query.ContainsKey("nothing");
            ResourceRequestsWithTotalPagesModel requestsWithTotalPages = await GetRequestsFromDataSource(page, watched, completed, rejected, nothing);
            var requests = requestsWithTotalPages.Requests;
            var viewModel = new ResourceViewModel<ResourceRequestWithNicknameModel>
            {
                Resources = requests,
                CurrentPage = page,
                TotalPages = requestsWithTotalPages.TotalPages
            };

            return View(viewModel);
        }
        public async Task<List<ResourceModel>> GetResourcesFromDataSource(int page)
        {
            return await _context.Resources.Skip((page - 1) * pageSizeResources).Take(pageSizeResources).ToListAsync();
        }
        public async Task<int> GetRequestsAmountFromDataSource()
        {
            return await _context.ResourceRequests.CountAsync();
        }
        public async Task<int> GetResourcesAmountFromDataSource()
        {
            return await _context.Resources.CountAsync();
        }
        public async Task<List<CommentWithNicknameModel>> GetResourceCommentsFromDataSource(int id, int page = 1)
        {
            var result = await (from comment in _context.Comments
                                join user in _context.Users
                                on comment.UserId equals user.Id
                                where comment.ResourceId == id
                                select new CommentWithNicknameModel
                                {
                                    CommentText = comment.Text,
                                    Nickname = user.Nickname,
                                    CreatedAt = DateOnly.FromDateTime(comment.CreatedAt)
                                })
                                .Skip((page - 1) * pageSizeComments)
                                .Take(pageSizeComments)
                                .ToListAsync();
            return result;
        }
        public async Task<int> GetResourceCommentsAmountFromDataSource(int id)
        {
            var result = await _context.Comments.Where(x => x.ResourceId == id).CountAsync();
            return result;
        }
        [HttpPost]
        public async Task<IActionResult> LoadMoreComments(int resourceId, int page)
        {
            double commentsAmount = await GetResourceCommentsAmountFromDataSource(resourceId);
            if (Math.Ceiling(commentsAmount / pageSizeComments) < page) return Json(new { success = true });
            var comments = await GetResourceCommentsFromDataSource(resourceId, page);
            return Json(comments);
        }
        [Authorize]
        public async Task<IActionResult> SendComment(string text, int id)
        {
            try
            {
                double commentsAmount = await GetResourceCommentsAmountFromDataSource(id);
                string userId = _userManager.GetUserId(User);
                ResourceCommentModel comment = new ResourceCommentModel(text, userId, id);
                var user = await _userManager.FindByIdAsync(userId);
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                CommentWithNicknameModel model = new CommentWithNicknameModel()
                {
                    ResourceId = comment.ResourceId,
                    Nickname = user.Nickname,
                    CommentText = comment.Text,
                    CreatedAt = DateOnly.FromDateTime(comment.CreatedAt)
                };
                await _commentHubContext.Clients.All.SendAsync("NewComment", model);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while processing the chat request. Details1: " + ex.Message;
                return Json(new { success = false, error = errorMessage });
            }
        }

    }
}
