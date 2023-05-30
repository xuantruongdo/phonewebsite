namespace WebApplication5.DTO
{
    public class OrderDTO
    {
        public int TotalPrice { get; set; }
        public int UserId { get; set; }
        public int PaymentId { get; set; }
        public int StatusId { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
