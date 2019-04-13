using System.Collections.Generic;

namespace Tetas.Domain.Entities
{
    using Helpers;
    using System;

    public class Group : BaseEntity, IBaseEntity
    {

        public string Link { get; set; }

        public string PictureUrl { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public ApplicationUser Owner { get; set; }

        public GroupType Type { get; set; }

        public Privacy Privacy { get; set; }

        public ICollection<GroupPost> GroupPosts { get; set; }

        public ICollection<GroupMember> GroupMembers { get; set; }

    }
}
