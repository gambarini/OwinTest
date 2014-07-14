using Owin;
using System.Web.Http;
using System.Data.Entity;

namespace Console
{

	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			HttpConfiguration config = new HttpConfiguration ();
			config.MapHttpAttributeRoutes ();
			config.EnsureInitialized ();

			app.UseWebApi (config);
			app.UseNancy ();

			Database.SetInitializer<TestCtx>(null);
		}
	} 
}
