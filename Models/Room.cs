using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManagement.Models
{
    public class Room
    {
        public int RoomId { get; set; } // Primary Key
        public int HotelId { get; set; } // Foreign Key
        public string RoomType { get; set; } // E.g., Single, Double, Suite
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } // Room availability

        public Hotel Hotel { get; set; } // Navigation property
    }
}