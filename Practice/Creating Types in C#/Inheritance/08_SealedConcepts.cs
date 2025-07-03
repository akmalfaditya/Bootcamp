using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates the 'sealed' keyword in C# inheritance.
    /// The sealed keyword prevents further inheritance or overriding, providing
    /// control over how inheritance hierarchies can be extended.
    /// 
    /// Key concepts:
    /// 1. Sealed classes - cannot be inherited
    /// 2. Sealed methods - cannot be overridden further
    /// 3. When and why to use sealed
    /// 4. Performance benefits of sealed classes
    /// </summary>

    // Base class for our sealed demonstrations
    public class Shape
    {
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = "White";
        
        public Shape(string name)
        {
            Name = name;
            Console.WriteLine($"Shape constructor: Creating {name}");
        }
        
        // Virtual method that can be overridden
        public virtual double CalculateArea()
        {
            Console.WriteLine("Base Shape.CalculateArea() called");
            return 0.0;
        }
        
        // Virtual method that can be overridden
        public virtual double CalculatePerimeter()
        {
            Console.WriteLine("Base Shape.CalculatePerimeter() called");
            return 0.0;
        }
        
        // Virtual method for display
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Shape: {Name}, Color: {Color}");
            Console.WriteLine($"Area: {CalculateArea():F2}");
            Console.WriteLine($"Perimeter: {CalculatePerimeter():F2}");
        }
        
        // Virtual method that will be sealed in derived class
        public virtual void DrawShape()
        {
            Console.WriteLine($"Drawing basic {Name}...");
        }
        
        // Virtual method that will have different sealing behavior
        public virtual void Rotate(double degrees)
        {
            Console.WriteLine($"Rotating {Name} by {degrees} degrees");
        }
    }

    // First level derived class
    public class Polygon : Shape
    {
        public int NumberOfSides { get; set; }
        
        public Polygon(string name, int sides) : base(name)
        {
            NumberOfSides = sides;
            Console.WriteLine($"Polygon constructor: {sides}-sided polygon");
        }
        
        // Override the virtual method
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Number of sides: {NumberOfSides}");
        }
        
        // Override and then SEAL this method - no further overriding allowed
        public sealed override void DrawShape()
        {
            Console.WriteLine($"Drawing polygon {Name} with {NumberOfSides} sides using advanced graphics...");
            Console.WriteLine("This drawing method is SEALED - cannot be overridden further!");
        }
        
        // Override but don't seal - can be overridden again
        public override void Rotate(double degrees)
        {
            Console.WriteLine($"Rotating polygon {Name} around its center by {degrees} degrees");
        }
    }

    // Second level derived class - inherits from Polygon
    public class Rectangle : Polygon
    {
        public double Width { get; set; }
        public double Height { get; set; }
        
        public Rectangle(double width, double height) : base("Rectangle", 4)
        {
            Width = width;
            Height = height;
            Console.WriteLine($"Rectangle constructor: {width} x {height}");
        }
        
        public override double CalculateArea()
        {
            return Width * Height;
        }
        
        public override double CalculatePerimeter()
        {
            return 2 * (Width + Height);
        }
        
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Dimensions: {Width} x {Height}");
        }
        
        // CANNOT override DrawShape because it was sealed in Polygon!
        // public override void DrawShape() // This would cause a compile error
        // {
        //     Console.WriteLine("Cannot override sealed method!");
        // }
        
        // CAN override Rotate because it wasn't sealed
        public override void Rotate(double degrees)
        {
            Console.WriteLine($"Rotating rectangle ({Width} x {Height}) by {degrees} degrees");
            // Swap dimensions if rotating 90 or 270 degrees
            if (degrees % 180 == 90)
            {
                (Width, Height) = (Height, Width);
                Console.WriteLine($"Dimensions swapped to: {Width} x {Height}");
            }
        }
        
        // Rectangle-specific method
        public bool IsSquare()
        {
            return Math.Abs(Width - Height) < 0.001;
        }
    }

    // SEALED CLASS - cannot be inherited further
    public sealed class Circle : Shape
    {
        public double Radius { get; set; }
        
        public Circle(double radius) : base("Circle")
        {
            Radius = radius;
            Console.WriteLine($"Circle constructor: radius {radius}");
        }
        
        public override double CalculateArea()
        {
            return Math.PI * Radius * Radius;
        }
        
        public override double CalculatePerimeter()
        {
            return 2 * Math.PI * Radius;
        }
        
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Radius: {Radius}");
        }
        
        public override void DrawShape()
        {
            Console.WriteLine($"Drawing perfect circle with radius {Radius}");
            Console.WriteLine("Circle class is SEALED - no inheritance allowed!");
        }
        
        public override void Rotate(double degrees)
        {
            Console.WriteLine($"Circle rotation has no visual effect (still a circle after {degrees} degrees)");
        }
        
        // Circle-specific methods
        public double GetDiameter() => Radius * 2;
        public double GetCircumference() => CalculatePerimeter();
    }

    // This would cause a compile error because Circle is sealed:
    // public class Ellipse : Circle  // Error: Cannot inherit from sealed class 'Circle'
    // {
    // }

    // Another sealed class example - specialized rectangle
    public sealed class Square : Rectangle
    {
        public Square(double side) : base(side, side)
        {
            Console.WriteLine($"Square constructor: side length {side}");
        }
        
        // Override to provide square-specific implementation
        public override void DisplayInfo()
        {
            Console.WriteLine($"Square: {Name}, Color: {Color}");
            Console.WriteLine($"Side length: {Width}");
            Console.WriteLine($"Area: {CalculateArea():F2}");
            Console.WriteLine($"Perimeter: {CalculatePerimeter():F2}");
        }
        
        // Square-specific method
        public void Resize(double newSide)
        {
            Width = Height = newSide;
            Console.WriteLine($"Square resized to {newSide} x {newSide}");
        }
        
        // Override rotation to maintain square properties
        public override void Rotate(double degrees)
        {
            Console.WriteLine($"Rotating square by {degrees} degrees (remains square)");
            // No dimension changes needed for square
        }
    }

    // This would cause a compile error because Square is sealed:
    // public class SpecialSquare : Square  // Error: Cannot inherit from sealed class 'Square'
    // {
    // }

    // Demonstration of why you might want to seal methods or classes
    public class SecuritySensitiveClass
    {
        protected string _securityToken = "SECRET_TOKEN_123";
        
        // Virtual method that handles security
        public virtual bool Authenticate(string password)
        {
            Console.WriteLine("Base authentication logic");
            return password == "admin123";
        }
        
        // Virtual method that will be sealed - critical security logic that shouldn't be overridden
        public virtual void LogSecurityEvent(string eventDescription)
        {
            Console.WriteLine($"[SECURITY LOG] {DateTime.Now}: {eventDescription}");
            Console.WriteLine("This security logging will be sealed in derived class!");
        }
        
        public virtual void PerformSecureOperation()
        {
            Console.WriteLine("Performing secure operation...");
            LogSecurityEvent("Secure operation performed");
        }
    }

    public class ExtendedSecurityClass : SecuritySensitiveClass
    {
        // Can override this method
        public override bool Authenticate(string password)
        {
            Console.WriteLine("Extended authentication with additional checks");
            bool baseResult = base.Authenticate(password);
            
            // Add additional validation
            if (baseResult && password.Length >= 8)
            {
                LogSecurityEvent($"Successful authentication with strong password");
                return true;
            }
            
            LogSecurityEvent($"Authentication failed");
            return false;
        }
        
        // Override and SEAL the security logging method
        public sealed override void LogSecurityEvent(string eventDescription)
        {
            Console.WriteLine($"[ENHANCED SECURITY LOG] {DateTime.Now}: {eventDescription}");
            Console.WriteLine("This enhanced security logging is now SEALED - cannot be overridden further!");
            
            // Call base implementation for audit trail
            base.LogSecurityEvent(eventDescription);
        }
        
        public override void PerformSecureOperation()
        {
            Console.WriteLine("Extended secure operation with additional steps");
            base.PerformSecureOperation();
            LogSecurityEvent("Extended secure operation completed");
        }
    }

    // Final sealed class in our security hierarchy
    public sealed class FinalSecurityClass : ExtendedSecurityClass
    {
        public override bool Authenticate(string password)
        {
            Console.WriteLine("Final security implementation - maximum protection");
            return base.Authenticate(password) && password.Contains("@");
        }
        
        // CANNOT override LogSecurityEvent because it was sealed in ExtendedSecurityClass
        // public override void LogSecurityEvent(string eventDescription) // This would cause compile error
        // {
        //     Console.WriteLine("Cannot override sealed security method!");
        // }
        
        // This class is sealed, so no further inheritance is possible
    }

    /// <summary>
    /// Demonstration class for sealed classes and methods
    /// </summary>
    public static class SealedDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== SEALED CLASSES AND METHODS DEMONSTRATION ===\n");
            
            // 1. DEMONSTRATE INHERITANCE HIERARCHY WITH SEALED METHODS
            Console.WriteLine("1. INHERITANCE HIERARCHY WITH SEALED METHODS:");
            Console.WriteLine("Notice how sealed methods prevent further overriding\n");
            
            var rectangle = new Rectangle(10, 5);
            rectangle.Color = "Blue";
            
            var square = new Square(7);
            square.Color = "Red";
            
            Console.WriteLine("--- Rectangle ---");
            rectangle.DisplayInfo();
            rectangle.DrawShape(); // Uses sealed method from Polygon
            rectangle.Rotate(90);
            Console.WriteLine($"Is square: {rectangle.IsSquare()}");
            Console.WriteLine();
            
            Console.WriteLine("--- Square (sealed class) ---");
            square.DisplayInfo();
            square.DrawShape(); // Uses sealed method from Polygon
            square.Rotate(45);
            square.Resize(10);
            Console.WriteLine();
            
            // 2. DEMONSTRATE SEALED CLASS - CIRCLE
            Console.WriteLine("2. SEALED CLASS DEMONSTRATION:");
            Console.WriteLine("Circle is sealed - cannot be inherited\n");
            
            var circle = new Circle(5.5);
            circle.Color = "Green";
            
            Console.WriteLine("--- Circle (sealed class) ---");
            circle.DisplayInfo();
            circle.DrawShape();
            circle.Rotate(180);
            Console.WriteLine($"Diameter: {circle.GetDiameter():F2}");
            Console.WriteLine($"Circumference: {circle.GetCircumference():F2}");
            Console.WriteLine();
            
            // 3. DEMONSTRATE POLYMORPHIC BEHAVIOR WITH SEALED CLASSES
            Console.WriteLine("3. POLYMORPHIC BEHAVIOR:");
            Console.WriteLine("Sealed classes still work polymorphically through base references\n");
            
            Shape[] shapes = { rectangle, square, circle };
            
            foreach (Shape shape in shapes)
            {
                Console.WriteLine($"--- {shape.Name} through Shape reference ---");
                shape.DisplayInfo();
                shape.DrawShape();
                shape.Rotate(30);
                Console.WriteLine();
            }
            
            // 4. DEMONSTRATE SECURITY CLASS WITH SEALED METHODS
            Console.WriteLine("4. SECURITY CLASSES WITH SEALED METHODS:");
            Console.WriteLine("Sealed methods protect critical functionality\n");
            
            var basicSecurity = new SecuritySensitiveClass();
            var extendedSecurity = new ExtendedSecurityClass();
            var finalSecurity = new FinalSecurityClass();
            
            Console.WriteLine("--- Basic Security Class ---");
            basicSecurity.PerformSecureOperation();
            Console.WriteLine($"Auth result: {basicSecurity.Authenticate("admin123")}");
            Console.WriteLine();
            
            Console.WriteLine("--- Extended Security Class ---");
            extendedSecurity.PerformSecureOperation();
            Console.WriteLine($"Auth result: {extendedSecurity.Authenticate("admin123")}");
            Console.WriteLine($"Auth with strong password: {extendedSecurity.Authenticate("strongpass123")}");
            Console.WriteLine();
            
            Console.WriteLine("--- Final Security Class (sealed) ---");
            finalSecurity.PerformSecureOperation();
            Console.WriteLine($"Auth result: {finalSecurity.Authenticate("admin123")}");
            Console.WriteLine($"Auth with email format: {finalSecurity.Authenticate("admin@test.com")}");
            Console.WriteLine();
            
            // 5. DEMONSTRATE POLYMORPHIC SECURITY USAGE
            Console.WriteLine("5. POLYMORPHIC SECURITY USAGE:");
            SecuritySensitiveClass[] securityObjects = { basicSecurity, extendedSecurity, finalSecurity };
            
            string[] testPasswords = { "weak", "admin123", "strongpass123", "admin@test.com" };
            
            foreach (var securityObj in securityObjects)
            {
                Console.WriteLine($"--- {securityObj.GetType().Name} ---");
                foreach (var password in testPasswords)
                {
                    bool result = securityObj.Authenticate(password);
                    Console.WriteLine($"Password '{password}': {(result ? "SUCCESS" : "FAILED")}");
                }
                Console.WriteLine();
            }
            
            // 6. PERFORMANCE AND DESIGN BENEFITS
            Console.WriteLine("6. BENEFITS OF SEALED CLASSES AND METHODS:");
            ShowSealedBenefits();
        }
        
        private static void ShowSealedBenefits()
        {
            Console.WriteLine("=== BENEFITS OF USING SEALED ===");
            Console.WriteLine();
            
            Console.WriteLine("SEALED CLASSES:");
            Console.WriteLine("✓ Performance: JIT compiler can optimize method calls (no virtual dispatch)");
            Console.WriteLine("✓ Security: Prevents malicious inheritance that could bypass security");
            Console.WriteLine("✓ Design clarity: Makes it clear the class is complete and shouldn't be extended");
            Console.WriteLine("✓ API stability: Users can't break your class by inheriting incorrectly");
            Console.WriteLine();
            
            Console.WriteLine("SEALED METHODS:");
            Console.WriteLine("✓ Partial performance benefits for specific methods");
            Console.WriteLine("✓ Security: Protects critical method implementations");
            Console.WriteLine("✓ Design control: Allows overriding up to a point, then locks implementation");
            Console.WriteLine("✓ Template pattern: Define algorithm structure but seal specific steps");
            Console.WriteLine();
            
            Console.WriteLine("WHEN TO USE SEALED:");
            Console.WriteLine("• Utility classes that shouldn't be extended (like Math, String)");
            Console.WriteLine("• Performance-critical classes where virtual dispatch is expensive");
            Console.WriteLine("• Security-sensitive classes that could be exploited through inheritance");
            Console.WriteLine("• Classes that represent final, complete concepts (like specific shapes)");
            Console.WriteLine("• When you want to prevent misuse of your API through inheritance");
            Console.WriteLine();
            
            Console.WriteLine("WHEN NOT TO USE SEALED:");
            Console.WriteLine("• When extensibility is a design goal");
            Console.WriteLine("• In library code where users might want to extend functionality");
            Console.WriteLine("• When following the Open/Closed Principle (open for extension)");
            Console.WriteLine("• Early in development when requirements might change");
        }
    }
}
