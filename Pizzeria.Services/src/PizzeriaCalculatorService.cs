using Pizzeria.Core.Models;
using Pizzeria.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pizzeria.Services.Calculation
{
    public class PizzeriaCalculatorService
    {
        private readonly IOrderProvider _orderProvider;
        private readonly IOrderValidator _validator;
        private readonly IProductProvider _productProvider;
        private readonly IIngredientProvider _ingredientProvider;
        private readonly OrderCalculator _orderCalculator;
        private readonly IngredientCalculator _ingredientCalculator;

        public PizzeriaCalculatorService(
            IOrderProvider orderProvider,
            IOrderValidator validator,
            IProductProvider productProvider,
            IIngredientProvider ingredientProvider,
            OrderCalculator orderCalculator,
            IngredientCalculator ingredientCalculator)
        {
            _orderProvider = orderProvider;
            _validator = validator;
            _productProvider = productProvider;
            _ingredientProvider = ingredientProvider;
            _orderCalculator = orderCalculator;
            _ingredientCalculator = ingredientCalculator;
        }

        public void PrintSummary()
        {
            var entries = _orderProvider.GetOrders().ToList();
            var invalidEntries = _validator.Validate(entries).ToList();
            var validEntries = entries.Except(invalidEntries.Select(x => x.entry)).ToList();
            var orders = validEntries.GroupBy(e => e.OrderId).Select(g => new Order
            {
                OrderId = g.Key,
                Entries = g.ToList(),
                DeliveryAddress = g.First().DeliveryAddress,
                DeliveryAt = g.First().DeliveryAt,
                CreatedAt = g.First().CreatedAt
            }).ToList();

            // Calculate totals
            foreach (var order in orders)
            {
                var total = _orderCalculator.CalculateTotalPrice(order.Entries);
                Console.WriteLine($"Order {order.OrderId}: Total Price = ${total:0.00}, Delivery: {order.DeliveryAt}, Address: {order.DeliveryAddress}");
            }
            // Ingredients summary
            var ingredientTotals = _ingredientCalculator.CalculateTotalIngredients(validEntries);
            Console.WriteLine("\nTotal Ingredients Required:");
            foreach (var kv in ingredientTotals)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value:0.##}");
            }
            // Invalid orders
            if (invalidEntries.Any())
            {
                Console.WriteLine("\nInvalid Orders:");
                foreach (var (entry, error) in invalidEntries)
                {
                    Console.WriteLine($"OrderId: {entry.OrderId}, Error: {error}");
                }
            }
        }
    }
}
