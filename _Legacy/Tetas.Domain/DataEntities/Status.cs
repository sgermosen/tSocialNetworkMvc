namespace Tetas.Domain.DataEntities
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        public string Name { get; set; }

        public string TableClass { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }
        [JsonIgnore]
        public ICollection<GroupMember> GroupMembers { get; set; }
    }
}
