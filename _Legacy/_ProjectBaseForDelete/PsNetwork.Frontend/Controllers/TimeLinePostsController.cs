using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PsNetwork.Domain;
using PsNetwork.Frontend.Helpers;
using PsNetwork.Frontend.Models;

namespace PsNetwork.Frontend.Controllers
{
    public class TimeLinePostsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        public async Task<int> GetUserId()
        {
            //if (Session["UserId"] != null && Convert.ToInt32(Session["UserId"]) != 0) return Convert.ToInt32(Session["UserId"]);
            var manager =
                new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            // Session["UserId"] = await UsersHelper.GetUserId(currentUser.Email);
            return await UsersHelper.GetUserId(currentUser.Email);
            //return Convert.ToInt32(Session["UserId"]);
        }


        public async Task<ActionResult> Comment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var timeLinePost = await db.TimeLinePosts.FindAsync(id);
            if (timeLinePost == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");

            var comment = new Comment();
            comment.TimeLinePostId = (int) id;
            return View(comment);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                //db.Entry(comment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", comment.UserId);
            return View(comment);
        }

        public async Task<ActionResult> Index()
        {
            var userid = await GetUserId();
            var timeLinePosts =   db.TimeLinePosts.Include(t => t.User);
            return View(await timeLinePosts.ToListAsync());
        }

        // GET: TimeLinePosts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeLinePost timeLinePost = await db.TimeLinePosts.FindAsync(id);
            if (timeLinePost == null)
            {
                return HttpNotFound();
            }
            return View(timeLinePost);
        }

        // GET: TimeLinePosts/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");
            return View();
        }

        // POST: TimeLinePosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( TimeLinePost timeLinePost)
        {
            if (ModelState.IsValid)
            {
                db.TimeLinePosts.Add(timeLinePost);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", timeLinePost.UserId);
            return View(timeLinePost);
        }

        
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeLinePost timeLinePost = await db.TimeLinePosts.FindAsync(id);
            if (timeLinePost == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", timeLinePost.UserId);
            return View(timeLinePost);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TimeLinePostId,UserId,PostStr")] TimeLinePost timeLinePost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeLinePost).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", timeLinePost.UserId);
            return View(timeLinePost);
        }

        // GET: TimeLinePosts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeLinePost timeLinePost = await db.TimeLinePosts.FindAsync(id);
            if (timeLinePost == null)
            {
                return HttpNotFound();
            }
            return View(timeLinePost);
        }

        // POST: TimeLinePosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TimeLinePost timeLinePost = await db.TimeLinePosts.FindAsync(id);
            db.TimeLinePosts.Remove(timeLinePost);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
