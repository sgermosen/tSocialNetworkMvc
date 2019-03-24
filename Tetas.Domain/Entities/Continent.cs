namespace Tetas.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;
    public class Continent
    {
        [Key]
        public int ContinentId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(5, ErrorMessage = "Max length is {1}")]
        [Index("Continent_Code_Index", IsUnique = true)]
        public string Code { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(50, ErrorMessage = "Max length is {1}")]
        [Index("Continent_Name_Index", IsUnique = true)]
        [Display(Name = "Continent")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(25, ErrorMessage = "Max length is {1}")]
        [Index("Continent_Demonym_Index", IsUnique = true)]
        public string Demonym { get; set; }

        [JsonIgnore]
        public virtual ICollection<Country> Countries { get; set; }
    }
}
