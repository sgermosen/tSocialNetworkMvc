namespace Tetas.Domain.DataEntities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class GroupMember
    {
        [Key]
        public int GroupMemberId { get; set; }

        public int GroupId { get; set; }

        public string UserId { get; set; }

        public int StatusId { get; set; }

        public DateTime ApplicationDate { get; set; }

        [JsonIgnore]
        public virtual  Group Group { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual Status Status { get; set; }

    }
}
