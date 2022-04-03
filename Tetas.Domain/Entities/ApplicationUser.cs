namespace Tetas.Domain.Entities
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public string NickName { get; set; }
        
        public string PictureUrl { get; set; }

        public string Bio { get; set; }

        public string FullName
        {
            get {
                var ret = $"{Name} {Lastname}";
                if (!string.IsNullOrEmpty(NickName))
                {
                    ret = $"{ret} ({NickName})";
                }
                return ret;
            }
        }

        public ICollection<GroupMember> GroupMembers { get; set; }

    }
}
