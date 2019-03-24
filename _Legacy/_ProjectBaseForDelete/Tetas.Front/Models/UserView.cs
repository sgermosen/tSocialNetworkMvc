namespace Tetas.Front.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.ComponentModel.DataAnnotations.Schema;
    using Domain.DataEntities;

    [NotMapped]
    public class UserView : User
    {

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Email { get; set; }

        [Display(Name = "Imagen")]
        public HttpPostedFileBase ImageFile { get; set; }




    }
}