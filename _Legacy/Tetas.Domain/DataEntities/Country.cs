namespace Tetas.Domain.DataEntities
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        public string Name { get; set; }

        public string Denomym { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }
    }
}
