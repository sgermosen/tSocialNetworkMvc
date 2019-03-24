namespace Tetas.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;
    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        public int ContinentId { get; set; }

        //  [Required(ErrorMessage = "This field is required")]
        [MaxLength(5, ErrorMessage = "Max length is {1}")]
        [Index("Country_Code_Index", IsUnique = true)]
        public string Code { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(50, ErrorMessage = "Max length is {1}")]
        [Index("Country_Name_Index", IsUnique = true)]
        [Display(Name = "Country")]
        public string Name { get; set; }

        //  [Required(ErrorMessage = "This field is required")]
        [MaxLength(25, ErrorMessage = "Max length is {1}")]
        [Index("Country_Demonym_Index", IsUnique = true)]
        public string Demonym { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Imagen { get; set; }

        [JsonIgnore]
        public virtual Continent Continent { get; set; }

        [JsonIgnore]
        public virtual ICollection<Person> Persons { get; set; }
    }
}
