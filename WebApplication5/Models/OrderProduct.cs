namespace WebApplication5.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; }
        public Color Color { get; set; }
    }
}
