using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;
using HotelManagementSystem.Data.Context;

namespace HotelManagementSystem.Data.Repositories
{
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(HotelDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Room>> GetRoomsWithDetailsAsync()
        {
            return await _dbContext.Rooms
                .Include(r => r.RoomTypeDetails)
                .Include(r => r.StatusHistory)
                .Include(r => r.Reservations)
                .ToListAsync();
        }

        public async Task<Room> GetRoomWithDetailsAsync(int id)
        {
            return await _dbContext.Rooms
                .Include(r => r.RoomTypeDetails)
                .Include(r => r.StatusHistory)
                .Include(r => r.Reservations)
                    .ThenInclude(res => res.Guest)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime fromDate, DateTime toDate)
        {
            var rooms = await GetRoomsWithDetailsAsync();
            return rooms.Where(r => r.IsAvailable(fromDate, toDate)).ToList();
        }

        public async Task<IEnumerable<Room>> GetRoomsByStatusAsync(DateTime date, RoomStatusType status)
        {
            var rooms = await GetRoomsWithDetailsAsync();
            return rooms.Where(r => r.GetStatusForDate(date) == status).ToList();
        }

        public async Task<IEnumerable<Room>> GetRoomsByTypeAsync(RoomTypeEnum roomType)
        {
            return await _dbContext.Rooms
                .Include(r => r.RoomTypeDetails)
                .Where(r => r.RoomType == roomType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> SearchRoomsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetRoomsWithDetailsAsync();

            return await _dbContext.Rooms
                .Include(r => r.RoomTypeDetails)
                .Include(r => r.StatusHistory)
                .Where(r => r.RoomNumber.Contains(searchTerm) || 
                            r.Description.Contains(searchTerm) ||
                            r.RoomTypeDetails.Name.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<Dictionary<RoomStatusType, int>> GetRoomStatusSummaryAsync(DateTime date)
        {
            var rooms = await GetRoomsWithDetailsAsync();
            
            return rooms
                .GroupBy(r => r.GetStatusForDate(date))
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task UpdateRoomStatusAsync(int roomId, RoomStatusType status, DateTime date, string notes = null, string updatedBy = null)
        {
            var roomStatus = new RoomStatus
            {
                RoomId = roomId,
                Status = status,
                Date = date,
                Notes = notes,
                UpdatedBy = updatedBy
            };

            await _dbContext.RoomStatuses.AddAsync(roomStatus);
            await _dbContext.SaveChangesAsync();
        }
    }
}