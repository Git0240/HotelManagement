using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Models
{
    public class Admin
    {
        public int AdminId { get; set; } // Primary Key
        public string Username { get; set; } // Admin username
        public string Password { get; set; } // Admin password (hashed in production)

        public int RoleId { get; set; } // Foreign Key
        public Role Role { get; set; } // Navigation property
    }

}