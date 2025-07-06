using Hotel_Server.Models;

namespace Hotel_Server.DTO
{
    public class RoomWIthFalitires
    {
        public int RoomId { get; set; }
        public string? TypeR { get; set; }

        public decimal PricePerNight { get; set; }
        public string? Description { get; set; }

        public string? MainImageUrl { get; set; }
        public List<RoomFacility> falitires { get; set; }
    }
}
