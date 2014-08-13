using System.Data.Entity;

namespace Console
{
	public class TestCtx : DbContext
	{
		public TestCtx () : base("testContextPg")
		{

		}

		public DbSet<TestDb> Tests { get; set; }

		protected override void OnModelCreating (DbModelBuilder modelBuilder)
		{
			base.OnModelCreating (modelBuilder);

			//Helps with Postgres public schema and lowercase column names
			/*modelBuilder.Entity<TestDb> ().ToTable ("test","public");
			modelBuilder.Entity<TestDb> ().Property (prop => prop.Id).HasColumnName("id");
			modelBuilder.Entity<TestDb> ().Property (prop => prop.Name).HasColumnName("name");;
			modelBuilder.Entity<TestDb> ().Property (prop => prop.Description).HasColumnName("description");
			modelBuilder.Entity<TestDb> ().Property (prop => prop.CreationDate).HasColumnName("creationdate");
			*/
		}
	}

}

