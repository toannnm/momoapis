using Serilog;
using Serilog.Events;
using System.Diagnostics;

public static class LoggingMiddleware
{
    public static IServiceCollection AddLoggingMiddleware(this IServiceCollection services, string logFolder = "Logs")
    {
        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Default", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Fatal)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Update", LogEventLevel.Fatal)
                .WriteTo.Console()
                .WriteTo.File($"{logFolder}/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        services.AddSingleton(Log.Logger);

        return services;
    }
}

public class WriteLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Stopwatch _stopwatch;

    public WriteLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
        _stopwatch = new Stopwatch();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Ghi log trước khi xử lý yêu cầu
        _stopwatch.Start();
        Log.Information($"\nReceived request: {context.Request.Method} {context.Request.Path} | Remote IP: {context.Connection.RemoteIpAddress}");

        // Xử lý yêu cầu và chuyển tiếp đến các middleware tiếp theo trong pipeline
        await _next(context);

        // Ghi log sau khi xử lý yêu cầu
        _stopwatch.Stop();
        Log.Information($"Completed request: {context.Request.Method} {context.Request.Path} | Duration: {_stopwatch.ElapsedMilliseconds} ms | Status code: {context.Response.StatusCode}\n");
        _stopwatch.Reset();
    }
}