using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatApplication.Models;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using StackExchange.Redis;

namespace ChatApplication.UserService
{
	public interface IUserService : IService
	{
		IEnumerable<UserDetails> UsersOnline();
		UserDetails AddUser(string sid, string name);
		UserDetails RemoveUser(string sid);
	}

	/// <summary>
	/// An instance of this class is created for each service instance by the Service Fabric runtime.
	/// </summary>
	public class UserService : StatelessService, IUserService, IDisposable
	{
		private readonly IDatabase _cache;
		public UserService(StatelessServiceContext context)
			: base(context)
		{
			_cache = RedisConnector.Connection.GetDatabase();
		}

		public UserDetails AddUser(string sid, string name)
		{
			if (!_cache.HashExists("Users", $"{sid}"))
			{
				var user = new UserDetails
				{
					Id = sid,
					Name = name
				};
				_cache.HashSet("Users", sid, name);
				return user;
			}
			return null;
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public UserDetails RemoveUser(string sid)
		{
			string name = _cache.HashGet("Users", sid);
			if (!string.IsNullOrEmpty(name))
			{
				var user = new UserDetails
				{
					Id = sid,
					Name = name
				};
				_cache.HashDelete("Users", sid);
				return user;
			}
			return null;
		}

		public IEnumerable<UserDetails> UsersOnline()
		{
			List<UserDetails> users = new List<UserDetails>();
			users = _cache.HashGetAll("Users")?.Select(u => new UserDetails()
			{
				Id = u.Name,
				Name = u.Value
			}).ToList();

			return users;
		}

		/// <summary>
		/// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
		/// </summary>
		/// <returns>A collection of listeners.</returns>
		protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
		{
			//return this.CreateServiceRemotingInstanceListeners();
			return new ServiceInstanceListener[0];
			//return new[]
			//{
			//	new ServiceInstanceListener(context =>
			//		new FabricTransportServiceRemotingListener(context, this))
			//};
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
