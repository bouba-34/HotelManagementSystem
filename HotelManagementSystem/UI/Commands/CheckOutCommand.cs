using System;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Interfaces;

namespace HotelManagementSystem.UI.Commands
{
    public class CheckOutCommand : CommandBase
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;
        private readonly int _roomId;
        private readonly DateTime _date;

        public CheckOutCommand(
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
            // Find the active reservation for this room
            var reservations = await _reservationService.GetReservationsByRoomAsync(_roomId);
            var reservation = reservations.FirstOrDefault(r => r.Status == "CheckedIn");

            if (reservation == null)
                return false;

            // Check out the guest
            bool checkOutSuccessful = await _reservationService.CheckOutAsync(
                reservation.Id,
                _date,
                _executedBy);

            if (checkOutSuccessful)
            {
                // Update room status to CleaningInProgress
                await _roomService.UpdateRoomStatusAsync(
                    _roomId,
                    RoomStatusType.CleaningInProgress,
                    _date,
                    $"Guest checked out from reservation #{reservation.ReservationNumber}",
                    _executedBy);
            }

            return checkOutSuccessful;
        }
    }
}