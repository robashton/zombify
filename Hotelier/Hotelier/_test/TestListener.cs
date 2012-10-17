using System;
using System.Net;
using System.Web;

namespace Hotelier
{
	public class TestListener
	{
		private HttpListener listener;
		
		public TestListener()
		{
			listener = new HttpListener();		
			listener.Prefixes.Add ("http://localhost:9000/");
		}
		
		public void Start() 
		{
			listener.Start ();		
			listener.BeginGetContext(onBeginGetContext, listener);
		}
		
		private void onBeginGetContext (IAsyncResult result)
		{
			var context = this.listener.EndGetContext(result);
			var request = context.Request;
			var response = context.Response;
		 	HandleRequest(request);
		    response.StatusCode = 200;
			using(var stream = response.OutputStream){}
			this.listener.BeginGetContext(onBeginGetContext, this.listener);
		}
		
		private void HandleRequest(HttpWebRequest request) {
			var qs = HttpUtility.ParseQueryString(request.RequestUri.Query);
			var method = qs.Get("method");
			var data = qs.Get("data");
		}
	}
}

