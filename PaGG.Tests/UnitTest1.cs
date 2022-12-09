using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.HttpSys;
using Moq;
using Newtonsoft.Json;
using PaGG.Backstage;
using PaGG.Business;
using PaGG.Controllers;
using PaGG.Core.Models;
using System.Collections;
using System.IO;
using System.Text;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace PaGG.Tests
{
	public class SetRequestContentTest
	{
		private readonly AccountsController _target;
		private readonly AccountOperations _accountOperations;
		private readonly DatabaseOperations _databaseOperations;
		private Mock<HttpRequest> _mockRequest;

		public SetRequestContentTest()
		{
			var mockContext = new Mock<HttpContext>();
			_mockRequest = new Mock<HttpRequest>();
			
			mockContext.Setup(context => context.Request).Returns(_mockRequest.Object);

			_databaseOperations = new DatabaseOperations();
			_accountOperations = new AccountOperations(_databaseOperations);
			_target = new AccountsController(_accountOperations)
			{
				ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext() { HttpContext = new DefaultHttpContext() }
			};
		}

		[Fact]
		public async Task TryingToSetRequestContentAsync()
		{
			var account = new Account() { AccountOwner = "guerra" };
			
			var json = JsonConvert.SerializeObject(account);
			var byteArray = Encoding.ASCII.GetBytes(json);
			var memoryStream = new MemoryStream(byteArray);

			_target.Request.Body = memoryStream;
			//var myCookies = new MyCookieCollection(new Dictionary<string, string>() { { "aba", "caba" } });
			//_mockRequest.Setup(req => req.Cookies).Returns(myCookies);
			//_mockRequest.Setup(req => req.Body).Returns(memoryStream);

			var result = await _target.PerformTestApi();
			var resultAsString = (string)result.Value;

			Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
			Assert.Contains("guerra", resultAsString);
		}
	}

	public class MyCookieCollection : IRequestCookieCollection
	{
		private Dictionary<string, string> cookies;

		public MyCookieCollection()
		{
			cookies = new Dictionary<string, string>();
		}

		public MyCookieCollection(Dictionary<string, string> cookies)
		{
			this.cookies = cookies;
		}

		string? IRequestCookieCollection.this[string key]
			=> cookies.ContainsKey(key) ? cookies[key] : null;

		public int Count => cookies.Count;

		public ICollection<string> Keys => cookies.Keys;

		public bool ContainsKey(string key) => cookies.ContainsKey(key);

		public bool TryGetValue(string key, out string? value)
		{
			return cookies.TryGetValue(key, out value);
		}

		IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
		{
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}