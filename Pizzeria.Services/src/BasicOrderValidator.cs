using System;
using System.Collections.Generic;
using System.Linq;
using Pizzeria.Core.Models;
using Pizzeria.Core.Interfaces;

namespace Pizzeria.Services.Validation
{
    public class BasicOrderValidator : IOrderValidator
    {
        private readonly IProductProvider _productProvider;
        private List<Product> _productsCache;
        public BasicOrderValidator(IProductProvider productProvider)
        {
            _productProvider = productProvider;
            _productsCache = _productProvider.GetProducts().ToList();
        }
        public bool Validate(OrderEntry entry, out string error)
        {
            if (string.IsNullOrWhiteSpace(entry.OrderId))
            {
                error = "OrderId is required.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(entry.ProductId))
            {
                error = "ProductId is required.";
                return false;
            }
            // ProductId must exist in catalog
            var product = _productsCache.FirstOrDefault(p => p.ProductId == entry.ProductId);
            if (product == null)
            {
                error = $"ProductId '{entry.ProductId}' not found in product catalog.";
                return false;
            }
            if (entry.Quantity <= 0)
            {
                error = "Quantity must be greater than zero.";
                return false;
            }
            if (entry.DeliveryAt < entry.CreatedAt)
            {
                error = "DeliveryAt cannot be before CreatedAt.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(entry.DeliveryAddress))
            {
                error = "DeliveryAddress is required.";
                return false;
            }
            error = string.Empty;
            return true;
        }

        public IEnumerable<(OrderEntry entry, string error)> Validate(IEnumerable<OrderEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (!Validate(entry, out var error))
                    yield return (entry, error);
            }
        }
    }
}
