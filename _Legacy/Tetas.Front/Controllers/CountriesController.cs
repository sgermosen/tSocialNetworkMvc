namespace Tetas.Front.Controllers
{
    using Domain.Repositories;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Domain.DataEntities;

    public class CountriesController : BaseController
    {
        private readonly CountryRepository _repository;

        protected override void Seed()
        {
            base.Seed();
        }

        public CountriesController()
        {
            _repository = new CountryRepository(Db);
        }

        // GET: Countries
        public async Task<ActionResult> Index()
        {
            var countries = await _repository.FindByClause();
            return View(countries);
        }

        // GET: Countries/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var country = await _repository.FindById(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        public ActionResult Create()
        {
            return View();
        }      
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Country country)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(country);
                //db.Countries.Add(country);
                //await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(country);
        }

        public async Task<ActionResult> Edit(int id)
        {
             
            var country = await _repository.FindById(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(country);
                return RedirectToAction("Index");
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            
            var country = await _repository.FindById(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var country =  await _repository.FindById(id);
            await _repository.Delete(country);
            return RedirectToAction("Index");
        }

       
    }
}
