using System.Web.Http;
using System.Net.Http;
using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;

namespace Console
{
	[RoutePrefix("api/Test")]
	public class TestController : ApiController
	{
		[HttpGet]
		[Route("{name:maxlength(10)}")]
		public HttpResponseMessage GetTest(string name = "Default")
		{
			TestDb test = null;

			using (var ctx = new TestCtx ())
			{
				test = ctx.Tests.SingleOrDefault (x => x.Name == name);
			}

			if (test == null)
				return Request.CreateErrorResponse (HttpStatusCode.NotFound, name + " does not exists.");

			var testResponse = new TestResponse {
				Count = test.Name.Length,
				Date = test.CreationDate,
				Name = test.Name,
				Description = test.Description
			};

			return Request.CreateResponse (HttpStatusCode.OK, testResponse);
		}

		[HttpGet]
		[Route()]
		public HttpResponseMessage GetAll(){

			using (var ctx = new TestCtx ())
			{
				var all = ctx.Tests.ToList().Select(x => new TestResponse {
					Count = x.Name.Length,
					Date = x.CreationDate,
					Name = x.Name,
					Description = x.Description
				});

				return Request.CreateResponse (HttpStatusCode.OK, all);
			}
		}

		[HttpPost]
		[Route()]
		public HttpResponseMessage Post(TestPostDto dto)
		{
			TestDb test = null;

			if(string.IsNullOrWhiteSpace (dto.Name) || dto.Name.Any (x => x == ' '))
				return Request.CreateErrorResponse (HttpStatusCode.BadRequest, "Name should not have white spaces.");
			
			if(dto.Name.Length > 10)
				return Request.CreateErrorResponse (HttpStatusCode.BadRequest, "Name should not exceed 10 length.");

			using (var ctx = new TestCtx ()) {

				test = ctx.Tests.SingleOrDefault (x => x.Name == dto.Name);

				if (test != null)
					return Request.CreateErrorResponse (HttpStatusCode.BadRequest, dto.Name + " already exists.");

				test = new TestDb {
					Id = Guid.NewGuid(),
					Name = dto.Name.ToLower(),
					CreationDate = DateTime.Now,
					Description = dto.Description
				};

				ctx.Tests.Add (test);
				ctx.SaveChanges ();
			}

			return Request.CreateResponse (System.Net.HttpStatusCode.Created, test);
		}

	}
}

