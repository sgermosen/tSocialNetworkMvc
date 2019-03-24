namespace Tetas.Domain.DataEntities
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Privacy
    {
        [Key]
        public int PrivacyId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Group> Groups { get; set; }

    }
}
