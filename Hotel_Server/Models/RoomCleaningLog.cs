using System;
using System.Collections.Generic;

namespace Hotel_Server.Models;

public partial class RoomCleaningLog
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime? CleanedAt { get; set; }

    public string? Notes { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
