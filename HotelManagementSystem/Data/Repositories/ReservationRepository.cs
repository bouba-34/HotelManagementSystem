using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.Data.Context;

namespace HotelManagementSystem.Data.Repositories
{
    public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(HotelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Reservation>> GetReservationsWithDetailsAsync()
        {
            return await _dbContext.Reservations
                .Include(r => r.Room)
                    .ThenInclude(room => room.RoomTypeDetails)
                .Include(r => r.Guest)
                .ToListAsync();
        }

        public async Task<Reservation> GetReservationWithDetailsAsync(int id)
        {
            return await _dbContext.Reservations
                .Include(r => r.Room)
                    .ThenInclude(room => room.RoomTypeDetails)
                .Include(r => r.Guest)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Reservation> GetReservationByNumberAsync(string reservationNumber)
        {
            return await _dbContext.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .FirstOrDefaultAsync(r => r.ReservationNumber == reservationNumber);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByGuestAsync(int guestId)
        {
            return await _dbContext.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Where(r => r.GuestId == guestId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByRoomAsync(int roomId)
        {
            return await _dbContext.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Where(r => r.RoomId == roomId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Where(r => 
                    (r.CheckInDate <= endDate && r.CheckOutDate >= startDate) &&
                    r.Status != "Cancelled" && r.Status != "NoShow")
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByStatusAsync(string status)
        {
            return await _dbContext.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Where(r => r.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetCheckInsForDateAsync(DateTime date)
        {
            return await _dbContext.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Where(r => r.CheckInDate.Date == date.Date && r.Status == "Confirmed")
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetCheckOutsForDateAsync(DateTime date)
        {
            return await _dbContext.Reservations
                .Include(r => r.Room)
                .Include(r => r.Guest)
                .Where(r => r.CheckOutDate.Date == date.Date && r.Status == "CheckedIn")
                .ToListAsync();
        }

        public async Task<decimal> GetRevenueForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var completedReservations = await _dbContext.Reservations
                .Where(r => 
                    (r.ActualCheckOutDate >= startDate && r.ActualCheckOutDate <= endDate) &&
                    r.Status == "CheckedOut" && r.IsPaid)
                .ToListAsync();

            return completedReservations.Sum(r => r.TotalPrice);
        }
    }
}
