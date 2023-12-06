using Serilog;
using Starter.API;
using Starter.Identity.Database;
using Starter.Persistence.Database;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starter API starting");

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
     .WriteTo.Console()
     .ReadFrom.Configuration(context.Configuration));

var app = builder
       .ConfigureServices()
       .ConfigurePipeline();

app.UseSerilogRequestLogging();


//Clear migration

//if (app.Environment.IsDevelopment())
//{
//    await app.ResetDatabaseAsync();    
//}

//Seed data 
await app.Services.InitialiseDatabaseAsync();
await app.Services.InitialiseAppDatabaseAsync();

app.Run();
