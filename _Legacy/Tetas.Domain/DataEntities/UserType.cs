namespace Tetas.Domain.DataEntities
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class UserType
    {
        [Key]
        public int UserTypeId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }

    }
}
