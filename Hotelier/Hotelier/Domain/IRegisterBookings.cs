using System;

namespace Hotelier
{
	public interface IRegisterBookings
	{
		Booking FindBookingForRoom(string roomid);
	}
}

