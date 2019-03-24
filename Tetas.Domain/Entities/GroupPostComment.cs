namespace Tetas.Domain.Entities
{
    using Domain.Helpers;
    using System;

    public class GroupPostComment : BaseEntity, IBaseEntity
    {        

        public string Body { get; set; }
              
        public DateTime CreationDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public ApplicationUser Owner { get; set; }

        public GroupPost Post { get; set; }
        
    }
}
