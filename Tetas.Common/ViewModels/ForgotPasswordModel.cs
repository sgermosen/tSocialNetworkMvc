namespace Tetas.Common.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
