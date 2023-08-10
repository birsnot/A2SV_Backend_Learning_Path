using Blog.Controllers;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests;

public class PostsControllerTests
    {
        private DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithPosts()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Posts.Add(new Post { Id = 1, Title = "Post 1", Content = "Content 1" });
                context.Posts.Add(new Post { Id = 2, Title = "Post 2", Content = "Content 2" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new PostsController(context);

                var result = await controller.Get();

                var okResult = Assert.IsType<OkObjectResult>(result);
                var posts = Assert.IsAssignableFrom<List<Post>>(okResult.Value);
                Assert.Equal(2, posts.Count);
            }
        }

        [Fact]
        public async Task Get_WithValidPostId_ReturnsOkResultWithPost()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Posts.Add(new Post { Id = 1, Title = "Post 1", Content = "Content 1" });
                context.Posts.Add(new Post { Id = 2, Title = "Post 2", Content = "Content 2" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new PostsController(context);

                var result = await controller.Get(2);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var post = Assert.IsAssignableFrom<Post>(okResult.Value);
                Assert.Equal("Post 2", post.Title);
            }
        }

        [Fact]
        public async Task Get_WithInvalidPostId_ReturnsBadRequest()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Posts.Add(new Post { Id = 1, Title = "Post 1", Content = "Content 1" });
                context.Posts.Add(new Post { Id = 2, Title = "Post 2", Content = "Content 2" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new PostsController(context);

                var result = await controller.Get(3);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid Id", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task Post_AddsPostAndReturnsCreatedAtAction()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                var controller = new PostsController(context);
                var newPost = new Post { Id = 1, Title = "New Post", Content = "New Content" };

                var result = await controller.Post(newPost);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                var post = Assert.IsAssignableFrom<Post>(createdAtActionResult.Value);
                Assert.Equal("New Post", post.Title);

                using (var dbContext = new AppDbContext(options))
                {
                    var savedPost = await dbContext.Posts.FindAsync(1);
                    Assert.Equal("New Post", savedPost.Title);
                }
            }
        }

        [Fact]
        public async Task Patch_WithValidPostId_UpdatesPostAndReturnsNoContent()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Posts.Add(new Post { Id = 1, Title = "Post 1", Content = "Content 1" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new PostsController(context);

                var result = await controller.Patch(1, "Updated Title");

                Assert.IsType<NoContentResult>(result);

                using (var dbContext =new AppDbContext(options))
                {
                    var updatedPost = await dbContext.Posts.FindAsync(1);
                    Assert.Equal("Updated Title", updatedPost.Title);
                }
            }
        }

        [Fact]
        public async Task Patch_WithInvalidPostId_ReturnsBadRequest()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Posts.Add(new Post { Id = 1, Title = "Post 1", Content = "Content 1" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new PostsController(context);

                var result = await controller.Patch(2, "Updated Title");

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid id", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task Put_WithValidPostId_UpdatesPostAndReturnsNoContent()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Posts.Add(new Post { Id = 1, Title = "Post 1", Content = "Content 1" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new PostsController(context);
                var updatedPost = new Post { Title = "Updated Title", Content = "Updated Content" };

                var result = await controller.Put(1, updatedPost);

                Assert.IsType<NoContentResult>(result);

                using (var dbContext = new AppDbContext(options))
                {
                    var post = await dbContext.Posts.FindAsync(1);
                    Assert.Equal("Updated Title", post.Title);
                    Assert.Equal("Updated Content", post.Content);
                }
            }
        }

        [Fact]
        public async Task Put_WithInvalidPostId_ReturnsBadRequest()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Posts.Add(new Post { Id = 1, Title = "Post 1", Content = "Content 1" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new PostsController(context);
                var updatedPost = new Post { Title = "Updated Title", Content = "Updated Content" };

                var result = await controller.Put(2, updatedPost);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid id", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task Delete_WithValidPostId_RemovesPostAndReturnsNoContent()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Posts.Add(new Post { Id = 1, Title = "Post 1", Content = "Content 1" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new PostsController(context);

                var result = await controller.Delete(1);

                Assert.IsType<NoContentResult>(result);

                using (var dbContext = new AppDbContext(options))
                {
                    var post = await dbContext.Posts.FindAsync(1);
                    Assert.Null(post);
                }
            }
        }

        [Fact]
        public async Task Delete_WithInvalidPostId_ReturnsBadRequest()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Posts.Add(new Post { Id = 1, Title = "Post 1", Content = "Content 1" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new PostsController(context);

                var result = await controller.Delete(2);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid id", badRequestResult.Value);
            }
        }
    }