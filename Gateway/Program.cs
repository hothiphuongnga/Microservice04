var builder = WebApplication.CreateBuilder(args);

// Thêm CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Kích hoạt CORS - phải đứng đầu
app.UseCors("AllowAll");

// HTTPS Redirection
app.UseHttpsRedirection();

//log 
app.Use(async (context, next) =>
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("==================== Incoming Request ====================");
    Console.WriteLine($"{context.Request.Method} {context.Request.Headers.Host}{context.Request.Path}");
    Console.ResetColor();
    await next();
});

// ReverseProxy phải ở cuối
app.MapReverseProxy();

app.Run();
