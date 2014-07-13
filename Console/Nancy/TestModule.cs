using Nancy;
using Nancy.ModelBinding;
using System.Linq;
using System;
using System.Dynamic;

namespace Console
{
	public class TestModule : NancyModule
	{
		public TestModule () : base("/nancy/test")
		{
			Get[""] = _ => {

				using (var ctx = new TestCtx ())
				{
					var all = ctx.Tests.ToList().Select(x => new TestResponse {
						Count = x.Name.Length,
						Date = x.CreationDate,
						Name = x.Name,
						Description = x.Description
					});

					return Negotiate.WithModel(all).WithStatusCode(HttpStatusCode.OK);
				}
			};

			Get ["{name}"] = parameters => {

				string name = parameters.name;

				TestDb test = null;

				using (var ctx = new TestCtx ())
				{
					test = ctx.Tests.SingleOrDefault (x => x.Name == name);
				}

				if (test == null)
					return Negotiate.WithModel(new MessageResponse { Message = parameters.name + " does not exists." }).WithStatusCode(HttpStatusCode.NotFound);

				var testResponse = new TestResponse {
					Count = test.Name.Length,
					Date = test.CreationDate,
					Name = test.Name,
					Description = test.Description
				};

				return Negotiate.WithModel(testResponse).WithStatusCode(HttpStatusCode.OK);
			};

			Post [""] = parameters => {
				var dto = this.Bind<TestPostDto>();

				TestDb test = null;

				if(string.IsNullOrWhiteSpace (dto.Name) || dto.Name.Any (x => x == ' '))
					return Negotiate.WithModel(new MessageResponse { Message = "Name should not have white spaces." }).WithStatusCode(HttpStatusCode.BadRequest);
					
				if(dto.Name.Length > 10)
					return Negotiate.WithModel(new MessageResponse { Message = "Name should not exceed 10 length." }).WithStatusCode(HttpStatusCode.BadRequest);
					
				using (var ctx = new TestCtx ()) {

					test = ctx.Tests.SingleOrDefault (x => x.Name == dto.Name);

					if (test != null)
						return Negotiate.WithModel(new MessageResponse { Message = dto.Name + " already exists." }).WithStatusCode(HttpStatusCode.BadRequest);
					
					test = new TestDb {
						Id = Guid.NewGuid(),
						Name = dto.Name.ToLower(),
						CreationDate = DateTime.Now,
						Description = dto.Description
					};

					ctx.Tests.Add (test);
					ctx.SaveChanges ();
				}

				return Negotiate.WithModel(test).WithStatusCode(HttpStatusCode.Created);
			};
		}
	}
}

