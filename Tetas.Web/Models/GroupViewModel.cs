using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tetas.Web.Models
{
    using Domain.Entities;

    public class GroupViewModel:Group
    {
        [Display(Name = "Group")]
        [Range(1, 10, ErrorMessage = "You must select a Group.")]
        public long TypeId { get; set; }

        [Display(Name = "Privacy")]
        [Range(1, 10, ErrorMessage = "You must select a Privacy.")]
        public long PrivacyId { get; set; }

        public IEnumerable<SelectListItem> Privacies { get; set; }
        public IEnumerable<SelectListItem> GroupTypes { get; set; }
    }
}
