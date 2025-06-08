using Xunit;
using Pizzeria.Core.Models;
using Pizzeria.Core.Interfaces;
using Pizzeria.Infrastructure.Providers;
using Pizzeria.Infrastructure.Parsers;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Pizzeria.Infrastructure.Tests
{
    public class JsonOrderProviderTests
    {
        [Fact]
        public void CanReadOrdersFromJsonFile()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "[\n  {\"OrderId\": \"ORD001\",\"ProductId\": \"PZ001\",\"Quantity\": 2,\"DeliveryAt\": \"2025-06-07T18:00:00\",\"CreatedAt\": \"2025-06-06T12:00:00\",\"DeliveryAddress\": \"123 Main St\"}\n]");
            var provider = new JsonOrderProvider(tempFile);
            // Act
            var orders = provider.GetOrders().ToList();
            // Assert
            Assert.Single(orders);
            Assert.Equal("ORD001", orders[0].OrderId);
            Assert.Equal("PZ001", orders[0].ProductId);
            Assert.Equal(2, orders[0].Quantity);
            File.Delete(tempFile);
        }
    }

    public class JsonProductProviderTests
    {
        [Fact]
        public void CanReadProductsFromJsonFile()
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "[\n  {\"ProductId\": \"PZ001\",\"ProductName\": \"Margherita\",\"Price\": 10.0}\n]");
            var provider = new Infrastructure.Providers.JsonProductProvider(tempFile);
            var products = provider.GetProducts().ToList();
            Assert.Single(products);
            Assert.Equal("PZ001", products[0].ProductId);
            Assert.Equal("Margherita", products[0].ProductName);
            Assert.Equal(10.0m, products[0].Price);
            File.Delete(tempFile);
        }
    }

    public class JsonIngredientProviderTests
    {
        [Fact]
        public void CanReadIngredientsFromJsonFile()
        {
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "[\n  {\"ProductId\": \"PZ001\",\"Ingredients\": [\n    {\"Name\": \"Dough\", \"Amount\": 0.3},\n    {\"Name\": \"Tomato Sauce\", \"Amount\": 0.1}\n  ]}\n]");
            var provider = new Infrastructure.Providers.JsonIngredientProvider(tempFile);
            var productIngredients = provider.GetProductIngredients().ToList();
            Assert.Single(productIngredients);
            Assert.Equal("PZ001", productIngredients[0].ProductId);
            Assert.Equal(2, productIngredients[0].Ingredients.Count);
            Assert.Equal("Dough", productIngredients[0].Ingredients[0].Name);
            File.Delete(tempFile);
        }
    }
}
