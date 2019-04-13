namespace Tetas.Domain.Entities
{
    using Domain.Helpers;
    using System;

    public class GroupMember : BaseEntity, IBaseEntity
    {

        public DateTime ApplicationDate { get; set; }

        public ApplicationUser User { get; set; }

        public Group Group { get; set; }

        public bool State { get; set; }

        public bool Banned { get; set; }

        public bool Applied { get; set; }

        public MemberType MemberType { get; set; }

    }
}
