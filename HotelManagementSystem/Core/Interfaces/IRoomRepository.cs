using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.Core.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<IEnumerable<Room>> GetRoomsWithDetailsAsync();
        Task<Room> GetRoomWithDetailsAsync(int id);
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Room>> GetRoomsByStatusAsync(DateTime date, RoomStatusType status);
        Task<IEnumerable<Room>> GetRoomsByTypeAsync(RoomTypeEnum roomType);
        Task<IEnumerable<Room>> SearchRoomsAsync(string searchTerm);
        Task<Dictionary<RoomStatusType, int>> GetRoomStatusSummaryAsync(DateTime date);
        Task UpdateRoomStatusAsync(int roomId, RoomStatusType status, DateTime date, string notes = null, string updatedBy = null);
    }
}