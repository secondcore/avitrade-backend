using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    // Within each group, there are different roles that different users assume
    public class Role
    {
        public const string Admins = "Admins";
        public const string Executives = "Executives";
        public const string IT = "IT";
        public const string Marketing = "Marketing";
        public const string Sales = "Sales";
        public const string Finance = "Finance";
        public const string Support = "Support";
        public const string Pilot = "Pilot";

        [Key, DatabaseGenerated((DatabaseGeneratedOption.None))]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // MKA - Removed 30AUG2012 - the user should have one single role
        //public ICollection<User> Users { get; set; }

        public Role()
        {
            // MKA - Removed 30AUG2012 - the user should have one single role
            //Users = new List<User>();
        }
    }
}
