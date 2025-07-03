using Hotel_Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Server.Controllers
{
    public class BookingController : Controller
    {
        private readonly HotelDbContext _context;

        public BookingController(HotelDbContext context)
        {
            _context = context;
        }

        // ✅ Получить все бронирования
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings
                .Include(b => b.Room)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        // ✅ Получить бронирование по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return booking;
        }

        // ✅ Создать бронирование
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            if (booking.CheckIn >= booking.CheckOut)
                return BadRequest("Дата выезда должна быть позже даты заезда.");

            // Проверка доступности номера на выбранные даты
            var overlaps = await _context.Bookings
                .AnyAsync(b => b.RoomId == booking.RoomId &&
                    ((booking.CheckIn >= b.CheckIn && booking.CheckIn < b.CheckOut) ||
                     (booking.CheckOut > b.CheckIn && booking.CheckOut <= b.CheckOut) ||
                     (booking.CheckIn <= b.CheckIn && booking.CheckOut >= b.CheckOut)));

            if (overlaps)
                return Conflict("Номер уже забронирован на выбранные даты.");

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }

        // ✅ Обновить бронирование (по желанию)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, Booking updated)
        {
            if (id != updated.Id)
                return BadRequest();

            var exists = await _context.Bookings.AnyAsync(b => b.Id == id);
            if (!exists)
                return NotFound();

            _context.Entry(updated).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ Удалить бронирование
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
