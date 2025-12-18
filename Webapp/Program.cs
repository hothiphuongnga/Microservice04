using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Webapp.Data;
using Webapp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Đăng ký HttpClient với base address từ Gateway
builder.Services.AddHttpClient("ProductApi", client =>
{
    var gatewayUrl = builder.Configuration["Gateway:BaseUrl"];
    client.BaseAddress = new Uri(gatewayUrl+ "/product-service/api/");
});
builder.Services.AddHttpClient("UserApi", client =>
{
    var gatewayUrl = builder.Configuration["Gateway:BaseUrl"];
    client.BaseAddress = new Uri(gatewayUrl+ "/user-service/api/");
});
builder.Services.AddHttpClient("OrderApi", client =>
{
    var gatewayUrl = builder.Configuration["Gateway:BaseUrl"];
    client.BaseAddress = new Uri(gatewayUrl+ "/order-service/api/");
});
builder.Services.AddHttpClient("PaymentApi", client =>
{
    var gatewayUrl = builder.Configuration["Gateway:BaseUrl"];
    client.BaseAddress = new Uri(gatewayUrl+ "/payment-service/api/");
});

builder.Services.AddHttpClient("Gateway", client =>
{
    var gatewayUrl = builder.Configuration["Gateway:BaseUrl"];
    client.BaseAddress = new Uri(gatewayUrl);
});

// Đăng ký AuthService
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
