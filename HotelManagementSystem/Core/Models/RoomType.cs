using System.ComponentModel.DataAnnotations;
using HotelManagementSystem.Core.Enums;

namespace HotelManagementSystem.Core.Models
{
    public class RoomType
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public RoomTypeEnum Type { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        public decimal BasePrice { get; set; }
        
        public int MaxCapacity { get; set; }
        
        public virtual ICollection<Room> Rooms { get; set; }
        
        public RoomType()
        {
            Rooms = new List<Room>();
        }
    }
}