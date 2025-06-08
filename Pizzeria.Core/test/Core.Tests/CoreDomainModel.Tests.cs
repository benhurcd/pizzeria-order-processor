using Xunit;
using Pizzeria.Core.Models;

namespace Pizzeria.Core.Tests
{
    public class OrderEntryTests
    {
        [Fact]
        public void CanCreateOrderEntry()
        {
            var entry = new OrderEntry
            {
                OrderId = "ORD001",
                ProductId = "PZ001",
                Quantity = 2,
                DeliveryAt = new DateTime(2025, 6, 7, 18, 0, 0),
                CreatedAt = new DateTime(2025, 6, 6, 12, 0, 0),
                DeliveryAddress = "123 Main St"
            };
            Assert.Equal("ORD001", entry.OrderId);
            Assert.Equal("PZ001", entry.ProductId);
            Assert.Equal(2, entry.Quantity);
            Assert.Equal(new DateTime(2025, 6, 7, 18, 0, 0), entry.DeliveryAt);
            Assert.Equal(new DateTime(2025, 6, 6, 12, 0, 0), entry.CreatedAt);
            Assert.Equal("123 Main St", entry.DeliveryAddress);
        }
    }

    public class ProductTests
    {
        [Fact]
        public void CanCreateProduct()
        {
            var product = new Product
            {
                ProductId = "PZ001",
                ProductName = "Margherita",
                Price = 10.0m
            };
            Assert.Equal("PZ001", product.ProductId);
            Assert.Equal("Margherita", product.ProductName);
            Assert.Equal(10.0m, product.Price);
        }
    }

    public class IngredientTests
    {
        [Fact]
        public void CanCreateIngredient()
        {
            var ingredient = new Ingredient
            {
                Name = "Dough",
                Amount = 0.3m
            };
            Assert.Equal("Dough", ingredient.Name);
            Assert.Equal(0.3m, ingredient.Amount);
        }
    }

    public class ProductIngredientTests
    {
        [Fact]
        public void CanCreateProductIngredient()
        {
            var pi = new ProductIngredient
            {
                ProductId = "PZ001",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Name = "Dough", Amount = 0.3m },
                    new Ingredient { Name = "Tomato Sauce", Amount = 0.1m }
                }
            };
            Assert.Equal("PZ001", pi.ProductId);
            Assert.Equal(2, pi.Ingredients.Count);
            Assert.Equal("Dough", pi.Ingredients[0].Name);
        }
    }
}
