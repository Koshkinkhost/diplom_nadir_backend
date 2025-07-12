using System;
using System.Collections.Generic;

namespace Hotel_Server.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? Position { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }
    public string Password_ {  get; set; }

    public virtual ICollection<RoomCleaningLog> RoomCleaningLogs { get; set; } = new List<RoomCleaningLog>();
}
