using System;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Interfaces;

namespace HotelManagementSystem.UI.Commands
{
    public class CheckInCommand : CommandBase
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;
        private readonly int _roomId;
        private readonly DateTime _date;

        public CheckInCommand(
            IReservationService reservationService,
            IRoomService roomService,
            int roomId,
            DateTime date,
            string executedBy)
            : base(executedBy)
        {
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
            _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            _roomId = roomId;
            _date = date;
        }

        public override async Task<bool> ExecuteAsync()
        {
            // Find a reservation for this room on the selected date
            var reservations = await _reservationService.GetReservationsByRoomAsync(_roomId);
            var reservation = reservations.FirstOrDefault(r =>
                r.CheckInDate.Date <= _date.Date &&
                r.CheckOutDate.Date >= _date.Date &&
                r.Status == "Confirmed");

            if (reservation == null)
                return false;

            // Check in the guest
            bool checkInSuccessful = await _reservationService.CheckInAsync(
                reservation.Id,
                _date,
                _executedBy);

            if (checkInSuccessful)
            {
                // Update room status to Occupied
                await _roomService.UpdateRoomStatusAsync(
                    _roomId,
                    RoomStatusType.Occupied,
                    _date,
                    $"Guest checked in for reservation #{reservation.ReservationNumber}",
                    _executedBy);
            }

            return checkInSuccessful;
        }
    }
}
