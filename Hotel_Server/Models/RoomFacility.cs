using System;
using System.Collections.Generic;

namespace Hotel_Server.Models;

public partial class RoomFacility
{
    public int Id { get; set; }

    public int RoomId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Room Room { get; set; } = null!;
}
