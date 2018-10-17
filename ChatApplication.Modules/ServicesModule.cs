using Autofac;
using ChatApplication.ChatService;
using ChatApplication.UserService;
using System;

namespace ChatApplication.Modules
{
    public class ServicesModule : Module
    {
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<UserService.UserService>().As<IUserService>();
			builder.RegisterType<ChatService.ChatService>().As<IChatService>();
			base.Load(builder);
		}
	}
}
