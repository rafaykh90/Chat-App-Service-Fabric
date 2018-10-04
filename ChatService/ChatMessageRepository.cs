using Castle.Core.Configuration;
using ChatApplication.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.ChatService
{
	public interface IChatMessageRepository
	{
		Task AddMessage(ChatMessage message);
		Task<IEnumerable<ChatMessage>> GetTopMessages(int number = 100);
	}

	class ChatMessageRepository : IChatMessageRepository
	{
		private readonly string _chattableName;
		private readonly CloudTableClient _tableClient;
		private readonly IConfiguration _configuration;

		public ChatMessageRepository()
		{
			//_configuration = configuration;

			var accountName = "";
			var accountKey = "";
			_chattableName = "";

			var storageCredentials = new StorageCredentials(accountName, accountKey);
			var storageAccount = new CloudStorageAccount(storageCredentials, true);
			_tableClient = storageAccount.CreateCloudTableClient();
		}

		public async Task<IEnumerable<ChatMessage>> GetTopMessages(int number = 100)
		{
			var table = _tableClient.GetTableReference(_chattableName);

			// Create the table if it doesn't exist.
			await table.CreateIfNotExistsAsync();

			string filter = TableQuery.GenerateFilterCondition(
				"PartitionKey",
				QueryComparisons.Equal,
				"chatmessages");

			var query = new TableQuery<ChatMessageTableEntity>()
				.Where(filter)
				.Take(number);

			var entities = await table.ExecuteQuerySegmentedAsync(query, null);

			var result = entities.Results.Select(entity =>
				new ChatMessage
				{
					Id = entity.RowKey,
					Date = entity.Timestamp,
					Message = entity.Message,
					Sender = entity.Sender
				}).OrderBy(m => m.Date);

			return result;
		}

		public async Task AddMessage(ChatMessage message)
		{
			var table = _tableClient.GetTableReference(_chattableName);

			// Create the table if it doesn't exist.
			await table.CreateIfNotExistsAsync();

			var chatMessage = new ChatMessageTableEntity(message.Id)
			{
				Message = message.Message,
				Sender = message.Sender
			};

			TableOperation insertOperation = TableOperation.Insert(chatMessage);

			// Execute the insert operation.
			var result = await table.ExecuteAsync(insertOperation);
		}
	}
}
