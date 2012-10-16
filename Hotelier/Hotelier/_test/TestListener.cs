using System;
using System.Net;

namespace Hotelier
{
	public class TestListener
	{
		private HttpListener listener;
		
		public TestListener ()
		{
			listener = new HttpListener();
			listener.BeginGetContext(this.onStartRequest);
			listener.EndGetContext(this.onEndRequest);
		}

		public void onStartRequest (object onStartRequest)
		{
			throw new NotImplementedException ();
		}

		public void onEndRequest (object onEndRequest)
		{
			throw new NotImplementedException ();
		}
	}
}

