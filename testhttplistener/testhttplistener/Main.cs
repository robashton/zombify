using System;
using System.Net;

namespace testhttplistener
{
	class MainClass
	{
		public static void onBeginGetContext (IAsyncResult result)
		{
			var listener = (HttpListener)result.AsyncState;
			var context = listener.EndGetContext(result);
			var request = context.Request;
			var response = context.Response;
			response.StatusCode = 200;
			using(var stream = response.OutputStream){}
			listener.BeginGetContext(onBeginGetContext, listener);
		}

		public static void onEndGetContext (Object context)
		{
			Console.WriteLine(context);
		}
		
		public static void Main (string[] args)
		{
			var listener = new HttpListener();
			
			listener.Prefixes.Add ("http://localhost:9000/");
			listener.Start ();		
			listener.BeginGetContext(onBeginGetContext, listener);
			
			while(true) 
				System.Threading.Thread.Sleep (1000);
		}
	}
}
