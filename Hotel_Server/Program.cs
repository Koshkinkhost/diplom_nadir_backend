using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Hotel_Server.Models;
using Hotel_Server;

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
            .WithOrigins("http://localhost:4200") // замени на адрес своего фронтенда
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();  // обязательно для куки
    });
});


// Cookie-based Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/auth/access-denied";
        options.Cookie.Name = "UserCookie";
    })
    .AddCookie("StaffScheme", options =>
    {
        options.LoginPath = "/staff/login";
        options.AccessDeniedPath = "/staff/access-denied";
        options.Cookie.Name = "StaffCookie";
    });

var app = builder.Build();

app.UseCors(); // Важно: ставим перед UseAuthentication
app.UseStaticFiles();
app.UseMiddleware<MiddleWareLog>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
