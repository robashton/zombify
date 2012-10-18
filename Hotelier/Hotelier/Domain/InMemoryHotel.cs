using System;
using System.Collections.Generic;
using System.Linq;

namespace Hotelier
{
	public class InMemoryHotel : IContainRooms
	{
		private Dictionary<string, Room> rooms = new Dictionary<string, Room>();
		
		public InMemoryHotel ()
		{
			
		}
		
		public void AddHotelRoom(string id, int number) {
			this.AddHotelRoom(id, number, 2);
		}
		
		public void AddHotelRoom(string id, int number, int capacity) {
			this.rooms[id] = new Room(id, number, capacity);		
		}
		
		public IEnumerable<Room> FindAllRooms() {
			return this.rooms.Values.ToArray();
		}
		
		public Room GetRoomById(string id) {
			return rooms[id];
		}
	}
}

