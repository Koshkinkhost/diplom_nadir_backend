using Hotel_Server.Models;

namespace Hotel_Server.DTO
{
    public class RoomWIthFalitires
    {
        public int id { get; set; }
        public string? Type { get; set; }
        public string status {  get; set; }
        public string number {  get; set; }
        public decimal PricePerNight { get; set; }
        public string? Description { get; set; }

        public string? MainImageUrl { get; set; }
        public List<RoomFacility> falitires { get; set; }
    }
}
