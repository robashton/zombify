using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Hotelier
{
	public class HomeController : Controller
	{
		IContainRooms rooms;
		IRegisterBookings bookings;
		
		public HomeController(IContainRooms rooms, IRegisterBookings bookings) {
			this.rooms = rooms;
			this.bookings = bookings;
		}
		
		public ActionResult Index() {
			return this.View(rooms.FindAllRooms()
             	.Select (room => new HomeView() {
					Id = room.Id,
					Number = room.Number,
					Booked = bookings.FindBookingForRoom(room.Id) != null
				})
			    .ToArray());
		}
	}
	
	public class HomeView 
	{
		public string Id;
		public int Number;
		public bool Booked;
	}
}

