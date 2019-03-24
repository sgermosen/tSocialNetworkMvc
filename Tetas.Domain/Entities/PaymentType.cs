namespace Tetas.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PaymentType
    {
        [Key]
        public int PaymentTypeId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(10, ErrorMessage = "Max length is {1}")]
        [Index("PaymentType_Code_Index", IsUnique = true)]
        public string Code { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(100, ErrorMessage = "Max length is {1}")]
        [Index("PaymentType_Name_Index", IsUnique = true)]
        [Display(Name = "Payment Type")]
        public string Name { get; set; }
        
    }
}
