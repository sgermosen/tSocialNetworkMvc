namespace Tetas.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;
    public class Ocupation
    {
        [Key]
        public int OcupationId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(50, ErrorMessage = "Max length is {1}")]
        [Index("Ocupation_Name_Index", IsUnique = true)]
        [Display(Name = "Ocupation")]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Person> Persons { get; set; }
    }
}
