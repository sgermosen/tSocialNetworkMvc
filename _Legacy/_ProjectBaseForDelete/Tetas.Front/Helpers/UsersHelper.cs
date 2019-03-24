namespace Tetas.Front.Helpers
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Web.Configuration;
    using Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class UsersHelper : IDisposable
    {

        //private static UsersHelper _instance;

        //public static UsersHelper GetInstance()
        //{
        //    return _instance ?? (_instance = new UsersHelper());
        //}

        //public UsersHelper()
        //{
        //    _instance = this;
        //}

        private static readonly ApplicationDbContext UserContext = new ApplicationDbContext();
        private static readonly LocalDataContext Db = new LocalDataContext();

        public static bool DeleteUser(string userName, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(UserContext));
            var userAsp = userManager.FindByEmail(userName);
            if (userAsp == null)
            {
                return false;
            }

            var response = userManager.RemoveFromRole(userAsp.Id, roleName);
            return response.Succeeded;
        }

        public static bool UpdateUserName(string currentUserName, string newUserName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(UserContext));
            var userAsp = userManager.FindByEmail(currentUserName);
            if (userAsp == null)
            {
                return false;
            }

            userAsp.UserName = newUserName;
            userAsp.Email = newUserName;
            var response = userManager.Update(userAsp);
            return response.Succeeded;
        }

        public static void CheckRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(UserContext));

            // Check to see if Role Exists, if not create it
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }
        }

        public static void CheckSuperUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(UserContext));
            var email = "sgrysoft@gmail.com";// WebConfigurationManager.AppSettings["AdminUser"];
            var password = "824455";// WebConfigurationManager.AppSettings["AdminPassWord"];
            var userAsp = userManager.FindByName(email);
            if (userAsp == null)
            {
                 CreateUserAsp(email, "Admin", password);
                return;
            }

            userManager.AddToRole(userAsp.Id, "Admin");
        }

        public static void CreateUserAsp(string email, string roleName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(UserContext));
            var userAsp = userManager.FindByEmail(email);
            if (userAsp == null)
            {
                userAsp = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                };

                userManager.Create(userAsp, email);
            }

            userManager.AddToRole(userAsp.Id, roleName);
        }

        public static void CreateUserAsp(string email, string roleName, string password)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(UserContext));

            var userAsp = new ApplicationUser
            {
                Email = email,
                UserName = email,
            };

            var result = userManager.Create(userAsp, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(userAsp.Id, roleName);
            }
        }

        public static string CreateUserAspBackId(string email, string roleName, string password)
        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(UserContext));

            var userAsp = new ApplicationUser
            {
                Email = email,
                UserName = email,
            };

            var result = userManager.Create(userAsp, password);
            if (result.Succeeded)
            {
                userManager.AddToRole(userAsp.Id, roleName);

            }
            return userAsp.Id;
        }

        //public static async Task<int> GetAuthorId(string email)
        //{
        //    var user = await Db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        //    return user?.AuthorId ?? 0;
        //}

        //public static async Task<List<UserRol>> GetUserRols(int userid)
        //{
        //    var userRols = await Db.UserRols
        //        .Where(p => p.UserId == userid)
        //        .ToListAsync();
        //    return userRols;

        //}

        //public static async Task<bool> IsAdmin(int userId)
        //{
        //    var userRols = await GetUserRols(userId);
        //    return userRols.Any(value => value.Rol.Level <= 2);
        //}

        //public static async Task<bool> IsSupervisor(int userId)
        //{
        //    var userRols = await GetUserRols(userId);
        //    return userRols.Any(value => value.Rol.Level <= 3);
        //}

        //public static async Task<bool> IsCashierOrUser(int userId)
        //{
        //    var userRols = await GetUserRols(userId);
        //    return userRols.Any(value => value.Rol.Level == 4);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="optionName"></param>
        /// <param name="action"> 1 index, 2 details, 3 create, 4 edit, 5 delete</param>
        /// <returns></returns>
        //public static async Task<bool> HavePermisionToAction(int userId, string optionName, int action)
        //{

        //    var userRols = await GetUserRols(userId);

        //    foreach (var rol in userRols)
        //    {
        //        foreach (var option in rol.Rol.OptionRols)
        //        {
        //            if (option.Option.Name == optionName)
        //            {
        //                switch (action)
        //                {
        //                    case 1:
        //                        if (option.Index)
        //                        {
        //                            return true;
        //                        }
        //                        break;
        //                    case 2:
        //                        if (option.Details)
        //                        {
        //                            return true;
        //                        }
        //                        break;
        //                    case 3:
        //                        if (option.Create)
        //                        {
        //                            return true;
        //                        }
        //                        break;
        //                    case 4:
        //                        if (option.Edit)
        //                        {
        //                            return true;
        //                        }
        //                        break;
        //                    case 5:
        //                        if (option.Delete)
        //                        {
        //                            return true;
        //                        }
        //                        break;

        //                }
        //            }
        //        }
        //    }
        //    //TODO: temporalmente se le asigna true para pruebas
        //    return true;
        //}

        /// <summary>
        /// Real this is a min value 0 root, 1 ultra admin (just praysoft) 2, admin, 3 supervisor, 4 user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public static async Task<int> MaxLevel(int userId)
        //{
        //    var userRols = await GetUserRols(userId);
        //    return userRols.Select(value => value.Rol.Level).Concat(new[] {10}).Min();
        //}

        //public static  List GetUserRols(int userid)
        //{
        //    var query = from pro in Db.UserRols.Where(p => p.UserId == userid)
        //               select new { pro.RolId, pro.Rol.Level };

        //    return query.ToList();
        //}

        //public static async Task<string> GetUserId(string email)
        //{
        //    var user = await Db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

        //    return user?.UserId ?? 0;
        //}

        //public static async Task PasswordRecovery(string email)
        //{
        //    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(UserContext));
        //    var userAsp = userManager.FindByEmail(email);
        //    if (userAsp == null)
        //    {
        //        return;
        //    }

        //    var random = new Random();
        //    var newPassword = $"{random.Next(100000, 999999)}";
        //    var response = await userManager.AddPasswordAsync(userAsp.Id, newPassword);
        //    if (response.Succeeded)
        //    {
        //        var subject = "Torneo Predicciones App - Recuperación de contraseña";
        //        var body = $@"
        //            <h1>Torneo Predicciones - Recuperación de contraseña</h1>
        //            <p>Su nueva contraseña es: <strong>{newPassword}</strong></p>
        //            <p>Por favor no olvide cambiarla por una de fácil recordación";

        //        await Emails.SendMail(email, subject, body);
        //    }
        //}

        public void Dispose()
        {
            UserContext.Dispose();
            Db.Dispose();
        }
    }
}