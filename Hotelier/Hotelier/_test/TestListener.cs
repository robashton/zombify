using System;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hotelier
{
	public class TestListener
	{
		private HttpListener listener;
		private Dictionary<string, object> handlers = new Dictionary<string, object>();
		
		public TestListener()
		{
			listener = new HttpListener();		
			listener.Prefixes.Add ("http://localhost:9000/");
		}
		
		public void RegisterHandler(Object handler) {
			handlers.Add(handler.GetType().Name, handler);
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
			throw new Exception("Erm" + qs.GetKey(1) + String.Join (" , ", qs.GetValues(1)));
			var data =  JObject.Parse(qs.Get("params"));
			
			/*
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
			*/
		}
	}
}

