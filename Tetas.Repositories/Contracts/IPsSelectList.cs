namespace Tetas.Repositories.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tetas.Domain.Entities;

    public interface IPsSelectList

    {
        Task<List<Privacy>> GetListPrivacies();

        Task<List<GroupType>> GetListGroupTypes();

        Task<Privacy> GetPrivacyAsync(long id);

        Task<GroupType> GetGroupTypeAsync(long id);
    }
}
