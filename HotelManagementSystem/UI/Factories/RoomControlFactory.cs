using System;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.UI.Controls;
using HotelManagementSystem.UI.ViewModels;

namespace HotelManagementSystem.UI.Factories
{
    public class RoomControlFactory
    {
        public RoomControl CreateRoomControl(Room room, DateTime selectedDate)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room));

            // Create the view model
            var viewModel = new RoomViewModel(room, selectedDate);
            
            // Create the control
            var control = new RoomControl
            {
                ViewModel = viewModel,
                Tag = room.Id // Store the room ID for easy reference
            };
            
            return control;
        }
    }
}