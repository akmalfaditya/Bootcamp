using System;

namespace DisposalPatternDemo
{
    /// <summary>
    /// This is the magic helper class that enables the anonymous disposal pattern.
    /// It lets you create IDisposable objects on the fly without defining full classes.
    /// </summary>
    public class Disposable : IDisposable
    {
        private Action? _onDispose;

        /// <summary>
        /// Creates a new Disposable that will execute the given action when disposed.
        /// </summary>
        public static Disposable Create(Action onDispose)
        {
            return new Disposable(onDispose);
        }

        private Disposable(Action onDispose)
        {
            _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
        }

        /// <summary>
        /// Executes the action and then clears it to prevent multiple executions.
        /// </summary>
        public void Dispose()
        {
            _onDispose?.Invoke();
            _onDispose = null; // Prevent multiple executions
        }
    }

    /// <summary>
    /// This class demonstrates the anonymous disposal pattern in action.
    /// It's a service that can be suspended and resumed, using IDisposable
    /// to guarantee the resume operation happens.
    /// </summary>
    public class SuspendableService
    {
        private int _suspendCount = 0;
        
        public string State => _suspendCount > 0 ? "Suspended" : "Running";

        public SuspendableService()
        {
            Console.WriteLine("🚀 SuspendableService: Service started");
        }

        /// <summary>
        /// This is the magic method that demonstrates anonymous disposal.
        /// It returns an IDisposable that will automatically resume operations
        /// when the using block is exited.
        /// </summary>
        public IDisposable SuspendOperations()
        {
            // Increment the suspend count
            _suspendCount++;
            Console.WriteLine($"⏸ SuspendableService: Operations suspended (count: {_suspendCount})");
            
            // Return a disposable that will decrement the count when disposed
            // This is the anonymous disposal pattern in action!
            return Disposable.Create(() => {
                _suspendCount--;
                Console.WriteLine($"▶ SuspendableService: Operations resumed (count: {_suspendCount})");
            });
        }

        /// <summary>
        /// Traditional approach (for comparison) - error-prone because you might forget to call Resume
        /// </summary>
        public void Suspend()
        {
            _suspendCount++;
            Console.WriteLine($"⏸ SuspendableService: Operations suspended (manual)");
        }

        /// <summary>
        /// Traditional approach (for comparison) - you have to remember to call this
        /// </summary>
        public void Resume()
        {
            if (_suspendCount > 0)
            {
                _suspendCount--;
                Console.WriteLine($"▶ SuspendableService: Operations resumed (manual)");
            }
        }
    }
}
