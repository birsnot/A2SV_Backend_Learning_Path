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

public class CommentsControllerTests
    {
        private DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithComments()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Comments.Add(new Comment { Id = 1, Text = "Comment 1" });
                context.Comments.Add(new Comment { Id = 2, Text = "Comment 2" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new CommentsController(context);

                var result = await controller.Get();

                var okResult = Assert.IsType<OkObjectResult>(result);
                var comments = Assert.IsAssignableFrom<List<Comment>>(okResult.Value);
                Assert.Equal(2, comments.Count);
            }
        }

        [Fact]
        public async Task Get_WithValidCommentId_ReturnsOkResultWithComment()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Comments.Add(new Comment { Id = 1, Text = "Comment 1" });
                context.Comments.Add(new Comment { Id = 2, Text = "Comment 2" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new CommentsController(context);

                var result = await controller.Get(2);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var comment = Assert.IsAssignableFrom<Comment>(okResult.Value);
                Assert.Equal("Comment 2", comment.Text);
            }
        }

        [Fact]
        public async Task Get_WithInvalidCommentId_ReturnsBadRequest()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Comments.Add(new Comment { Id = 1, Text = "Comment 1" });
                context.Comments.Add(new Comment { Id = 2, Text = "Comment 2" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new CommentsController(context);

                var result = await controller.Get(3);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid Id", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task Post_AddsCommentAndReturnsCreatedAtAction()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                var controller = new CommentsController(context);
                var newComment = new Comment { Id = 1, Text = "New Comment" };

                var result = await controller.Post(newComment);

                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                var comment = Assert.IsAssignableFrom<Comment>(createdAtActionResult.Value);
                Assert.Equal("New Comment", comment.Text);

                using (var dbContext = new AppDbContext(options))
                {
                    var savedComment = await dbContext.Comments.FindAsync(1);
                    Assert.Equal("New Comment", savedComment.Text);
                }
            }
        }

        [Fact]
        public async Task Put_WithValidCommentId_UpdatesCommentAndReturnsNoContent()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Comments.Add(new Comment { Id = 1, Text = "Comment 1" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new CommentsController(context);
                var updatedComment = new Comment { Id = 1, Text = "Updated Comment" };

                var result = await controller.Put(1, updatedComment);

                Assert.IsType<NoContentResult>(result);

                using (var dbContext = new AppDbContext(options))
                {
                    var comment = await dbContext.Comments.FindAsync(1);
                    Assert.Equal("Updated Comment", comment.Text);
                }
            }
        }

        [Fact]
        public async Task Put_WithInvalidCommentId_ReturnsBadRequest()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Comments.Add(new Comment { Id = 1, Text = "Comment 1" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new CommentsController(context);
                var updatedComment = new Comment { Id = 2, Text = "Updated Comment" };

                var result = await controller.Put(2, updatedComment);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid id", badRequestResult.Value);

                using (var dbContext = new AppDbContext(options))
                {
                    var comment = await dbContext.Comments.FindAsync(1);
                    Assert.Equal("Comment 1", comment.Text);
                }
            }
        }

        [Fact]
        public async Task Delete_WithValidCommentId_RemovesCommentAndReturnsNoContent()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Comments.Add(new Comment { Id = 1, Text = "Comment 1" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new CommentsController(context);

                var result = await controller.Delete(1);

                Assert.IsType<NoContentResult>(result);

                using (var dbContext = new AppDbContext(options))
                {
                    var comment = await dbContext.Comments.FindAsync(1);
                    Assert.Null(comment);
                }
            }
        }

        [Fact]
        public async Task Delete_WithInvalidCommentId_ReturnsBadRequest()
        {
            var options = CreateNewContextOptions();
            using (var context = new AppDbContext(options))
            {
                context.Comments.Add(new Comment { Id = 1, Text = "Comment 1" });
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var controller = new CommentsController(context);

                var result = await controller.Delete(2);

                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Invalid id", badRequestResult.Value);

                using (var dbContext = new AppDbContext(options))
                {
                    var comment = await dbContext.Comments.FindAsync(1);
                    Assert.Equal("Comment 1", comment.Text);
                }
            }
        }
    }