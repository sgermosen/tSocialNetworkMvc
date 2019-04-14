namespace Tetas.Domain.Entities
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class ApplicationUser : IdentityUser
    {
        //public string UserId { get; set; }
        //[Display(Name = "University ID")]
        //public string UniversityId { get; set; }
        //public int StatusId { get; set; }
        //public int UserTypeId { get; set; }
        //public string Name { get; set; }
        //public string LastName { get; set; }
        //public string Picture { get; set; }
        //public DateTime BornDate { get; set; }
        //public int GenderId { get; set; }
        //public int CountryId { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string NickName { get; set; }
        
        //[DataType(DataType.ImageUrl)]
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

        //public string FullName => $"{Name} {Lastname} ({NickName})";

        //public UserType UserType { get; set; }

        public ICollection<GroupMember> GroupMembers { get; set; }

    }
}
