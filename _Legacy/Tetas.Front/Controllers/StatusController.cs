using Tetas.Domain.DataEntities;

namespace Tetas.Front.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using Domain; 

    [Authorize]
    public class StatusController : BaseController
    {
        
        // GET: Status
        public async Task<ActionResult> Index()
        {
            return View(await Db.Status.ToListAsync());
        }

         

        // GET: Status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Status/Create
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Status status)
        {
            if (ModelState.IsValid)
            {
                Db.Status.Add(status);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(status);
        }

        // GET: Status/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var status = await Db.Status.FindAsync(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // POST: Status/Edit/5
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Status status)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(status).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(status);
        }

        
        
    }
}
