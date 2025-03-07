using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.Core.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomService _roomService;

        public ReservationService(
            IReservationRepository reservationRepository,
            IRoomRepository roomRepository,
            IRoomService roomService)
        {
            _reservationRepository = reservationRepository ?? throw new ArgumentNullException(nameof(reservationRepository));
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _reservationRepository.GetReservationsWithDetailsAsync();
        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            return await _reservationRepository.GetReservationWithDetailsAsync(id);
        }

        public async Task<Reservation> GetReservationByNumberAsync(string reservationNumber)
        {
            return await _reservationRepository.GetReservationByNumberAsync(reservationNumber);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByGuestAsync(int guestId)
        {
            return await _reservationRepository.GetReservationsByGuestAsync(guestId);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByRoomAsync(int roomId)
        {
            return await _reservationRepository.GetReservationsByRoomAsync(roomId);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _reservationRepository.GetReservationsByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByStatusAsync(string status)
        {
            return await _reservationRepository.GetReservationsByStatusAsync(status);
        }

        public async Task<IEnumerable<Reservation>> GetCheckInsForDateAsync(DateTime date)
        {
            return await _reservationRepository.GetCheckInsForDateAsync(date);
        }

        public async Task<IEnumerable<Reservation>> GetCheckOutsForDateAsync(DateTime date)
        {
            return await _reservationRepository.GetCheckOutsForDateAsync(date);
        }

        public async Task<decimal> GetRevenueForPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _reservationRepository.GetRevenueForPeriodAsync(startDate, endDate);
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            // Check if room is available for the requested dates
            bool isAvailable = await _roomService.IsRoomAvailableAsync(
                reservation.RoomId, 
                reservation.CheckInDate, 
                reservation.CheckOutDate);

            if (!isAvailable)
                throw new InvalidOperationException("Room is not available for the selected dates.");

            // Calculate total price based on room's base price and number of nights
            var room = await _roomRepository.GetRoomWithDetailsAsync(reservation.RoomId);
            int nights = reservation.GetNumberOfNights();
            reservation.TotalPrice = room.BasePrice * nights;

            // Set default values
            reservation.Status = "Confirmed";
            reservation.CreatedDate = DateTime.Now;

            var addedReservation = await _reservationRepository.AddAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            // Update room status to Reserved for the reservation dates
            await _roomService.UpdateRoomStatusAsync(
                reservation.RoomId, 
                RoomStatusType.Reserved, 
                reservation.CheckInDate, 
                $"Reserved via reservation #{reservation.ReservationNumber}",
                reservation.CreatedBy);

            return addedReservation;
        }

        public async Task<bool> UpdateReservationAsync(Reservation reservation)
        {
            var existingReservation = await _reservationRepository.GetReservationWithDetailsAsync(reservation.Id);
            if (existingReservation == null)
                return false;

            // Check if room is available for the new dates if they are different
            if (existingReservation.RoomId != reservation.RoomId ||
                existingReservation.CheckInDate != reservation.CheckInDate ||
                existingReservation.CheckOutDate != reservation.CheckOutDate)
            {
                bool isAvailable = await _roomService.IsRoomAvailableAsync(
                    reservation.RoomId,
                    reservation.CheckInDate,
                    reservation.CheckOutDate);

                if (!isAvailable)
                    throw new InvalidOperationException("Room is not available for the selected dates.");

                // Update room status for the new dates
                await _roomService.UpdateRoomStatusAsync(
                    reservation.RoomId,
                    RoomStatusType.Reserved,
                    reservation.CheckInDate,
                    $"Reserved via updated reservation #{reservation.ReservationNumber}",
                    reservation.ModifiedBy);
            }

            // Recalculate total price if dates or room changed
            if (existingReservation.RoomId != reservation.RoomId ||
                existingReservation.CheckInDate != reservation.CheckInDate ||
                existingReservation.CheckOutDate != reservation.CheckOutDate)
            {
                var room = await _roomRepository.GetRoomWithDetailsAsync(reservation.RoomId);
                int nights = reservation.GetNumberOfNights();
                reservation.TotalPrice = room.BasePrice * nights;
            }

            reservation.ModifiedDate = DateTime.Now;
            await _reservationRepository.UpdateAsync(reservation);
            await _reservationRepository.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> CancelReservationAsync(int id, string cancelledBy)
        {
            var reservation = await _reservationRepository.GetReservationWithDetailsAsync(id);
            if (reservation == null)
                return false;

            reservation.Status = "Cancelled";
            reservation.ModifiedDate = DateTime.Now;
            reservation.ModifiedBy = cancelledBy;

            await _reservationRepository.UpdateAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            // Update room status back to Available
            await _roomService.UpdateRoomStatusAsync(
                reservation.RoomId,
                RoomStatusType.Available,
                DateTime.Now,
                $"Reservation #{reservation.ReservationNumber} cancelled",
                cancelledBy);

            return true;
        }

        public async Task<bool> CheckInAsync(int reservationId, DateTime actualCheckInDate, string checkedInBy)
        {
            var reservation = await _reservationRepository.GetReservationWithDetailsAsync(reservationId);
            if (reservation == null || reservation.Status != "Confirmed")
                return false;

            reservation.Status = "CheckedIn";
            reservation.ActualCheckInDate = actualCheckInDate;
            reservation.ModifiedDate = DateTime.Now;
            reservation.ModifiedBy = checkedInBy;

            await _reservationRepository.UpdateAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            // Update room status to Occupied
            await _roomService.UpdateRoomStatusAsync(
                reservation.RoomId,
                RoomStatusType.Occupied,
                actualCheckInDate,
                $"Guest checked in for reservation #{reservation.ReservationNumber}",
                checkedInBy);

            return true;
        }

        public async Task<bool> CheckOutAsync(int reservationId, DateTime actualCheckOutDate, string checkedOutBy)
        {
            var reservation = await _reservationRepository.GetReservationWithDetailsAsync(reservationId);
            if (reservation == null || reservation.Status != "CheckedIn")
                return false;

            reservation.Status = "CheckedOut";
            reservation.ActualCheckOutDate = actualCheckOutDate;
            reservation.ModifiedDate = DateTime.Now;
            reservation.ModifiedBy = checkedOutBy;

            await _reservationRepository.UpdateAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            // Update room status to CleaningInProgress
            await _roomService.UpdateRoomStatusAsync(
                reservation.RoomId,
                RoomStatusType.CleaningInProgress,
                actualCheckOutDate,
                $"Guest checked out from reservation #{reservation.ReservationNumber}",
                checkedOutBy);

            return true;
        }

        public async Task<bool> ExtendStayAsync(int reservationId, DateTime newCheckOutDate, string modifiedBy)
        {
            var reservation = await _reservationRepository.GetReservationWithDetailsAsync(reservationId);
            if (reservation == null || reservation.Status != "CheckedIn")
                return false;

            // Check if the room is available for the extended period
            bool isAvailable = await _roomService.IsRoomAvailableAsync(
                reservation.RoomId,
                reservation.CheckOutDate.AddDays(1),
                newCheckOutDate);

            if (!isAvailable)
                throw new InvalidOperationException("Room is not available for the extended stay period.");

            // Calculate additional charges
            var room = await _roomRepository.GetRoomWithDetailsAsync(reservation.RoomId);
            int additionalNights = (newCheckOutDate.Date - reservation.CheckOutDate.Date).Days;
            decimal additionalCharge = room.BasePrice * additionalNights;

            reservation.CheckOutDate = newCheckOutDate;
            reservation.TotalPrice += additionalCharge;
            reservation.ModifiedDate = DateTime.Now;
            reservation.ModifiedBy = modifiedBy;

            await _reservationRepository.UpdateAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MarkAsPaidAsync(int reservationId, decimal amountPaid, string paidBy)
        {
            var reservation = await _reservationRepository.GetReservationWithDetailsAsync(reservationId);
            if (reservation == null)
                return false;

            // Update the deposit or mark as fully paid if amount matches or exceeds total
            if (amountPaid >= reservation.TotalPrice)
            {
                reservation.IsPaid = true;
                reservation.Deposit = reservation.TotalPrice; // Full amount paid
            }
            else
            {
                // Partial payment
                reservation.Deposit = amountPaid;
            }

            reservation.ModifiedDate = DateTime.Now;
            reservation.ModifiedBy = paidBy;

            await _reservationRepository.UpdateAsync(reservation);
            await _reservationRepository.SaveChangesAsync();

            return true;
        }
    }
}
