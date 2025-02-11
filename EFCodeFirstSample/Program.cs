using Data;
using DataContracts.Services;
using DataServices.Services;
using EFCodeFirstSample.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("EFCodeFirstSample"));
});

builder.Services.AddScoped<IVideoGameService, VideoGameService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Video Games Library",
        Description = "An ASP.NET Core Web API for managing video games",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//can add multiple exception handler classes here.
//Using event chaining until an exception handler returns true, all the handler will execute
builder.Services.AddExceptionHandler<AppExceptionHandler>();

//use code to configure serilog to write to file
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Information()
//    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
//    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
//    //.MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
//    .Enrich.FromLogContext()
//    .WriteTo.File("Logs/Log-.txt", rollingInterval: RollingInterval.Day,
//        outputTemplate: "{Timestamp:HH:mm:ss} {RequestId,13} -[{Level:u3}] {Message} ({EventId:x8}){NewLine}{Exception}") //serilog adds date part after file name
//    .CreateLogger();

//get serilog configuration from appSettings
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(_ => { });//doesn't required any parameter to run using IExceptionHandler in .net 8

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSerilogRequestLogging();

app.Run();
