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
using Mono.Security.Protocol.Tls;
using Mono.WebServer;

namespace Zombify.Server
{
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
      var webSource = new XSPWebSource (IPAddress.Parse("0.0.0.0"), port, true);
			ApplicationServer server = new ApplicationServer (webSource, dir);
      server.AddApplicationsFromCommandLine ("/:.");
			VPathToHost vh = server.GetSingleApp ();
			vh.CreateHost (server, webSource);
			server.AppHost = vh.AppHost;
		  if (server.Start (true, null, 500) == false)
        return 2;

      var otherServerLol = (Server)server.AppHost.Domain.CreateInstanceFromAndUnwrap( 
         GetType().Assembly.Location, GetType().FullName);
      server.AppHost.Domain.AssemblyResolve += new ResolveEventHandler(otherServerLol.ResolveAssembly);
      AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.ResolveAssembly);

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

    private Assembly ResolveAssembly(object sender, ResolveEventArgs e) {
      Console.WriteLine(Assembly.GetExecutingAssembly().ToString());
      return null;
    }
  }
}
