using System;
using System.Collections.Generic;

namespace Hotel_Server.Models;

public partial class Guest
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? PassportNumber { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
