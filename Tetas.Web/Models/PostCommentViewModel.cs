using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tetas.Domain.Entities;

namespace Tetas.Web.Models
{
    public class PostCommentViewModel:PostComment
    {
       public long PostId { get; set; }

        public string OwnerId { get; set; }
    }
}
