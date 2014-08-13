using Microsoft.Owin.Hosting;

namespace Console
{

	class MainClass
	{
		const string HttpLocalHost = "http://*:9000";

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

}
