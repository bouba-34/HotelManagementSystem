using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.Core.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int id);
        Task<Reservation> GetReservationByNumberAsync(string reservationNumber);
        Task<IEnumerable<Reservation>> GetReservationsByGuestAsync(int guestId);
        Task<IEnumerable<Reservation>> GetReservationsByRoomAsync(int roomId);
        Task<IEnumerable<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Reservation>> GetReservationsByStatusAsync(string status);
        Task<IEnumerable<Reservation>> GetCheckInsForDateAsync(DateTime date);
        Task<IEnumerable<Reservation>> GetCheckOutsForDateAsync(DateTime date);
        Task<decimal> GetRevenueForPeriodAsync(DateTime startDate, DateTime endDate);
        Task<Reservation> CreateReservationAsync(Reservation reservation);
        Task<bool> UpdateReservationAsync(Reservation reservation);
        Task<bool> CancelReservationAsync(int id, string cancelledBy);
        Task<bool> CheckInAsync(int reservationId, DateTime actualCheckInDate, string checkedInBy);
        Task<bool> CheckOutAsync(int reservationId, DateTime actualCheckOutDate, string checkedOutBy);
        Task<bool> ExtendStayAsync(int reservationId, DateTime newCheckOutDate, string modifiedBy);
        Task<bool> MarkAsPaidAsync(int reservationId, decimal amountPaid, string paidBy);
    }
}