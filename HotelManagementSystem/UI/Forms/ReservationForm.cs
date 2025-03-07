// HotelManagementSystem.UI/Forms/ReservationForm.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.UI.ViewModels;
using HotelManagementSystem.UI.Utilities;

namespace HotelManagementSystem.UI.Forms
{
    public partial class ReservationForm : Form
    {
        private readonly ReservationViewModel _viewModel;
        private readonly IRoomService _roomService;
        private readonly IGuestService _guestService;
        
        public ReservationForm(
            IReservationService reservationService,
            IRoomService roomService,
            IGuestService guestService,
            int? reservationId = null)
        {
            InitializeComponent();
            
            _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            _guestService = guestService ?? throw new ArgumentNullException(nameof(guestService));
            
            _viewModel = new ReservationViewModel(reservationService, roomService, guestService);
            
            // Set up data bindings
            SetupDataBindings();
            
            // If a reservation ID is provided, load the existing reservation
            if (reservationId.HasValue)
            {
                // This will be executed in the Form_Load event to ensure UI is ready
                Text = $"Edit Reservation #{reservationId}";
                Tag = reservationId; // Store ID for later loading
            }
            else
            {
                Text = "New Reservation";
            }
        }
        
        private void SetupDataBindings()
        {
            // Set up bindings between controls and view model properties
            dtpCheckInDate.DataBindings.Add("Value", _viewModel, nameof(_viewModel.CheckInDate), false, DataSourceUpdateMode.OnPropertyChanged);
            dtpCheckOutDate.DataBindings.Add("Value", _viewModel, nameof(_viewModel.CheckOutDate), false, DataSourceUpdateMode.OnPropertyChanged);
            numGuests.DataBindings.Add("Value", _viewModel, nameof(_viewModel.NumberOfGuests), false, DataSourceUpdateMode.OnPropertyChanged);
            txtNotes.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Notes), false, DataSourceUpdateMode.OnPropertyChanged);
            chkIsPaid.DataBindings.Add("Checked", _viewModel, nameof(_viewModel.IsPaid), false, DataSourceUpdateMode.OnPropertyChanged);
            numDeposit.DataBindings.Add("Value", _viewModel, nameof(_viewModel.Deposit), false, DataSourceUpdateMode.OnPropertyChanged);
            
            // Set up calculated properties
            lblTotalPrice.DataBindings.Add("Text", _viewModel, nameof(_viewModel.TotalPrice), true, DataSourceUpdateMode.OnPropertyChanged, 0, "C2");
            
            // Room availability indication
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.IsRoomAvailable))
                {
                    this.InvokeIfRequired(() => {
                        lblAvailability.Text = _viewModel.IsRoomAvailable ? "Available" : "Not Available";
                        lblAvailability.ForeColor = _viewModel.IsRoomAvailable ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                    });
                }
            };
        }
        
        private async void ReservationForm_Load(object sender, EventArgs e)
        {
            await this.ExecuteWithUIFeedbackAsync(async () =>
            {
                try
                {
                    // Load rooms for the combo box
                    var rooms = await _roomService.GetAllRoomsAsync();
                    cboRoom.DataSource = rooms.ToList();
                    cboRoom.DisplayMember = "RoomNumber";
                    cboRoom.ValueMember = "Id";
                    
                    // Load guests for the combo box
                    var guests = await _guestService.GetAllGuestsAsync();
                    cboGuest.DataSource = guests.ToList();
                    cboGuest.DisplayMember = "FullName";
                    cboGuest.ValueMember = "Id";
                    
                    // Load existing reservation if editing
                    if (Tag is int reservationId)
                    {
                        await _viewModel.LoadReservationAsync(reservationId);
                        
                        // Set selected values
                        if (_viewModel.Reservation?.Id > 0)
                        {
                            cboRoom.SelectedValue = _viewModel.Reservation.RoomId;
                            cboGuest.SelectedValue = _viewModel.Reservation.GuestId;
                        }
                    }
                    else
                    {
                        await _viewModel.InitializeForNewReservationAsync();
                    }
                }
                catch (Exception ex)
                {
                    this.ShowError($"Error loading data: {ex.Message}");
                }
            });
        }
        
        private void cboRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRoom.SelectedItem is Room selectedRoom)
            {
                _viewModel.SelectedRoom = selectedRoom;
            }
        }
        
        private void cboGuest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGuest.SelectedItem is Guest selectedGuest)
            {
                _viewModel.SelectedGuest = selectedGuest;
            }
        }
        
        private async void btnSave_Click(object sender, EventArgs e)
        {
            await this.ExecuteWithUIFeedbackAsync(async () =>
            {
                try
                {
                    // Validate input
                    if (cboRoom.SelectedItem == null)
                    {
                        this.ShowError("Please select a room.", "Validation Error");
                        return;
                    }
                    
                    if (cboGuest.SelectedItem == null)
                    {
                        this.ShowError("Please select a guest.", "Validation Error");
                        return;
                    }
                    
                    if (!_viewModel.IsRoomAvailable)
                    {
                        bool proceed = this.Confirm(
                            "The selected room is not available for the chosen dates. Do you want to continue anyway?",
                            "Room Not Available");
                            
                        if (!proceed)
                            return;
                    }
                    
                    // Save the reservation
                    bool success = await _viewModel.SaveReservationAsync();
                    
                    if (success)
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        this.ShowError("Failed to save the reservation.");
                    }
                }
                catch (Exception ex)
                {
                    this.ShowError($"Error saving reservation: {ex.Message}");
                }
            });
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}