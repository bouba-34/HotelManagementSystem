using System;
using System.Drawing;
using System.Windows.Forms;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.UI.ViewModels
{
    public class RoomViewModel : ViewModelBase
    {
        private readonly Room _room;
        private RoomStatusType _currentStatus;
        private DateTime _selectedDate;
        private bool _isSelected;

        public int Id => _room.Id;
        public string RoomNumber => _room.RoomNumber;
        public int Floor => _room.Floor;
        public RoomTypeEnum RoomType => _room.RoomType;
        public string RoomTypeName => _room.RoomTypeDetails?.Name ?? RoomType.ToString();
        public decimal BasePrice => _room.BasePrice;
        public int Capacity => _room.Capacity;
        public bool HasWifi => _room.HasWifi;
        public bool HasMinibar => _room.HasMinibar;
        public bool HasBalcony => _room.HasBalcony;
        public string Description => _room.Description;

        public RoomStatusType CurrentStatus
        {
            get => _currentStatus;
            set => SetProperty(ref _currentStatus, value);
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    // Update status based on the selected date
                    CurrentStatus = _room.GetStatusForDate(value);
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public Color StatusColor
        {
            get
            {
                return CurrentStatus switch
                {
                    RoomStatusType.Available => Color.LightGreen,
                    RoomStatusType.Occupied => Color.LightCoral,
                    RoomStatusType.Reserved => Color.LightBlue,
                    RoomStatusType.UnderMaintenance => Color.Orange,
                    RoomStatusType.CleaningInProgress => Color.LightYellow,
                    _ => Color.LightGray
                };
            }
        }

        public RoomViewModel(Room room, DateTime selectedDate)
        {
            _room = room ?? throw new ArgumentNullException(nameof(room));
            _selectedDate = selectedDate;
            _currentStatus = room.GetStatusForDate(selectedDate);
        }

        public ContextMenuStrip GetContextMenu()
        {
            var menu = new ContextMenuStrip();
            
            // Add menu items based on room status
            switch (CurrentStatus)
            {
                case RoomStatusType.Available:
                    menu.Items.Add("Reserve Room", null, (s, e) => 
                        OnReserveRoomRequested?.Invoke(this, EventArgs.Empty));
                    menu.Items.Add("Mark Under Maintenance", null, (s, e) => 
                        OnMaintenanceRequested?.Invoke(this, EventArgs.Empty));
                    break;
                
                case RoomStatusType.Reserved:
                    menu.Items.Add("Check In", null, (s, e) => 
                        OnCheckInRequested?.Invoke(this, EventArgs.Empty));
                    menu.Items.Add("Cancel Reservation", null, (s, e) => 
                        OnCancelReservationRequested?.Invoke(this, EventArgs.Empty));
                    break;
                
                case RoomStatusType.Occupied:
                    menu.Items.Add("Check Out", null, (s, e) => 
                        OnCheckOutRequested?.Invoke(this, EventArgs.Empty));
                    menu.Items.Add("Extend Stay", null, (s, e) => 
                        OnExtendStayRequested?.Invoke(this, EventArgs.Empty));
                    break;
                
                case RoomStatusType.CleaningInProgress:
                    menu.Items.Add("Mark as Available", null, (s, e) => 
                        OnMarkAsAvailableRequested?.Invoke(this, EventArgs.Empty));
                    break;
                
                case RoomStatusType.UnderMaintenance:
                    menu.Items.Add("Mark as Available", null, (s, e) => 
                        OnMarkAsAvailableRequested?.Invoke(this, EventArgs.Empty));
                    break;
            }
            
            // Add common menu items
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("View Details", null, (s, e) => 
                OnViewDetailsRequested?.Invoke(this, EventArgs.Empty));
            
            return menu;
        }
        
        // Events for context menu actions
        public event EventHandler OnReserveRoomRequested;
        public event EventHandler OnCheckInRequested;
        public event EventHandler OnCheckOutRequested;
        public event EventHandler OnCancelReservationRequested;
        public event EventHandler OnExtendStayRequested;
        public event EventHandler OnMaintenanceRequested;
        public event EventHandler OnMarkAsAvailableRequested;
        public event EventHandler OnViewDetailsRequested;
    }
}


/*using System;
using System.Drawing;
using System.Windows.Forms;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.UI.ViewModels
{
    public class RoomViewModel : ViewModelBase
    {
        private readonly Room _room;
        private readonly Room _localRoomCopy; // Create a local copy to avoid direct access to the entity
        private RoomStatusType _currentStatus;
        private DateTime _selectedDate;
        private bool _isSelected;

        public int Id => _room.Id;
        public string RoomNumber => _room.RoomNumber;
        public int Floor => _room.Floor;
        public RoomTypeEnum RoomType => _room.RoomType;
        public string RoomTypeName => _room.RoomTypeDetails?.Name ?? RoomType.ToString();
        public decimal BasePrice => _room.BasePrice;
        public int Capacity => _room.Capacity;
        public bool HasWifi => _room.HasWifi;
        public bool HasMinibar => _room.HasMinibar;
        public bool HasBalcony => _room.HasBalcony;
        public string Description => _room.Description;

        public RoomStatusType CurrentStatus
        {
            get => _currentStatus;
            set => SetProperty(ref _currentStatus, value);
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (SetProperty(ref _selectedDate, value))
                {
                    // Update status using the local copy to avoid EF Core tracking issues
                    UpdateStatusFromDate(value);
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public Color StatusColor
        {
            get
            {
                return CurrentStatus switch
                {
                    RoomStatusType.Available => Color.LightGreen,
                    RoomStatusType.Occupied => Color.LightCoral,
                    RoomStatusType.Reserved => Color.LightBlue,
                    RoomStatusType.UnderMaintenance => Color.Orange,
                    RoomStatusType.CleaningInProgress => Color.LightYellow,
                    _ => Color.LightGray
                };
            }
        }

        public RoomViewModel(Room room, DateTime selectedDate)
        {
            // Store the original room entity
            _room = room ?? throw new ArgumentNullException(nameof(room));
            
            // Make a local copy with all essential data
            _localRoomCopy = new Room
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Floor = room.Floor,
                RoomType = room.RoomType,
                BasePrice = room.BasePrice,
                Capacity = room.Capacity,
                HasWifi = room.HasWifi,
                HasMinibar = room.HasMinibar,
                HasBalcony = room.HasBalcony,
                Description = room.Description,
                StatusHistory = new System.Collections.Generic.List<RoomStatus>(room.StatusHistory ?? new System.Collections.Generic.List<RoomStatus>())
            };
            
            _selectedDate = selectedDate;
            
            // Initialize status using our local copy
            UpdateStatusFromDate(selectedDate);
        }
        
        // Private method to update status without accessing the EF Core tracked entity
        private void UpdateStatusFromDate(DateTime date)
        {
            if (_localRoomCopy.StatusHistory == null || _localRoomCopy.StatusHistory.Count == 0)
            {
                CurrentStatus = RoomStatusType.Available;
                return;
            }
            
            // Find status with most recent date less than or equal to selected date
            RoomStatus mostRecentStatus = null;
            DateTime mostRecentDate = DateTime.MinValue;
            
            foreach (var status in _localRoomCopy.StatusHistory)
            {
                if (status.Date <= date && status.Date > mostRecentDate)
                {
                    mostRecentStatus = status;
                    mostRecentDate = status.Date;
                }
            }
            
            CurrentStatus = mostRecentStatus?.Status ?? RoomStatusType.Available;
        }

        // Action methods that can be safely called from outside
        public void RequestReservation() 
        { 
            OnReserveRoomRequested?.Invoke(this, EventArgs.Empty);
        }
        
        public void RequestCheckIn() 
        { 
            OnCheckInRequested?.Invoke(this, EventArgs.Empty);
        }
        
        public void RequestCheckOut() 
        { 
            OnCheckOutRequested?.Invoke(this, EventArgs.Empty); 
        }
        
        public void RequestCancelReservation() 
        { 
            OnCancelReservationRequested?.Invoke(this, EventArgs.Empty); 
        }
        
        public void RequestExtendStay() 
        { 
            OnExtendStayRequested?.Invoke(this, EventArgs.Empty); 
        }
        
        public void RequestMaintenance() 
        { 
            OnMaintenanceRequested?.Invoke(this, EventArgs.Empty); 
        }
        
        public void RequestMarkAsAvailable() 
        { 
            OnMarkAsAvailableRequested?.Invoke(this, EventArgs.Empty); 
        }
        
        public void RequestViewDetails() 
        { 
            OnViewDetailsRequested?.Invoke(this, EventArgs.Empty); 
        }
        
        // Events for context menu actions
        public event EventHandler OnReserveRoomRequested;
        public event EventHandler OnCheckInRequested;
        public event EventHandler OnCheckOutRequested;
        public event EventHandler OnCancelReservationRequested;
        public event EventHandler OnExtendStayRequested;
        public event EventHandler OnMaintenanceRequested;
        public event EventHandler OnMarkAsAvailableRequested;
        public event EventHandler OnViewDetailsRequested;
    }
}*/