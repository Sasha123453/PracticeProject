using Elfie.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PracticeProject.Models;

namespace PracticeProject.Areas.Identity.Data;

public class ApplicationContext : IdentityDbContext<User>
{
    public DbSet<ResourceModel> Resources { get; set; }
    public DbSet<ResourceRequestModel> ResourceRequests { get; set; }
    public DbSet<ResourceCommentModel> Comments { get; set; }
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
