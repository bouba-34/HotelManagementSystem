using System;
using System.ComponentModel.DataAnnotations;
using HotelManagementSystem.Core.Enums;

namespace HotelManagementSystem.Core.Models
{
    public class RoomStatus
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int RoomId { get; set; }
        
        public virtual Room Room { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public RoomStatusType Status { get; set; }
        
        public string Notes { get; set; }
        
        public string UpdatedBy { get; set; }
    }
}