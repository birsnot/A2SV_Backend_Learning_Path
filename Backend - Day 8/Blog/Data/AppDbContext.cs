using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data;

public class AppDbContext : DbContext
{
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasOne(p => p.Post)
                .WithMany(c => c.Comments)
                .HasForeignKey(x => x.PostId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Comment_Post");
        });
    }
}