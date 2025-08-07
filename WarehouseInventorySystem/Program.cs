using System;
using System.Collections.Generic;
using System.Linq;

namespace WarehouseInventorySystem
{
    // a. Marker Interface for Inventory Items
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    // b. ElectronicItem class implementing IInventoryItem
    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public string Brand { get; }
        public int WarrantyMonths { get; }

        public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }

        public override string ToString()
        {
            return $"Electronic - ID: {Id}, Name: {Name}, Quantity: {Quantity}, Brand: {Brand}, Warranty: {WarrantyMonths} months";
        }
    }

    // c. GroceryItem class implementing IInventoryItem
    public class GroceryItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; }

        public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }

        public override string ToString()
        {
            return $"Grocery - ID: {Id}, Name: {Name}, Quantity: {Quantity}, Expires: {ExpiryDate:yyyy-MM-dd}";
        }
    }

    // e. Custom Exception Classes
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message) : base(message) { }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message) { }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

    // d. Generic Inventory Repository
    public class InventoryRepository<T> where T : IInventoryItem
    {
        private Dictionary<int, T> _items;

        public InventoryRepository()
        {
            _items = new Dictionary<int, T>();
        }

        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
            {
                throw new DuplicateItemException($"Item with ID {item.Id} already exists in inventory.");
            }
            _items[item.Id] = item;
        }

        public T GetItemById(int id)
        {
            if (!_items.ContainsKey(id))
            {
                throw new ItemNotFoundException($"Item with ID {id} not found in inventory.");
            }
            return _items[id];
        }

        public void RemoveItem(int id)
        {
            if (!_items.ContainsKey(id))
            {
                throw new ItemNotFoundException($"Cannot remove item with ID {id} - not found in inventory.");
            }
            _items.Remove(id);
        }

        public List<T> GetAllItems()
        {
            return _items.Values.ToList();
        }

        public void UpdateQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new InvalidQuantityException($"Quantity cannot be negative. Provided: {newQuantity}");
            }

            if (!_items.ContainsKey(id))
            {
                throw new ItemNotFoundException($"Cannot update quantity for item with ID {id} - not found in inventory.");
            }

            _items[id].Quantity = newQuantity;
        }
    }

    // f. WareHouseManager Class
    public class WareHouseManager
    {
        private InventoryRepository<ElectronicItem> _electronics;
        private InventoryRepository<GroceryItem> _groceries;

        public WareHouseManager()
        {
            _electronics = new InventoryRepository<ElectronicItem>();
            _groceries = new InventoryRepository<GroceryItem>();
        }

        public void SeedData()
        {
            Console.WriteLine("--- Seeding Sample Data ---");
            try
            {
                // Add 2-3 ElectronicItems
                _electronics.AddItem(new ElectronicItem(1, "Samsung Galaxy S23", 15, "Samsung", 24));
                _electronics.AddItem(new ElectronicItem(2, "MacBook Pro M3", 8, "Apple", 12));
                _electronics.AddItem(new ElectronicItem(3, "Sony WH-1000XM5", 25, "Sony", 12));

                // Add 2-3 GroceryItems
                _groceries.AddItem(new GroceryItem(101, "Organic Milk", 50, DateTime.Now.AddDays(7)));
                _groceries.AddItem(new GroceryItem(102, "Whole Wheat Bread", 30, DateTime.Now.AddDays(5)));
                _groceries.AddItem(new GroceryItem(103, "Free-Range Eggs", 40, DateTime.Now.AddDays(14)));

                Console.WriteLine("Sample data seeded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding data: {ex.Message}");
            }
            Console.WriteLine();
        }

        public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
        {
            try
            {
                var items = repo.GetAllItems();
                if (items.Count == 0)
                {
                    Console.WriteLine("No items found in inventory.");
                }
                else
                {
                    foreach (var item in items)
                    {
                        Console.WriteLine($"  • {item}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error printing items: {ex.Message}");
            }
            Console.WriteLine();
        }

        public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
        {
            try
            {
                var item = repo.GetItemById(id);
                int newQuantity = item.Quantity + quantity;
                repo.UpdateQuantity(id, newQuantity);
                Console.WriteLine($"Successfully increased stock for item ID {id}. New quantity: {newQuantity}");
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine($"Failed to increase stock: {ex.Message}");
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine($"Failed to increase stock: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error increasing stock: {ex.Message}");
            }
        }

        public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
        {
            try
            {
                var item = repo.GetItemById(id); // Get item first to show what we're removing
                repo.RemoveItem(id);
                Console.WriteLine($"Successfully removed item: {item.Name} (ID: {id})");
            }
            catch (ItemNotFoundException ex)
            {
                Console.WriteLine($"Failed to remove item: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error removing item: {ex.Message}");
            }
        }

        public void RunDemonstration()
        {
            Console.WriteLine("=== Warehouse Inventory Management System ===");
            Console.WriteLine();

            // ii. Call SeedData()
            SeedData();

            // iii. Print all grocery items
            Console.WriteLine("--- All Grocery Items ---");
            PrintAllItems(_groceries);

            // iv. Print all electronic items
            Console.WriteLine("--- All Electronic Items ---");
            PrintAllItems(_electronics);

            // v. Try operations that should cause exceptions
            Console.WriteLine("--- Testing Exception Handling ---");

            // Try to add a duplicate item
            Console.WriteLine("1. Trying to add duplicate item...");
            try
            {
                _electronics.AddItem(new ElectronicItem(1, "Duplicate Phone", 5, "Generic", 6));
            }
            catch (DuplicateItemException ex)
            {
                Console.WriteLine($"Caught expected exception: {ex.Message}");
            }
            Console.WriteLine();

            // Try to remove a non-existent item
            Console.WriteLine("2. Trying to remove non-existent item...");
            RemoveItemById(_electronics, 999);
            Console.WriteLine();

            // Try to update with invalid quantity
            Console.WriteLine("3. Trying to update with negative quantity...");
            try
            {
                _electronics.UpdateQuantity(1, -5);
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine($"Caught expected exception: {ex.Message}");
            }
            Console.WriteLine();

            // Demonstrate successful operations
            Console.WriteLine("--- Successful Operations ---");
            Console.WriteLine("Increasing stock for Samsung Galaxy S23...");
            IncreaseStock(_electronics, 1, 10);
            Console.WriteLine();

            Console.WriteLine("Final Electronic Items after stock increase:");
            PrintAllItems(_electronics);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // i. Instantiate WareHouseManager
            WareHouseManager manager = new WareHouseManager();
            
            // Run the demonstration
            manager.RunDemonstration();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
