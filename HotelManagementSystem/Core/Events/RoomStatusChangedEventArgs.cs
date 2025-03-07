using System;
using HotelManagementSystem.Core.Enums;

namespace HotelManagementSystem.Core.Events
{
    public class RoomStatusChangedEventArgs : EventArgs
    {
        public int RoomId { get; }
        public RoomStatusType OldStatus { get; }
        public RoomStatusType NewStatus { get; }
        public DateTime Date { get; }

        public RoomStatusChangedEventArgs(int roomId, RoomStatusType oldStatus, RoomStatusType newStatus, DateTime date)
        {
            RoomId = roomId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Date = date;
        }
    }
}