using System;

namespace HotelManagementSystem.Core.Events
{
    public class DateChangedEventArgs : EventArgs
    {
        public DateTime NewDate { get; }

        public DateChangedEventArgs(DateTime newDate)
        {
            NewDate = newDate;
        }
    }
}