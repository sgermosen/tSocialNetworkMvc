namespace Tetas.Web.Controllers.Api
{
    using Domain.Entities;
    using Domain.Helpers;
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Repositories.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Tetas.Web.Models.Api;

    [ApiController]
    [Route("api/posts")]
    [Produces("application/json")]
    [IgnoreAntiforgeryToken]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PostsApiController : ControllerBase
    {
        private readonly IPost _postRepository;
        private readonly IUserHelper _userHelper;
        private readonly IContentSanitizer _sanitizer;
        private readonly INotificationService _notifications;

        public PostsApiController(IPost postRepository, IUserHelper userHelper,
            IContentSanitizer sanitizer, INotificationService notifications)
        {
            _postRepository = postRepository;
            _userHelper = userHelper;
            _sanitizer = sanitizer;
            _notifications = notifications;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> Get()
        {
            var posts = await _postRepository.GetPostWithComments("")
                .OrderByDescending(p => p.Date)
                .Take(30)
                .ToListAsync();

            return Ok(posts.Select(ToDto));
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<PostDto>> Get(long id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(ToDto(post));
        }

        [HttpPost]
        public async Task<ActionResult<PostDto>> Create(CreatePostRequest request)
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return Unauthorized();
            }

            var post = new Post
            {
                Name = request.Name,
                Body = _sanitizer.Sanitize(request.Body),
                Owner = user,
                Date = DateTime.UtcNow
            };

            await _postRepository.AddAsync(post);

            return CreatedAtAction(nameof(Get), new { id = post.Id }, ToDto(post));
        }

        [HttpPost("{id:long}/comments")]
        public async Task<ActionResult<CommentDto>> Comment(long id, CreateCommentRequest request)
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return Unauthorized();
            }

            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var comment = new PostComment
            {
                Name = request.Name,
                Body = _sanitizer.Sanitize(request.Body),
                Owner = user,
                Post = post,
                Date = DateTime.UtcNow
            };

            await _postRepository.AddCommentAsync(comment);

            if (post.Owner != null && post.Owner.Id != user.Id)
            {
                await _notifications.NotifyAsync(post.Owner.Id, user.FullName,
                    $"{user.FullName} commented on your post", $"/Posts/Details/{post.Id}");
            }

            return Ok(new CommentDto
            {
                Id = comment.Id,
                Name = comment.Name,
                Body = comment.Body,
                Date = comment.Date,
                AuthorName = user.FullName,
                AuthorEmail = user.Email
            });
        }

        [HttpPost("{id:long}/reactions")]
        public async Task<ActionResult<ReactionSummaryDto>> React(long id, ReactionRequest request)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (!Enum.TryParse<ReactionType>(request?.Type, true, out var type))
            {
                type = ReactionType.Like;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var summary = await _postRepository.ToggleReactionAsync(id, userId, type);

            if (summary.MyReaction != null && post.Owner != null && post.Owner.Id != userId)
            {
                var actor = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                await _notifications.NotifyAsync(post.Owner.Id, actor?.FullName,
                    $"{actor?.FullName} reacted to your post", $"/Posts/Details/{post.Id}");
            }

            return Ok(new ReactionSummaryDto
            {
                Total = summary.Total,
                MyReaction = summary.MyReaction?.ToString()
            });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (post.Owner.Email != User.Identity.Name)
            {
                return Forbid();
            }

            post.Deleted = true;
            await _postRepository.UpdateAsync(post);

            return NoContent();
        }

        private PostDto ToDto(Post post)
        {
            var me = User.Identity.Name;
            var myId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reactions = post.Reactions ?? new List<Reaction>();
            var myReaction = reactions.FirstOrDefault(r => r.OwnerId == myId);
            return new PostDto
            {
                Id = post.Id,
                Name = post.Name,
                Body = post.Body,
                Date = post.Date,
                UpdatedDate = post.UpdatedDate,
                AuthorName = post.Owner?.FullName,
                AuthorEmail = post.Owner?.Email,
                IsMine = post.Owner?.Email == me,
                ReactionCount = reactions.Count,
                MyReaction = myReaction?.Type.ToString(),
                Comments = (post.PostComments ?? Enumerable.Empty<PostComment>())
                    .Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Body = c.Body,
                        Date = c.Date,
                        AuthorName = c.Owner?.FullName,
                        AuthorEmail = c.Owner?.Email
                    }).ToList()
            };
        }
    }
}
