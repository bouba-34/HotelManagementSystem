using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.Data.Context;

namespace HotelManagementSystem.Data.Repositories
{
    public class GuestRepository : RepositoryBase<Guest>, IGuestRepository
    {
        public GuestRepository(HotelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Guest>> GetGuestsWithDetailsAsync()
        {
            return await _dbContext.Guests
                .Include(g => g.Reservations)
                .ToListAsync();
        }

        public async Task<Guest> GetGuestWithDetailsAsync(int id)
        {
            return await _dbContext.Guests
                .Include(g => g.Reservations)
                    .ThenInclude(r => r.Room)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
        
        public async Task<Guest> GetGuestByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;
                
            return await _dbContext.Guests
                .FirstOrDefaultAsync(g => g.Email == email);
        }
        
        public async Task<Guest> GetGuestByIdentificationAsync(string identificationType, string identificationNumber)
        {
            if (string.IsNullOrWhiteSpace(identificationType) || string.IsNullOrWhiteSpace(identificationNumber))
                return null;
                
            return await _dbContext.Guests
                .FirstOrDefaultAsync(g => 
                    g.IdentificationType == identificationType && 
                    g.IdentificationNumber == identificationNumber);
        }

        public async Task<IEnumerable<Guest>> SearchGuestsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetGuestsWithDetailsAsync();

            return await _dbContext.Guests
                .Include(g => g.Reservations)
                .Where(g => 
                    g.FirstName.Contains(searchTerm) || 
                    g.LastName.Contains(searchTerm) || 
                    g.Email.Contains(searchTerm) || 
                    g.Phone.Contains(searchTerm) ||
                    g.IdentificationNumber.Contains(searchTerm))
                .ToListAsync();
        }
    }
}