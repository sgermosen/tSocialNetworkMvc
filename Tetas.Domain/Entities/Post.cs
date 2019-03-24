using System;
using System.Collections.Generic;
using System.Text;
using Tetas.Domain.Helpers;

namespace Tetas.Domain.Entities
{
    public class Post:   BaseEntity, IBaseEntity
    {
        //[AllowHtml]
        //[DataType(DataType.MultilineText)]
        public string Body { get; set; }

       // [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public ApplicationUser Owner { get; set; }

        //public Privacy Privacy { get; set; }

        public IEnumerable<PostComment> PostComments { get; set; }

    }
}
