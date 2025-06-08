using Xunit;
using Pizzeria.Core.Models;
using Pizzeria.Core.Interfaces;
using Pizzeria.Services.Calculation;
using Pizzeria.Services.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Pizzeria.Services.Tests
{
    public class OrderCalculatorTests
    {
        private class TestProductProvider : IProductProvider
        {
            public IEnumerable<Product> GetProducts() => new[] {
                new Product { ProductId = "PZ001", ProductName = "Margherita", Price = 10.0m },
                new Product { ProductId = "PZ002", ProductName = "Pepperoni", Price = 12.5m }
            };
        }

        [Fact]
        public void CalculatesTotalPrice_Correctly()
        {
            var provider = new TestProductProvider();
            var calc = new OrderCalculator(provider);
            var entries = new[] {
                new OrderEntry { OrderId = "ORD1", ProductId = "PZ001", Quantity = 2, DeliveryAt = System.DateTime.Now, CreatedAt = System.DateTime.Now, DeliveryAddress = "A" },
                new OrderEntry { OrderId = "ORD1", ProductId = "PZ002", Quantity = 1, DeliveryAt = System.DateTime.Now, CreatedAt = System.DateTime.Now, DeliveryAddress = "A" }
            };
            var total = calc.CalculateTotalPrice(entries);
            Assert.Equal(32.5m, total);
        }
    }

    public class IngredientCalculatorTests
    {
        private class TestIngredientProvider : IIngredientProvider
        {
            public IEnumerable<ProductIngredient> GetProductIngredients() => new[] {
                new ProductIngredient {
                    ProductId = "PZ001",
                    Ingredients = new List<Ingredient> {
                        new Ingredient { Name = "Dough", Amount = 0.3m },
                        new Ingredient { Name = "Tomato Sauce", Amount = 0.1m }
                    }
                },
                new ProductIngredient {
                    ProductId = "PZ002",
                    Ingredients = new List<Ingredient> {
                        new Ingredient { Name = "Dough", Amount = 0.3m },
                        new Ingredient { Name = "Pepperoni", Amount = 0.07m }
                    }
                }
            };
        }

        [Fact]
        public void CalculatesTotalIngredients_Correctly()
        {
            var provider = new TestIngredientProvider();
            var calc = new IngredientCalculator(provider);
            var entries = new[] {
                new OrderEntry { ProductId = "PZ001", Quantity = 1, OrderId = "ORD1", DeliveryAt = System.DateTime.Now, CreatedAt = System.DateTime.Now, DeliveryAddress = "A" },
                new OrderEntry { ProductId = "PZ002", Quantity = 2, OrderId = "ORD2", DeliveryAt = System.DateTime.Now, CreatedAt = System.DateTime.Now, DeliveryAddress = "A" }
            };
            var totals = calc.CalculateTotalIngredients(entries);
            Assert.Equal(0.9m, totals["Dough"]); // 1*0.3 + 2*0.3
            Assert.Equal(0.1m, totals["Tomato Sauce"]); // 1*0.1
            Assert.Equal(0.14m, totals["Pepperoni"]); // 2*0.07
        }
    }

    public class BasicOrderValidatorTests
    {
        private class TestProductProvider : IProductProvider
        {
            public IEnumerable<Product> GetProducts() => new[] {
                new Product { ProductId = "PZ001", ProductName = "Margherita", Price = 10.0m }
            };
        }

        [Fact]
        public void ValidatesOrderEntry_Correctly()
        {
            var provider = new TestProductProvider();
            var validator = new BasicOrderValidator(provider);
            var valid = new OrderEntry {
                OrderId = "ORD1",
                ProductId = "PZ001",
                Quantity = 1,
                DeliveryAt = new System.DateTime(2025, 6, 8),
                CreatedAt = new System.DateTime(2025, 6, 7),
                DeliveryAddress = "123 Main St"
            };
            Assert.True(validator.Validate(valid, out var _));

            var invalid = new OrderEntry {
                OrderId = "ORD2",
                ProductId = "INVALID",
                Quantity = 1,
                DeliveryAt = new System.DateTime(2025, 6, 8),
                CreatedAt = new System.DateTime(2025, 6, 7),
                DeliveryAddress = "123 Main St"
            };
            Assert.False(validator.Validate(invalid, out var error));
            Assert.Contains("not found", error);
        }
    }
}
