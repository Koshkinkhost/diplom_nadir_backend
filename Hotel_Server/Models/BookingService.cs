using System;
using System.Collections.Generic;

namespace Hotel_Server.Models;

public partial class BookingService
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public int ServiceId { get; set; }

    public int Quantity { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
