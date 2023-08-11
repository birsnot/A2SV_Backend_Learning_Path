using Blog.Controllers;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests;

public class PostsControllerTests
{
    private DbContextOptions<AppDbContext> CreateNewContextOptions()
    {
        // Create a new in-memory database for each test
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Get_ReturnsAllPosts()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            context.Posts.AddRange(new List<Post>
            {
                new Post { Id = 1, Title = "Post 1", Content = "Content 1" },
                new Post { Id = 2, Title = "Post 2", Content = "Content 2" },
                new Post { Id = 3, Title = "Post 3", Content = "Content 3" }
            });
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new PostsController(context);

            // Act
            var result = await controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var posts = Assert.IsAssignableFrom<List<Post>>(okResult.Value);
            Assert.Equal(3, posts.Count);
        }
    }

    [Fact]
    public async Task Get_WithValidPostId_ReturnsPostWithComments()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            context.Posts.Add(new Post
            {
                Id = 1,
                Title = "Test Post",
                Content = "Test Content",
                Comments = new List<Comment>
                {
                    new Comment { Id = 1, Text = "Comment 1" },
                    new Comment { Id = 2, Text = "Comment 2" }
                }
            });
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new PostsController(context);

            // Act
            var result = await controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var post = Assert.IsAssignableFrom<Post>(okResult.Value);
            Assert.Equal(1, post.Id);
            Assert.Equal("Test Post", post.Title);
            Assert.Equal("Test Content", post.Content);
            Assert.Equal(2, post.Comments.Count);
        }
    }

    [Fact]
    public async Task Get_WithInvalidPostId_ReturnsNotFound()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new PostsController(context);

            // Act
            var result = await controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }

    [Fact]
    public async Task Post_WithValidPost_ReturnsCreatedAtAction()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new PostsController(context);
            var post = new Post { Id = 1, Title = "New Post", Content = "New Content" };

            // Act
            var result = await controller.Post(post);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("Get", createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues?["postId"]);
            var createdPost = Assert.IsAssignableFrom<Post>(createdAtActionResult.Value);
            Assert.Equal(1, createdPost.Id);
            Assert.Equal("New Post", createdPost.Title);
            Assert.Equal("New Content", createdPost.Content);
        }
    }

    [Fact]
    public async Task Put_WithValidPostId_UpdatesPost()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            context.Posts.Add(new Post { Id = 1, Title = "Old Post", Content = "Old Content" });
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new PostsController(context);
            var updatedPost = new Post { Id = 1, Title = "Updated Post", Content = "Updated Content" };

            // Act
            var result = await controller.Put(1, updatedPost);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var post = await context.Posts.FindAsync(1);
            Assert.Equal("Updated Post", post?.Title);
            Assert.Equal("Updated Content", post?.Content);
        }
    }

    [Fact]
    public async Task Put_WithInvalidPostId_ReturnsNotFound()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new PostsController(context);
            var updatedPost = new Post { Id = 1, Title = "Updated Post", Content = "Updated Content" };

            // Act
            var result = await controller.Put(1, updatedPost);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }

    [Fact]
    public async Task Delete_WithValidPostId_DeletesPost()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            context.Posts.Add(new Post { Id = 1, Title = "Post to delete", Content = "Content to delete" });
            await context.SaveChangesAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new PostsController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var post = await context.Posts.FindAsync(1);
            Assert.Null(post);
        }
    }

    [Fact]
    public async Task Delete_WithInvalidPostId_ReturnsNotFound()
    {
        // Arrange
        var options = CreateNewContextOptions();
        await using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureCreatedAsync();
        }

        await using (var context = new AppDbContext(options))
        {
            var controller = new PostsController(context);

            // Act
            var result = await controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}