namespace Tetas.Domain.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataEntities;

    //repository from contry, than receive a country object, 
    //which force the implementation of the generic interface irepository 
    public interface IGroup : IRepository<Group, int>
    {
        Task<List<GroupMember>> GetMembers(Func<GroupMember, bool> selector = null);

        Task<GroupMember> AddMemberAsync(GroupMember entity);

        Task<GroupMember> GetMemberByClause(Func<GroupMember, bool> selector = null);
    }
     
}
