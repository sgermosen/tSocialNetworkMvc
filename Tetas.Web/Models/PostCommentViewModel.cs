namespace Tetas.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Tetas.Domain.Entities;

    public class PostCommentViewModel  
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long PostId { get; set; }

        public string OwnerId { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime Date { get; set; } 

    }
}
