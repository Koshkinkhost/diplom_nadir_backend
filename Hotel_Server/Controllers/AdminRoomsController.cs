using Hotel_Server.DTO;
using Hotel_Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Server.Controllers
{
    [Route("api/admin/rooms")]
    [ApiController]
    public class AdminRoomsController : Controller
    {


        private readonly HotelDbContext _context;

        public AdminRoomsController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomWIthFalitires>>> GetAllRooms()
        {
            return await _context.Rooms.Select(u=>new RoomWIthFalitires
            {
                id=u.Id,
                PricePerNight=u.PricePerNight,
                Description=u.Description,
                MainImageUrl=u.MainImageUrl,
                status=u.Status,
                number=u.Number,
                falitires = u.RoomFacilities.ToList(),

            }).ToListAsync();
        }

        // GET: api/admin/rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.Include(r => r.RoomFacilities).FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
                return NotFound();

            return room;
        }

        // POST: api/admin/rooms
        [HttpPost]
        public async Task<ActionResult<Room>> CreateRoom([FromBody] RoomDTO room)
        {
            // Проверка: есть ли уже такая комната
            bool numberExists = await _context.Rooms.AnyAsync(r => r.Number == room.number);
            if (numberExists)
                return Conflict($"Комната с номером {room.number} уже существует.");

            var r = new Room
            {
                Description = room.Description,
                Number = room.number,
                Status = room.Status,
                MainImageUrl = room.MainImageUrl,
                PricePerNight = room.PricePerNight,
                Type = room.Type,
                Capacity = room.Capacity,
            };
            _context.Rooms.Add(r);
            await _context.SaveChangesAsync();

            if (room.falitires != null)
            {
                foreach (var facilityDto in room.falitires)
                {
                    var r1 = new RoomFacility
                    {
                        RoomId = r.Id,
                        Name = facilityDto.name,
                    };
                    _context.RoomFacilities.Add(r1);
                }
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoom), new { id = r.Id }, r);
        }


        // PUT: api/admin/rooms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            if (id != updatedRoom.Id)
                return BadRequest("Room ID mismatch");

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return NotFound();

            // Проверка: есть ли другая комната с таким номером
            bool numberTaken = await _context.Rooms
                .AnyAsync(r => r.Number == updatedRoom.Number && r.Id != id);
            if (numberTaken)
                return Conflict($"Комната с номером {updatedRoom.Number} уже существует.");

            room.Type = updatedRoom.Type;

            room.Description = updatedRoom.Description;
            room.PricePerNight = updatedRoom.PricePerNight;
            room.MainImageUrl = updatedRoom.MainImageUrl;
            room.RoomFacilities = updatedRoom.RoomFacilities;
            room.Number = updatedRoom.Number; // обновление допустимо только если уникально

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/admin/rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var faciliti=_context.RoomFacilities.Where(r=>r.RoomId == id);
            _context.RemoveRange(faciliti);
            await _context.SaveChangesAsync();
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
        
}
