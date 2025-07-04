namespace Hotel_Server.DTO
{
    public class BookingDTO
    {
        public int Id { get; set; }

        public int GuestId { get; set; }

        public int RoomId { get; set; }

        public DateOnly CheckIn { get; set; }

        public DateOnly CheckOut { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
