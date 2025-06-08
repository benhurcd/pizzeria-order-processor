using Pizzeria.Core.Models;
using Pizzeria.Core.Interfaces;

namespace Pizzeria.Services.Calculation
{
    public class OrderCalculator
    {
        private readonly IProductProvider _productProvider;
        public OrderCalculator(IProductProvider productProvider)
        {
            _productProvider = productProvider;
        }
        public decimal CalculateTotalPrice(IEnumerable<OrderEntry> entries)
        {
            var products = _productProvider.GetProducts().ToDictionary(p => p.ProductId, p => p.Price);
            decimal total = 0;
            foreach (var entry in entries)
            {
                if (products.TryGetValue(entry.ProductId, out var price))
                {
                    total += price * entry.Quantity;
                }
            }
            return total;
        }
    }
}
