using System;
using System.Collections.Generic;
using System.Text;
using Tetas.Domain.Entities;

namespace Tetas.Common.ViewModels
{
    public class GroupModel : Group
    {
        public bool State { get; set; }

        public bool Banned { get; set; }

        public bool Applied { get; set; }

        public bool IsAdmin { get; set; }
    }
}
