using System;

namespace Hotelier
{
	public class Room
	{
		public readonly string Id;
		public readonly int Number;
		public readonly int Capacity;
		
		public Room (string id, int number, int capacity)
		{
			this.Id = id;
			this.Number = number;
			this.Capacity = capacity;
		}
	}
}

