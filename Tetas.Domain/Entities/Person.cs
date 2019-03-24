using System;
using System.Collections.Generic;
using System.Text;

namespace Tetas.Domain.Entities
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        //[Required(ErrorMessage = "This field is required")]
        [MaxLength(15, ErrorMessage = "Max length is {1}")]
        [Display(Name = "DNI/RUC")]
        public string Rnc { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(50, ErrorMessage = "Max length is {1}")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(50, ErrorMessage = "Max length is {1}")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Born Date")]
        ////[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? BornDate { get; set; }

        [Display(Name = "Gender")]
        public int GenderId { get; set; }

        [Display(Name = "School Level")]
        public int? SchoolLevelId { get; set; }

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [MaxLength(50, ErrorMessage = "Max length is {1}")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(15, ErrorMessage = "Max length is {1}")]
        [DataType(DataType.PhoneNumber)]
        public string Tel { get; set; }

        [MaxLength(15, ErrorMessage = "Max length is {1}")]
        [DataType(DataType.PhoneNumber)]
        public string Cel { get; set; }

        [Display(Name = "Marital Situation")]
        public int MaritalSituationId { get; set; }

        [Display(Name = "Ocupation")]
        public int OcupationId { get; set; }

        [Display(Name = "Religion")]
        public int ReligionId { get; set; }

        // [Required(ErrorMessage = "This field is required")]
        [MaxLength(200, ErrorMessage = "Max length is {1}")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Imagen { get; set; }


        [JsonIgnore]
        public virtual Gender Gender { get; set; }
        [JsonIgnore]
        public virtual MaritalSituation MaritalSituation { get; set; }
        [JsonIgnore]
        public virtual Ocupation Ocupation { get; set; }
        [JsonIgnore]
        public virtual Religion Religion { get; set; }
        [JsonIgnore]
        public virtual Country Country { get; set; }
        [JsonIgnore]
        public virtual Status Status { get; set; }
        [JsonIgnore]
        public virtual SchoolLevel SchoolLevel { get; set; }

    }
}
