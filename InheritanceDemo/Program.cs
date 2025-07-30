using System;

namespace InheritanceDemo
{
    // Base class Animal
    public class Animal
    {
        // Virtual method that can be overridden by derived classes
        public virtual void MakeSound()
        {
            Console.WriteLine("Some generic sound");
        }
    }

    // Derived class Dog that inherits from Animal
    public class Dog : Animal
    {
        // Override the MakeSound method
        public override void MakeSound()
        {
            Console.WriteLine("Bark");
        }
    }

    // Derived class Cat that inherits from Animal
    public class Cat : Animal
    {
        // Override the MakeSound method
        public override void MakeSound()
        {
            Console.WriteLine("Meow");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Inheritance and Method Overriding Demo ===");
            Console.WriteLine();

            // Create instances of Animal, Dog, and Cat
            Animal genericAnimal = new Animal();
            Dog myDog = new Dog();
            Cat myCat = new Cat();

            // Call MakeSound() method on each instance
            Console.WriteLine("Generic Animal:");
            genericAnimal.MakeSound();

            Console.WriteLine("\nDog:");
            myDog.MakeSound();

            Console.WriteLine("\nCat:");
            myCat.MakeSound();

            // Demonstrate polymorphism
            Console.WriteLine("\n--- Polymorphism Demo ---");
            Animal[] animals = { new Animal(), new Dog(), new Cat() };
            
            for (int i = 0; i < animals.Length; i++)
            {
                Console.Write($"Animal {i + 1}: ");
                animals[i].MakeSound();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
