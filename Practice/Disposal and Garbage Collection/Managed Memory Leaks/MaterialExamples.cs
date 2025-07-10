// Direct Examples from the Training Material
// These classes replicate the exact scenarios described in the material

using System;
using System.Linq;

namespace ManagedMemoryLeaks
{
    // EXACT REPLICA of the material's Host/Client example
    // This demonstrates the classic event handler memory leak
    
    // The event publisher (Host from material)
    public class Host
    {
        public event EventHandler? Click; // Declares an event
        
        public void TriggerClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
        
        public int SubscriberCount => Click?.GetInvocationList().Length ?? 0;
    }

    // The event subscriber (Client from material) - BAD VERSION
    public class Client
    {
        private Host _host;
        
        public Client(Host host)
        {
            _host = host;
            _host.Click += HostClicked; // Client subscribes to Host's Click event
        }
        
        void HostClicked(object? sender, EventArgs e) 
        { 
            /* ... event handling logic ... */ 
        }
        
        // NO DISPOSE METHOD - this is the problem!
        // The Host keeps this Client alive via the event subscription
    }

    // The event subscriber (Client from material) - GOOD VERSION
    public class ClientWithDisposal : IDisposable
    {
        private Host _host;
        private bool _disposed = false;
        
        public ClientWithDisposal(Host host)
        {
            _host = host;
            _host.Click += HostClicked; // Client subscribes to Host's Click event
        }
        
        void HostClicked(object? sender, EventArgs e) 
        { 
            if (!_disposed)
            {
                /* ... event handling logic ... */ 
            }
        }
        
        public void Dispose()
        {
            if (!_disposed)
            {
                // Crucial step: Unhook the event handler
                _host.Click -= HostClicked;
                _disposed = true;
            }
        }
    }

    // A test class to demonstrate the leak (from material)
    public class Test
    {
        private static Host _host = new Host(); // A static Host, acting as a root
        
        public static void CreateClients()
        {
            Client[] clients = Enumerable.Range(0, 1000)
                .Select(i => new Client(_host)) // Creates 1000 Client instances
                .ToArray();
            // At this point, the 'clients' array variable goes out of scope.
            // You might expect the 1000 Client objects to be eligible for collection.
            // But they're not! The static _host holds references to all of them.
        }
        
        public static void CreateProperClients()
        {
            ClientWithDisposal[] clients = Enumerable.Range(0, 1000)
                .Select(i => new ClientWithDisposal(_host))
                .ToArray();

            // ... do something with clients ...

            // When done, dispose of each client to unhook the event
            Array.ForEach(clients, c => c.Dispose());
        }
        
        public static Host GetHost() => _host;
    }

    // EXACT REPLICA of the material's Timer example
    public class Foo
    {
        private System.Timers.Timer _timer;
        
        public Foo()
        {
            _timer = new System.Timers.Timer { Interval = 1000 };
            _timer.Elapsed += tmr_Elapsed; // Foo subscribes to _timer.Elapsed
            _timer.Start();
        }
        
        void tmr_Elapsed(object? sender, System.Timers.ElapsedEventArgs e) 
        { 
            /* ... timer handling logic ... */ 
        }
        
        // NO DISPOSE METHOD - this is the problem!
        // The runtime keeps _timer alive, _timer keeps this Foo alive
    }

    // GOOD VERSION of Foo with proper disposal
    public class FooWithDisposal : IDisposable
    {
        private System.Timers.Timer _timer;
        private bool _disposed = false;
        
        public FooWithDisposal()
        {
            _timer = new System.Timers.Timer { Interval = 1000 };
            _timer.Elapsed += tmr_Elapsed; // Foo subscribes to _timer.Elapsed
            _timer.Start();
        }
        
        void tmr_Elapsed(object? sender, System.Timers.ElapsedEventArgs e) 
        { 
            if (!_disposed)
            {
                /* ... timer handling logic ... */ 
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _timer.Dispose(); // Crucial: Stops the timer and releases its runtime reference
                _disposed = true;
            }
        }
    }

    // Material's Threading Timer Examples
    public class ThreadingTimerExamples
    {
        // This replicates the exact Main() method from the material
        public static void DemonstrateThreadingTimerBehavior()
        {
            Console.WriteLine("Material Example: Threading Timer Behavior");
            Console.WriteLine("==========================================");
            
            // Exact code from material (might not tick if compiled in release mode)
            var tmr = new System.Threading.Timer(TimerTick, null, 1000, 1000);
            GC.Collect(); // Force a collection attempt
            System.Threading.Thread.Sleep(10000); // Wait 10 seconds
            
            // Timer might be collected before it ticks!
        }
        
        public static void DemonstrateProperThreadingTimer()
        {
            Console.WriteLine("Material Solution: Proper Threading Timer");
            Console.WriteLine("========================================");
            
            // Proper solution from material using 'using' statement
            using (var tmr = new System.Threading.Timer(TimerTick, null, 1000, 1000))
            {
                GC.Collect();
                System.Threading.Thread.Sleep(10000); // Wait 10 seconds
            } // tmr.Dispose() is called implicitly here
        }
        
        static void TimerTick(object? notUsed) 
        { 
            Console.WriteLine("tick"); 
        }
    }

    // Demonstration class for the exact material examples
    public static class MaterialExampleRunner
    {
        public static void RunHostClientExample()
        {
            Console.WriteLine("MATERIAL EXAMPLE: Host/Client Event Handler Leak");
            Console.WriteLine("================================================");
            
            var host = Test.GetHost();
            Console.WriteLine($"Host subscribers before: {host.SubscriberCount}");
            
            // Create clients - they will leak!
            Test.CreateClients();
            
            Console.WriteLine($"Host subscribers after creating clients: {host.SubscriberCount}");
            
            // Force GC - clients won't be collected
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            Console.WriteLine($"Host subscribers after GC: {host.SubscriberCount}");
            Console.WriteLine("Clients are still alive because Host holds references!");
            
            // Now demonstrate proper cleanup
            Console.WriteLine("\nCreating properly disposable clients...");
            Test.CreateProperClients();
            
            Console.WriteLine($"Host subscribers after proper cleanup: {host.SubscriberCount}");
            Console.WriteLine();
        }
        
        public static void RunTimerExample()
        {
            Console.WriteLine("MATERIAL EXAMPLE: Timer Memory Leak");
            Console.WriteLine("===================================");
            
            // Create Foo instances - they will leak!
            Console.WriteLine("Creating Foo instances with timers...");
            for (int i = 0; i < 10; i++)
            {
                var foo = new Foo();
                // foo goes out of scope but timer keeps it alive
            }
            
            Console.WriteLine("Foo instances went out of scope, but they're still alive!");
            Console.WriteLine("The runtime holds their timers, timers hold the Foo instances");
            
            // Force GC
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            Console.WriteLine("Even after GC, Foo instances are still alive and timers are still running");
            
            // Now demonstrate proper cleanup
            Console.WriteLine("\nCreating properly disposable Foo instances...");
            var properFoos = new List<FooWithDisposal>();
            for (int i = 0; i < 10; i++)
            {
                properFoos.Add(new FooWithDisposal());
            }
            
            Console.WriteLine("Disposing Foo instances properly...");
            foreach (var foo in properFoos)
            {
                foo.Dispose();
            }
            properFoos.Clear();
            
            Console.WriteLine("Proper cleanup completed - timers stopped and disposed");
            Console.WriteLine();
        }
    }
}
