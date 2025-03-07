using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelManagementSystem.Core.Enums;
using HotelManagementSystem.Core.Interfaces;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.Core.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            return await _roomRepository.GetRoomsWithDetailsAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _roomRepository.GetRoomWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime fromDate, DateTime toDate)
        {
            return await _roomRepository.GetAvailableRoomsAsync(fromDate, toDate);
        }

        public async Task<IEnumerable<Room>> GetRoomsByStatusAsync(DateTime date, RoomStatusType status)
        {
            return await _roomRepository.GetRoomsByStatusAsync(date, status);
        }

        public async Task<IEnumerable<Room>> GetRoomsByTypeAsync(RoomTypeEnum roomType)
        {
            return await _roomRepository.GetRoomsByTypeAsync(roomType);
        }

        public async Task<IEnumerable<Room>> SearchRoomsAsync(string searchTerm)
        {
            return await _roomRepository.SearchRoomsAsync(searchTerm);
        }

        public async Task<Dictionary<RoomStatusType, int>> GetRoomStatusSummaryAsync(DateTime date)
        {
            return await _roomRepository.GetRoomStatusSummaryAsync(date);
        }

        public async Task<Room> AddRoomAsync(Room room)
        {
            var addedRoom = await _roomRepository.AddAsync(room);
            await _roomRepository.SaveChangesAsync();
            return addedRoom;
        }

        public async Task UpdateRoomAsync(Room room)
        {
            await _roomRepository.UpdateAsync(room);
            await _roomRepository.SaveChangesAsync();
        }

        public async Task DeleteRoomAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room != null)
            {
                await _roomRepository.DeleteAsync(room);
                await _roomRepository.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Room with ID {id} not found.");
            }
        }

        public async Task<bool> UpdateRoomStatusAsync(int roomId, RoomStatusType status, DateTime date, string notes = null, string updatedBy = null)
        {
            var room = await _roomRepository.GetByIdAsync(roomId);
            if (room == null)
                return false;

            await _roomRepository.UpdateRoomStatusAsync(roomId, status, date, notes, updatedBy);
            return true;
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime fromDate, DateTime toDate)
        {
            var room = await _roomRepository.GetRoomWithDetailsAsync(roomId);
            if (room == null)
                return false;

            return room.IsAvailable(fromDate, toDate);
        }
    }
}