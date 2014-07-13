using System.Data.Entity;

namespace Console
{
	public class TestCtx : DbContext
	{
		public TestCtx () : base("testContext")
		{

		}

		public DbSet<TestDb> Tests { get; set; }
	}

}

