using System;

namespace InterfacesDemo
{
   
    public interface IMovable
    {
        void Move();
    }

  
    public interface IVehicle
    {
        void Start();
        void Stop();
        string GetVehicleType();
    }

   
    public class Car : IMovable, IVehicle
    {
        private string brand;
        private bool isRunning;

        public Car(string brand)
        {
            this.brand = brand;
            this.isRunning = false;
        }

       
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

    
        public void Start()
        {
            isRunning = true;
            Console.WriteLine($"{brand} car engine started");
        }

      
        public void Stop()
        {
            isRunning = false;
            Console.WriteLine($"{brand} car engine stopped");
        }

      
        public string GetVehicleType()
        {
            return $"{brand} Car";
        }
    }

    public class Bicycle : IMovable
    {
        private string type;

        public Bicycle(string type)
        {
            this.type = type;
        }

      
        public void Move()
        {
            Console.WriteLine("Bicycle is moving");
        }

        public string GetBicycleType()
        {
            return $"{type} Bicycle";
        }
    }

   
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

          
            Car myCar = new Car("Toyota");
            Bicycle myBicycle = new Bicycle("Mountain");
            Airplane myPlane = new Airplane("Boeing 737");

           
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

            Console.WriteLine("--- Polymorphism with Interfaces ---");
            IMovable[] movableObjects = { myCar, myBicycle, myPlane };

           
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
