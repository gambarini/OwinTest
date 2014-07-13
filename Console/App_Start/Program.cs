using Owin;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using System.Data.Entity;

namespace Console
{

	class MainClass
	{
		const string HttpLocalHost = "http://localhost:9000";

		public static void Main (string[] args)
		{
			using (WebApp.Start<Startup> (HttpLocalHost))
			{
				System.Console.WriteLine("Listening on " + HttpLocalHost);
				System.Console.WriteLine("Press [enter] to quit...");
				System.Console.ReadLine();
			}
		}
	}

	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			HttpConfiguration config = new HttpConfiguration ();
			config.MapHttpAttributeRoutes ();
			config.EnsureInitialized ();

			app.UseWebApi (config);

			Database.SetInitializer<TestCtx>(null);
		}
	} 
}
