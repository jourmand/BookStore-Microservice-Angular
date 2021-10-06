# BookStore MicroService Angular
Sample .NET Core application, powered by Microsoft, based on a simplified microservices architecture.
## Getting Started
Basic requirements to start projects:
- .Net 5
- SqlServer
- RabbitMq
- NodeJs

To run solution, Projects `Endpoints.HttpGateway`, `Endpoints.Oauth` & `Endpoints.WebApi` should be config to start together.

To do that, select properties on BookStore solution, then under Multiple startup projects, select start action for that project's name.

Before start projects, setting up connection strings in appsetting file of each project. Also, there is need to config rabbitMq port connection, username & passwords.
To view the guest angular panel, start that separately. To do that, in root of book-seller directory, run `ng serve -o` in command line. for login use this credentials:
> Password: admin@123
> 
> Username: admin@live.com
## Technologies
> - .Net 5.0
> - Identity Server 4
> - Swagger
> - MassTransit
> - Ocelot Gateway
> - .NET Framework
> - Angular
