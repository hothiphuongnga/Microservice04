using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Repositories;
using PaymentService.Repositories.Base;
using PaymentService.Services;

var builder = WebApplication.CreateBuilder(args);

// === ĐĂNG KÝ CÁC SERVICE (DEPENDENCY INJECTION) ===

// Đăng ký DbContext, cấu hình sử dụng SQL Server với chuỗi kết nối từ appsettings.json
builder.Services.AddDbContext<PaymentDbServiceContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
    options.UseSqlServer(connectionString);
});

builder.Services.AddRazorPages();          // Hỗ trợ Razor Pages
builder.Services.AddServerSideBlazor();    // Hỗ trợ Blazor Server
builder.Services.AddControllers();         // Hỗ trợ API Controllers
builder.Services.AddSwaggerGen();          // Hỗ trợ Swagger (OpenAPI) cho tài liệu API

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPaymentService, PaymentServicee>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// Đăng ký AutoMapper để hỗ trợ ánh xạ giữa các đối tượng
builder.Services.AddAutoMapper(cf=>{}, typeof(PaymentMapping));




var app = builder.Build();

// === CẤU HÌNH MIDDLEWARE PIPELINE ===

// Kích hoạt Swagger & giao diện Swagger UI cho API docs & thử nghiệm
app.UseSwagger();
app.UseSwaggerUI();

// Tự động chuyển hướng HTTP sang HTTPS (bảo mật)
app.UseHttpsRedirection();

// Cho phép truy cập các file tĩnh (CSS, JS, ảnh, ...)
app.UseStaticFiles();

// Kích hoạt định tuyến
app.UseRouting();

// Map các endpoint cho Controller API, RazorPages, Blazor và fallback
app.MapControllers();

app.Run();