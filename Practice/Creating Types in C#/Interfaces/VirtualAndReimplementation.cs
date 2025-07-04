using System;

namespace Interfaces
{
    /// <summary>
    /// Virtual Interface Implementation and Reimplementation
    /// This covers some advanced scenarios when dealing with interface inheritance
    /// </summary>

    // We'll use this simple interface for our examples
    public interface IUndoable_Advanced
    {
        void Undo();
    }

    #region Virtual Interface Implementation
    
    /// <summary>
    /// PATTERN 1: Virtual Interface Implementation
    /// This is the recommended approach - mark interface implementations as virtual
    /// to allow proper polymorphic behavior in derived classes
    /// </summary>
    public class BasicTextBox : IUndoable_Advanced
    {
        // Mark as virtual to allow subclasses to override
        public virtual void Undo() => Console.WriteLine("BasicTextBox.Undo");
    }

    public class AdvancedTextBox : BasicTextBox
    {
        // Override the virtual implementation
        public override void Undo() => Console.WriteLine("AdvancedTextBox.Undo");
    }

    #endregion

    #region Interface Reimplementation

    /// <summary>
    /// PATTERN 2: Interface Reimplementation
    /// This is more advanced - a subclass can "hijack" an interface implementation
    /// Most effective when the base class uses explicit implementation
    /// </summary>
    public class Document : IUndoable_Advanced
    {
        // Explicit implementation - only accessible via interface cast
        void IUndoable_Advanced.Undo() => Console.WriteLine("Document.Undo");
    }

    // Notice: We re-declare the interface here!
    public class Report : Document, IUndoable_Advanced
    {
        // New implicit (public) implementation
        // This "hijacks" the interface when called on Report instances
        public void Undo() => Console.WriteLine("Report.Undo (reimplemented)");
    }

    #endregion

    #region Protected Virtual Helper Pattern

    /// <summary>
    /// PATTERN 3: Protected Virtual Helper (BEST PRACTICE)
    /// This gives you the benefits of explicit implementation with the flexibility
    /// of virtual methods. This is the most robust pattern.
    /// </summary>
    public class Editor : IUndoable_Advanced
    {
        // Explicit implementation delegates to protected virtual method
        void IUndoable_Advanced.Undo() => UndoCore();

        // Subclasses override this instead of the interface method
        protected virtual void UndoCore() => Console.WriteLine("Editor.Undo");
    }

    public class HtmlEditor : Editor
    {
        // Override the protected virtual method
        protected override void UndoCore() => Console.WriteLine("HtmlEditor.Undo");
    }

    #endregion

    /// <summary>
    /// Demo class to show all three patterns in action
    /// </summary>
    public static class VirtualImplementationDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Virtual and Reimplementation Demo ===\n");

            // Pattern 1: Virtual/Override (Clean and predictable)
            Console.WriteLine("--- Pattern 1: Virtual/Override ---");
            AdvancedTextBox advancedText = new AdvancedTextBox();
            advancedText.Undo(); // AdvancedTextBox.Undo

            IUndoable_Advanced undoableAdvanced = advancedText;
            undoableAdvanced.Undo(); // Still AdvancedTextBox.Undo (polymorphism works)

            // Pattern 2: Reimplementation (Powerful but confusing)
            Console.WriteLine("\n--- Pattern 2: Reimplementation ---");
            Report report = new Report();
            report.Undo(); // Report.Undo (reimplemented)

            IUndoable_Advanced undoableReport = report;
            undoableReport.Undo(); // Report.Undo (interface hijacked)

            // Pattern 3: Protected Virtual Helper (Best practice)
            Console.WriteLine("\n--- Pattern 3: Protected Virtual Helper ---");
            HtmlEditor htmlEditor = new HtmlEditor();
            IUndoable_Advanced undoableHtml = htmlEditor;
            undoableHtml.Undo(); // HtmlEditor.Undo (via protected override)
        }
    }
}
