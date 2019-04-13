using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tetas.Domain.Entities;

namespace Tetas.Web.Models
{
    public class GroupPostViewModel : GroupPost
    {
       public long GroupId { get; set; }

        public string OwnerId { get; set; }

        public bool IsAdmin { get; set; }
    }
}
