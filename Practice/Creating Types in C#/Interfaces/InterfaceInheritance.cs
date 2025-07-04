using System;

namespace Interfaces
{
    /// <summary>
    /// Interface inheritance - building contracts on top of other contracts
    /// This is like saying "if you can undo, maybe you can also redo"
    /// The derived interface inherits ALL members from the base interface
    /// </summary>
    
    // Base contract - simple undo capability
    public interface IUndoable
    {
        void Undo();
    }

    // Extended contract - builds on undo, adds redo
    // Any class implementing IRedoable MUST also implement IUndoable
    public interface IRedoable : IUndoable
    {
        void Redo(); // New requirement
        // Note: Undo() is inherited from IUndoable
    }

    /// <summary>
    /// Text editor that supports both undo and redo
    /// Must implement BOTH Undo() and Redo() because IRedoable extends IUndoable
    /// </summary>
    public class TextEditor : IRedoable
    {
        private string _currentText = "";
        private string _previousText = "";

        // Required by IUndoable (inherited through IRedoable)
        public void Undo()
        {
            Console.WriteLine("TextEditor: Undoing last edit...");
            var temp = _currentText;
            _currentText = _previousText;
            _previousText = temp;
        }

        // Required by IRedoable
        public void Redo()
        {
            Console.WriteLine("TextEditor: Redoing last edit...");
            // In a real app, you'd have a proper undo/redo stack
            Undo(); // Simple toggle for demo
        }

        public void EditText(string newText)
        {
            _previousText = _currentText;
            _currentText = newText;
            Console.WriteLine($"TextEditor: Text changed to '{newText}'");
        }

        public void Save()
        {
            Console.WriteLine("TextEditor: Document saved");
        }
    }

    /// <summary>
    /// Worker interface - basic work contract
    /// </summary>
    public interface IWorker
    {
        void Work();
    }

    /// <summary>
    /// Cleaning robot interface with specific work type
    /// Same method name as IWorker.Work(), but different context
    /// </summary>
    public interface ICleaningRobot
    {
        void Work(); // This will conflict with IWorker.Work()
    }

    /// <summary>
    /// Security robot interface with its own work definition
    /// Another Work() method - now we have three!
    /// </summary>
    public interface ISecurityRobot
    {
        void Work(); // Yet another Work() method
    }

    /// <summary>
    /// Multi-function robot that can be many things
    /// Demonstrates explicit interface implementation to resolve conflicts
    /// Same method name, but different behaviors depending on interface
    /// </summary>
    public class MultiFunctionRobot : IWorker, ICleaningRobot, ISecurityRobot
    {
        public string RobotId { get; private set; }

        public MultiFunctionRobot()
        {
            RobotId = $"ROBOT-{DateTime.Now.Ticks % 10000}";
        }

        // Default/implicit implementation - this is what gets called 
        // when you call Work() directly on the object
        public void Work()
        {
            Console.WriteLine($"{RobotId}: Performing general work tasks");
        }

        // Explicit implementation for ICleaningRobot
        // Only accessible when cast to ICleaningRobot
        void ICleaningRobot.Work()
        {
            Console.WriteLine($"{RobotId}: Cleaning mode activated - vacuuming and sanitizing");
        }

        // Explicit implementation for ISecurityRobot  
        // Only accessible when cast to ISecurityRobot
        void ISecurityRobot.Work()
        {
            Console.WriteLine($"{RobotId}: Security mode activated - patrolling and monitoring");
        }

        // Additional robot capabilities
        public void SelfDiagnostic()
        {
            Console.WriteLine($"{RobotId}: Running self-diagnostic... All systems operational");
        }
    }

    /// <summary>
    /// Basic text box with simple undo functionality
    /// This will be our "base" implementation
    /// </summary>
    public class TextBox : IUndoable
    {
        protected string _content = "";

        public virtual void Undo()
        {
            Console.WriteLine("TextBox: Basic undo operation");
            _content = ""; // Simple undo - just clear
        }

        public virtual void SetText(string text)
        {
            _content = text;
            Console.WriteLine($"TextBox: Content set to '{text}'");
        }
    }

    /// <summary>
    /// Rich text box that reimplements undo with enhanced functionality
    /// Shows how derived classes can provide better implementations
    /// Same interface, upgraded behavior
    /// </summary>
    public class RichTextBox : TextBox, IUndoable
    {
        private System.Collections.Generic.Stack<string> _undoStack = new();

        public override void SetText(string text)
        {
            // Save current state before changing
            if (!string.IsNullOrEmpty(_content))
            {
                _undoStack.Push(_content);
            }
            
            base.SetText(text);
        }

        // Reimplementation of IUndoable.Undo with enhanced functionality
        public new void Undo()
        {
            Console.WriteLine("RichTextBox: Enhanced undo with history stack");
            
            if (_undoStack.Count > 0)
            {
                _content = _undoStack.Pop();
                Console.WriteLine($"RichTextBox: Restored to '{_content}'");
            }
            else
            {
                Console.WriteLine("RichTextBox: Nothing to undo");
            }
        }

        public void ShowUndoStackSize()
        {
            Console.WriteLine($"RichTextBox: Undo history has {_undoStack.Count} items");
        }
    }
}
