namespace Tetas.Repositories.Implementations
{
    using System.Threading.Tasks;
    using Domain.Entities;
    using Infraestructure;
    using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
    using Repositories.Contracts;

    public class LoginRepository:ILogin<LoginModel>
    {
        public LoginRepository(ApplicationDbContext context) 
        {
        }

        public Task<bool> IsValidLogin(long id)
        {
            throw new System.NotImplementedException();
        }
    }
     
}
