using System;

namespace Interfaces
{
    /// <summary>
    /// Basic shape interface - the classic example everyone uses
    /// Think of this as a "shape contract" - any shape must know its area and perimeter
    /// Simple, focused, and demonstrates the core concept perfectly
    /// </summary>
    public interface IShape
    {
        // No implementation, just the contract
        // Any class implementing this MUST provide these methods
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
    /// Communication device interface
    /// Defines what it means to be able to communicate
    /// </summary>
    public interface ICommunicationDevice
    {
        void SendMessage(string message);
        void MakeCall(string phoneNumber);
    }

    /// <summary>
    /// Entertainment device interface
    /// Defines what it means to provide entertainment
    /// </summary>
    public interface IEntertainmentDevice
    {
        void PlayMusic(string songName);
        void PlayVideo(string videoName);
    }

    /// <summary>
    /// Smart device that can do multiple things
    /// Implements multiple interfaces - it's multi-talented!
    /// This is where interfaces really shine vs single inheritance
    /// </summary>
    public class SmartDevice : ICommunicationDevice, IEntertainmentDevice
    {
        public string DeviceName { get; private set; }

        public SmartDevice(string deviceName)
        {
            DeviceName = deviceName;
        }

        // ICommunicationDevice implementation
        public void SendMessage(string message)
        {
            Console.WriteLine($"{DeviceName}: Sending message: '{message}'");
        }

        public void MakeCall(string phoneNumber)
        {
            Console.WriteLine($"{DeviceName}: Calling {phoneNumber}...");
        }

        // IEntertainmentDevice implementation
        public void PlayMusic(string songName)
        {
            Console.WriteLine($"{DeviceName}: 🎵 Playing music: {songName}");
        }

        public void PlayVideo(string videoName)
        {
            Console.WriteLine($"{DeviceName}: 📹 Playing video: {videoName}");
        }

        // Additional methods specific to SmartDevice
        public void TakePicture()
        {
            Console.WriteLine($"{DeviceName}: 📸 Taking a picture...");
        }

        public void BrowseInternet(string url)
        {
            Console.WriteLine($"{DeviceName}: 🌐 Browsing to {url}");
        }
    }
}
