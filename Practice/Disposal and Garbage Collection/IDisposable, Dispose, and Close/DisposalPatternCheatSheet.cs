using System;

namespace DisposalPatternDemo
{
    /// <summary>
    /// Quick reference guide showing disposal pattern examples in a condensed format.
    /// This serves as a cheat sheet for developers implementing IDisposable.
    /// </summary>
    public static class DisposalPatternCheatSheet
    {
        /// <summary>
        /// Prints a quick reference guide to the console
        /// </summary>
        public static void PrintQuickReference()
        {
            Console.WriteLine("=== IDisposable Pattern Quick Reference ===\n");

            Console.WriteLine("1. BASIC PATTERN:");
            Console.WriteLine("   public class MyClass : IDisposable");
            Console.WriteLine("   {");
            Console.WriteLine("       private bool _disposed = false;");
            Console.WriteLine("       private SomeResource _resource;");
            Console.WriteLine("");
            Console.WriteLine("       public void Dispose()");
            Console.WriteLine("       {");
            Console.WriteLine("           Dispose(disposing: true);");
            Console.WriteLine("           GC.SuppressFinalize(this);");
            Console.WriteLine("       }");
            Console.WriteLine("");
            Console.WriteLine("       protected virtual void Dispose(bool disposing)");
            Console.WriteLine("       {");
            Console.WriteLine("           if (!_disposed)");
            Console.WriteLine("           {");
            Console.WriteLine("               if (disposing)");
            Console.WriteLine("               {");
            Console.WriteLine("                   _resource?.Dispose(); // Dispose managed resources");
            Console.WriteLine("               }");
            Console.WriteLine("               // Free unmanaged resources here");
            Console.WriteLine("               _disposed = true;");
            Console.WriteLine("           }");
            Console.WriteLine("       }");
            Console.WriteLine("   }\n");

            Console.WriteLine("2. USAGE PATTERNS:");
            Console.WriteLine("   // Manual disposal");
            Console.WriteLine("   var obj = new MyClass();");
            Console.WriteLine("   try { /* use obj */ }");
            Console.WriteLine("   finally { obj.Dispose(); }");
            Console.WriteLine("");
            Console.WriteLine("   // Automatic disposal (PREFERRED)");
            Console.WriteLine("   using (var obj = new MyClass())");
            Console.WriteLine("   {");
            Console.WriteLine("       // use obj - Dispose() called automatically");
            Console.WriteLine("   }\n");

            Console.WriteLine("3. ASYNC DISPOSAL (.NET Core 3.0+):");
            Console.WriteLine("   public class AsyncClass : IAsyncDisposable");
            Console.WriteLine("   {");
            Console.WriteLine("       public async ValueTask DisposeAsync()");
            Console.WriteLine("       {");
            Console.WriteLine("           await SomeAsyncCleanup();");
            Console.WriteLine("           GC.SuppressFinalize(this);");
            Console.WriteLine("       }");
            Console.WriteLine("   }");
            Console.WriteLine("");
            Console.WriteLine("   // Usage:");
            Console.WriteLine("   await using (var obj = new AsyncClass())");
            Console.WriteLine("   {");
            Console.WriteLine("       // use obj - DisposeAsync() called automatically");
            Console.WriteLine("   }\n");

            Console.WriteLine("4. BEST PRACTICES:");
            Console.WriteLine("   ✅ DO: Always use 'using' statements when possible");
            Console.WriteLine("   ✅ DO: Unsubscribe from events in Dispose()");
            Console.WriteLine("   ✅ DO: Dispose nested disposable objects you own");
            Console.WriteLine("   ✅ DO: Check disposed state before operations");
            Console.WriteLine("   ✅ DO: Make Dispose() safe to call multiple times");
            Console.WriteLine("");
            Console.WriteLine("   ❌ DON'T: Forget to call Dispose() on disposable objects");
            Console.WriteLine("   ❌ DON'T: Use disposed objects");
            Console.WriteLine("   ❌ DON'T: Dispose objects you don't own");
            Console.WriteLine("   ❌ DON'T: Throw exceptions from Dispose() methods");
            Console.WriteLine("   ❌ DON'T: Access managed objects in finalizers\n");

            Console.WriteLine("5. COMMON SCENARIOS:");
            Console.WriteLine("   • File/Stream operations → Always dispose");
            Console.WriteLine("   • Database connections → Always dispose");
            Console.WriteLine("   • Network resources → Always dispose");
            Console.WriteLine("   • Graphics resources → Always dispose");
            Console.WriteLine("   • Event subscriptions → Unsubscribe in Dispose()");
            Console.WriteLine("   • Timers → Dispose to stop them");
            Console.WriteLine("   • Sensitive data → Clear in Dispose()\n");

            Console.WriteLine("6. DISPOSAL STATE CHECKING:");
            Console.WriteLine("   private void ThrowIfDisposed()");
            Console.WriteLine("   {");
            Console.WriteLine("       if (_disposed)");
            Console.WriteLine("           throw new ObjectDisposedException(GetType().Name);");
            Console.WriteLine("   }\n");

            Console.WriteLine("Remember: Resource management is not optional in production code!");
            Console.WriteLine("When in doubt, implement IDisposable and use 'using' statements.\n");
        }
    }
}
