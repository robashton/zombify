using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zombify.Server
{
    public class TestListener : MarshalByRefObject
    {
        private HttpListener listener;
        private Dictionary<string, object> handlers = new Dictionary<string, object>();
		
        public TestListener()
        {
            listener = new HttpListener();		
        }
		
        public void RegisterHandler(Object handler) {
            handlers.Add(handler.GetType().Name, handler);
        }
		
        public void Start(int port) 
        {
            listener.Prefixes.Add (String.Format("http://localhost:{0}/", port));
            listener.Start ();		
            listener.BeginGetContext(onBeginGetContext, listener);
            this.RegisterRouteOfDoom();
        }

        private void RegisterRouteOfDoom() {
            System.Web.Routing.RouteTable.Routes.Add(new System.Web.Routing.Route(
             "i/am/a/teapot",
             new ZombificationRouteHandler(this)
             ));
        }

        private void onBeginGetContext (IAsyncResult result)
        {
            var context = this.listener.EndGetContext(result);
            var request = context.Request;
            var response = context.Response;
            try {
                HandleRequest(request);				
                response.StatusCode = 200;
                using(var stream = response.OutputStream){}
            } catch(Exception ex) {
                response.StatusCode = 500;
                using(var stream = response.OutputStream) {
                    using(var writer = new StreamWriter(stream)) {
                        writer.Write (ex.ToString());
                    }
                }
            }			
            this.listener.BeginGetContext(onBeginGetContext, this.listener);
        }
		
        private void HandleRequest(HttpListenerRequest request) {
            var qs = request.QueryString;
            var methodInfoString = qs.Get("method");
            var data =  JObject.Parse(qs.Get("params"));
			
            var methodInfoComponents = methodInfoString.Split ('.');
            var type = methodInfoComponents[0];
            var methodName = methodInfoComponents[1];
            var handler = handlers[type];
            var method = handler.GetType().GetMethods()
                .Where (x=> x.Name == methodName).FirstOrDefault(x => x.GetParameters().Length == data.Children().Count());
			
            var args = new List<object>();

            foreach(var par in method.GetParameters()) {
                var val = data.GetValue(par.Name);
                var obj = JsonConvert.DeserializeObject(val.ToString(), par.ParameterType);
                args.Add (obj);
            }
            method.Invoke (handler, args.ToArray());
        }

        public bool HasHandlers()
        {
            return this.handlers.Any();
        }
    }
}