using HotelManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Models
{
    public class Role
    {
        public int RoleId { get; set; } // Primary Key
        public string RoleName { get; set; } // Role name (e.g., "SuperAdmin", "Manager")
        public string Description { get; set; } // Optional description

        public ICollection<Admin> Admins { get; set; } // Navigation property to link admins
    }
}