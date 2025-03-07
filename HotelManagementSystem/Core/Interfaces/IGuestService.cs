using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.Core.Interfaces
{
    public interface IGuestService
    {
        Task<IEnumerable<Guest>> GetAllGuestsAsync();
        Task<Guest> GetGuestByIdAsync(int id);
        Task<IEnumerable<Guest>> SearchGuestsAsync(string searchTerm);
        Task<Guest> GetGuestByEmailAsync(string email);
        Task<Guest> GetGuestByIdentificationAsync(string identificationType, string identificationNumber);
        Task<Guest> CreateGuestAsync(Guest guest);
        Task<bool> UpdateGuestAsync(Guest guest);
        Task<bool> DeleteGuestAsync(int id);
    }
}