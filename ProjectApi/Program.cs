using FurnitureStore.Models;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Helpers;
using ProjectApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Đọc chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

// Đăng ký DbContext
builder.Services.AddDbContext<FurnitureStoreContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<EmailService>();

// Đăng ký AutoMapper với cấu hình `MappingProfile`
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(typeof(Program));

// Đăng ký Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Trong Configure
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add static file serving
app.UseStaticFiles();

// Use CORS
app.UseCors("AllowAll");

// Add Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Thêm middleware Session
app.UseSession();

app.MapControllers();

app.Run();
