using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.Core.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetReservationsWithDetailsAsync();
        Task<Reservation> GetReservationWithDetailsAsync(int id);
        Task<Reservation> GetReservationByNumberAsync(string reservationNumber);
        Task<IEnumerable<Reservation>> GetReservationsByGuestAsync(int guestId);
        Task<IEnumerable<Reservation>> GetReservationsByRoomAsync(int roomId);
        Task<IEnumerable<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Reservation>> GetReservationsByStatusAsync(string status);
        Task<IEnumerable<Reservation>> GetCheckInsForDateAsync(DateTime date);
        Task<IEnumerable<Reservation>> GetCheckOutsForDateAsync(DateTime date);
        Task<decimal> GetRevenueForPeriodAsync(DateTime startDate, DateTime endDate);
    }
}