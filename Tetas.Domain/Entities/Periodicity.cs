namespace Tetas.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Periodicity
    {
        [Key]
        public int PeriodicityId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Max length is {1}")]
        [Index("Periodicity_Code_Index", IsUnique = true)]
        public string Code { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Max length is {1}")]
        [Index("Periodicity_Name_Index", IsUnique = true)]
        [Display(Name = "Periodicity")]
        public string Name { get; set; }
        
       
    }
}
