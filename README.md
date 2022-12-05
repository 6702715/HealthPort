# Healthport

HealthPort is a test project for developing test automation scripts on C# and .NET

## Requirements

For raising up the project we need to have:

1. Microsoft SQL Server (2017 or higher)
2. Docker

   ![1670237890782](image/README/1670237890782.png)
3. Microsoft DOTNET6
4. Newman (for API testing)

HealthPort is cross-platform solution and can be run on Linux, MacOS, and Windows

## How to deploy the solution

### Database preparing

1. Create Database "HealthPort"
2. Execute SQL-script HealthPortBack/sql_scripts/mssqlserver/items_create_table.sql
3. Execute SQL-script HealthPortBack/sql_scripts/mssqlserver/dbo.items.sql

   ![1670237340186](image/README/1670237340186.png)

### Mocking

As a mock system we use Wiremock
For running Wiremock:
Go to HealthPortBack/wiremock and run start_wiremock.sh

![1670236842110](image/README/1670236842110.png)

### API Documentation

As an API Doc server, we use Swagger
For running Swagger:
Go to HealthPortBack/swagger and run start_swagger.sh

![1670237130721](image/README/1670237130721.png)

## How to run the solution

### Running Back API Server

Set MS SQL Server parameters in database_configuration.json
You can keep parameters in database_sqlite3_configuration.json as is
Set Wiremock parameters in external_services.json

Go to HealthPortBack and execute:
dotnet run -project ./HealthPortBack/HealthPortBack.csproj --urls http://0.0.0.0:5045
Or
You can run the solution in VSCode via a Terminal task "Run Back"

### Running Front

Set a Back API Server address in HealthPortFront/wwwroot/appsettings.json

Go to HealthPortFront and execute:
dotnet run -project ./HealthPortFront/HealthPortFront.csproj --urls http://0.0.0.0:5044
Or
You can run the solution in VSCode via a Terminal task "Run Front"

![1670236933936](image/README/1670236933936.png)

## Testing

The project has Smoke tests in the MS Excel file HealthPort_E2E_Smoke_TS.xlsx inside the solution

![1670235759230](image/README/1670235759230.png)

### API testing Postman/Newman

API tests for Postman are contained in api_tests directory
Before running set environment parameters in "Dev Environment.postman_environment.json"
You can run a collection via Postman

![1670236425172](image/README/1670236425172.png)

Or
You can run it via Newman with helps a script run_newman_test.sh in api_tests directory

![1670237560742](image/README/1670237560742.png)

### GUI testing Automa

Automa tests are contained in HealthPortE2ETests/AutomaScripts

![1670235583475](image/README/1670235583475.png)

### GUI testing Selenium

For running Selenium tests

Set up edgedriver on your system (or driver for any of your favorite browser) in ".driver" directory
Modify UnitTest1.cs for your Front-server address (and your driver if you've changed)
Run GUI/Selenium tests via command:
dotnet test
