# Chat Application Backend on Service Fabric

This application has been converted from the Chat-App-React-Spa to be used as a Service Fabric Application.

### Services
- ##### UserService
    
    A Stateless service to keep track or Users. This service implements the interface ``IUserService`` which is injeceted in the Controllers to used in other places.
    ###### Methods
    - `UsersOnline();`
    - `AddUser(string sid, string name)`
    - `RemoveUser(string sid)`


- ##### ChatService
    
    A Stateless service to perform In-Chat operations. This service implements the interface ``IChatService`` which is injeceted in the Controllers to used in other places.
    ###### Methods
    - `GetAllInitially()`
    - `CreateNewMessage(string senderName, string message)`


- ##### ChatBackend
    
    A Stateless .Net Core Backend having API Controllers and SignalR Hub (ChatHub). This is similar to the standard .Net Core Web Api.

*For Further Information of the methods and API refer this [documentation](www.github.com/rafaykh90)*


### Differences from the Actual App
This app follow the IoC principle in order to break the application into Smaller services to be used in the Service Fabric cluster. To ensure the IoC the application uses a famour library *Autofac*. IoC is ensured via various patters e.g. Service Locater, Delegates etc. This application uses the DI pattern which is already very common in .Net Core Framework.

### Autofac
In this project Autofac is used in a different way in the Stateless Services. `UserService` as well as `ChatService` uses the [Autofac.ServiceFabric](https://www.nuget.org/packages/Autofac.ServiceFabric) package which is a library specifically created to be used with the Service Fabric services.

The first step is to install the `Autofac.ServiceFabric` package into your service project and create a `ContainerBuilder` to register your services in the regular manner and then extension methods are called on the ContainerBuilder to register some internal Autofac types that will automatically provide the appropriate factory functions to the Service Fabric runtime.

Example `Program.cs` for `UserService`:
```
// Start with the trusty old container builder.
var builder = new ContainerBuilder();

// Register the Autofac magic for Service Fabric support.
builder.RegisterServiceFabricSupport();

// Register the stateless service.
builder.RegisterStatelessService<UserService>("UserServiceType");

using (builder.Build())
{
	    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(UserService).Name);

		// Prevents this host process from terminating so services keep running.
	    Thread.Sleep(Timeout.Infinite);
}
```

This creates the IoC container for the Stateless Service. Notice the `RegisterStatelessService` extension method, which is specifically used for registering Service Fabric Stateless services.

#### Why Autofac
There are a few famour IoC Container libraries for .Net other than Autofac such as Unity, Ninject and StructureMap. All these libraries has extensive APIs and good documentations and provides an amzing support for the .Net Core Framework. For a person who is quite new to IoC, Autofac has the API which is easy to learn for a beginner. Another reason for using Autofac is that, while searching for libraries I found the `Autofac.ServiceFabric` really simple and easy to implement. 
