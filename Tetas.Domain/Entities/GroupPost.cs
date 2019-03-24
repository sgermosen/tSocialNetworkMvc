 using System.Collections.Generic;

namespace Tetas.Domain.Entities
{
    using Helpers;
    using System;

    public class GroupPost : BaseEntity, IBaseEntity
    {        

        public string Body { get; set; }
              
        public DateTime CreationDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public ApplicationUser Owner { get; set; }

        public Group Group { get; set; }

        public ICollection<GroupPostComment> GroupPostComments { get; set; }

    }
}
