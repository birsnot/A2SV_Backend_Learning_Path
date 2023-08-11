using Blog.Controllers;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests;

public class CommentsControllerTests
{
    private DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
        // Create a new in-memory database for each test
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Get_ReturnsAllComments()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            context.Comments.AddRange(new List<Comment>
            {
                new Comment { Id = 1, Text = "Comment 1" },
                new Comment { Id = 2, Text = "Comment 2" },
                new Comment { Id = 3, Text = "Comment 3" }
            });
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new CommentsController(context);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var comments = Assert.IsAssignableFrom<List<Comment>>(okResult.Value);
            Assert.Equal(3, comments.Count);
        }
    }

    [Fact]
    public async Task Get_WithValidCommentId_ReturnsComment()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            context.Comments.Add(new Comment { Id = 1, Text = "Test Comment" });
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new CommentsController(context);

            // Act
            var result = await controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var comment = Assert.IsAssignableFrom<Comment>(okResult.Value);
            Assert.Equal(1, comment.Id);
            Assert.Equal("Test Comment", comment.Text);
        }
    }

    [Fact]
    public async Task Get_WithInvalidCommentId_ReturnsNotFound()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new CommentsController(context);

            // Act
            var result = await controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }

    [Fact]
    public async Task Post_WithValidComment_ReturnsCreatedAtAction()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new CommentsController(context);
            var comment = new Comment { Id = 1, Text = "New Comment" };

            // Act
            var result = await controller.Post(comment);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues?["commentId"]);
            var createdComment = Assert.IsAssignableFrom<Comment>(createdAtActionResult.Value);
            Assert.Equal(1, createdComment.Id);
            Assert.Equal("New Comment", createdComment.Text);
        }
    }

    [Fact]
    public async Task Put_WithValidCommentId_UpdatesComment()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            context.Comments.Add(new Comment { Id = 1, Text = "Old Comment" });
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new CommentsController(context);
            var updatedComment = new Comment { Id = 1, Text = "Updated Comment" };

            // Act
            var result = await controller.Put(1, updatedComment);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var comment = await context.Comments.FindAsync(1);
            Assert.Equal("Updated Comment", comment?.Text);
        }
    }

    [Fact]
    public async Task Put_WithInvalidCommentId_ReturnsBadRequest()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using(var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new CommentsController(context);
            var updatedComment = new Comment { Id = 1, Text = "Updated Comment" };

            // Act
            var result = await controller.Put(2, updatedComment);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }

    [Fact]
    public async Task Delete_WithValidCommentId_DeletesComment()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            context.Comments.Add(new Comment { Id = 1, Text = "Test Comment" });
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new CommentsController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(context.Comments.Find(1));
        }
    }

    [Fact]
    public async Task Delete_WithInvalidCommentId_ReturnsNotFound()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new CommentsController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}