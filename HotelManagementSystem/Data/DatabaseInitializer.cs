using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.Data.Context;

namespace HotelManagementSystem.Data
{
    public class DatabaseInitializer
    {
        public static async Task InitializeAsync(HotelDbContext context)
        {
            try
            {
                Console.WriteLine("Initializing database...");

                // Vérifier si la base de données est accessible
                if (!await context.Database.CanConnectAsync())
                {
                    throw new Exception("Unable to connect to the database.");
                }

                // Appliquer les migrations
                await context.Database.MigrateAsync();
                Console.WriteLine("Database migration applied successfully.");

                // === Seed Room Types ===
                await SeedRoomTypesAsync(context);

                // === Seed Rooms ===
                await SeedRoomsAsync(context);

                // === Seed Room Statuses ===
                await SeedRoomStatusesAsync(context);

                // === Seed Guests ===
                await SeedGuestsAsync(context);

                // === Seed Reservations ===
                await SeedReservationsAsync(context);

                Console.WriteLine("Database initialization complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                throw;
            }
        }

        private static async Task SeedRoomTypesAsync(HotelDbContext context)
        {
            if (!await context.RoomTypes.AnyAsync())
            {
                context.RoomTypes.AddRange(
                    new RoomType { Type = RoomTypeEnum.Single, Name = "Single Room", Description = "A room with a single bed for one person", BasePrice = 99.99m, MaxCapacity = 1 },
                    new RoomType { Type = RoomTypeEnum.Double, Name = "Double Room", Description = "A room with a double bed for up to two people", BasePrice = 149.99m, MaxCapacity = 2 },
                    new RoomType { Type = RoomTypeEnum.Twin, Name = "Twin Room", Description = "A room with two single beds for up to two people", BasePrice = 149.99m, MaxCapacity = 2 },
                    new RoomType { Type = RoomTypeEnum.Suite, Name = "Suite", Description = "A luxurious suite with a separate living area", BasePrice = 299.99m, MaxCapacity = 4 },
                    new RoomType { Type = RoomTypeEnum.Deluxe, Name = "Deluxe Room", Description = "A premium room with extra amenities", BasePrice = 199.99m, MaxCapacity = 2 },
                    new RoomType { Type = RoomTypeEnum.Presidential, Name = "Presidential Suite", Description = "The most luxurious suite available", BasePrice = 499.99m, MaxCapacity = 6 }
                );

                await context.SaveChangesAsync();
                Console.WriteLine("Room types seeded.");
            }
        }

        private static async Task SeedRoomsAsync(HotelDbContext context)
        {
            if (!await context.Rooms.AnyAsync())
            {
                var roomTypes = await context.RoomTypes.ToListAsync();

                for (int floor = 1; floor <= 4; floor++)
                {
                    for (int room = 1; room <= 10; room++)
                    {
                        RoomTypeEnum roomType;
                        bool hasWifi = true, hasMinibar = false, hasBalcony = false;

                        switch (floor)
                        {
                            case 1: roomType = RoomTypeEnum.Single; break;
                            case 2: roomType = room <= 5 ? RoomTypeEnum.Double : RoomTypeEnum.Twin; hasMinibar = true; break;
                            case 3: roomType = RoomTypeEnum.Deluxe; hasMinibar = true; hasBalcony = room % 2 == 0; break;
                            case 4: roomType = room <= 8 ? RoomTypeEnum.Suite : RoomTypeEnum.Presidential; hasMinibar = true; hasBalcony = true; break;
                            default: roomType = RoomTypeEnum.Single; break;
                        }

                        var selectedRoomType = roomTypes.FirstOrDefault(rt => rt.Type == roomType);
                        if (selectedRoomType != null)
                        {
                            context.Rooms.Add(new Room
                            {
                                RoomNumber = $"{floor}{room:D2}",
                                Floor = floor,
                                RoomType = roomType,
                                RoomTypeId = selectedRoomType.Id,
                                BasePrice = selectedRoomType.BasePrice,
                                Capacity = selectedRoomType.MaxCapacity,
                                HasWifi = hasWifi,
                                HasMinibar = hasMinibar,
                                HasBalcony = hasBalcony,
                                Description = $"Room {floor}{room:D2} - {selectedRoomType.Description}"
                            });
                        }
                    }
                }

                await context.SaveChangesAsync();
                Console.WriteLine("Rooms seeded.");
            }
        }

        private static async Task SeedRoomStatusesAsync(HotelDbContext context)
        {
            if (!await context.RoomStatuses.AnyAsync())
            {
                var rooms = await context.Rooms.ToListAsync();
                var random = new Random();

                foreach (var room in rooms)
                {
                    context.RoomStatuses.Add(new RoomStatus
                    {
                        RoomId = room.Id,
                        Status = RoomStatusType.Available,
                        Date = DateTime.UtcNow.AddDays(-7), // Conversion UTC
                        Notes = "Initial status",
                        UpdatedBy = "System"
                    });

                    int randomStatus = random.Next(0, 10);

                    if (randomStatus < 3) context.RoomStatuses.Add(CreateRoomStatus(room.Id, RoomStatusType.Occupied));
                    else if (randomStatus < 5) context.RoomStatuses.Add(CreateRoomStatus(room.Id, RoomStatusType.Reserved));
                    else if (randomStatus < 6) context.RoomStatuses.Add(CreateRoomStatus(room.Id, RoomStatusType.UnderMaintenance));
                    else if (randomStatus < 7) context.RoomStatuses.Add(CreateRoomStatus(room.Id, RoomStatusType.CleaningInProgress));
                }

                await context.SaveChangesAsync();
                Console.WriteLine("Room statuses seeded.");
            }
        }

        private static async Task SeedGuestsAsync(HotelDbContext context)
        {
            if (!await context.Guests.AnyAsync())
            {
                context.Guests.AddRange(
                    new Guest 
                    { 
                        FirstName = "John", 
                        LastName = "Doe", 
                        Email = "john.doe@example.com", 
                        Phone = "123-456-7890", 
                        Address = "123 Main St", 
                        City = "New York", 
                        State = "NY", 
                        ZipCode = "10001", 
                        Country = "USA", 
                        IdentificationType = "Passport", 
                        IdentificationNumber = "AB123456",
                        DateOfBirth = new DateTime(1985, 5, 20).ToUniversalTime(),
                        Notes = "VIP customer"
                    },
                    new Guest 
                    { 
                        FirstName = "Jane", 
                        LastName = "Smith", 
                        Email = "jane.smith@example.com", 
                        Phone = "234-567-8901", 
                        Address = "456 Oak Ave", 
                        City = "Los Angeles", 
                        State = "CA", 
                        ZipCode = "90001", 
                        Country = "USA", 
                        IdentificationType = "Driver's License", 
                        IdentificationNumber = "DL789012",
                        DateOfBirth = new DateTime(1990, 8, 15).ToUniversalTime(),
                        Notes = "Regular visitor"
                    },
                    new Guest 
                    { 
                        FirstName = "Robert", 
                        LastName = "Johnson", 
                        Email = "robert.johnson@example.com", 
                        Phone = "345-678-9012", 
                        Address = "789 Pine Rd", 
                        City = "Chicago", 
                        State = "IL", 
                        ZipCode = "60601", 
                        Country = "USA", 
                        IdentificationType = "ID Card", 
                        IdentificationNumber = "ID345678",
                        DateOfBirth = new DateTime(1978, 12, 3).ToUniversalTime(),
                        Notes = "Prefers quiet rooms"
                    }
                );

                await context.SaveChangesAsync();
                Console.WriteLine("Guests seeded.");
            }
        }

        private static async Task SeedReservationsAsync(HotelDbContext context)
        {
            if (!await context.Reservations.AnyAsync())
            {
                // Execute each query separately to avoid concurrent operations
                var rooms = await context.Rooms.Take(5).ToListAsync();
                var guests = await context.Guests.ToListAsync();
                
                var random = new Random();

                foreach (var room in rooms)
                {
                    var guest = guests[random.Next(guests.Count)];
                    var checkInDate = DateTime.UtcNow; // Conversion UTC
                    var checkOutDate = checkInDate.AddDays(random.Next(1, 7));

                    context.Reservations.Add(new Reservation
                    {
                        RoomId = room.Id,
                        GuestId = guest.Id,
                        CheckInDate = checkInDate,
                        CheckOutDate = checkOutDate,
                        ActualCheckInDate = null, // Si la réservation n'est pas encore effectuée
                        ActualCheckOutDate = null, // Si la réservation n'est pas encore terminée
                        NumberOfGuests = random.Next(1, room.Capacity + 1),
                        TotalPrice = room.BasePrice * (checkOutDate - checkInDate).Days,
                        Deposit = null, // Valeur par défaut si pas de dépôt
                        IsPaid = false, // Non payé au départ
                        Status = "Confirmed",
                        Notes = "Demo reservation",
                        CreatedDate = DateTime.UtcNow, // Conversion UTC
                        CreatedBy = "System",
                        ModifiedDate = DateTime.UtcNow, // La date de modification est la même au départ
                        ModifiedBy = "System" // Valeur par défaut, pourrait être modifiée plus tard si nécessaire
                    });
                }

                await context.SaveChangesAsync();
                Console.WriteLine("Reservations seeded.");
            }
        }

        private static RoomStatus CreateRoomStatus(int roomId, RoomStatusType status)
        {
            return new RoomStatus
            {
                RoomId = roomId,
                Status = status,
                Date = DateTime.UtcNow, // Conversion UTC
                Notes = "Demo status",
                UpdatedBy = "System"
            };
        }

        // Synchronous version for backward compatibility
        public static void Initialize(HotelDbContext context)
        {
            try
            {
                // Call the async method and wait for it to complete
                InitializeAsync(context).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Initialize: {ex.Message}");
                throw;
            }
        }
    }
}