using System.Web.Mvc;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Hotelier
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private TestListener listener = new TestListener();
        private WindsorContainer container = new WindsorContainer();

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
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterComponents();
            RegisterTestHooks();
            ControllerBuilder.Current.SetControllerFactory(container.Resolve<IControllerFactory>());
            listener.Start();
        }

        private void RegisterTestHooks()
        {
            var hotel = container.Resolve<IContainRooms>();

            // This is now an IPC object
            listener.RegisterHandler(hotel);
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