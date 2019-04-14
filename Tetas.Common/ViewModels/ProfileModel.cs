using System.ComponentModel.DataAnnotations;

namespace Tetas.Common.ViewModels
{
    public class ProfileModel
    {
        public string Name { get; set; }

        public string Lastname { get; set; }

        public string NickName { get; set; }

        public string Phone { get; set; }

        public string PictureUrl { get; set; }

        public string Email { get; set; }

       // [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        public bool IsMe { get; set; }

        // public string FullName => $"{Name} {Lastname} ({NickName})";

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
    }
}
