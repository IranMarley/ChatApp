# ChatApp

Simple browser-based chat application using .NET

## How to use:
- You will need Visual Studio 2017.
- You need to change the database name in the web.config of the presentation project (\src\ChatApp.MVC\Web.config).
- you can change the RabbitMQ credentials in the file (\src\ChatApp.Application\Service\RabbitMQService).
- For development tests it is not necessary to install the service (Windows Service), just define the two projects to start together and perform a Debug.
