namespace Tetas.Domain.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        public int GroupTypeId { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public string Picture { get; set; }

        public int? PrivacyId { get; set; }

        public DateTime CreationDate { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        [JsonIgnore]
        public virtual GroupType GroupType { get; set; }
        [JsonIgnore]
        public virtual Privacy Privacy { get; set; }
        [JsonIgnore]
        public ICollection<GroupMember> GroupMembers { get; set; }

    }
}
