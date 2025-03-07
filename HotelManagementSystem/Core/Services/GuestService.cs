using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.Core.Services
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _guestRepository;

        public GuestService(IGuestRepository guestRepository)
        {
            _guestRepository = guestRepository ?? throw new ArgumentNullException(nameof(guestRepository));
        }

        public async Task<IEnumerable<Guest>> GetAllGuestsAsync()
        {
            return await _guestRepository.GetGuestsWithDetailsAsync();
        }

        public async Task<Guest> GetGuestByIdAsync(int id)
        {
            return await _guestRepository.GetGuestWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Guest>> SearchGuestsAsync(string searchTerm)
        {
            return await _guestRepository.SearchGuestsAsync(searchTerm);
        }

        public async Task<Guest> GetGuestByEmailAsync(string email)
        {
            return await _guestRepository.GetGuestByEmailAsync(email);
        }

        public async Task<Guest> GetGuestByIdentificationAsync(string identificationType, string identificationNumber)
        {
            return await _guestRepository.GetGuestByIdentificationAsync(identificationType, identificationNumber);
        }

        public async Task<Guest> CreateGuestAsync(Guest guest)
        {
            // Check if guest with same email or identification already exists
            var existingByEmail = !string.IsNullOrEmpty(guest.Email) 
                ? await _guestRepository.GetGuestByEmailAsync(guest.Email) 
                : null;
                
            var existingById = await _guestRepository.GetGuestByIdentificationAsync(
                guest.IdentificationType, 
                guest.IdentificationNumber);

            if (existingByEmail != null)
                throw new InvalidOperationException($"Guest with email {guest.Email} already exists.");

            if (existingById != null)
                throw new InvalidOperationException($"Guest with identification {guest.IdentificationType} {guest.IdentificationNumber} already exists.");

            var addedGuest = await _guestRepository.AddAsync(guest);
            await _guestRepository.SaveChangesAsync();
            return addedGuest;
        }

        public async Task<bool> UpdateGuestAsync(Guest guest)
        {
            var existingGuest = await _guestRepository.GetByIdAsync(guest.Id);
            if (existingGuest == null)
                return false;

            // Check if email is being changed and if new email is already in use
            if (!string.IsNullOrEmpty(guest.Email) && 
                existingGuest.Email != guest.Email)
            {
                var existingByEmail = await _guestRepository.GetGuestByEmailAsync(guest.Email);
                if (existingByEmail != null && existingByEmail.Id != guest.Id)
                    throw new InvalidOperationException($"Guest with email {guest.Email} already exists.");
            }

            // Check if identification is being changed and if new identification is already in use
            if (existingGuest.IdentificationType != guest.IdentificationType ||
                existingGuest.IdentificationNumber != guest.IdentificationNumber)
            {
                var existingById = await _guestRepository.GetGuestByIdentificationAsync(
                    guest.IdentificationType,
                    guest.IdentificationNumber);

                if (existingById != null && existingById.Id != guest.Id)
                    throw new InvalidOperationException($"Guest with identification {guest.IdentificationType} {guest.IdentificationNumber} already exists.");
            }

            await _guestRepository.UpdateAsync(guest);
            await _guestRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGuestAsync(int id)
        {
            var guest = await _guestRepository.GetGuestWithDetailsAsync(id);
            if (guest == null)
                return false;

            // Check if guest has any active reservations
            if (guest.Reservations.Any(r => 
                r.Status == "Confirmed" || 
                r.Status == "CheckedIn"))
            {
                throw new InvalidOperationException("Cannot delete guest with active reservations.");
            }

            await _guestRepository.DeleteAsync(guest);
            await _guestRepository.SaveChangesAsync();
            return true;
        }
    }
}