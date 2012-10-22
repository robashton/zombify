namespace Zombify.Server
{
    public class ZombificationRouteHandler  : System.Web.Routing.IRouteHandler
    {
        private TestListener listener;

        public ZombificationRouteHandler(TestListener listener)
        {
            this.listener = listener;
        }
        public System.Web.IHttpHandler GetHttpHandler(System.Web.Routing.RequestContext context) {
            return new ZombificationHttpHandler(listener);
        }
    }
}