using Xunit;
using System.Diagnostics;

namespace Pizzeria.ConsoleApp.Tests
{
    public class SmokeTests
    {
        [Fact]
        public void TestProjectLoads()
        {
            Assert.True(true);
        }
    }

    public class ConsoleAppIntegrationTests
    {
        [Fact]
        public void Program_RunsWithoutException()
        {
            // Arrange
            var process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run --project ../../src/ConsoleApp.csproj";
            process.StartInfo.WorkingDirectory = System.IO.Path.GetFullPath("../../..");
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            // Act
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // Assert
            Assert.Contains("Order", output); // Should print order summary
            Assert.Contains("Total Ingredients Required", output); // Should print ingredients summary
            Assert.Equal(0, process.ExitCode);
        }
    }

    public class FunctionalTests
    {
        [Fact]
        public void Program_ProcessesCustomJsonFiles_CorrectOutput()
        {
            // Arrange: create temp files for functional test
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var ordersPath = Path.Combine(tempDir, "orders.json");
            var productsPath = Path.Combine(tempDir, "products.json");
            var ingredientsPath = Path.Combine(tempDir, "ingredients.json");
            File.WriteAllText(ordersPath, @"[
  {""OrderId"": ""ORD100"",""ProductId"": ""PZ001"",""Quantity"": 1,""DeliveryAt"": ""2025-06-08T18:00:00"",""CreatedAt"": ""2025-06-07T12:00:00"",""DeliveryAddress"": ""789 Test St""},
  {""OrderId"": ""ORD101"",""ProductId"": ""PZ002"",""Quantity"": 2,""DeliveryAt"": ""2025-06-08T19:00:00"",""CreatedAt"": ""2025-06-07T13:00:00"",""DeliveryAddress"": ""456 Test Ave""}
]");
            File.WriteAllText(productsPath, @"[
  {""ProductId"": ""PZ001"",""ProductName"": ""Margherita"",""Price"": 10.0},
  {""ProductId"": ""PZ002"",""ProductName"": ""Pepperoni"",""Price"": 12.5}
]");
            File.WriteAllText(ingredientsPath, @"[
  {""ProductId"": ""PZ001"",""Ingredients"": [
    {""Name"": ""Dough"", ""Amount"": 0.3},
    {""Name"": ""Tomato Sauce"", ""Amount"": 0.1},
    {""Name"": ""Mozzarella"", ""Amount"": 0.15},
    {""Name"": ""Basil"", ""Amount"": 0.01}
  ]},
  {""ProductId"": ""PZ002"",""Ingredients"": [
    {""Name"": ""Dough"", ""Amount"": 0.3},
    {""Name"": ""Tomato Sauce"", ""Amount"": 0.1},
    {""Name"": ""Mozzarella"", ""Amount"": 0.15},
    {""Name"": ""Pepperoni"", ""Amount"": 0.07}
  ]}
]");

            var process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"run --project ../../src/ConsoleApp.csproj -- --orderFile \"{ordersPath}\" --productFile \"{productsPath}\" --ingredientsFile \"{ingredientsPath}\"";
            process.StartInfo.WorkingDirectory = System.IO.Path.GetFullPath("../../..");
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            // Act
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // Assert
            Assert.Contains("Order ORD100: Total Price = $10.00", output);
            Assert.Contains("Order ORD101: Total Price = $25.00", output);
            Assert.Contains("Dough: 0.9", output); // 1*0.3 + 2*0.3 = 0.9
            Assert.Contains("Pepperoni: 0.14", output); // 2*0.07
            Assert.Equal(0, process.ExitCode);

            // Cleanup
            Directory.Delete(tempDir, true);
        }

        [Fact]
        public void Program_ProcessesInvalidOrder_ShowsInvalidOrderInOutput()
        {
            // Arrange: create temp files for functional test with an invalid order (invalid ProductId)
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var ordersPath = Path.Combine(tempDir, "orders.json");
            var productsPath = Path.Combine(tempDir, "products.json");
            var ingredientsPath = Path.Combine(tempDir, "ingredients.json");
            File.WriteAllText(ordersPath, @"[
  {""OrderId"": ""ORD200"",""ProductId"": ""INVALID"",""Quantity"": 1,""DeliveryAt"": ""2025-06-08T18:00:00"",""CreatedAt"": ""2025-06-07T12:00:00"",""DeliveryAddress"": ""789 Test St""}
]");
            File.WriteAllText(productsPath, @"[
  {""ProductId"": ""PZ001"",""ProductName"": ""Margherita"",""Price"": 10.0}
]");
            File.WriteAllText(ingredientsPath, @"[
  {""ProductId"": ""PZ001"",""Ingredients"": [
    {""Name"": ""Dough"", ""Amount"": 0.3},
    {""Name"": ""Tomato Sauce"", ""Amount"": 0.1},
    {""Name"": ""Mozzarella"", ""Amount"": 0.15},
    {""Name"": ""Basil"", ""Amount"": 0.01}
  ]}
]");

            var process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"run --project ../../src/ConsoleApp.csproj -- --orderFile \"{ordersPath}\" --productFile \"{productsPath}\" --ingredientsFile \"{ingredientsPath}\"";
            process.StartInfo.WorkingDirectory = System.IO.Path.GetFullPath("../../..");
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            // Act
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // Assert
            Assert.Contains("Invalid Orders:", output);
            Assert.Contains("OrderId: ORD200", output);
            Assert.Contains("not found in product catalog", output);
            Assert.Equal(0, process.ExitCode);

            // Cleanup
            Directory.Delete(tempDir, true);
        }

        [Fact]
        public void Program_HandlesInvalidOrder_Gracefully()
        {
            // Arrange: create temp files with an invalid order (invalid ProductId)
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            var ordersPath = Path.Combine(tempDir, "orders.json");
            var productsPath = Path.Combine(tempDir, "products.json");
            var ingredientsPath = Path.Combine(tempDir, "ingredients.json");
            File.WriteAllText(ordersPath, @"[
  {""OrderId"": ""ORD200"",""ProductId"": ""INVALID"",""Quantity"": 1,""DeliveryAt"": ""2025-06-08T18:00:00"",""CreatedAt"": ""2025-06-07T12:00:00"",""DeliveryAddress"": ""123 Invalid St""}
]");
            File.WriteAllText(productsPath, @"[
  {""ProductId"": ""PZ001"",""ProductName"": ""Margherita"",""Price"": 10.0}
]");
            File.WriteAllText(ingredientsPath, @"[
  {""ProductId"": ""PZ001"",""Ingredients"": [
    {""Name"": ""Dough"", ""Amount"": 0.3},
    {""Name"": ""Tomato Sauce"", ""Amount"": 0.1},
    {""Name"": ""Mozzarella"", ""Amount"": 0.15},
    {""Name"": ""Basil"", ""Amount"": 0.01}
  ]}
]");

            var process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"run --project ../../src/ConsoleApp.csproj -- --orderFile \"{ordersPath}\" --productFile \"{productsPath}\" --ingredientsFile \"{ingredientsPath}\"";
            process.StartInfo.WorkingDirectory = System.IO.Path.GetFullPath("../../..");
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;

            // Act
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            // Assert: Should mention invalid order or error, but not throw or exit with error
            Assert.Contains("ORD200", output + error); // Order ID should be referenced
            Assert.Contains("invalid", (output + error).ToLower()); // Should mention invalid
            Assert.Equal(0, process.ExitCode);

            // Cleanup
            Directory.Delete(tempDir, true);
        }
    }

}
