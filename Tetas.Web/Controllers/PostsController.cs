namespace Tetas.Web.Controllers
{
    using Domain.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Repositories.Contracts;
    using System;
    using System.Threading.Tasks;

    public class PostsController : Controller
    {
        private readonly IPost _postRepository;
        private readonly IUserHelper _userHelper;

        public PostsController(IPost postRepository, IUserHelper userHelper)
        {
            _postRepository = postRepository;
            _userHelper = userHelper;
        }
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //public OtherClass(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        //public void YourMethodName()
        //{
        //    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    // or
        //    var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //}
        public async Task<IActionResult> Index()
        {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value // will give the user's userId
            //var userName = User.FindFirst(ClaimTypes.Name).Value // will give the user's userName
            //var userEmail = User.FindFirst(ClaimTypes.Email).Value // will give the user's Email
            ViewBag.Email = User.Identity.Name;
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            ViewBag.MyPost = _postRepository.GetPostWithComments(user.Id);

            return View();
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postRepository.FindByIdAsync(id.Value);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }
                post.Owner = user;
                post.Date = DateTime.UtcNow;

                await _postRepository.AddAsync(post);

                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postRepository.FindByIdAsync(id.Value);

            if (post == null)
            {
                return NotFound();
            }

            if (post.Owner.Email != User.Identity.Name)
            {
                return Unauthorized();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                post.UpdatedDate = DateTime.UtcNow;

                try
                {
                    await _postRepository.UpdateAsync(post);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _postRepository.FindByIdAsync(id.Value);

            if (post == null)
            {
                return NotFound();
            }
            if (post.Owner.Email != User.Identity.Name)
            {
                return Unauthorized();
            }

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var post = await _postRepository.FindByIdAsync(id);
            await _postRepository.DeleteAsync(post);
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(long id)
        {
            return _postRepository.Exists(id);
        }
    }
}