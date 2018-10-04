using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;
using ChatApplication.Models;
using ChatApplication.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;

namespace ChatBackend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
		private readonly IUserService _userService;
		//private readonly FabricClient fabricClient;
		//private readonly string reverseProxyBaseUri;
		//private readonly StatelessServiceContext serviceContext;

		public ValuesController(/*StatelessServiceContext context, FabricClient fabricClient*/IUserService userService)
		{
			_userService = userService;
			//this.fabricClient = fabricClient;
			//this.serviceContext = context;
			//this.reverseProxyBaseUri = "http://localhost:19081/";//Environment.GetEnvironmentVariable("ReverseProxyBaseUri");
		}

		// GET api/values
		[HttpGet]
        public IEnumerable<UserDetails> Get()
        {
			//Uri serviceName = ChatBackend.GetUserServiceName(this.serviceContext);
			//Uri proxyAddress = this.GetProxyAddress(serviceName);
			//var service = new ServiceProxyFactory().CreateServiceProxy<IUserService>(proxyAddress);

			return _userService.UsersOnline();
		}

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

		//private Uri GetProxyAddress(Uri serviceName)
		//{
		//	return new Uri($"{this.reverseProxyBaseUri}{serviceName.AbsolutePath}");
		//}
	}
}
