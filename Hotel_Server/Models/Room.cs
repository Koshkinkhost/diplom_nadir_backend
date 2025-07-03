using System;
using System.Collections.Generic;

namespace Hotel_Server.Models;

public partial class Room
{
    public int Id { get; set; }

    public string Number { get; set; } = null!;

    public string? Type { get; set; }

    public decimal PricePerNight { get; set; }

    public int Capacity { get; set; }

    public string? Description { get; set; }

    public string? MainImageUrl { get; set; }

    public string? GalleryJson { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<RoomCleaningLog> RoomCleaningLogs { get; set; } = new List<RoomCleaningLog>();

    public virtual ICollection<RoomFacility> RoomFacilities { get; set; } = new List<RoomFacility>();
}
