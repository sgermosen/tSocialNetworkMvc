using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace Tetas.Front.Models
{
    using System.Web;
    using Domain.DataEntities;
    public class GroupView : Group
    {
        [Display(Name = "Imagen")]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}