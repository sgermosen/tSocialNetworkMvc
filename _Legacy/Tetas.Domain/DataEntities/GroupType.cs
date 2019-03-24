namespace Tetas.Domain.DataEntities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class GroupType
    {
        [Key]
        public int GroupTypeId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual  ICollection<Group> Groups { get; set; }

    }
}
