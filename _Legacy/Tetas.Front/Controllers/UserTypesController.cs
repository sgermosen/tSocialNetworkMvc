using Tetas.Domain.DataEntities;

namespace Tetas.Front.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using Domain;

    [Authorize]
    public class UserTypesController : BaseController
    { 

        // GET: UserTypes
        public async Task<ActionResult> Index()
        {
            return View(await Db.UserTypes.ToListAsync());
        }

       
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserTypes/Create
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( UserType userType)
        {
            if (ModelState.IsValid)
            {
                Db.UserTypes.Add(userType);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(userType);
        }

        // GET: UserTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userType = await Db.UserTypes.FindAsync(id);
            if (userType == null)
            {
                return HttpNotFound();
            }
            return View(userType);
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( UserType userType)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(userType).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(userType);
        }

      
    }
}
