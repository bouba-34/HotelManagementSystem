using System;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.UI.Commands
{
    public class ReserveRoomCommand : CommandBase
    {
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;
        private readonly int _roomId;
        private readonly int _guestId;
        private readonly DateTime _checkInDate;
        private readonly DateTime _checkOutDate;
        private readonly int _numberOfGuests;

        public ReserveRoomCommand(
            IRoomService roomService,
            IReservationService reservationService,
            int roomId,
            int guestId,
            DateTime checkInDate,
            DateTime checkOutDate,
            int numberOfGuests,
            string executedBy)
            : base(executedBy)
        {
            _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
            _roomId = roomId;
            _guestId = guestId;
            _checkInDate = checkInDate;
            _checkOutDate = checkOutDate;
            _numberOfGuests = numberOfGuests;
        }

        public override async Task<bool> ExecuteAsync()
        {
            // Check if room is available for the selected dates
            bool isAvailable = await _roomService.IsRoomAvailableAsync(_roomId, _checkInDate, _checkOutDate);
            if (!isAvailable)
                return false;

            // Get room details to calculate price
            var room = await _roomService.GetRoomByIdAsync(_roomId);
            if (room == null)
                return false;

            // Calculate total price
            int nights = (_checkOutDate.Date - _checkInDate.Date).Days;
            decimal totalPrice = room.BasePrice * nights;

            // Create reservation
            var reservation = new Reservation
            {
                RoomId = _roomId,
                GuestId = _guestId,
                CheckInDate = _checkInDate,
                CheckOutDate = _checkOutDate,
                NumberOfGuests = _numberOfGuests,
                TotalPrice = totalPrice,
                Status = "Confirmed",
                CreatedDate = DateTime.Now,
                CreatedBy = _executedBy
            };

            await _reservationService.CreateReservationAsync(reservation);

            // Update room status
            await _roomService.UpdateRoomStatusAsync(
                _roomId,
                RoomStatusType.Reserved,
                _checkInDate,
                $"Reserved via reservation #{reservation.ReservationNumber}",
                _executedBy);

            return true;
        }
    }
}
