using System;

namespace InterfacesDemo
{
    // Interface IMovable with Move method
    public interface IMovable
    {
        void Move();
    }

    // Additional interface to demonstrate multiple interface implementation
    public interface IVehicle
    {
        void Start();
        void Stop();
        string GetVehicleType();
    }

    // Car class that implements IMovable and IVehicle interfaces
    public class Car : IMovable, IVehicle
    {
        private string brand;
        private bool isRunning;

        public Car(string brand)
        {
            this.brand = brand;
            this.isRunning = false;
        }

        // Implementation of IMovable.Move()
        public void Move()
        {
            if (isRunning)
            {
                Console.WriteLine("Car is moving");
            }
            else
            {
                Console.WriteLine("Car needs to be started first!");
            }
        }

        // Implementation of IVehicle.Start()
        public void Start()
        {
            isRunning = true;
            Console.WriteLine($"{brand} car engine started");
        }

        // Implementation of IVehicle.Stop()
        public void Stop()
        {
            isRunning = false;
            Console.WriteLine($"{brand} car engine stopped");
        }

        // Implementation of IVehicle.GetVehicleType()
        public string GetVehicleType()
        {
            return $"{brand} Car";
        }
    }

    // Bicycle class that implements IMovable interface
    public class Bicycle : IMovable
    {
        private string type;

        public Bicycle(string type)
        {
            this.type = type;
        }

        // Implementation of IMovable.Move()
        public void Move()
        {
            Console.WriteLine("Bicycle is moving");
        }

        public string GetBicycleType()
        {
            return $"{type} Bicycle";
        }
    }

    // Additional class to demonstrate interface usage
    public class Airplane : IMovable, IVehicle
    {
        private string model;
        private bool isFlying;

        public Airplane(string model)
        {
            this.model = model;
            this.isFlying = false;
        }

        public void Move()
        {
            if (isFlying)
            {
                Console.WriteLine("Airplane is flying through the sky");
            }
            else
            {
                Console.WriteLine("Airplane is taxiing on the ground");
            }
        }

        public void Start()
        {
            Console.WriteLine($"{model} airplane engines started");
        }

        public void Stop()
        {
            isFlying = false;
            Console.WriteLine($"{model} airplane engines stopped");
        }

        public string GetVehicleType()
        {
            return $"{model} Airplane";
        }

        public void TakeOff()
        {
            isFlying = true;
            Console.WriteLine($"{model} airplane is taking off!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Interfaces Demo ===");
            Console.WriteLine();

            // Create instances of Car and Bicycle
            Car myCar = new Car("Toyota");
            Bicycle myBicycle = new Bicycle("Mountain");
            Airplane myPlane = new Airplane("Boeing 737");

            // Demonstrate basic interface usage
            Console.WriteLine("--- Basic Interface Usage ---");
            
            Console.WriteLine($"Vehicle: {myCar.GetVehicleType()}");
            myCar.Start();
            myCar.Move();
            myCar.Stop();

            Console.WriteLine();

            Console.WriteLine($"Vehicle: {myBicycle.GetBicycleType()}");
            myBicycle.Move();

            Console.WriteLine();

            Console.WriteLine($"Vehicle: {myPlane.GetVehicleType()}");
            myPlane.Start();
            myPlane.Move();
            myPlane.TakeOff();
            myPlane.Move();
            myPlane.Stop();

            Console.WriteLine();

            // Demonstrate polymorphism with interfaces
            Console.WriteLine("--- Polymorphism with Interfaces ---");
            IMovable[] movableObjects = { myCar, myBicycle, myPlane };

            // Start vehicles that can be started
            myCar.Start();
            myPlane.Start();
            myPlane.TakeOff();

            Console.WriteLine("\nMoving all objects:");
            for (int i = 0; i < movableObjects.Length; i++)
            {
                Console.Write($"Object {i + 1}: ");
                movableObjects[i].Move();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
