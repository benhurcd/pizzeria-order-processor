namespace Pizzeria.Core.Models
{
    public class OrderEntry
    {
        public required string OrderId { get; set; }
        public required string ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveryAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string DeliveryAddress { get; set; }
    }

    public class Product
    {
        public required string ProductId { get; set; }
        public required string ProductName { get; set; }
        public decimal Price { get; set; }
    }

    public class Ingredient
    {
        public required string Name { get; set; }
        public decimal Amount { get; set; }
    }

    public class ProductIngredient
    {
        public required string ProductId { get; set; }
        public required List<Ingredient> Ingredients { get; set; }
    }

    public class Order
    {
        public required string OrderId { get; set; }
        public List<OrderEntry> Entries { get; set; } = new();
        public required string DeliveryAddress { get; set; }
        public DateTime DeliveryAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
