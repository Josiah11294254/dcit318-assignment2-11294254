using System;

namespace AbstractClassesDemo
{
    // Abstract base class Shape
    public abstract class Shape
    {
        // Abstract method that must be implemented by derived classes
        public abstract double GetArea();

        // Non-abstract method that can be used by all derived classes
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"This is a shape with area: {GetArea():F2}");
        }
    }

    // Derived class Circle that implements the abstract GetArea method
    public class Circle : Shape
    {
        private double radius;

        public Circle(double radius)
        {
            this.radius = radius;
        }

        public double Radius 
        { 
            get { return radius; } 
            set { radius = value; } 
        }

        // Implementation of abstract method GetArea
        public override double GetArea()
        {
            return Math.PI * radius * radius;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Circle with radius {radius:F2} has area: {GetArea():F2}");
        }
    }

    // Derived class Rectangle that implements the abstract GetArea method
    public class Rectangle : Shape
    {
        private double width;
        private double height;

        public Rectangle(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        public double Width 
        { 
            get { return width; } 
            set { width = value; } 
        }

        public double Height 
        { 
            get { return height; } 
            set { height = value; } 
        }

        // Implementation of abstract method GetArea
        public override double GetArea()
        {
            return width * height;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Rectangle with width {width:F2} and height {height:F2} has area: {GetArea():F2}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Abstract Classes and Methods Demo ===");
            Console.WriteLine();

            // Create instances of Circle and Rectangle
            Circle myCircle = new Circle(5.0);
            Rectangle myRectangle = new Rectangle(4.0, 6.0);

            // Display their areas
            Console.WriteLine("Shape Areas:");
            myCircle.DisplayInfo();
            myRectangle.DisplayInfo();

            Console.WriteLine();

            // Demonstrate polymorphism with abstract class
            Console.WriteLine("--- Polymorphism with Abstract Classes ---");
            Shape[] shapes = { new Circle(3.0), new Rectangle(2.5, 4.0), new Circle(7.2) };

            for (int i = 0; i < shapes.Length; i++)
            {
                Console.Write($"Shape {i + 1}: ");
                Console.WriteLine($"Area = {shapes[i].GetArea():F2}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
