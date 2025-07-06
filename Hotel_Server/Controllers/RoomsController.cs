using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel_Server.Models;
using Microsoft.AspNetCore.Authorization;
using Hotel_Server.DTO;

namespace Hotel_Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly HotelDbContext _context;

    public RoomsController(HotelDbContext context)
    {
        _context = context;
    }

    // GET: api/rooms
    [HttpGet]
    //public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
    //{
    //    return await _context.Rooms.ToListAsync();
    //}
    


    //// GET: api/rooms/5
    //[HttpGet("{id}")]
    public async Task<List<RoomWIthFalitires>> GetRooms()
    {
        var room = await _context.Rooms.Include(d => d.RoomFacilities).Select(u => new RoomWIthFalitires
        {
            RoomId = u.Id,
            TypeR = u.Type,
            PricePerNight = u.PricePerNight,
            Description = u.Description,
            MainImageUrl = u.MainImageUrl,
            falitires = u.RoomFacilities.ToList(),
        }).ToListAsync();
        if (room == null)
            return new List<RoomWIthFalitires>();

        return room;
    }

    // POST: api/rooms
    [Authorize] // только авторизованные (например, админ) могут создавать
    [HttpPost]
    public async Task<ActionResult<Room>> CreateRoom(Room room)
    {
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRooms), new { id = room.Id }, room);
    }

    // PUT: api/rooms/5
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(int id, Room updatedRoom)
    {
        if (id != updatedRoom.Id)
            return BadRequest();

        _context.Entry(updatedRoom).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Rooms.Any(r => r.Id == id))
                return NotFound();
            throw;
        }

        return NoContent();
    }
    // GET: api/rooms/popular?count=3
    [HttpGet("popular")]
    public async Task<ActionResult<IEnumerable<Room>>> GetPopularRooms(int count = 3)
    {
        var popularRoomIds = await _context.Bookings
            .GroupBy(b => b.RoomId)
            .Select(g => new
            {
                RoomId = g.Key,
                BookingsCount = g.Count()
            })
            .OrderByDescending(g => g.BookingsCount)
            .Take(count)
            .Select(g => g.RoomId)
            .ToListAsync();

        var rooms = await _context.Rooms
            .Where(r => popularRoomIds.Contains(r.Id))
            .ToListAsync();

        // Чтобы сохранить порядок по популярности:
        var sortedRooms = popularRoomIds
            .Select(id => rooms.First(r => r.Id == id))
            .ToList();

        return sortedRooms;
    }


    // DELETE: api/rooms/5
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null)
            return NotFound();

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
