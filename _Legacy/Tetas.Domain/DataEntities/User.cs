using System.ComponentModel.DataAnnotations.Schema;

namespace Tetas.Domain.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class User
    {

        [Key]
       // [Required]
       // [Index("User_Index_uId", IsUnique = true)]
        public string UserId { get; set; }
        [Display(Name = "University ID")]
        public string UniversityId { get; set; }
        public int StatusId { get; set; }
        public int UserTypeId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public DateTime BornDate { get; set; }
        public int GenderId { get; set; }
        public int CountryId { get; set; }

        [JsonIgnore] public virtual Gender Gender { get; set; }
        [JsonIgnore] public virtual Country Country { get; set; }
        [JsonIgnore]
        public virtual UserType UserType { get; set; }
        [JsonIgnore]
        public virtual Status Status { get; set; }
        [JsonIgnore]
        public ICollection<UserPost> UserPosts { get; set; }
        [JsonIgnore]
        public ICollection<PostComment> PostComments { get; set; }
        [JsonIgnore]
        public ICollection<Group> Groups { get; set; }
        [JsonIgnore]
        public ICollection<GroupMember> GroupMembers { get; set; }

    }
}
