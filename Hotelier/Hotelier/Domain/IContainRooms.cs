using System;
using System.Collections.Generic;

namespace Hotelier
{
	public interface IContainRooms
	{
		IEnumerable<Room> FindAllRooms();
		Room GetRoomById(string id);
	}
}

