namespace Tetas.Repositories.Implementations
{
    using Infraestructure;
    using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
    using Repositories.Contracts;
    using System.Threading.Tasks;

    public class LoginRepository : ILogin<LoginModel>
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
