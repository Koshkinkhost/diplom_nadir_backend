using Hotel_Server.DTO;
using Hotel_Server.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hotel_Server.Controllers
{
    [Route("api/[controller]")]
    public class StaffController : Controller
    {
        private readonly HotelDbContext _context;

        public StaffController(HotelDbContext context)
        {
            _context = context;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] StaffDTO dto)
        {
            var staff = await _context.Employees.FirstOrDefaultAsync(s =>
                s.Email == dto.Email && s.Password_ == dto.Password); 

            if (staff == null)
                return Unauthorized("Неверный email или пароль");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, staff.Id.ToString()),
                new Claim(ClaimTypes.Name, staff.FullName),
                new Claim(ClaimTypes.Role, "Manager")

            };

            var identity = new ClaimsIdentity(claims, "staff_cookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("StaffScheme", principal); // схема должна быть настроена

            return Ok(new
            {
                Id= staff.Id,
                staff.FullName,
                Role="Manager"



            });
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("StaffScheme");
            return Ok("Выход выполнен");
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var staffId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var staff = await _context.Employees.FindAsync(staffId);

            if (staff == null)
                return NotFound("Сотрудник не найден");

            return Ok(new
            {
                staff.Id,
                staff.FullName,
                staff.Email
            });
        }
    }
}
