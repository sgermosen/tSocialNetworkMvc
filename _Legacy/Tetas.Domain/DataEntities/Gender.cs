namespace Tetas.Domain.DataEntities
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    public class Gender
    {
        [Key]
        public int GenderId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }
    }
}
