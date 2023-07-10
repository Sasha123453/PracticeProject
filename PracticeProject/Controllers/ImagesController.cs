using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace PracticeProject.Controllers
{
    [Route("getimage")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [Route("getimage/{folderName}/{imageName}")]
        public PhysicalFileResult GetImage(string folderName, string imageName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{folderName}", imageName);
            return PhysicalFile(filePath, "image/jpg");
        }
    }
}
