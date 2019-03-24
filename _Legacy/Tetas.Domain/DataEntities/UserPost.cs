namespace Tetas.Domain.DataEntities
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserPost
    {
        [Key]
        public int UserPostId { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }

        [Display(Name="Head Post")]
        [Required(ErrorMessage="The Field {0} is required")]
        public string HeadPost { get; set; }
        
        [DataType(DataType.MultilineText)]
        [Display(Name = "Post")]
        [Required(ErrorMessage = "The Field {0} is required")]
        public string StrPost { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public ICollection<PostComment> PostComments { get; set; }
    }
}
