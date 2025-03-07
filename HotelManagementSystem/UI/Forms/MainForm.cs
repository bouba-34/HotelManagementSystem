using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HotelManagementSystem.Core.Events;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.UI.Controls;
using HotelManagementSystem.UI.Factories;
using HotelManagementSystem.UI.ViewModels;
using HotelManagementSystem.UI.Utilities;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.UI.Forms
{
    public partial class MainForm : Form
    {
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;
        private readonly IGuestService _guestService;
        private readonly RoomControlFactory _roomControlFactory;
        private readonly MainViewModel _viewModel;
        
        private readonly Dictionary<int, RoomControl> _roomControls = new Dictionary<int, RoomControl>();
        
        public MainForm(
            IRoomService roomService,
            IReservationService reservationService,
            IGuestService guestService)
        {
            InitializeComponent();
            
            _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
            _guestService = guestService ?? throw new ArgumentNullException(nameof(guestService));
            
            _roomControlFactory = new RoomControlFactory();
            _viewModel = new MainViewModel(_roomService, _reservationService, _guestService);
            
            // Set up event handlers
            _viewModel.DateChanged += ViewModel_DateChanged;
            
            // Initialize date picker with today's date
            dtpSelectedDate.Value = DateTime.Today;
            
            // Set status message
            lblStatus.Text = "Ready";
        }
        
        private async void MainForm_Load(object sender, EventArgs e)
        {
            await this.ExecuteWithUIFeedbackAsync(async () =>
            {
                try
                {
                    // Set the view model's selected date
                    _viewModel.SelectedDate = dtpSelectedDate.Value;
                    
                    // Load data from the database
                    await _viewModel.LoadDataAsync();
                    
                    // Populate room grid
                    PopulateRoomGrid();
                    
                    // Update summary panel
                    summaryPanel.RoomStatusSummary = _viewModel.RoomStatusSummary;
                    
                    // Update status
                    lblStatus.Text = $"Loaded {_viewModel.Rooms.Count} rooms";
                }
                catch (Exception ex)
                {
                    this.ShowError($"Error loading data: {ex.Message}");
                }
            });
        }
        
        private void ViewModel_DateChanged(object sender, DateChangedEventArgs e)
        {
            // Update the date picker to match the view model
            if (dtpSelectedDate.Value != e.NewDate)
                dtpSelectedDate.Value = e.NewDate;
                
            // Update the summary panel
            summaryPanel.RoomStatusSummary = _viewModel.RoomStatusSummary;
        }
        
        private void dtpSelectedDate_ValueChanged(object sender, EventArgs e)
        {
            // Update the view model with the new date
            _viewModel.SelectedDate = dtpSelectedDate.Value;
            lblStatus.Text = $"Date changed to {dtpSelectedDate.Value.ToShortDateString()}";
        }
        
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Update the view model's search query
            _viewModel.SearchQuery = txtSearch.Text;
            
            // Filter the room grid
            FilterRoomGrid();
            
            // Update status
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
                lblStatus.Text = "Showing all rooms";
            else
                lblStatus.Text = $"Filtering rooms with '{txtSearch.Text}'";
        }
        
        private void PopulateRoomGrid()
        {
            // Clear existing room controls
            flowLayoutPanelRooms.Controls.Clear();
            _roomControls.Clear();
            
            // Create room controls for each room in the view model
            foreach (var roomViewModel in _viewModel.Rooms)
            {
                var room = new Room
                {
                    Id = roomViewModel.Id,
                    RoomNumber = roomViewModel.RoomNumber,
                    Floor = roomViewModel.Floor,
                    RoomType = roomViewModel.RoomType,
                    BasePrice = roomViewModel.BasePrice,
                    Capacity = roomViewModel.Capacity,
                    HasWifi = roomViewModel.HasWifi,
                    HasMinibar = roomViewModel.HasMinibar,
                    HasBalcony = roomViewModel.HasBalcony,
                    Description = roomViewModel.Description
                };
                
                var roomControl = _roomControlFactory.CreateRoomControl(room, _viewModel.SelectedDate);
                
                // Add to the flow layout panel
                flowLayoutPanelRooms.Controls.Add(roomControl);
                
                // Add to the dictionary for easy lookup
                _roomControls[roomViewModel.Id] = roomControl;
            }
        }
        
        private void FilterRoomGrid()
        {
            string searchQuery = _viewModel.SearchQuery?.ToLower() ?? string.Empty;
            
            foreach (var roomControl in _roomControls.Values)
            {
                var roomViewModel = roomControl.ViewModel;
                
                if (string.IsNullOrWhiteSpace(searchQuery))
                {
                    // Show all rooms
                    roomControl.Visible = true;
                }
                else
                {
                    // Filter rooms based on search query
                    bool isMatch = roomViewModel.RoomNumber.ToLower().Contains(searchQuery) ||
                                  roomViewModel.RoomTypeName.ToLower().Contains(searchQuery) ||
                                  roomViewModel.CurrentStatus.ToString().ToLower().Contains(searchQuery);
                                  
                    roomControl.Visible = isMatch;
                }
            }
        }
        
        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            // In a real implementation, this would open a form to add a new room
            this.ShowInfo("Add Room functionality would be implemented here.", "Feature Notice");
        }
        
        private async void btnNewReservation_Click(object sender, EventArgs e)
        {
            // Create and show reservation form
            using (var reservationForm = new ReservationForm(_reservationService, _roomService, _guestService))
            {
                if (reservationForm.ShowDialog(this) == DialogResult.OK)
                {
                    // Refresh the room display after a new reservation is created
                    await this.ExecuteWithUIFeedbackAsync(async () =>
                    {
                        await _viewModel.LoadDataAsync();
                        PopulateRoomGrid();
                        summaryPanel.RoomStatusSummary = _viewModel.RoomStatusSummary;
                        lblStatus.Text = "Reservation created successfully";
                    });
                }
            }
        }
        
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            // Reload the data from the database
            await this.ExecuteWithUIFeedbackAsync(async () =>
            {
                try
                {
                    await _viewModel.LoadDataAsync();
                    
                    // Refresh the UI
                    PopulateRoomGrid();
                    summaryPanel.RoomStatusSummary = _viewModel.RoomStatusSummary;
                    
                    lblStatus.Text = "Data refreshed";
                }
                catch (Exception ex)
                {
                    this.ShowError($"Error refreshing data: {ex.Message}");
                }
            });
        }
    }
}