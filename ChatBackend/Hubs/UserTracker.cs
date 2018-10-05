using ChatApplication.Models;
using ChatApplication.UserService;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBackend.Hubs
{
	public interface IUserTracker
	{
		IEnumerable<UserDetails> UsersOnline();
		void AddUser(string sid, string name);
		void RemoveUser(string sid);
	}

	public class UserTracker : IUserTracker
	{
		//private static ConcurrentBag<UserDetails> joinedUsers = new ConcurrentBag<UserDetails>();
		private readonly IHubContext<ChatHub> _chatHubContext;
		private readonly IUserService _userService;

		public UserTracker(IHubContext<ChatHub> chatHubContext, IUserService userService)
		{
			_chatHubContext = chatHubContext;
			_userService = userService;
		}

		public void AddUser(string sid, string name)
		{
			//if (!joinedUsers.Any(x => x.Id == sid))
			//{
			//	var user = new UserDetails
			//	{
			//		Id = sid,
			//		Name = name
			//	};
			//	joinedUsers.Add(user);
			//}

			var user = _userService.AddUser(sid, name);
			if(user != null)
				_chatHubContext.Clients.All.SendAsync("UserLoggedOn", user);
		}

		public void RemoveUser(string sid)
		{
			//var user = joinedUsers.FirstOrDefault(x => x.Id == sid);
			//if (user != null)
			//{
			//	joinedUsers.ToList().Remove(user);
			//}

			var user = _userService.RemoveUser(sid);
			if(user != null)
				_chatHubContext.Clients.All.SendAsync("UserLoggedOff", user);
		}

		public IEnumerable<UserDetails> UsersOnline()
		{
			//return joinedUsers;
			return _userService.UsersOnline();
		}
	}
}
