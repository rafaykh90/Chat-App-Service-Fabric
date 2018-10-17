using Autofac;
using Autofac.Extensions.DependencyInjection;
using ChatApplication.ChatService;
using ChatApplication.Modules;
using ChatApplication.UserService;
using ChatBackend.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChatBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

		public IContainer ApplicationContainer { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
        {
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddMvc();
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAllOrigins",
				config => config.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials());
			});
			services.AddSignalR();

			var builder = new ContainerBuilder();
			builder.Populate(services);
			builder.RegisterModule(new ServicesModule());
			builder.RegisterType<UserTracker>().As<IUserTracker>();
			this.ApplicationContainer = builder.Build();

			// Create the IServiceProvider based on the container.
			return new AutofacServiceProvider(this.ApplicationContainer);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			app.UseCors("AllowAllOrigins");

			app.UseSignalR(routes =>
			{
				routes.MapHub<ChatHub>("/chat");
			});

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller}/{action=Index}/{id?}");
			});

			app.UseMvc();
        }
    }
}
