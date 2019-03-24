using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PsNetwork.Domain;
using PsNetwork.Frontend.Models;

namespace PsNetwork.Frontend.Helpers
{
    public class UsersHelper : IDisposable
    {
        private static readonly ApplicationDbContext UserContext = new ApplicationDbContext();
        private static readonly DataContext Db = new DataContext();

    
        public static async Task<int> GetUserId(string email)
        {
            var user = await Db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            return user?.UserId ?? 0;
        }

        public void Dispose()
        {
            UserContext.Dispose();
            Db.Dispose();
        }
    }
}