namespace Tetas.Domain.Contracts
{
    using DataEntities;

    //repository from contry, than receive a country object, 
    //which force the implementation of the generic interface irepository 
    public interface IUser : IRepository<User, int>
    {
       
    }
     
}
