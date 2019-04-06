using Tetas.Domain.Entities;

namespace Tetas.Repositories.Contracts
{
    using Domain.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IPsSelectList
        
    {

        Task<List<Privacy>> GetListPrivacies();

        Task<List<GroupType>> GetListGroupTypes();

        Task<Privacy> GetPrivacyAsync(long id);

        Task<GroupType> GetGroupTypeAsync(long id);
    }
}
