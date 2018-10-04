﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApplication.ChatService;
using ChatApplication.Models;
using ChatBackend.Hubs;
using Microsoft.AspNetCore.Mvc;

namespace ChatBackend.Controllers
{
	[Route("api/[controller]")]
	public class ChatController : Controller
	{
		private readonly IChatService _chatService;
		private readonly IUserTracker _userTracker;

		public ChatController(IChatService chatService, IUserTracker userTracker)
		{
			_chatService = chatService;
			_userTracker = userTracker;
		}

		[HttpGet("[action]")]
		public IEnumerable<UserDetails> LoggedOnUsers()
		{
			return _userTracker.UsersOnline();
		}

		[HttpGet("[action]")]
		public async Task<IEnumerable<ChatMessage>> InitialMessages()
		{
			return await _chatService.GetAllInitially();
		}
	}
}
