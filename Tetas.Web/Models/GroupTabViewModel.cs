using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tetas.Domain.Entities;

namespace Tetas.Web.Models
{
    public class GroupTabViewModel: Group
    {
        public Tab ActiveTab { get; set; }
    }

    public enum Tab
    {
        Info,
        GroupMembers,
        GroupPosts
    }
}
