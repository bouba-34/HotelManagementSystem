using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Services;
using HotelManagementSystem.UI.ViewModels;

namespace HotelManagementSystem.UI.Controls
{
    public partial class RoomControl : UserControl
    {
        private RoomViewModel _viewModel;
        private RoomService _roomService;
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RoomViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != null)
                {
                    // Unsubscribe from old view model events
                    _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
                    _viewModel.OnReserveRoomRequested -= HandleReserveRoomRequested;

                }
                
                _viewModel = value;
                
                if (_viewModel != null)
                {
                    // Subscribe to new view model events
                    _viewModel.PropertyChanged += ViewModel_PropertyChanged;
                    _viewModel.OnReserveRoomRequested += HandleReserveRoomRequested;

                    
                    // Update UI
                    UpdateUI();
                }
            }
        }
        
        public RoomControl()
        {
            InitializeComponent();
            
            // Abonnez-vous à l'événement dans le constructeur
            if (_viewModel != null)
            {
                _viewModel.OnReserveRoomRequested += HandleReserveRoomRequested;
            }
        }
        
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update UI when view model properties change
            if (e.PropertyName == nameof(RoomViewModel.CurrentStatus) ||
                e.PropertyName == nameof(RoomViewModel.IsSelected))
            {
                UpdateUI();
            }
        }
        
        private void UpdateUI()
        {
            if (_viewModel == null)
                return;
                
            // Update room number label
            lblRoomNumber.Text = _viewModel.RoomNumber;
            
            // Update room type label
            lblRoomType.Text = _viewModel.RoomTypeName;
            
            // Update status label
            lblStatus.Text = _viewModel.CurrentStatus.ToString();
            
            // Update price label
            lblPrice.Text = $"${_viewModel.BasePrice:N2}";
            
            // Update background color based on status
            BackColor = _viewModel.StatusColor;
            
            // Update border based on selection state
            if (_viewModel.IsSelected)
            {
                BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                BorderStyle = BorderStyle.FixedSingle;
            }
            
            // Update features icons
            picWifi.Visible = _viewModel.HasWifi;
            picMinibar.Visible = _viewModel.HasMinibar;
            picBalcony.Visible = _viewModel.HasBalcony;
        }
        
        private void RoomControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && _viewModel != null)
            {
                // Show context menu
                ContextMenuStrip menu = _viewModel.GetContextMenu();
                menu.Show(this, e.Location);
            }
        }
        
        private void RoomControl_Paint(object sender, PaintEventArgs e)
        {
            // Optional: Add custom painting logic here
        }
        
        private void HandleReserveRoomRequested(object sender, EventArgs e)
        {
            // Code à exécuter lorsque la chambre est demandée pour réservation
            //Console.WriteLine("La chambre a été réservée !");
            // Ajoutez ici la logique pour réserver la chambre
            
            //var reservationViewModel = new ReservationViewModel
            //{
                //SelectedRoom = _roomService.GetRoomByIdAsync(_viewModel.Id).Result
                
            //}
            
        }
    }
}


/*using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.UI.ViewModels;

namespace HotelManagementSystem.UI.Controls
{
    public partial class RoomControl : UserControl
    {
        private RoomViewModel _viewModel;
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RoomViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                if (_viewModel != null)
                {
                    // Unsubscribe from old view model events
                    _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
                }
                
                _viewModel = value;
                
                if (_viewModel != null)
                {
                    // Subscribe to new view model events
                    _viewModel.PropertyChanged += ViewModel_PropertyChanged;
                    
                    // Update UI
                    UpdateUI();
                }
            }
        }
        
        public RoomControl()
        {
            InitializeComponent();
            
            // Enable mouse events
            this.MouseClick += RoomControl_MouseClick;
        }
        
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Update UI when view model properties change
            if (e.PropertyName == nameof(RoomViewModel.CurrentStatus) ||
                e.PropertyName == nameof(RoomViewModel.IsSelected))
            {
                UpdateUI();
            }
        }
        
        private void UpdateUI()
        {
            if (_viewModel == null)
                return;
                
            // Update room number label
            lblRoomNumber.Text = _viewModel.RoomNumber;
            
            // Update room type label
            lblRoomType.Text = _viewModel.RoomTypeName;
            
            // Update status label
            lblStatus.Text = _viewModel.CurrentStatus.ToString();
            
            // Update price label
            lblPrice.Text = $"${_viewModel.BasePrice:N2}";
            
            // Update background color based on status
            BackColor = _viewModel.StatusColor;
            
            // Update border based on selection state
            if (_viewModel.IsSelected)
            {
                BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                BorderStyle = BorderStyle.FixedSingle;
            }
            
            // Update features icons
            picWifi.Visible = _viewModel.HasWifi;
            picMinibar.Visible = _viewModel.HasMinibar;
            picBalcony.Visible = _viewModel.HasBalcony;
        }
        
        private void RoomControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (_viewModel == null)
                return;
                
            if (e.Button == MouseButtons.Right)
            {
                // Create and show the context menu
                ContextMenuStrip menu = CreateContextMenu();
                menu.Show(this, e.Location);
            }
            else if (e.Button == MouseButtons.Left)
            {
                // Select this room on left click
                _viewModel.IsSelected = true;
            }
        }
        
        private ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();
            
            if (_viewModel == null)
                return menu;
                
            // Add menu items based on room status
            switch (_viewModel.CurrentStatus)
            {
                case RoomStatusType.Available:
                    AddMenuItem(menu, "Reserve Room", HandleReserveRoom);
                    AddMenuItem(menu, "Mark Under Maintenance", HandleMaintenanceRequest);
                    break;
                
                case RoomStatusType.Reserved:
                    AddMenuItem(menu, "Check In", HandleCheckIn);
                    AddMenuItem(menu, "Cancel Reservation", HandleCancelReservation);
                    break;
                
                case RoomStatusType.Occupied:
                    AddMenuItem(menu, "Check Out", HandleCheckOut);
                    AddMenuItem(menu, "Extend Stay", HandleExtendStay);
                    break;
                
                case RoomStatusType.CleaningInProgress:
                    AddMenuItem(menu, "Mark as Available", HandleMarkAsAvailable);
                    break;
                
                case RoomStatusType.UnderMaintenance:
                    AddMenuItem(menu, "Mark as Available", HandleMarkAsAvailable);
                    break;
            }
            
            // Add common menu items
            menu.Items.Add(new ToolStripSeparator());
            AddMenuItem(menu, "View Details", HandleViewDetails);
            
            return menu;
        }
        
        private void AddMenuItem(ContextMenuStrip menu, string text, EventHandler handler)
        {
            var item = new ToolStripMenuItem(text);
            item.Click += handler;
            menu.Items.Add(item);
        }
        
        // Event handlers for context menu items
        private void HandleReserveRoom(object sender, EventArgs e)
        {
            _viewModel?.RequestReservation();
        }
        
        private void HandleMaintenanceRequest(object sender, EventArgs e)
        {
            _viewModel?.RequestMaintenance();
        }
        
        private void HandleCheckIn(object sender, EventArgs e)
        {
            _viewModel?.RequestCheckIn();
        }
        
        private void HandleCancelReservation(object sender, EventArgs e)
        {
            _viewModel?.RequestCancelReservation();
        }
        
        private void HandleCheckOut(object sender, EventArgs e)
        {
            _viewModel?.RequestCheckOut();
        }
        
        private void HandleExtendStay(object sender, EventArgs e)
        {
            _viewModel?.RequestExtendStay();
        }
        
        private void HandleMarkAsAvailable(object sender, EventArgs e)
        {
            _viewModel?.RequestMarkAsAvailable();
        }
        
        private void HandleViewDetails(object sender, EventArgs e)
        {
            _viewModel?.RequestViewDetails();
        }
        
        private void RoomControl_Paint(object sender, PaintEventArgs e)
        {
            // Optional: Add custom painting logic here
        }
    }
}*/