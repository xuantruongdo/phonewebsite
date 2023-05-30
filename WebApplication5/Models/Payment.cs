namespace WebApplication5.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Order> Orders { get; set; }

    }
}
