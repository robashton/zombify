using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using Mono.WebServer;

namespace Zombify.Server
{
  using Mono.Security.Protocol.Tls;

	public class MainClass
	{
		public static void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = (Exception)e.ExceptionObject;

			Console.WriteLine ("Handling exception type {0}", ex.GetType ().Name);
			Console.WriteLine ("Message is {0}", ex.Message);
			Console.WriteLine ("IsTerminating is set to {0}", e.IsTerminating);
			if (e.IsTerminating)
				Console.WriteLine (ex);
		}

		public static int Main (string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler (CurrentDomain_UnhandledException);
      var server = new Server();
      return server.Run();
		}
	}
  
  public class Server : MarshalByRefObject
  {
    public int Run() {
      var port = Int32.Parse(System.Environment.GetEnvironmentVariable("PORT"));
      var dir = System.Environment.GetEnvironmentVariable("ROOT");
      var webSource = new XSPWebSource (IPAddress.Parse("0.0.0.0"), port, false);
			ApplicationServer server = new ApplicationServer (webSource, dir);
      server.SingleApplication = true;
      server.AddApplicationsFromCommandLine ("/:.");
			VPathToHost vh = server.GetSingleApp ();
			vh.CreateHost (server, webSource);
			server.AppHost = vh.AppHost;
		  if (server.Start (true, null, 500) == false)
        return 2;
      var listener = (TestListener)vh.AppHost.Domain.CreateInstanceFromAndUnwrap(
          GetType().Assembly.Location,
          typeof(TestListener).FullName);
      listener.Start(9000);

      bool doSleep;
      while (true) {
        doSleep = false;
        try {
          Console.ReadLine ();
          break;
        } catch (IOException) {
          // This might happen on appdomain unload
          // until the previous threads are terminated.
          doSleep = true;
        } catch (ThreadAbortException) {
          doSleep = true;
        }
        if (doSleep) {
          Thread.Sleep (500);
        }
      }

      server.Stop ();
  
      return 1337;
    }
  }

  // Register a route with System.Web.Routing
  // Implement an HTTP Handler which is returned by that route
  // HttpHandler can ask for the current Application
  // Look for the method 'GetZombieHandlers' on that type whatver it is
  // Execute that mother fo'

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
			listener.Start ();		
			listener.Prefixes.Add (String.Format("http://localhost:{0}/", port));
			listener.BeginGetContext(onBeginGetContext, listener);
      this.ScanForHandlers();
		}

    private void ScanForHandlers() {
      AppDomain.Current.
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
								.Where (x=> x.Name == methodName)
								.Where(x => x.GetParameters().Length == data.Children().Count())
								.FirstOrDefault();
			
			var args = new List<object>();

			foreach(var par in method.GetParameters()) {
				var val = data.GetValue(par.Name);
				var obj = JsonConvert.DeserializeObject(val.ToString(), par.ParameterType);
				args.Add (obj);
			}
			method.Invoke (handler, args.ToArray());
		}
	}
}
