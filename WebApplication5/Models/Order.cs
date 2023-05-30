namespace WebApplication5.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int TotalPrice { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
    }
}
