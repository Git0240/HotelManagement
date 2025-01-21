using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HotelManagement.Models
{
    public class Booking
    {
        public int BookingId { get; set; } // Primary Key

        [Required]
        public int RoomId { get; set; } // Foreign Key to Room

        [Required]
        public string CustomerName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckIn { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CheckOut { get; set; }

        public Room Room { get; set; } // Navigation property
        public decimal Revenue
        {
            get
            {
                var days = (CheckOut - CheckIn).Days;
                return Room.Price * days;
            }
        }

    }


}