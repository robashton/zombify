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
using System.IO;
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
      private string dir;
      private int port;

      public void CopyBinariesToBin()
      {
          var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
          var destpath = Path.Combine(dir, "bin");
          try
          {
              File.Copy(Path.Combine(path, "Zombify.Server.exe"), Path.Combine(destpath, "Zombify.Server.exe"), true);
              File.Copy(Path.Combine(path, "Mono.WebServer.dll"), Path.Combine(destpath, "Mono.WebServer.exe"), true);
              File.Copy(Path.Combine(path, "Mono.Security.dll"), Path.Combine(destpath, "Mono.Security.dll"), true);
              File.Copy(Path.Combine(path, "Newtonsoft.Json.dll"), Path.Combine(destpath, "Newtonsoft.Json.dll"), true); } catch (Exception ex)
          {
              Console.WriteLine(ex.ToString());
          }
      }
      public int Run()
      {
          port = Int32.Parse(System.Environment.GetEnvironmentVariable("PORT"));
          dir = System.Environment.GetEnvironmentVariable("ROOT");
          CopyBinariesToBin();
          var webSource = new XSPWebSource(IPAddress.Parse("0.0.0.0"), port, false);

          var server = new ApplicationServer(webSource, dir);
          server.SingleApplication = true;
          server.AddApplicationsFromCommandLine("/:.");
          var vh = server.GetSingleApp();
          vh.CreateHost(server, webSource);
          server.AppHost = vh.AppHost;
          if (server.Start(true, null, 500) == false)
              return 2;

          var listener = (TestListener) vh.AppHost.Domain.CreateInstanceFromAndUnwrap(
              GetType().Assembly.Location, typeof (TestListener).FullName);
          listener.Start(9000);

          bool doSleep;
          while (true)
          {
              doSleep = false;
              try
              {
                  Console.ReadLine();
                  break;
              }
              catch (IOException)
              {
                  // This might happen on appdomain unload
                  // until the previous threads are terminated.
                  doSleep = true;
              }
              catch (ThreadAbortException)
              {
                  doSleep = true;
              }
              if (doSleep)
              {
                  Thread.Sleep(500);
              }
          }
          server.Stop();
          return 1337;
      }
  }
}
