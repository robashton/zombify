using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace Hotelier
{
	public class MvcApplication : System.Web.HttpApplication
	{
		private TestListener listener = new TestListener();
		private WindsorContainer container = new WindsorContainer();
			
		public static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

			routes.MapRoute (
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" }
            );

		}

		protected void Application_Start ()
		{
			RegisterRoutes (RouteTable.Routes);
			RegisterComponents();
			RegisterTestHooks();
			ControllerBuilder.Current.SetControllerFactory(container.Resolve<IControllerFactory>());
			listener.Start();
		}
		
		private void RegisterTestHooks() {
			var hotel = container.Resolve<IContainRooms>();
			
			// This is now an IPC object
			listener.RegisterHandler(hotel);
		}
		
		private void RegisterComponents() {
			
			container.Register (
				Component.For<IControllerFactory>()
				.ImplementedBy<WindsorControllerFactory>());
			
			container.Register (
				Component.For<IController>()
				.ImplementedBy<HomeController>()
				.Named ("homecontroller")
				.LifestyleTransient());
			
			container.Register (
				Component.For <IController>()
				.ImplementedBy<RoomController>()
				.Named ("roomcontroller")
				.LifestyleTransient());
	
			container.Register(
				Component.For<IContainRooms, IRegisterBookings>()
				.ImplementedBy<InMemoryHotel>()
				);
			
		}
	}
}
