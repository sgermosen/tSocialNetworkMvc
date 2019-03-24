namespace Tetas.Front.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using Models;
    using Tools;
    using Domain.Repositories;
    using Domain.DataEntities;

    public class GroupsController : BaseController
    {
        private GroupRepository _repository;

        protected override void Seed()
        {
            _repository = new GroupRepository(Db);
            base.Seed();
        }

        public async Task<ActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id ))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //TODO: verified this
            //var groups = Db.Groups.Include(g => g.GroupType).Include(g => g.User)
            //    .Where(p => p.UserId == (int)id);

            var groups = await _repository.FindByClause(p => p.UserId == id);

            return View(groups);
        }

        public async Task<ActionResult> IndexRequest(int id)
        {
            var userId = await GetUserId();
            ViewBag.ConectedUserId = userId;

            var member = _repository.GetMembers(p => p.GroupId == id);

            return View(member);
        }

        public async Task<ActionResult> IndexAll()
        {
            var userId = await GetUserId();
            ViewBag.ConectedUserId = userId;

            var groups = await _repository.FindByClause();

            return View(groups);
        }

        public async Task<ActionResult> Apply(int id)
        {
            var userId = await GetUserId();

            var group = await _repository.FindById(id);

            if (group == null)
            {
                return View("Error");
            }

            var member = new GroupMember
            {
                UserId = userId,
                StatusId = 6,
                GroupId = id,
                ApplicationDate = DateTime.Now.ToUniversalTime()
            };

            await _repository.AddMemberAsync(member);

            return RedirectToAction("IndexAll");
        }

        public async Task<ActionResult> ManageUsers(string id, int statusid, int groupid)
        {
            //var member = await Db.GroupMembers.FirstOrDefaultAsync(p => p.UserId == id && p.GroupId == groupid);
            var member = await _repository.GetMemberByClause(
                p => p.UserId == id && p.GroupId == groupid);

            if (member == null)
            {
                return HttpNotFound();
            }

            member.StatusId = statusid;

            return RedirectToAction($"Details/{groupid}");
        }

        public async Task<ActionResult> Details(int id)
        {
            var userId = await GetUserId();
            var group = await _repository.FindById(id);

            if (group == null)
            {
                return HttpNotFound();
            }

            ViewBag.ConectedUserId = userId;
            return View(group);
        }

        public ActionResult Create(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.GroupTypeId = new SelectList(Db.GroupTypes, "GroupTypeId", "Name");
            ViewBag.PrivacyId = new SelectList(Db.Privacies, "PrivacyId", "Name");

            var newGroup = new GroupView { UserId = id };
            return View(newGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(GroupView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                const string folder = "~/Content/GroupPics";

                if (view.ImageFile != null)
                {
                    pic = Files.UploadPhoto(view.ImageFile, folder, "");
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var group = new Group
                {
                    CreationDate = DateTime.Today.ToUniversalTime(),
                    GroupId = view.GroupId,
                    UserId = view.UserId,
                    GroupTypeId = view.GroupTypeId,
                    Link = view.Link,
                    Name = view.Name
                };

                group.Picture = pic;

                await _repository.AddAsync(group);

                var member = new GroupMember
                {
                    UserId = view.UserId,
                    GroupId = group.GroupId,
                    ApplicationDate = DateTime.Today.ToUniversalTime(),
                    StatusId = 1
                };

                await _repository.AddMemberAsync(member);

                return RedirectToAction($"Index/{view.UserId}");
            }

            ViewBag.GroupTypeId = new SelectList(Db.GroupTypes, "GroupTypeId", "Name", view.GroupTypeId);
            return View(view);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var group = await _repository.FindById(id);

            if (group == null)
            {
                return HttpNotFound();
            }

            ViewBag.GroupTypeId = new SelectList(Db.GroupTypes, "GroupTypeId", "Name", group.GroupTypeId);
            ViewBag.UserId = new SelectList(Db.Users, "UserId", "Email", group.UserId);

            return View(group);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Group group)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(group);
                return RedirectToAction("Index");
            }

            ViewBag.GroupTypeId = new SelectList(Db.GroupTypes, "GroupTypeId", "Name", group.GroupTypeId);
            ViewBag.UserId = new SelectList(Db.Users, "UserId", "Email", group.UserId);
            return View(group);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var group = await _repository.FindById(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var group = await _repository.FindById(id);
            await _repository.Delete(group);
            return RedirectToAction("Index");
        }

    }
}
