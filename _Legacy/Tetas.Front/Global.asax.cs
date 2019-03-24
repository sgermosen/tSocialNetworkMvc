using Tetas.Front.Helpers;

namespace Tetas.Front
{
    using System.Data.Entity;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Models.LocalDataContext,
                Migrations.Configuration>());
            CheckRolesAndSuperUser();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CheckRolesAndSuperUser()
        {
            UsersHelper.CheckRole("Admin");
            UsersHelper.CheckRole("User");

            //UsersHelper.CheckRole("Administrador");
            //UsersHelper.CheckRole("Medico");
            //UsersHelper.CheckRole("Cajero");
            //UsersHelper.CheckRole("Supervisor");
            //UsersHelper.CheckRole("Promotor");
            //UsersHelper.CheckRole("Callcenter");
            //UsersHelper.CheckRole("Secretaria");
            //UsersHelper.CheckRole("Tecnico");
            //UsersHelper.CheckRole("Tesorero");
            //   [Authorize(Roles = "Admin,Administrador,Medico,Cajero,Supervisor,Promotor,Callcenter,Secretaria,Tecnico")]
            UsersHelper.CheckSuperUser();
        }
    }
}
