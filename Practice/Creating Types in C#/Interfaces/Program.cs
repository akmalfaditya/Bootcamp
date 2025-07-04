using System;
using System.Collections;

namespace Interfaces
{
    /// <summary>
    /// Welcome to the complete C# Interfaces masterclass!
    /// 
    /// An interface is a contract that defines WHAT a type can do, not HOW it does it.
    /// Think of it like a job description - it lists the requirements,
    /// but doesn't tell you exactly how to fulfill them.
    /// 
    /// We'll cover everything from the basics to the most advanced features.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║               C# INTERFACES MASTERCLASS                     ║");
            Console.WriteLine("║          From Basic Contracts to Advanced Features          ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");

            // Let's walk through each concept step by step
            DemonstrateBasicConcepts();
            DemonstrateInterfaceInheritance();
            DemonstrateExplicitImplementation();
            DemonstrateVirtualAndReimplementation();
            DemonstrateBoxing();
            DemonstrateModernFeatures();
            DemonstrateClassVsInterfaceDesign();

            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    MASTERCLASS COMPLETE!                    ║");
            Console.WriteLine("║   You now understand interfaces from basic to advanced!     ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void SectionHeader(string title)
        {
            Console.WriteLine($"\n┌─ {title} ".PadRight(65, '─') + "┐");
        }

        static void SectionFooter()
        {
            Console.WriteLine("└".PadRight(65, '─') + "┘\n");
        }

        /// <summary>
        /// 1. Basic Interface Concepts
        /// Understanding what interfaces are and how they work
        /// </summary>
        static void DemonstrateBasicConcepts()
        {
            SectionHeader("1. BASIC INTERFACE CONCEPTS");
            
            Console.WriteLine("An interface is a contract. Let's see this with IEnumerator:");
            
            // Using our Countdown class that implements IEnumeratorDemo
            IEnumeratorDemo countdown = new Countdown();
            
            Console.Write("Countdown: ");
            while (countdown.MoveNext())
            {
                Console.Write(countdown.Current + " ");
            }
            Console.WriteLine("\n");
            
            Console.WriteLine("Key points:");
            Console.WriteLine("✓ Interface defines WHAT to do, not HOW");
            Console.WriteLine("✓ Classes provide the actual implementation");
            Console.WriteLine("✓ You can treat objects as their interface type (polymorphism)");
            
            // Multiple interfaces example
            Console.WriteLine("\nMultiple Interfaces Demo:");
            var phone = new SmartDevice("iPhone 15");
            
            // Use as communication device
            ICommunicationDevice comm = phone;
            comm.SendMessage("Hello Interface World!");
            
            // Use as entertainment device
            IEntertainmentDevice entertainment = phone;
            entertainment.PlayMusic("Bohemian Rhapsody");
            
            Console.WriteLine("\n✓ One class can implement multiple interfaces");
            Console.WriteLine("✓ This is impossible with class inheritance (single inheritance rule)");
            
            SectionFooter();
        }

        /// <summary>
        /// 2. Interface Inheritance
        /// How interfaces can extend other interfaces
        /// </summary>
        static void DemonstrateInterfaceInheritance()
        {
            SectionHeader("2. INTERFACE INHERITANCE");
            
            Console.WriteLine("Interfaces can inherit from other interfaces:");
            Console.WriteLine("IRedoable extends IUndoable - it inherits Undo() and adds Redo()");
            
            var editor = new TextEditor();
            editor.EditText("Hello World");
            
            // Can use as IUndoable
            IUndoable undoable = editor;
            undoable.Undo();
            
            // Can also use as IRedoable (which includes IUndoable)
            IRedoable redoable = editor;
            redoable.Redo();
            redoable.Undo(); // Still has access to inherited method
            
            Console.WriteLine("\n✓ Derived interfaces inherit all members from base interfaces");
            Console.WriteLine("✓ Implementing classes must implement ALL inherited members");
            
            SectionFooter();
        }

        /// <summary>
        /// 3. Explicit Interface Implementation
        /// Solving name collisions and hiding specialized members
        /// </summary>
        static void DemonstrateExplicitImplementation()
        {
            SectionHeader("3. EXPLICIT INTERFACE IMPLEMENTATION");
            
            ExplicitImplementationDemo.RunDemo();
            
            Console.WriteLine("\n✓ Explicit implementation solves method name collisions");
            Console.WriteLine("✓ Explicitly implemented members are only accessible via interface cast");
            Console.WriteLine("✓ Use this to hide specialized interface members from public API");
            
            SectionFooter();
        }

        /// <summary>
        /// 4. Virtual Implementation and Reimplementation
        /// Advanced inheritance scenarios with interfaces
        /// </summary>
        static void DemonstrateVirtualAndReimplementation()
        {
            SectionHeader("4. VIRTUAL IMPLEMENTATION & REIMPLEMENTATION");
            
            VirtualImplementationDemo.RunDemo();
            
            Console.WriteLine("\n✓ Mark interface implementations as virtual for proper polymorphism");
            Console.WriteLine("✓ Subclasses can reimplement interfaces to 'hijack' the contract");
            Console.WriteLine("✓ Protected virtual helper pattern is the most robust approach");
            
            SectionFooter();
        }

        /// <summary>
        /// 5. Boxing with Value Types
        /// Understanding performance implications
        /// </summary>
        static void DemonstrateBoxing()
        {
            SectionHeader("5. INTERFACES AND BOXING");
            
            BoxingDemo.RunDemo();
            
            Console.WriteLine("\n✓ Casting struct to interface causes boxing (performance cost)");
            Console.WriteLine("✓ The interface holds a copy, not a reference to the original");
            Console.WriteLine("✓ Be mindful of this in performance-critical code");
            
            SectionFooter();
        }

        /// <summary>
        /// 6. Modern Interface Features
        /// C# 8+ and C# 11+ enhancements
        /// </summary>
        static void DemonstrateModernFeatures()
        {
            SectionHeader("6. MODERN INTERFACE FEATURES (C# 8+/11+)");
            
            ModernFeaturesDemo.RunDemo();
            
            Console.WriteLine("\n✓ Default interface members enable API evolution without breaking changes");
            Console.WriteLine("✓ Static interface members support advanced patterns like Generic Math");
            Console.WriteLine("✓ Static abstract members enable static polymorphism");
            
            SectionFooter();
        }

        /// <summary>
        /// 7. Design Philosophy: When to Use Classes vs Interfaces
        /// The big picture design decisions
        /// </summary>
        static void DemonstrateClassVsInterfaceDesign()
        {
            SectionHeader("7. CLASS VS INTERFACE DESIGN PHILOSOPHY");
            
            ClassVsInterfaceDemo.RunDemo();
            
            Console.WriteLine("\n✓ Use classes for 'is a' relationships (shared identity/implementation)");
            Console.WriteLine("✓ Use interfaces for 'can do' relationships (shared capabilities)");
            Console.WriteLine("✓ This design enables flexibility and extensibility");
            Console.WriteLine("✓ Interfaces are essential for dependency injection and testing");
            
            SectionFooter();
        }
    }
}
