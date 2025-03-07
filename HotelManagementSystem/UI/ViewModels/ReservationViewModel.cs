using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.UI.ViewModels
{
    public class ReservationViewModel : ViewModelBase
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;
        private readonly IGuestService _guestService;
        
        private Reservation _reservation;
        private Room _selectedRoom;
        private Guest _selectedGuest;
        private DateTime _checkInDate;
        private DateTime _checkOutDate;
        private int _numberOfGuests;
        private string _notes;
        private bool _isPaid;
        private decimal _deposit;
        
        public Reservation Reservation => _reservation;
        
        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (SetProperty(ref _selectedRoom, value))
                {
                    // When room changes, update total price
                    UpdateTotalPrice();
                }
            }
        }
        
        public Guest SelectedGuest
        {
            get => _selectedGuest;
            set => SetProperty(ref _selectedGuest, value);
        }
        
        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                if (SetProperty(ref _checkInDate, value))
                {
                    // Ensure check-out date is after check-in date
                    if (CheckOutDate <= CheckInDate)
                        CheckOutDate = CheckInDate.AddDays(1);
                        
                    UpdateTotalPrice();
                    CheckRoomAvailability();
                }
            }
        }
        
        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                if (value <= CheckInDate)
                    value = CheckInDate.AddDays(1);
                    
                if (SetProperty(ref _checkOutDate, value))
                {
                    UpdateTotalPrice();
                    CheckRoomAvailability();
                }
            }
        }
        
        public int NumberOfGuests
        {
            get => _numberOfGuests;
            set => SetProperty(ref _numberOfGuests, value);
        }
        
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }
        
        public bool IsPaid
        {
            get => _isPaid;
            set => SetProperty(ref _isPaid, value);
        }
        
        public decimal Deposit
        {
            get => _deposit;
            set
            {
                if (SetProperty(ref _deposit, value))
                {
                    // If deposit equals or exceeds total price, mark as paid
                    if (Deposit >= TotalPrice)
                        IsPaid = true;
                    else
                        IsPaid = false;
                }
            }
        }
        
        public decimal TotalPrice { get; private set; }
        public bool IsRoomAvailable { get; private set; }
        
        public ReservationViewModel(
            IReservationService reservationService,
            IRoomService roomService,
            IGuestService guestService)
        {
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
            _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            _guestService = guestService ?? throw new ArgumentNullException(nameof(guestService));
            
            _reservation = new Reservation();
            _checkInDate = DateTime.Today;
            _checkOutDate = DateTime.Today.AddDays(1);
            _numberOfGuests = 1;
            _isPaid = false;
            _deposit = 0;
            
            IsRoomAvailable = false;
        }
        
        public async Task InitializeForNewReservationAsync()
        {
            _reservation = new Reservation
            {
                CheckInDate = CheckInDate,
                CheckOutDate = CheckOutDate,
                NumberOfGuests = NumberOfGuests,
                Status = "Pending",
                IsPaid = IsPaid,
                Deposit = Deposit,
                Notes = Notes,
                CreatedDate = DateTime.Now,
                CreatedBy = "User" // In a real app, this would be the current user
            };
        }
        
        public async Task LoadReservationAsync(int reservationId)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(reservationId);
            if (reservation != null)
            {
                _reservation = reservation;
                _selectedRoom = reservation.Room;
                _selectedGuest = reservation.Guest;
                _checkInDate = reservation.CheckInDate;
                _checkOutDate = reservation.CheckOutDate;
                _numberOfGuests = reservation.NumberOfGuests;
                _notes = reservation.Notes;
                _isPaid = reservation.IsPaid;
                _deposit = reservation.Deposit ?? 0;
                
                TotalPrice = reservation.TotalPrice;
                
                OnPropertyChanged(nameof(SelectedRoom));
                OnPropertyChanged(nameof(SelectedGuest));
                OnPropertyChanged(nameof(CheckInDate));
                OnPropertyChanged(nameof(CheckOutDate));
                OnPropertyChanged(nameof(NumberOfGuests));
                OnPropertyChanged(nameof(Notes));
                OnPropertyChanged(nameof(IsPaid));
                OnPropertyChanged(nameof(Deposit));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        
        public async Task<bool> SaveReservationAsync()
        {
            if (SelectedRoom == null || SelectedGuest == null)
                return false;
                
            // Update reservation properties
            _reservation.RoomId = SelectedRoom.Id;
            _reservation.GuestId = SelectedGuest.Id;
            _reservation.CheckInDate = CheckInDate;
            _reservation.CheckOutDate = CheckOutDate;
            _reservation.NumberOfGuests = NumberOfGuests;
            _reservation.Notes = Notes;
            _reservation.IsPaid = IsPaid;
            _reservation.Deposit = Deposit;
            _reservation.TotalPrice = TotalPrice;
            
            if (_reservation.Id == 0)
            {
                // New reservation
                await _reservationService.CreateReservationAsync(_reservation);
            }
            else
            {
                // Existing reservation
                await _reservationService.UpdateReservationAsync(_reservation);
            }
            
            return true;
        }
        
        private void UpdateTotalPrice()
        {
            if (SelectedRoom != null)
            {
                int nights = (CheckOutDate.Date - CheckInDate.Date).Days;
                TotalPrice = SelectedRoom.BasePrice * nights;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        
        private async void CheckRoomAvailability()
        {
            if (SelectedRoom != null)
            {
                IsRoomAvailable = await _roomService.IsRoomAvailableAsync(SelectedRoom.Id, CheckInDate, CheckOutDate);
                OnPropertyChanged(nameof(IsRoomAvailable));
            }
        }
    }
}

/*using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.UI.ViewModels
{
    public class ReservationViewModel : ViewModelBase
    {
        private readonly IReservationService _reservationService;
        private readonly IRoomService _roomService;
        private readonly IGuestService _guestService;
        
        // Add semaphore for thread safety
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        
        private Reservation _reservation;
        private Room _selectedRoom;
        private Guest _selectedGuest;
        private DateTime _checkInDate;
        private DateTime _checkOutDate;
        private int _numberOfGuests;
        private string _notes;
        private bool _isPaid;
        private decimal _deposit;
        private bool _isCheckingAvailability = false;
        
        public Reservation Reservation => _reservation;
        
        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                if (SetProperty(ref _selectedRoom, value))
                {
                    // When room changes, update total price
                    UpdateTotalPrice();
                    
                    // Check availability asynchronously
                    _ = SafeCheckRoomAvailabilityAsync();
                }
            }
        }
        
        public Guest SelectedGuest
        {
            get => _selectedGuest;
            set => SetProperty(ref _selectedGuest, value);
        }
        
        public DateTime CheckInDate
        {
            get => _checkInDate;
            set
            {
                if (SetProperty(ref _checkInDate, value))
                {
                    // Ensure check-out date is after check-in date
                    if (CheckOutDate <= CheckInDate)
                        CheckOutDate = CheckInDate.AddDays(1);
                        
                    UpdateTotalPrice();
                    
                    // Check availability asynchronously
                    _ = SafeCheckRoomAvailabilityAsync();
                }
            }
        }
        
        public DateTime CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                if (value <= CheckInDate)
                    value = CheckInDate.AddDays(1);
                    
                if (SetProperty(ref _checkOutDate, value))
                {
                    UpdateTotalPrice();
                    
                    // Check availability asynchronously
                    _ = SafeCheckRoomAvailabilityAsync();
                }
            }
        }
        
        public int NumberOfGuests
        {
            get => _numberOfGuests;
            set => SetProperty(ref _numberOfGuests, value);
        }
        
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }
        
        public bool IsPaid
        {
            get => _isPaid;
            set => SetProperty(ref _isPaid, value);
        }
        
        public decimal Deposit
        {
            get => _deposit;
            set
            {
                if (SetProperty(ref _deposit, value))
                {
                    // If deposit equals or exceeds total price, mark as paid
                    if (Deposit >= TotalPrice)
                        IsPaid = true;
                    else
                        IsPaid = false;
                }
            }
        }
        
        public decimal TotalPrice { get; private set; }
        public bool IsRoomAvailable { get; private set; }
        
        public ReservationViewModel(
            IReservationService reservationService,
            IRoomService roomService,
            IGuestService guestService)
        {
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
            _roomService = roomService ?? throw new ArgumentNullException(nameof(roomService));
            _guestService = guestService ?? throw new ArgumentNullException(nameof(guestService));
            
            _reservation = new Reservation();
            _checkInDate = DateTime.Today;
            _checkOutDate = DateTime.Today.AddDays(1);
            _numberOfGuests = 1;
            _isPaid = false;
            _deposit = 0;
            
            IsRoomAvailable = false;
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
        
        public async Task InitializeForNewReservationAsync()
        {
            await SafeExecuteAsync(async () => 
            {
                _reservation = new Reservation
                {
                    CheckInDate = CheckInDate,
                    CheckOutDate = CheckOutDate,
                    NumberOfGuests = NumberOfGuests,
                    Status = "Pending",
                    IsPaid = IsPaid,
                    Deposit = Deposit,
                    Notes = Notes,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "User" // In a real app, this would be the current user
                };
            });
        }
        
        public async Task LoadReservationAsync(int reservationId)
        {
            await SafeExecuteAsync(async () =>
            {
                var reservation = await _reservationService.GetReservationByIdAsync(reservationId);
                if (reservation != null)
                {
                    _reservation = reservation;
                    _selectedRoom = reservation.Room;
                    _selectedGuest = reservation.Guest;
                    _checkInDate = reservation.CheckInDate;
                    _checkOutDate = reservation.CheckOutDate;
                    _numberOfGuests = reservation.NumberOfGuests;
                    _notes = reservation.Notes;
                    _isPaid = reservation.IsPaid;
                    _deposit = reservation.Deposit ?? 0;
                    
                    TotalPrice = reservation.TotalPrice;
                    
                    OnPropertyChanged(nameof(SelectedRoom));
                    OnPropertyChanged(nameof(SelectedGuest));
                    OnPropertyChanged(nameof(CheckInDate));
                    OnPropertyChanged(nameof(CheckOutDate));
                    OnPropertyChanged(nameof(NumberOfGuests));
                    OnPropertyChanged(nameof(Notes));
                    OnPropertyChanged(nameof(IsPaid));
                    OnPropertyChanged(nameof(Deposit));
                    OnPropertyChanged(nameof(TotalPrice));
                }
            });
        }
        
        public async Task<bool> SaveReservationAsync()
        {
            // Exécute l'opération en toute sécurité avec le sémaphore
            return await SafeExecuteAsync(async () =>
            {
                if (SelectedRoom == null || SelectedGuest == null)
                    return false;  // On retourne "false" si les informations sont manquantes

                // Mise à jour des propriétés de réservation
                _reservation.RoomId = SelectedRoom.Id;
                _reservation.GuestId = SelectedGuest.Id;
                _reservation.CheckInDate = CheckInDate;
                _reservation.CheckOutDate = CheckOutDate;
                _reservation.NumberOfGuests = NumberOfGuests;
                _reservation.Notes = Notes;
                _reservation.IsPaid = IsPaid;
                _reservation.Deposit = Deposit;
                _reservation.TotalPrice = TotalPrice;

                // Si l'ID de la réservation est 0, on crée une nouvelle réservation
                if (_reservation.Id == 0)
                {
                    await _reservationService.CreateReservationAsync(_reservation);
                }
                else
                {
                    // Sinon, on met à jour la réservation existante
                    await _reservationService.UpdateReservationAsync(_reservation);
                }

                return true;  // Retourner "true" une fois l'opération réussie
            });
        }

        // Méthode SafeExecuteAsync ajustée pour accepter un retour booléen
        private async Task<T> SafeExecuteAsync<T>(Func<Task<T>> operation)
        {
            await _semaphore.WaitAsync();
            try
            {
                return await operation();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        
        private void UpdateTotalPrice()
        {
            if (SelectedRoom != null)
            {
                int nights = (CheckOutDate.Date - CheckInDate.Date).Days;
                TotalPrice = SelectedRoom.BasePrice * nights;
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
        
        // This method just starts the check - it won't wait
        private async Task SafeCheckRoomAvailabilityAsync()
        {
            // Skip if already checking or if room not selected
            if (_isCheckingAvailability || SelectedRoom == null)
                return;
                
            _isCheckingAvailability = true;
            
            try
            {
                await _semaphore.WaitAsync();
                try
                {
                    IsRoomAvailable = await _roomService.IsRoomAvailableAsync(
                        SelectedRoom.Id, CheckInDate, CheckOutDate);
                    OnPropertyChanged(nameof(IsRoomAvailable));
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            finally
            {
                _isCheckingAvailability = false;
            }
        }
    }
}*/
