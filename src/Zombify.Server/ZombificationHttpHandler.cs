using System;
using System.Collections.Generic;
using System.Reflection;

namespace Zombify.Server
{
    public class ZombificationHttpHandler : System.Web.IHttpHandler
    {
        private TestListener listener;
        public bool IsReusable { get { return false; } }

        public ZombificationHttpHandler(TestListener listener)
        {
            this.listener = listener;
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            var method = context.ApplicationInstance.GetType()
                .GetMethod("RetrieveZombieHandlers", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            if (method == null)
            {
                Console.WriteLine("No handlers found on application type {0}",
                                  context.ApplicationInstance.GetType().FullName);
                return;
            }

            Console.WriteLine("Checking for handlers");
            if(listener.HasHandlers()) return;
            Console.WriteLine("Scanning for handlers");
            var handlers = (IEnumerable<object>)method.Invoke(null, new object []{});
            foreach(var handler in handlers )
            {
                Console.WriteLine("Registering handler {0}", handler.GetType().FullName);
                this.listener.RegisterHandler(handler);
            }
        }
    }
}