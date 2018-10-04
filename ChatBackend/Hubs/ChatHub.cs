using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApplication.ChatService;
using Microsoft.AspNetCore.SignalR;

namespace ChatBackend.Hubs
{
	public class ChatHub : Hub
	{
		private readonly IChatService _chatService;
		private readonly IUserTracker _userTracker;

		public ChatHub(IChatService chatService, IUserTracker userTracker)
		{
			_userTracker = userTracker;
			_chatService = chatService;
		}

		/// <summary>
		/// Invoked from the ClientApp when user enters a message
		/// 'MessageAdded' callback is used to show the new message on all the clients 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public async Task AddMessage(string username, string message)
		{
			var chatMessage = await _chatService.CreateNewMessage(username, message);
			// Call the MessageAdded method to update clients.
			await Clients.All.SendAsync("MessageAdded", chatMessage);
		}

		public override Task OnConnectedAsync()
		{
			//Add Custom functionality here if required
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			_userTracker.RemoveUser(Context.ConnectionId);
			return base.OnDisconnectedAsync(exception);
		}

		/// <summary>
		/// Invoked from WebsocketService in ClientApp when the connection is successfully created
		/// </summary>
		/// <param name="username"></param>
		public void UserConnected(string username)
		{
			var id = Context.ConnectionId;
			_userTracker.AddUser(id, username);
		}
	}
}
