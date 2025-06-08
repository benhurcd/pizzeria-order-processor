using Pizzeria.Core.Models;
using Pizzeria.Core.Interfaces;
using System.Text.Json;

namespace Pizzeria.Infrastructure.Parsers
{
    public class JsonOrderProvider : IOrderProvider
    {
        private readonly string _filePath;
        public JsonOrderProvider(string filePath) => _filePath = filePath;
        public IEnumerable<OrderEntry> GetOrders()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<OrderEntry>>(json) ?? new List<OrderEntry>();
        }
    }
}
