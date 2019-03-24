using Tetas.Domain.DataEntities;

namespace Tetas.Front.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using Domain;

    public class PrivaciesController : BaseController
    {
        
        // GET: Privacies
        public async Task<ActionResult> Index()
        {
            return View(await Db.Privacies.ToListAsync());
        }

        // GET: Privacies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var privacy = await Db.Privacies.FindAsync(id);
            if (privacy == null)
            {
                return HttpNotFound();
            }
            return View(privacy);
        }

        // GET: Privacies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Privacies/Create
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Privacy privacy)
        {
            if (ModelState.IsValid)
            {
                Db.Privacies.Add(privacy);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(privacy);
        }

        // GET: Privacies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var privacy = await Db.Privacies.FindAsync(id);
            if (privacy == null)
            {
                return HttpNotFound();
            }
            return View(privacy);
        }

        // POST: Privacies/Edit/5
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( Privacy privacy)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(privacy).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(privacy);
        }

        // GET: Privacies/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var privacy = await Db.Privacies.FindAsync(id);
            if (privacy == null)
            {
                return HttpNotFound();
            }
            return View(privacy);
        }

        // POST: Privacies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var privacy = await Db.Privacies.FindAsync(id);
            if (privacy != null) Db.Privacies.Remove(privacy);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        
    }
}
