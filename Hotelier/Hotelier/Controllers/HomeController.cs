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
		
		public HomeController(IContainRooms rooms) {
			this.rooms = rooms;
		}
		public ActionResult Index() {
			return this.View(rooms.FindAllRooms());
		}
	}
}

