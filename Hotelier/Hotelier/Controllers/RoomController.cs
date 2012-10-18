using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Hotelier
{
	public class RoomController : Controller
	{
		IContainRooms rooms;
		
		public RoomController(IContainRooms rooms) {
			this.rooms = rooms;
		}
		
		public ActionResult Index(string id) {
			return View (rooms.GetRoomById(id));
		}
	}
	
 	public class RoomIndexInput {
		public string Id { get; set; }
	}
}

