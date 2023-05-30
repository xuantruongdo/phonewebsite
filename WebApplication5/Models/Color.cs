﻿namespace WebApplication5.Models
{
    public class Color
    {
        public int Id { get; set; }
        public string ColorName { get; set; }
        public int Stock { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
