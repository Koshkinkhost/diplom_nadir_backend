using System;
using System.Collections.Generic;

namespace Hotel_Server.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public int GuestId { get; set; }

    public int BookingId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Guest Guest { get; set; } = null!;
}
