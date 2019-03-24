namespace Tetas.Front.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public abstract class GenericController<T, TRepo>
        where T : class
        where TRepo : IBaseRepository, new()
    {
        private IBaseRepository repository;

        public GenericController()
        {
            repository = new TRepo();
        }

        public virtual ActionResult Details(int id)
        {
            var model = repository.Get<T>(id);
            return View(model);
        }
    }
}