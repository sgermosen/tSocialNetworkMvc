namespace Tetas.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(10, ErrorMessage = "Max length is {1}")]
        [Index("Currency_Code_Index", IsUnique = true)]
        public string Code { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Max length is {1}")]
        [Index("Currency_Name_Index", IsUnique = true)]
        [Display(Name = "Currency")]
        public string Name { get; set; }

         
    }
}
