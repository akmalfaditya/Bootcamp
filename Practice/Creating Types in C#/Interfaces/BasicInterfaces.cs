using System;
using System.Collections;

namespace Interfaces
{
    /// <summary>
    /// Let's start with the fundamentals - what exactly IS an interface?
    /// An interface is a contract that says "if you want to be THIS type of thing,
    /// you MUST be able to do THESE specific things"
    /// 
    /// Here's the classic IEnumerator from System.Collections - perfect example!
    /// It says "if you want to be enumerable, you must know how to move next,
    /// what your current item is, and how to reset yourself"
    /// </summary>
    public interface IEnumeratorDemo // Using our own name to avoid conflicts
    {
        bool MoveNext();      // Method - can you advance to the next item?
        object Current { get; } // Read-only Property - what's the current item?
        void Reset();         // Method - can you go back to the beginning?
        
        // Notice: NO implementation here, just the contract
        // Notice: NO access modifiers - interface members are implicitly public
        // Notice: NO fields allowed - interfaces define behavior, not data
    }

    /// <summary>
    /// Here's how you implement an interface - you fulfill the contract
    /// Even though this class is internal, once cast to the interface,
    /// its interface methods become publicly accessible
    /// </summary>
    internal class Countdown : IEnumeratorDemo
    {
        int count = 11;

        // We MUST implement every member of the interface
        // These implementations are public even though the class is internal
        public bool MoveNext() => count-- > 0;
        public object Current => count;
        public void Reset() { throw new NotSupportedException(); }
    }

    // Let's also keep the shape example - it's great for understanding polymorphism
    public interface IShape
    {
        // Clean, simple contract - any shape must know these two things
        double Area();
        double Perimeter();
    }

    /// <summary>
    /// Circle implementing the shape contract
    /// Shows how different classes can fulfill the same interface differently
    /// </summary>
    public class Circle : IShape
    {
        public double Radius { get; set; }

        public Circle(double radius)
        {
            Radius = radius;
        }

        // Circle's way of calculating area
        public double Area() => Math.PI * Radius * Radius;

        // Circle's way of calculating perimeter
        public double Perimeter() => 2 * Math.PI * Radius;

        public override string ToString() => $"Circle (r={Radius})";
    }

    /// <summary>
    /// Square implementing the same interface, different math
    /// Same contract, completely different implementation
    /// </summary>
    public class Square : IShape
    {
        public double Side { get; set; }

        public Square(double side)
        {
            Side = side;
        }

        // Square's way of calculating area
        public double Area() => Side * Side;

        // Square's way of calculating perimeter
        public double Perimeter() => 4 * Side;

        public override string ToString() => $"Square (side={Side})";
    }

    /// <summary>
    /// Triangle - another shape with its own implementation
    /// Shows how easy it is to add new types that fit the contract
    /// </summary>
    public class Triangle : IShape
    {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }

        public Triangle(double sideA, double sideB, double sideC)
        {
            SideA = sideA;
            SideB = sideB;
            SideC = sideC;
        }

        // Triangle's area using Heron's formula
        public double Area()
        {
            double s = Perimeter() / 2; // semi-perimeter
            return Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
        }

        // Triangle's perimeter - just add all sides
        public double Perimeter() => SideA + SideB + SideC;

        public override string ToString() => $"Triangle ({SideA}, {SideB}, {SideC})";
    }

    /// <summary>
    /// Multiple interfaces demo - this is where interfaces really shine!
    /// A class can only inherit from ONE base class, but it can implement
    /// as many interfaces as it wants. This gives incredible flexibility.
    /// </summary>
    public interface ICommunicationDevice
    {
        void SendMessage(string message);
        void MakeCall(string phoneNumber);
    }

    public interface IEntertainmentDevice
    {
        void PlayMusic(string songName);
        void PlayVideo(string videoName);
    }

    /// <summary>
    /// This smartphone can do communication AND entertainment
    /// Try doing this with class inheritance - you can't!
    /// This is the power of interfaces - multiple capabilities in one type
    /// </summary>
    public class SmartDevice : ICommunicationDevice, IEntertainmentDevice
    {
        public string DeviceName { get; private set; }

        public SmartDevice(string deviceName)
        {
            DeviceName = deviceName;
        }

        // Implementing ICommunicationDevice
        public void SendMessage(string message)
        {
            Console.WriteLine($"{DeviceName}: Sending message: '{message}'");
        }

        public void MakeCall(string phoneNumber)
        {
            Console.WriteLine($"{DeviceName}: Calling {phoneNumber}...");
        }

        // Implementing IEntertainmentDevice  
        public void PlayMusic(string songName)
        {
            Console.WriteLine($"{DeviceName}: 🎵 Playing music: {songName}");
        }

        public void PlayVideo(string videoName)
        {
            Console.WriteLine($"{DeviceName}: 📹 Playing video: {videoName}");
        }

        // Additional device-specific functionality
        public void TakePicture()
        {
            Console.WriteLine($"{DeviceName}: 📸 Taking a picture...");
        }
    }
}
