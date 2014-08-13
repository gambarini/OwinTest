using Owin;
using System.Web.Http;
using System.Data.Entity;
using Nancy;

namespace Console
{

	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			StaticConfiguration.DisableErrorTraces = false;

			app.Use ((ctx, next) => {
				ctx.TraceOutput.WriteLine(ctx.Request.RemoteIpAddress);

				return next.Invoke();
			});

			HttpConfiguration config = new HttpConfiguration ();
			config.MapHttpAttributeRoutes ();
			config.EnsureInitialized ();

			app.UseWebApi (config);
			app.UseNancy ();

			//Database.SetInitializer<TestCtx>(null);
		}
	} 
}
