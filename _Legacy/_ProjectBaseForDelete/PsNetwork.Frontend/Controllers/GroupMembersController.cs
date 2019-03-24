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
    public class GroupMembersController : Controller
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

        // GET: GroupMembers/Delete/5
        public async Task<ActionResult> Join(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var groupMember = await db.GroupMembers.FindAsync(id);
            if (groupMember == null)
            {
                return HttpNotFound();
            }
            return View(groupMember);
        }

        // POST: GroupMembers/Delete/5
        [HttpPost, ActionName("Join")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JoinConfirmed(int id)
        {
            GroupMember groupMember = await db.GroupMembers.FindAsync(id);
            db.GroupMembers.Remove(groupMember);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: GroupMembers/Delete/5
        public async Task<ActionResult> Leave(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupMember groupMember = await db.GroupMembers.FindAsync(id);
            if (groupMember == null)
            {
                return HttpNotFound();
            }
            return View(groupMember);
        }

        // POST: GroupMembers/Delete/5
        [HttpPost, ActionName("Leave")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LeaveConfirmed(int id)
        {
            GroupMember groupMember = await db.GroupMembers.FindAsync(id);
            db.GroupMembers.Remove(groupMember);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Post(int id)
        {
            var userId = await GetUserId();
            var post = db.GroupPosts.Include(g => g.UserGroup).Where(p => p.UserGroupId == id);
            ViewBag.UserId = userId;

            ViewBag.UserGroupId = id;
            return View(await post.ToListAsync());
        }

        public async Task<ActionResult> Index()
        {
            var userId = await GetUserId();
            var groups = db.UserGroups.Include(g => g.User);
            ViewBag.UserId = userId;
            return View(await groups.ToListAsync());
        }

        // GET: GroupMembers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupMember groupMember = await db.GroupMembers.FindAsync(id);
            if (groupMember == null)
            {
                return HttpNotFound();
            }
            return View(groupMember);
        }

        public async Task<ActionResult> CreatePost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserGroup group = await db.UserGroups.FindAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            
            var groupPost = new GroupPost
            {
                UserId = group.UserId,
                UserGroupId = group.UserGroupId,
                PostDate = DateTime.Now.Date
            };

            return View(groupPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePost(GroupPost groupPost)
        {
            if (ModelState.IsValid)
            {
                db.GroupPosts.Add(groupPost);
                await db.SaveChangesAsync();
                return RedirectToAction(string.Format("Post/{0}", groupPost.UserGroupId));
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", groupPost.UserId);
            ViewBag.UserGroupId = new SelectList(db.UserGroups, "UserGroupId", "Name", groupPost.UserGroupId);
            return View(groupPost);
        }
        // GET: GroupMembers/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");
            ViewBag.UserGroupId = new SelectList(db.UserGroups, "UserGroupId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GroupMember groupMember)
        {
            if (ModelState.IsValid)
            {
                db.GroupMembers.Add(groupMember);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", groupMember.UserId);
            ViewBag.UserGroupId = new SelectList(db.UserGroups, "UserGroupId", "Name", groupMember.UserGroupId);
            return View(groupMember);
        }

        // GET: GroupMembers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupMember groupMember = await db.GroupMembers.FindAsync(id);
            if (groupMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", groupMember.UserId);
            ViewBag.UserGroupId = new SelectList(db.UserGroups, "UserGroupId", "Name", groupMember.UserGroupId);
            return View(groupMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GroupMember groupMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groupMember).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", groupMember.UserId);
            ViewBag.UserGroupId = new SelectList(db.UserGroups, "UserGroupId", "Name", groupMember.UserGroupId);
            return View(groupMember);
        }

        // GET: GroupMembers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupMember groupMember = await db.GroupMembers.FindAsync(id);
            if (groupMember == null)
            {
                return HttpNotFound();
            }
            return View(groupMember);
        }

        // POST: GroupMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GroupMember groupMember = await db.GroupMembers.FindAsync(id);
            db.GroupMembers.Remove(groupMember);
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
