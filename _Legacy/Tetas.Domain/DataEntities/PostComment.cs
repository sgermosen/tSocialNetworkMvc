namespace Tetas.Domain.DataEntities
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    
    public class PostComment
    {
        [Key]
        public int PostCommentId { get; set; }

        public int UserPostId { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime CommentDate { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ModifiedDate { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comment")]
        [Required(ErrorMessage = "The Field {0} is required")]
        public string StrComment { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual UserPost UserPost { get; set; }
    }
}
