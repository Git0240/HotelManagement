using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Models
{
    public class Hotel
    {
        public int HotelId { get; set; } // Primary Key
        public string Name { get; set; } // Hotel name
        public string Location { get; set; } // Hotel location
        public string Description { get; set; } // Hotel description
    }
}