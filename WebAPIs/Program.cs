using Application.Extensions;
using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Persistence;
using WebAPIs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterCoreServices(builder.Configuration)
                .RegisterPersistenceServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

RecurringJob.AddOrUpdate<MyBackgroundService>("SendMailJob", x => x.SendEmailToCustomer(), Cron.Daily);

//app.UseMiddleware<GlobalExceptionMiddeware>();
app.UseMiddleware<WriteLoggingMiddleware>();

app.MapHealthChecks("/healthchecking",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
