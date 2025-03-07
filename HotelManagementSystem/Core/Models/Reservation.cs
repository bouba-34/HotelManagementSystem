using System;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Core.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }
        
        public string ReservationNumber { get; set; }
        
        [Required]
        public int RoomId { get; set; }
        
        public virtual Room Room { get; set; }
        
        [Required]
        public int GuestId { get; set; }
        
        public virtual Guest Guest { get; set; }
        
        [Required]
        public DateTime CheckInDate { get; set; }
        
        [Required]
        public DateTime CheckOutDate { get; set; }
        
        public DateTime? ActualCheckInDate { get; set; }
        
        public DateTime? ActualCheckOutDate { get; set; }
        
        [Required]
        public decimal TotalPrice { get; set; }
        
        public decimal? Deposit { get; set; }
        
        public bool IsPaid { get; set; }
        
        [Required]
        public string Status { get; set; } // "Pending", "Confirmed", "CheckedIn", "CheckedOut", "Cancelled", "NoShow"
        
        public string Notes { get; set; }
        
        public int NumberOfGuests { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime? ModifiedDate { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public int GetNumberOfNights()
        {
            return (CheckOutDate.Date - CheckInDate.Date).Days;
        }
        
        public Reservation()
        {
            ReservationNumber = GenerateReservationNumber();
            CreatedDate = DateTime.Now;
            Status = "Pending";
        }
        
        private string GenerateReservationNumber()
        {
            return $"RES-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
}