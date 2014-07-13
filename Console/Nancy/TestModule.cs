using Nancy;
using Nancy.ModelBinding;
using System.Linq;
using System;

namespace Console
{
	public class TestModule : NancyModule
	{
		public TestModule ()
		{
			Get["/nancy/test"] = _ => {

				using (var ctx = new TestCtx ())
				{
					var all = ctx.Tests.ToList().Select(x => new TestResponse {
						Count = x.Name.Length,
						Date = x.CreationDate,
						Name = x.Name,
						Description = x.Description
					});

					return all;
				}
			};

			Get ["/nancy/test/{name}"] = parameters => {

				string name = parameters.name;

				TestDb test = null;

				using (var ctx = new TestCtx ())
				{
					test = ctx.Tests.SingleOrDefault (x => x.Name == name);
				}

				if (test == null)
					return parameters.name + " does not exists.";

				var testResponse = new TestResponse {
					Count = test.Name.Length,
					Date = test.CreationDate,
					Name = test.Name,
					Description = test.Description
				};

				return testResponse;
			};

			Post ["/nancy/test"] = parameters => {
				var dto = this.Bind<TestPostDto>();

				TestDb test = null;

				if(string.IsNullOrWhiteSpace (dto.Name) || dto.Name.Any (x => x == ' '))
					return "Name should not have white spaces.";

				if(dto.Name.Length > 10)
					return "Name should not exceed 10 length.";

				using (var ctx = new TestCtx ()) {

					test = ctx.Tests.SingleOrDefault (x => x.Name == dto.Name);

					if (test != null)
						return dto.Name + " already exists.";

					test = new TestDb {
						Id = Guid.NewGuid(),
						Name = dto.Name.ToLower(),
						CreationDate = DateTime.Now,
						Description = dto.Description
					};

					ctx.Tests.Add (test);
					ctx.SaveChanges ();
				}

				return test;
			};
		}
	}
}

