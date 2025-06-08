using Pizzeria.Core.Models;
using Pizzeria.Core.Interfaces;

namespace Pizzeria.Services.Calculation
{
    public class IngredientCalculator
    {
        private readonly IIngredientProvider _ingredientProvider;
        public IngredientCalculator(IIngredientProvider ingredientProvider)
        {
            _ingredientProvider = ingredientProvider;
        }
        public Dictionary<string, decimal> CalculateTotalIngredients(IEnumerable<OrderEntry> entries)
        {
            var productIngredients = _ingredientProvider.GetProductIngredients().ToDictionary(pi => pi.ProductId, pi => pi.Ingredients);
            var result = new Dictionary<string, decimal>();
            foreach (var entry in entries)
            {
                if (productIngredients.TryGetValue(entry.ProductId, out var ingredients))
                {
                    foreach (var ingredient in ingredients)
                    {
                        if (!result.ContainsKey(ingredient.Name))
                            result[ingredient.Name] = 0;
                        result[ingredient.Name] += ingredient.Amount * entry.Quantity;
                    }
                }
            }
            return result;
        }
    }
}
