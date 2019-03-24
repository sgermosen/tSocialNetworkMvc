using Tetas.Domain.DataEntities;

namespace Tetas.Front.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using Domain;

    public class GroupTypesController : BaseController
    { 

        // GET: GroupTypes
        public async Task<ActionResult> Index()
        {
            return View(await Db.GroupTypes.ToListAsync());
        }

        // GET: GroupTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var groupType = await Db.GroupTypes.FindAsync(id);
            if (groupType == null)
            {
                return HttpNotFound();
            }
            return View(groupType);
        }

        // GET: GroupTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GroupTypes/Create
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( GroupType groupType)
        {
            if (ModelState.IsValid)
            {
                Db.GroupTypes.Add(groupType);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(groupType);
        }

        // GET: GroupTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var groupType = await Db.GroupTypes.FindAsync(id);
            if (groupType == null)
            {
                return HttpNotFound();
            }
            return View(groupType);
        }

        // POST: GroupTypes/Edit/5
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( GroupType groupType)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(groupType).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(groupType);
        }

        // GET: GroupTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var groupType = await Db.GroupTypes.FindAsync(id);
            if (groupType == null)
            {
                return HttpNotFound();
            }
            return View(groupType);
        }

        // POST: GroupTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var groupType = await Db.GroupTypes.FindAsync(id);
            if (groupType != null) Db.GroupTypes.Remove(groupType);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

       
    }
}
