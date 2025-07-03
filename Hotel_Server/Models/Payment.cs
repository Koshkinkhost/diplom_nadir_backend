using System;
using System.Collections.Generic;

namespace Hotel_Server.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaidAt { get; set; }

    public string? Method { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
