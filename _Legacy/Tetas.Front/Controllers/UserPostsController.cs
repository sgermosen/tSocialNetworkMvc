namespace Tetas.Front.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Domain.Repositories;
    using Domain.DataEntities;

    public class UserPostsController : BaseController
    {
        private readonly PostRepository _repository;
        private readonly CommentRepository _cRepository;

        public UserPostsController()
        {
            _repository = new PostRepository(Db);
            _cRepository = new CommentRepository(Db);
        }
        protected override void Seed()
        {
           
            base.Seed();
        }

        #region Comments

        public async Task<ActionResult> CreateComment(int id)
        {
            var userId = await GetUserId();

            var userPost = await _repository.FindById(id);
            if (userPost == null)
            {
                return HttpNotFound();
            }

            var comment = new PostComment
            {
                UserPostId = id,
                UserId = userId
            };

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateComment(PostComment comment)
        {
            if (ModelState.IsValid)
            {
                comment.CommentDate = DateTime.Now.ToUniversalTime();
                await _cRepository.AddAsync(comment);               
                return RedirectToAction("Index", "Home");
            }
            return View(comment);
        }

        public async Task<ActionResult> EditComment(int id)
        {
            var userId = await GetUserId();

            var comment = await _cRepository.FindById(id);
            if (comment == null)
            {
                return View("Error");
            }
            //supposily the user than is editing is the logged user, but let's validate 
            return comment.UserId != userId ? View("Error") : View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditComment(PostComment comment)
        {
            if (!ModelState.IsValid) return View(comment);
            comment.ModifiedDate = DateTime.Now.ToUniversalTime();
            await _cRepository.UpdateAsync(comment);
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> DeleteComment(int id)
        {
            //  var userPost = await db.UserPosts.FindAsync( id);

            var comment = await _cRepository.FindById(id);
            if (comment == null)
            {
                return View("Error");
            }

            await _cRepository.Delete(comment);

            return RedirectToAction("Index", "Home");

        }

        #endregion

        #region Post
                     
        public async Task<ActionResult> Index()
        {
            var userPosts = await _repository.FindByClause();

            return View( userPosts);
        }

        public async Task<ActionResult> Details(int id)
        {             
            var userPost = await _repository.FindById(id);  
            if (userPost == null)
            {
                return HttpNotFound();
            }
            return View(userPost);
        }

        public async Task<ActionResult> Create()
        {
            var userId = await GetUserId();
            var userPost = new UserPost { UserId = userId };
            return View(userPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserPost userPost)
        {
            if (ModelState.IsValid)
            {
                userPost.PostDate = DateTime.Now.ToUniversalTime();
                
                await _repository.AddAsync(userPost);
               
                return RedirectToAction("Index", "Home");
            }

            ViewBag.UserId = new SelectList(Db.Users, "UserId", "Email", userPost.UserId);
            return View(userPost);
        }

        public async Task<ActionResult> Edit(int id)
        {           
            var userId = await GetUserId();
            var userPost = await _repository.FindById(id);
            if (userPost == null)
            {
                return HttpNotFound();
            }

            userPost.UserId = userId;

             return View(userPost);
        }
          
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserPost userPost)
        {
            if (!ModelState.IsValid) return View(userPost);
         
            await _repository.UpdateAsync(userPost);
            
            return RedirectToAction("Index", "Home");
        }
       
        public async Task<ActionResult> Delete(int id)
        {
            //  var userPost = await db.UserPosts.FindAsync( id);          
            //var userPost = await  Db.UserPosts.Include(p=>p.PostComments).FirstOrDefaultAsync(p=>p.UserPostId==id);

            var userPost = await _repository.GetByClause (p => p.UserPostId == id);

            if (userPost == null)
            {
                return View("Error");

            }

            foreach (var comment in userPost.PostComments.ToList())
            {
                try
                {
                    //if (userPost.PostComments.All(c => c.PostCommentId != comment.PostCommentId))
                    // db.PostComments.Remove(comment);
                    // await Db.PostComments.FindAsync(comment.PostCommentId);
                    var commToDelete =      await _cRepository.FindById(comment.PostCommentId);

                     if (commToDelete != null) await _cRepository.Delete(commToDelete);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
               
            }

            await _repository.Delete(userPost);
            //try
            //{
            //    await Db.SaveChangesAsync();
            //}
            //catch (Exception)
            //{
            //    return View("Error");
            //}

            //Db.UserPosts.Remove(userPost);

            //try
            //{
            //    await Db.SaveChangesAsync();
            //}
            //catch (Exception)
            //{
            //    return View("Error");
            //}

            return RedirectToAction("Index", "Home");           
          
        }


        #endregion       
    }
}
