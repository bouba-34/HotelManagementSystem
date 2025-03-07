using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.Core.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task<Room> GetRoomByIdAsync(int id);
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Room>> GetRoomsByStatusAsync(DateTime date, RoomStatusType status);
        Task<IEnumerable<Room>> GetRoomsByTypeAsync(RoomTypeEnum roomType);
        Task<IEnumerable<Room>> SearchRoomsAsync(string searchTerm);
        Task<Dictionary<RoomStatusType, int>> GetRoomStatusSummaryAsync(DateTime date);
        Task<Room> AddRoomAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task DeleteRoomAsync(int id);
        Task<bool> UpdateRoomStatusAsync(int roomId, RoomStatusType status, DateTime date, string notes = null, string updatedBy = null);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime fromDate, DateTime toDate);
    }
}