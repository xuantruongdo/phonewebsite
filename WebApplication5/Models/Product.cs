namespace WebApplication5.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int ImportPrice { get; set; }
        public int OldPrice { get; set; }
        public int CurrentPrice { get; set; }
        public Brand Brand { get; set; }
        public int BrandId { get; set; }
        public List<Color> Colors { get; set; }
    }
}
