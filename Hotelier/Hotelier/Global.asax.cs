using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Hotelier
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static WindsorContainer container = new WindsorContainer();

        public static IEnumerable<object> RetrieveZombieHandlers()
        {
            yield return container.Resolve<IContainRooms>();
        }

        public MvcApplication()
        {
            Console.WriteLine("Constructing MvcApplication");
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            Console.WriteLine("Application started");
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterComponents();
            ControllerBuilder.Current.SetControllerFactory(container.Resolve<IControllerFactory>());
        }


        private void RegisterComponents()
        {
            container.Register(
                Component.For<IControllerFactory>()
                .ImplementedBy<WindsorControllerFactory>());

            container.Register(
                Component.For<IController>()
                .ImplementedBy<HomeController>()
                .Named("homecontroller")
                .LifestyleTransient());

            container.Register(
                Component.For<IController>()
                .ImplementedBy<RoomController>()
                .Named("roomcontroller")
                .LifestyleTransient());

            container.Register(
                Component.For<IContainRooms, IRegisterBookings>()
                .ImplementedBy<InMemoryHotel>()
                );

        }
    }
}