using System;
using System.IO;
using System.Linq;
using Pizzeria.Core.Models;
using Pizzeria.Core.Interfaces;
using Pizzeria.Services.Validation;
using Pizzeria.Services.Calculation;
using Pizzeria.Infrastructure.Parsers;
using Pizzeria.Infrastructure.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Pizzeria.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var (orderFilePath, productFilePath, ingredientFilePath) = ParseFilePaths(args);
            var provider = BuildServiceProvider(orderFilePath, productFilePath, ingredientFilePath);
            var pizzeriaService = provider.GetRequiredService<Services.Calculation.PizzeriaCalculatorService>();
            pizzeriaService.PrintSummary();
        }

        private static (string orderFilePath, string productFilePath, string ingredientFilePath) ParseFilePaths(string[] args)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string? orderFilePath = null;
            string? productFilePath = null;
            string? ingredientFilePath = null;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--orderFile":
                    case "--o":
                        if (i + 1 < args.Length) orderFilePath = args[++i];
                        break;
                    case "--productFile":
                    case "--p":
                        if (i + 1 < args.Length) productFilePath = args[++i];
                        break;
                    case "--ingredientsFile":
                    case "--i":
                        if (i + 1 < args.Length) ingredientFilePath = args[++i];
                        break;
                }
            }
            if (orderFilePath == null)
            {
                orderFilePath = Path.Combine(baseDir, "orders.json");
                Console.WriteLine("[INFO] --orderFile not provided. Using default orders.json in application directory.");
            }
            if (productFilePath == null)
            {
                productFilePath = Path.Combine(baseDir, "products.json");
                Console.WriteLine("[INFO] --productFile not provided. Using default products.json in application directory.");
            }
            if (ingredientFilePath == null)
            {
                ingredientFilePath = Path.Combine(baseDir, "ingredients.json");
                Console.WriteLine("[INFO] --ingredientsFile not provided. Using default ingredients.json in application directory.");
            }
            if (!File.Exists(orderFilePath) || !File.Exists(productFilePath) || !File.Exists(ingredientFilePath))
            {
                throw new FileNotFoundException("One or more specified file paths do not exist.");
            }
            return (orderFilePath, productFilePath, ingredientFilePath);
        }

        private static ServiceProvider BuildServiceProvider(string orderFilePath, string productFilePath, string ingredientFilePath)
        {
            var services = new ServiceCollection();
            services.AddSingleton<IOrderProvider>(sp => new Infrastructure.Parsers.JsonOrderProvider(orderFilePath));
            services.AddSingleton<IProductProvider>(sp => new JsonProductProvider(productFilePath));
            services.AddSingleton<IIngredientProvider>(sp => new JsonIngredientProvider(ingredientFilePath));
            services.AddSingleton<IOrderValidator>(sp => new BasicOrderValidator(sp.GetRequiredService<IProductProvider>()));
            services.AddSingleton<OrderCalculator>(sp => new OrderCalculator(sp.GetRequiredService<IProductProvider>()));
            services.AddSingleton<IngredientCalculator>(sp => new IngredientCalculator(sp.GetRequiredService<IIngredientProvider>()));
            services.AddSingleton<PizzeriaCalculatorService>();
            return services.BuildServiceProvider();
        }
    }
}
