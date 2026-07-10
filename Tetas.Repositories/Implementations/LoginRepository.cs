namespace Tetas.Repositories.Implementations
{
    using Infraestructure;
    using Repositories.Contracts;
    using System.Threading.Tasks;
    using Tetas.Common.ViewModels;

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
