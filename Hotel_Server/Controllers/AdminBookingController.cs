using Hotel_Server.DTO;
using Hotel_Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Server.Controllers
{
    [ApiController]
    [Route("api/admin/bookings")]
    public class AdminBookingController : ControllerBase
    {
        private readonly HotelDbContext _context;

        public AdminBookingController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetAllBookings()
        {
            return await _context.Bookings.Include(b=>b.Guest).Select(u => new BookingDTO
            {
                Id=u.Id,
                GuestName=u.Guest.FullName,
                Status=u.Status,
                Room=u.Room.Type,
                CheckIn=u.CheckIn,
                CheckOut=u.CheckOut,
                CreatedAt=u.CreatedAt,


            }).ToListAsync();
        }

        // GET: api/admin/bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            var booking = await _context.Bookings.Where(g => g.Id == id).Select(u=>new BookingDTO
            {
                Id = u.Id,
                GuestName = u.Guest.FullName,
                Status = u.Status,
                CheckIn = u.CheckIn,
                CheckOut = u.CheckOut,

            }).FirstOrDefaultAsync();

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/admin/bookings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, BookingDTO updated)
        {
            if (id != updated.Id)
            {
                return BadRequest("ID не совпадает");
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            booking.CheckIn = updated.CheckIn;
            booking.CheckOut = updated.CheckOut;
            booking.Status = updated.Status;

            await _context.SaveChangesAsync();
            return Ok("Изменено");
        }

        // DELETE: api/admin/bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
