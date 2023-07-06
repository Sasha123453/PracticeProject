using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PracticeProject.Areas.Identity.Data;
using PracticeProject.Models;
using System.Drawing.Printing;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace PracticeProject.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        const int pageSizeComments = 8;
        const int pageSizeResources = 10; 
        public ResourcesController(UserManager<User> userManager, ApplicationContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ResourceRequest()
        {
            return View();
        }
        public async Task<IActionResult> SendRequest(string name, string link, string description)
        {
            try
            {
                string userId = _userManager.GetUserId(User);
                ResourceRequestModel resourceRequest = new ResourceRequestModel(name, link, description, userId);
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
        public async Task<List<ResourceModel>> GetResourcesFromDataSource(int page)
        {
            return await _context.Resources.Skip((page - 1) * pageSizeResources).Take(pageSizeResources).ToListAsync();
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
            int commentsAmount = await GetResourceCommentsAmountFromDataSource(resourceId);
            if (Math.Round((commentsAmount / pageSizeComments) + 0.5) < page) return StatusCode(409);
            var comments = await GetResourceCommentsFromDataSource(resourceId, page);
            return Json(comments);
        }
        public async Task<IActionResult> SendComment(string text, int id, int page)
        {
            try
            {
                if (!User.Identity.IsAuthenticated) return StatusCode(400);
                int commentsAmount = await GetResourceCommentsAmountFromDataSource(id);
                bool isNeedsToBeAdded = false;
                if (Math.Round((commentsAmount / pageSizeComments) + 0.5) < page || commentsAmount < pageSizeComments) isNeedsToBeAdded = true;
                string userId = _userManager.GetUserId(User);
                ResourceCommentModel comment = new ResourceCommentModel(text, userId, id);
                var user = await _userManager.FindByIdAsync(userId);
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return Json(new { success = true, nickname = user.Nickname, needsToBeAdded = isNeedsToBeAdded });
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while processing the chat request. Details: " + ex.Message;
                return Json(new { success = false, error = errorMessage });
            }
        }

    }
}
