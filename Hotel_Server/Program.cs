using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Hotel_Server.Models;

var builder = WebApplication.CreateBuilder(args);

// Add DB Context
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add MVC & Controllers
builder.Services.AddControllers();
builder.Services.AddMvc();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:4200") // ������ �� ����� ������ ���������
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();  // ����������� ��� ����
    });
});


// Cookie-based Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
        options.SlidingExpiration = true;
        options.Cookie.Name = "AuthCookie";
        options.Cookie.IsEssential = true;
    });

var app = builder.Build();

app.UseCors(); // �����: ������ ����� UseAuthentication
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
