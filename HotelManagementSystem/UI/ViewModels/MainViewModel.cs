/*using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Events;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;
        private readonly IGuestService _guestService;
        
        private DateTime _selectedDate;
        private string _searchQuery;
        private RoomViewModel _selectedRoom;
        private Dictionary<RoomStatusType, int> _roomStatusSummary;
        
        public ObservableCollection<RoomViewModel> Rooms { get; } = new ObservableCollection<RoomViewModel>();
        
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    // Update all room statuses and raise event
                    OnDateChanged(new DateChangedEventArgs(value));
                    RefreshRoomStatuses();
                }
            }
        }
        
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (SetProperty(ref _searchQuery, value))
                {
                    FilterRooms();
                }
            }
        }
        
        public RoomViewModel SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (_selectedRoom != null)
                    _selectedRoom.IsSelected = false;
                
                SetProperty(ref _selectedRoom, value);
                
                if (_selectedRoom != null)
                    _selectedRoom.IsSelected = true;
            }
        }
        
        public Dictionary<RoomStatusType, int> RoomStatusSummary
        {
            get => _roomStatusSummary;
            set => SetProperty(ref _roomStatusSummary, value);
        }
        
        // Event for date change
        public event EventHandler<DateChangedEventArgs> DateChanged;
        
        public MainViewModel(IRoomService roomService, IReservationService reservationService, IGuestService guestService)
        {
            _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
            _guestService = guestService ?? throw new ArgumentNullException(nameof(guestService));
            
            _selectedDate = DateTime.Today;
            _roomStatusSummary = new Dictionary<RoomStatusType, int>();
        }
        
        public async Task LoadDataAsync()
        {
            await LoadRoomsAsync();
            await RefreshSummaryAsync();
        }
        
        public async Task LoadRoomsAsync()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            
            Rooms.Clear();
            foreach (var room in rooms)
            {
                var roomViewModel = new RoomViewModel(room, SelectedDate);
                SetupRoomEvents(roomViewModel);
                Rooms.Add(roomViewModel);
            }
            
            RefreshRoomStatuses();
        }
        
        public void RefreshRoomStatuses()
        {
            foreach (var room in Rooms)
            {
                room.SelectedDate = SelectedDate;
            }
            
            // Update summary as well
            _ = RefreshSummaryAsync();
        }
        
        public async Task RefreshSummaryAsync()
        {
            RoomStatusSummary = await _roomService.GetRoomStatusSummaryAsync(SelectedDate);
        }
        
        private void FilterRooms()
        {
            // This would normally filter the rooms collection based on search query
            // For now, just simulating the filtering behavior
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                // Show all rooms
                foreach (var room in Rooms)
                {
                    // Set a property indicating this room should be visible
                    // In a real implementation, you might filter the collection
                }
            }
            else
            {
                // Filter rooms
                foreach (var room in Rooms)
                {
                    var isMatch = room.RoomNumber.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                 room.RoomTypeName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                 room.CurrentStatus.ToString().Contains(SearchQuery, StringComparison.OrdinalIgnoreCase);
                    
                    // Set a property indicating whether this room should be visible
                    // In a real implementation, you might filter the collection
                }
            }
        }
        
        protected virtual void OnDateChanged(DateChangedEventArgs e)
        {
            DateChanged?.Invoke(this, e);
        }
        
        private void SetupRoomEvents(RoomViewModel roomViewModel)
        {
            // Set up event handlers for room context menu actions
            roomViewModel.OnReserveRoomRequested += async (s, e) => await ReserveRoomAsync(roomViewModel);
            roomViewModel.OnCheckInRequested += async (s, e) => await CheckInAsync(roomViewModel);
            roomViewModel.OnCheckOutRequested += async (s, e) => await CheckOutAsync(roomViewModel);
            roomViewModel.OnCancelReservationRequested += async (s, e) => await CancelReservationAsync(roomViewModel);
            roomViewModel.OnExtendStayRequested += async (s, e) => await ExtendStayAsync(roomViewModel);
            roomViewModel.OnMaintenanceRequested += async (s, e) => await MarkUnderMaintenanceAsync(roomViewModel);
            roomViewModel.OnMarkAsAvailableRequested += async (s, e) => await MarkAsAvailableAsync(roomViewModel);
            roomViewModel.OnViewDetailsRequested += (s, e) => ViewRoomDetails(roomViewModel);
        }
        
        // These methods would implement the actual functionality for room operations
        private async Task ReserveRoomAsync(RoomViewModel roomViewModel)
        {
            // This would open a reservation form and handle reservation creation
            // For now, just changing the status
            await _roomService.UpdateRoomStatusAsync(
                roomViewModel.Id, 
                RoomStatusType.Reserved, 
                SelectedDate, 
                "Reserved via manual action", 
                "User");
                
            await LoadRoomsAsync();
        }
        
        private async Task CheckInAsync(RoomViewModel roomViewModel)
        {
            // This would find the reservation for this room and check in the guest
            var reservations = await _reservationService.GetReservationsByRoomAsync(roomViewModel.Id);
            var reservation = reservations.FirstOrDefault(r => 
                r.CheckInDate.Date <= SelectedDate.Date && 
                r.CheckOutDate.Date >= SelectedDate.Date &&
                r.Status == "Confirmed");
                
            if (reservation != null)
            {
                await _reservationService.CheckInAsync(reservation.Id, DateTime.Now, "User");
                await LoadRoomsAsync();
            }
        }
        
        private async Task CheckOutAsync(RoomViewModel roomViewModel)
        {
            // This would find the reservation for this room and check out the guest
            var reservations = await _reservationService.GetReservationsByRoomAsync(roomViewModel.Id);
            var reservation = reservations.FirstOrDefault(r => r.Status == "CheckedIn");
                
            if (reservation != null)
            {
                await _reservationService.CheckOutAsync(reservation.Id, DateTime.Now, "User");
                await LoadRoomsAsync();
            }
        }
        
        private async Task CancelReservationAsync(RoomViewModel roomViewModel)
        {
            // This would find the reservation for this room and cancel it
            var reservations = await _reservationService.GetReservationsByRoomAsync(roomViewModel.Id);
            var reservation = reservations.FirstOrDefault(r => 
                r.CheckInDate.Date <= SelectedDate.Date && 
                r.CheckOutDate.Date >= SelectedDate.Date &&
                r.Status == "Confirmed");
                
            if (reservation != null)
            {
                await _reservationService.CancelReservationAsync(reservation.Id, "User");
                await LoadRoomsAsync();
            }
        }
        
        private async Task ExtendStayAsync(RoomViewModel roomViewModel)
        {
            // This would open a dialog to get the new check-out date and extend the stay
            // For now, we'll just add one day to the stay
            var reservations = await _reservationService.GetReservationsByRoomAsync(roomViewModel.Id);
            var reservation = reservations.FirstOrDefault(r => r.Status == "CheckedIn");
                
            if (reservation != null)
            {
                await _reservationService.ExtendStayAsync(
                    reservation.Id, 
                    reservation.CheckOutDate.AddDays(1), 
                    "User");
                    
                await LoadRoomsAsync();
            }
        }
        
        private async Task MarkUnderMaintenanceAsync(RoomViewModel roomViewModel)
        {
            await _roomService.UpdateRoomStatusAsync(
                roomViewModel.Id, 
                RoomStatusType.UnderMaintenance, 
                SelectedDate, 
                "Marked for maintenance", 
                "User");
                
            await LoadRoomsAsync();
        }
        
        private async Task MarkAsAvailableAsync(RoomViewModel roomViewModel)
        {
            await _roomService.UpdateRoomStatusAsync(
                roomViewModel.Id, 
                RoomStatusType.Available, 
                SelectedDate, 
                "Marked as available", 
                "User");
                
            await LoadRoomsAsync();
        }
        
        private void ViewRoomDetails(RoomViewModel roomViewModel)
        {
            // This would open a dialog showing room details
            SelectedRoom = roomViewModel;
        }
    }
}*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Events;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;
        private readonly IGuestService _guestService;
        
        // Add semaphore to prevent concurrent DbContext operations
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        
        private DateTime _selectedDate;
        private string _searchQuery;
        private RoomViewModel _selectedRoom;
        private Dictionary<RoomStatusType, int> _roomStatusSummary;
        
        public ObservableCollection<RoomViewModel> Rooms { get; } = new ObservableCollection<RoomViewModel>();
        
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    // Raise the event first without triggering database operations
                    OnDateChanged(new DateChangedEventArgs(value));
                    
                    // Then asynchronously update room statuses
                    _ = UpdateRoomStatusesAsync(value);
                }
            }
        }
        
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (SetProperty(ref _searchQuery, value))
                {
                    FilterRooms();
                }
            }
        }
        
        public RoomViewModel SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (_selectedRoom != null)
                    _selectedRoom.IsSelected = false;
                
                SetProperty(ref _selectedRoom, value);
                
                if (_selectedRoom != null)
                    _selectedRoom.IsSelected = true;
            }
        }
        
        public Dictionary<RoomStatusType, int> RoomStatusSummary
        {
            get => _roomStatusSummary;
            set => SetProperty(ref _roomStatusSummary, value);
        }
        
        // Event for date change
        public event EventHandler<DateChangedEventArgs> DateChanged;
        
        public MainViewModel(IRoomService roomService, IReservationService reservationService, IGuestService guestService)
        {
            _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
            _guestService = guestService ?? throw new ArgumentNullException(nameof(guestService));
            
            _selectedDate = DateTime.Today;
            _roomStatusSummary = new Dictionary<RoomStatusType, int>();
        }
        
        public async Task LoadDataAsync()
        {
            // Use semaphore to prevent concurrent operations
            await _semaphore.WaitAsync();
            try
            {
                await LoadRoomsAsync();
                await RefreshSummaryAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public async Task LoadRoomsAsync()
        {
            // This method is now only called from within a semaphore lock
            var rooms = await _roomService.GetAllRoomsAsync();
            
            Rooms.Clear();
            foreach (var room in rooms)
            {
                var roomViewModel = new RoomViewModel(room, SelectedDate);
                SetupRoomEvents(roomViewModel);
                Rooms.Add(roomViewModel);
            }
            
            // Update the room statuses without triggering another DB call
            UpdateRoomStatusesLocal();
        }
        
        // This doesn't access the database, just updates local view models
        private void UpdateRoomStatusesLocal()
        {
            foreach (var room in Rooms)
            {
                room.SelectedDate = SelectedDate;
            }
        }
        
        // New async method to update statuses with DB operations
        private async Task UpdateRoomStatusesAsync(DateTime date)
        {
            await _semaphore.WaitAsync();
            try
            {
                // First update the local models
                UpdateRoomStatusesLocal();
                
                // Then refresh the summary from the database
                await RefreshSummaryAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public async Task RefreshSummaryAsync()
        {
            // This method is now only called from within a semaphore lock
            RoomStatusSummary = await _roomService.GetRoomStatusSummaryAsync(SelectedDate);
        }
        
        private void FilterRooms()
        {
            // This would normally filter the rooms collection based on search query
            // For now, just simulating the filtering behavior
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                // Show all rooms
                foreach (var room in Rooms)
                {
                    // Set a property indicating this room should be visible
                    // In a real implementation, you might filter the collection
                }
            }
            else
            {
                // Filter rooms
                foreach (var room in Rooms)
                {
                    var isMatch = room.RoomNumber.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                 room.RoomTypeName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                 room.CurrentStatus.ToString().Contains(SearchQuery, StringComparison.OrdinalIgnoreCase);
                    
                    // Set a property indicating whether this room should be visible
                    // In a real implementation, you might filter the collection
                }
            }
        }
        
        protected virtual void OnDateChanged(DateChangedEventArgs e)
        {
            DateChanged?.Invoke(this, e);
        }
        
        private void SetupRoomEvents(RoomViewModel roomViewModel)
        {
            // Set up event handlers for room context menu actions
            roomViewModel.OnReserveRoomRequested += async (s, e) => await SafeExecuteAsync(() => ReserveRoomAsync(roomViewModel));
            roomViewModel.OnCheckInRequested += async (s, e) => await SafeExecuteAsync(() => CheckInAsync(roomViewModel));
            roomViewModel.OnCheckOutRequested += async (s, e) => await SafeExecuteAsync(() => CheckOutAsync(roomViewModel));
            roomViewModel.OnCancelReservationRequested += async (s, e) => await SafeExecuteAsync(() => CancelReservationAsync(roomViewModel));
            roomViewModel.OnExtendStayRequested += async (s, e) => await SafeExecuteAsync(() => ExtendStayAsync(roomViewModel));
            roomViewModel.OnMaintenanceRequested += async (s, e) => await SafeExecuteAsync(() => MarkUnderMaintenanceAsync(roomViewModel));
            roomViewModel.OnMarkAsAvailableRequested += async (s, e) => await SafeExecuteAsync(() => MarkAsAvailableAsync(roomViewModel));
            roomViewModel.OnViewDetailsRequested += (s, e) => ViewRoomDetails(roomViewModel);
        }
        
        // Helper method to safely execute operations with semaphore
        private async Task SafeExecuteAsync(Func<Task> operation)
        {
            await _semaphore.WaitAsync();
            try
            {
                await operation();
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        // These methods would implement the actual functionality for room operations
        private async Task ReserveRoomAsync(RoomViewModel roomViewModel)
        {
            // This would open a reservation form and handle reservation creation
            // For now, just changing the status
            await _roomService.UpdateRoomStatusAsync(
                roomViewModel.Id, 
                RoomStatusType.Reserved, 
                SelectedDate, 
                "Reserved via manual action", 
                "User");
                
            await LoadRoomsAsync();
        }
        
        private async Task CheckInAsync(RoomViewModel roomViewModel)
        {
            // This would find the reservation for this room and check in the guest
            var reservations = await _reservationService.GetReservationsByRoomAsync(roomViewModel.Id);
            var reservation = reservations.FirstOrDefault(r => 
                r.CheckInDate.Date <= SelectedDate.Date && 
                r.CheckOutDate.Date >= SelectedDate.Date &&
                r.Status == "Confirmed");
                
            if (reservation != null)
            {
                await _reservationService.CheckInAsync(reservation.Id, DateTime.Now, "User");
                await LoadRoomsAsync();
            }
        }
        
        private async Task CheckOutAsync(RoomViewModel roomViewModel)
        {
            // This would find the reservation for this room and check out the guest
            var reservations = await _reservationService.GetReservationsByRoomAsync(roomViewModel.Id);
            var reservation = reservations.FirstOrDefault(r => r.Status == "CheckedIn");
                
            if (reservation != null)
            {
                await _reservationService.CheckOutAsync(reservation.Id, DateTime.Now, "User");
                await LoadRoomsAsync();
            }
        }
        
        private async Task CancelReservationAsync(RoomViewModel roomViewModel)
        {
            // This would find the reservation for this room and cancel it
            var reservations = await _reservationService.GetReservationsByRoomAsync(roomViewModel.Id);
            var reservation = reservations.FirstOrDefault(r => 
                r.CheckInDate.Date <= SelectedDate.Date && 
                r.CheckOutDate.Date >= SelectedDate.Date &&
                r.Status == "Confirmed");
                
            if (reservation != null)
            {
                await _reservationService.CancelReservationAsync(reservation.Id, "User");
                await LoadRoomsAsync();
            }
        }
        
        private async Task ExtendStayAsync(RoomViewModel roomViewModel)
        {
            // This would open a dialog to get the new check-out date and extend the stay
            // For now, we'll just add one day to the stay
            var reservations = await _reservationService.GetReservationsByRoomAsync(roomViewModel.Id);
            var reservation = reservations.FirstOrDefault(r => r.Status == "CheckedIn");
                
            if (reservation != null)
            {
                await _reservationService.ExtendStayAsync(
                    reservation.Id, 
                    reservation.CheckOutDate.AddDays(1), 
                    "User");
                    
                await LoadRoomsAsync();
            }
        }
        
        private async Task MarkUnderMaintenanceAsync(RoomViewModel roomViewModel)
        {
            await _roomService.UpdateRoomStatusAsync(
                roomViewModel.Id, 
                RoomStatusType.UnderMaintenance, 
                SelectedDate, 
                "Marked for maintenance", 
                "User");
                
            await LoadRoomsAsync();
        }
        
        private async Task MarkAsAvailableAsync(RoomViewModel roomViewModel)
        {
            await _roomService.UpdateRoomStatusAsync(
                roomViewModel.Id, 
                RoomStatusType.Available, 
                SelectedDate, 
                "Marked as available", 
                "User");
                
            await LoadRoomsAsync();
        }
        
        private void ViewRoomDetails(RoomViewModel roomViewModel)
        {
            // This would open a dialog showing room details
            SelectedRoom = roomViewModel;
        }
    }
}