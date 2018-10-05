using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;
using ChatApplication.ChatService;
using ChatApplication.Models;
using ChatApplication.UserService;
using ChatBackend.Hubs;
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
		private readonly IChatService _chatService;
		private readonly IUserTracker _userTracker;

		public ValuesController(IUserService userService, IChatService chatService, IUserTracker userTracker)
		{
			_userService = userService;
			_chatService = chatService;
			_userTracker = userTracker;
		}

		// GET api/values
		[HttpGet]
        public IEnumerable<ChatMessage> Get()
        {
			return _chatService.GetAllInitially().Result;
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
	}
}
