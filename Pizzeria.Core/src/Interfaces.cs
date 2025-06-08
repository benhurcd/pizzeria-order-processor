using Pizzeria.Core.Models;

namespace Pizzeria.Core.Interfaces
{
    public interface IOrderService
    {
        void CreateOrder(Order order);
        Order GetOrder(string orderId);
        void UpdateOrder(Order order);
        void DeleteOrder(string orderId);
    }

    public interface IProductService
    {
        void AddProduct(Product product);
        Product GetProduct(string productId);
        void UpdateProduct(Product product);
        void DeleteProduct(string productId);
    }

    public interface IInventoryService
    {
        void AddIngredient(Ingredient ingredient);
        Ingredient GetIngredient(string name);
        void UpdateIngredient(Ingredient ingredient);
        void DeleteIngredient(string name);
    }


    public interface IOrderProvider
    {
        IEnumerable<OrderEntry> GetOrders();
    }

    public interface IOrderValidator
    {
        bool Validate(OrderEntry entry, out string error);
        IEnumerable<(OrderEntry entry, string error)> Validate(IEnumerable<OrderEntry> entries);
    }

    public interface IProductProvider
    {
        IEnumerable<Product> GetProducts();
    }

    public interface IIngredientProvider
    {
        IEnumerable<ProductIngredient> GetProductIngredients();
    }
}
