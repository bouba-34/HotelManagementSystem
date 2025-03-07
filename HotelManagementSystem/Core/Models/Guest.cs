using System;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Core.Models
{
    public class Guest
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        
        [Phone]
        public string Phone { get; set; }
        
        public string Address { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }
        
        public string ZipCode { get; set; }
        
        public string Country { get; set; }
        
        [Required]
        public string IdentificationNumber { get; set; }
        
        public string IdentificationType { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        public string Notes { get; set; }
        
        public virtual ICollection<Reservation> Reservations { get; set; }
        
        public Guest()
        {
            Reservations = new List<Reservation>();
        }
        
        public string FullName => $"{FirstName} {LastName}";
    }
}