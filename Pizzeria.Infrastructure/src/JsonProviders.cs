using Pizzeria.Core.Models;
using Pizzeria.Core.Interfaces;
using System.Text.Json;

namespace Pizzeria.Infrastructure.Providers
{
    public class JsonProductProvider : IProductProvider
    {
        private readonly string _filePath;
        public JsonProductProvider(string filePath) => _filePath = filePath;
        public IEnumerable<Product> GetProducts()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
        }
    }

    public class JsonIngredientProvider : IIngredientProvider
    {
        private readonly string _filePath;
        public JsonIngredientProvider(string filePath) => _filePath = filePath;
        public IEnumerable<ProductIngredient> GetProductIngredients()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<ProductIngredient>>(json) ?? new List<ProductIngredient>();
        }
    }
}
