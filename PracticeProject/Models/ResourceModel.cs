using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticeProject.Models
{
    public class ResourceModel
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Link { get; set; }
        public string ImageName { get; set; }
        public string Type { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public ResourceModel () { }
        public ResourceModel(string name, string shortDescription, string longDescription, string link, string imageName, string type)
        {
            Name = name;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            Link = link;
            ImageName = imageName;
            Type = type;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
