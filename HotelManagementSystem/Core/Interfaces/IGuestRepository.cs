using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.Core.Interfaces
{
    public interface IGuestRepository : IRepository<Guest>
    {
        Task<IEnumerable<Guest>> GetGuestsWithDetailsAsync();
        Task<Guest> GetGuestWithDetailsAsync(int id);
        Task<IEnumerable<Guest>> SearchGuestsAsync(string searchTerm);
        Task<Guest> GetGuestByEmailAsync(string email);
        Task<Guest> GetGuestByIdentificationAsync(string identificationType, string identificationNumber);
    }
}