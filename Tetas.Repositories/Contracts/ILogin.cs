namespace Tetas.Repositories.Contracts
{
    using Domain.Entities;
    using System.Threading.Tasks;
    using Tetas.Common.ViewModels;

    public interface ILogin <LoginModel>
    {
        Task<bool> IsValidLogin(long id);
    }
     
}
