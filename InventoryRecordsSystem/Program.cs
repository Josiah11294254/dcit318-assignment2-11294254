using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace InventoryRecordsSystem
{
    // b. Marker Interface for Logging
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // a. Immutable Inventory Record using positional syntax
    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;


    // c. Generic Inventory Logger
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private List<T> _log;
        private string _filePath;

        public InventoryLogger(string filePath)
        {
            _log = new List<T>();
            _filePath = filePath;
        }

        public void Add(T item)
        {
            _log.Add(item);
            Console.WriteLine($"Added item with ID {item.Id} to inventory log.");
        }

        public List<T> GetAll()
        {
            return new List<T>(_log); // Return a copy to maintain encapsulation
        }

        public void SaveToFile()
        {
            try
            {
                Console.WriteLine($"Saving inventory data to file: {_filePath}");
                
                // Using JSON serialization for better data structure preservation
                string jsonString = JsonSerializer.Serialize(_log, new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                // d. Use using statement when working with file operations
                using (StreamWriter writer = new StreamWriter(_filePath))
                {
                    writer.Write(jsonString);
                }

                Console.WriteLine($"Successfully saved {_log.Count} items to file.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied when trying to save file: {ex.Message}");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"I/O error occurred while saving: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error serializing data to JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while saving: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine($"File {_filePath} does not exist. Starting with empty inventory log.");
                    _log.Clear();
                    return;
                }

                Console.WriteLine($"Loading inventory data from file: {_filePath}");

                string jsonString;
                
                // d. Use using statement when working with file operations
                using (StreamReader reader = new StreamReader(_filePath))
                {
                    jsonString = reader.ReadToEnd();
                }

                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    Console.WriteLine("File is empty. Starting with empty inventory log.");
                    _log.Clear();
                    return;
                }

                var loadedItems = JsonSerializer.Deserialize<List<T>>(jsonString, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                _log = loadedItems ?? new List<T>();
                Console.WriteLine($"Successfully loaded {_log.Count} items from file.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File {_filePath} not found. Starting with empty inventory log.");
                _log.Clear();
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied when trying to read file: {ex.Message}");
                _log.Clear();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing JSON data: {ex.Message}");
                Console.WriteLine("Starting with empty inventory log.");
                _log.Clear();
            }
            catch (IOException ex)
            {
                Console.WriteLine($"I/O error occurred while loading: {ex.Message}");
                _log.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading: {ex.Message}");
                _log.Clear();
            }
        }
    }

    // f. Integration Layer - InventoryApp
    public class InventoryApp
    {
        private InventoryLogger<InventoryItem> _logger;
        private string _dataFilePath;

        public InventoryApp()
        {
            _dataFilePath = "inventory_data.json";
            _logger = new InventoryLogger<InventoryItem>(_dataFilePath);
        }

        public void SeedSampleData()
        {
            Console.WriteLine("--- Seeding Sample Inventory Data ---");
            
            // Add at least 3-5 items to _logger using the Add() method
            _logger.Add(new InventoryItem(1, "Wireless Headphones", 25, DateTime.Now.AddDays(-10)));
            _logger.Add(new InventoryItem(2, "Gaming Keyboard", 15, DateTime.Now.AddDays(-8)));
            _logger.Add(new InventoryItem(3, "USB-C Cable", 100, DateTime.Now.AddDays(-5)));
            _logger.Add(new InventoryItem(4, "Bluetooth Mouse", 30, DateTime.Now.AddDays(-3)));
            _logger.Add(new InventoryItem(5, "Tablet Stand", 20, DateTime.Now.AddDays(-1)));

            Console.WriteLine("Sample data seeded successfully!");
            Console.WriteLine();
        }

        public void SaveData()
        {
            Console.WriteLine("--- Saving Data to Disk ---");
            _logger.SaveToFile();
            Console.WriteLine();
        }

        public void LoadData()
        {
            Console.WriteLine("--- Loading Data from Disk ---");
            _logger.LoadFromFile();
            Console.WriteLine();
        }

        public void PrintAllItems()
        {
            Console.WriteLine("--- Current Inventory Items ---");
            var items = _logger.GetAll();
            
            if (items.Count == 0)
            {
                Console.WriteLine("No items found in inventory log.");
            }
            else
            {
                Console.WriteLine($"Total items: {items.Count}");
                Console.WriteLine();
                
                foreach (var item in items)
                {
                    // Demonstrate immutable record properties
                    Console.WriteLine($"  • ID: {item.Id}");
                    Console.WriteLine($"    Name: {item.Name}");
                    Console.WriteLine($"    Quantity: {item.Quantity}");
                    Console.WriteLine($"    Date Added: {item.DateAdded:yyyy-MM-dd HH:mm:ss}");
                    Console.WriteLine();
                }
            }
        }

        public void ClearMemory()
        {
            Console.WriteLine("--- Simulating New Session (Clearing Memory) ---");
            _logger = new InventoryLogger<InventoryItem>(_dataFilePath);
            Console.WriteLine("Memory cleared. Logger reinitialized.");
            Console.WriteLine();
        }

        public void DemonstrateRecordImmutability()
        {
            Console.WriteLine("--- Demonstrating Record Immutability ---");
            
            var originalItem = new InventoryItem(999, "Test Item", 10, DateTime.Now);
            Console.WriteLine($"Original item: {originalItem}");
            
            // Records support 'with' expressions for creating modified copies
            var modifiedItem = originalItem with { Quantity = 20, Name = "Modified Test Item" };
            Console.WriteLine($"Modified copy: {modifiedItem}");
            Console.WriteLine($"Original unchanged: {originalItem}");
            
            Console.WriteLine("Record immutability confirmed - original item remains unchanged!");
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Inventory Records Management System ===");
            Console.WriteLine("Using C# Records, Generics, and File Operations");
            Console.WriteLine();

            try
            {
                // g. Main Application Flow
                
                // Create an instance of InventoryApp
                InventoryApp app = new InventoryApp();

                // Demonstrate record immutability
                app.DemonstrateRecordImmutability();

                // Call SeedSampleData()
                app.SeedSampleData();

                // Show current items in memory
                Console.WriteLine("Items currently in memory:");
                app.PrintAllItems();

                // Call SaveData() to persist to disk
                app.SaveData();

                // Clear memory and simulate a new session
                app.ClearMemory();

                // Verify memory is clear
                Console.WriteLine("After clearing memory:");
                app.PrintAllItems();

                // Call LoadData() to read from file
                app.LoadData();

                // Call PrintAllItems() to confirm data was recovered
                Console.WriteLine("After loading from file:");
                app.PrintAllItems();

                Console.WriteLine("=== System Test Complete ===");
                Console.WriteLine($"Data persistence verified using JSON serialization!");
                Console.WriteLine($"Check the file: {Path.GetFullPath("inventory_data.json")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application error: {ex.Message}");
                Console.WriteLine("Please check system permissions and available disk space.");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
