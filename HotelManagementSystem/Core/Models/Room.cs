using System;
using System.ComponentModel.DataAnnotations;
using HotelManagementSystem.Core.Enums;

namespace HotelManagementSystem.Core.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string RoomNumber { get; set; }
        
        public int Floor { get; set; }
        
        [Required]
        public RoomTypeEnum RoomType { get; set; }
        
        [Required]
        public decimal BasePrice { get; set; }
        
        public int Capacity { get; set; }
        
        public bool HasWifi { get; set; }
        
        public bool HasMinibar { get; set; }
        
        public bool HasBalcony { get; set; }
        
        public string Description { get; set; }
        
        public int RoomTypeId { get; set; }
        public virtual RoomType RoomTypeDetails { get; set; }
        
        public virtual ICollection<RoomStatus> StatusHistory { get; set; }
        
        public virtual ICollection<Reservation> Reservations { get; set; }
        
        public Room()
        {
            StatusHistory = new List<RoomStatus>();
            Reservations = new List<Reservation>();
        }
        
        public RoomStatusType GetStatusForDate(DateTime date)
        {
            var statusForDate = StatusHistory
                .Where(s => s.Date.Date <= date.Date)
                .OrderByDescending(s => s.Date)
                .FirstOrDefault();
                
            return statusForDate?.Status ?? RoomStatusType.Available;
        }
        
        public bool IsAvailable(DateTime fromDate, DateTime toDate)
        {
            // Check if room is under maintenance or cleaning during the period
            var unavailableStatus = StatusHistory
                .Where(s => s.Date.Date >= fromDate.Date && s.Date.Date <= toDate.Date)
                .Where(s => s.Status == RoomStatusType.UnderMaintenance || s.Status == RoomStatusType.CleaningInProgress)
                .Any();
                
            if (unavailableStatus)
                return false;
                
            // Check if there are any overlapping reservations
            return !Reservations.Any(r => 
                (fromDate <= r.CheckOutDate && toDate >= r.CheckInDate) && 
                r.Status != "Cancelled");
        }
    }
}
