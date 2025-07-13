using Hotel_Server.Models;
using System.ComponentModel.DataAnnotations;

namespace Hotel_Server.DTO
{
    public class RoomDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="NUMBER REQUIRED")]
        public string number { get; set; }

        public string? Type { get; set; }

        public decimal PricePerNight { get; set; }

        public int Capacity { get; set; }

        public string? Description { get; set; }

        public string? MainImageUrl { get; set; }

        public string? GalleryJson { get; set; }

        public string? Status { get; set; }
        public List<Facilities> falitires { get; set; }
    }
}
