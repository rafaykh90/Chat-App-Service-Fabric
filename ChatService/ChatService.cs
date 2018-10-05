using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatApplication.Models;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ChatApplication.ChatService
{
	public interface IChatService : IService
	{
		Task<IEnumerable<ChatMessage>> GetAllInitially();
		Task<ChatMessage> CreateNewMessage(string senderName, string message);
	}
	/// <summary>
	/// An instance of this class is created for each service instance by the Service Fabric runtime.
	/// </summary>
	public class ChatService : StatelessService, IChatService
    {
		private readonly IChatMessageRepository _repository;
		public ChatService(StatelessServiceContext context)
            : base(context)
        {
			_repository = new ChatMessageRepository();
		}

		public async Task<ChatMessage> CreateNewMessage(string senderName, string message)
		{
			var chatMessage = new ChatMessage(Guid.NewGuid())
			{
				Sender = senderName,
				Message = message
			};
			await _repository.AddMessage(chatMessage);

			return chatMessage;
		}

		public async Task<IEnumerable<ChatMessage>> GetAllInitially()
		{
			return await _repository.GetTopMessages();
		}

		/// <summary>
		/// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
		/// </summary>
		/// <returns>A collection of listeners.</returns>
		protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
