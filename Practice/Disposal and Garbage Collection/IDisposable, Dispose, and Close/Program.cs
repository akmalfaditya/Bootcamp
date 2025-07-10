using System;
using System.IO;

namespace DisposalPatternDemo
{
    /// <summary>
    /// Welcome to our hands-on training session on IDisposable, Dispose, and Close!
    /// 
    /// Today we'll explore how .NET handles resource cleanup through the IDisposable interface.
    /// Think of IDisposable as a contract that says "Hey, I'm holding onto something valuable
    /// (like a file handle or database connection) and I need you to tell me when you're done
    /// so I can clean up properly."
    /// 
    /// Remember: Dispose() is NOT about releasing managed memory - the GC handles that.
    /// It's about releasing UNMANAGED resources like file handles, network connections, etc.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== IDisposable Master Class: From Basics to Advanced Patterns ===\n");

            // Let's start with the fundamentals and work our way up
            Console.WriteLine("📚 LESSON 1: The 'using' Statement - Your Safety Net");
            DemonstrateUsingStatement();
            Console.WriteLine();

            Console.WriteLine("📚 LESSON 2: Standard Disposal Semantics - The Three Golden Rules");
            DemonstrateDisposalSemantics();
            Console.WriteLine();

            Console.WriteLine("📚 LESSON 3: Close() vs Dispose() - Know the Difference");
            DemonstrateCloseVsDispose();
            Console.WriteLine();

            Console.WriteLine("📚 LESSON 4: Chained Disposal - When Objects Own Other Objects");
            DemonstrateChainedDisposal();
            Console.WriteLine();

            Console.WriteLine("📚 LESSON 5: When NOT to Dispose - Breaking the Rules Safely");
            DemonstrateWhenNotToDispose();
            Console.WriteLine();

            Console.WriteLine("📚 LESSON 6: Clearing Fields in Dispose - Beyond Just Unmanaged Resources");
            DemonstrateFieldClearingInDispose();
            Console.WriteLine();

            Console.WriteLine("📚 LESSON 7: Anonymous Disposal Pattern - IDisposable on the Fly");
            DemonstrateAnonymousDisposal();
            Console.WriteLine();

            Console.WriteLine("🎯 Training Complete! You now know the ins and outs of proper resource management.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// LESSON 1: The 'using' statement is your best friend for disposal.
        /// It's syntactic sugar that the compiler transforms into a try/finally block,
        /// guaranteeing that Dispose() gets called even if an exception occurs.
        /// </summary>
        static void DemonstrateUsingStatement()
        {
            Console.WriteLine("Let's see the 'using' statement in action...\n");

            // First, let's create a temporary file for our demonstration
            string tempFile1 = Path.GetTempFileName();
            string tempFile2 = Path.GetTempFileName();
            File.WriteAllText(tempFile1, "Hello from IDisposable training!");
            File.WriteAllText(tempFile2, "Hello from IDisposable training!");

            // The classic 'using' statement - notice the curly braces
            Console.WriteLine("🔹 Classic 'using' statement:");
            using (var fileManager = new FileManager(tempFile1))
            {
                fileManager.ReadContent();
                Console.WriteLine("Inside the using block - file is open and ready");
            } // <-- Right here, Dispose() is automatically called!
            Console.WriteLine("✅ FileManager disposed automatically when leaving the using block\n");

            // The modern 'using' declaration - cleaner syntax for simple cases
            Console.WriteLine("🔹 Modern 'using' declaration (C# 8.0+):");
            {
                using var fileManager2 = new FileManager(tempFile2);
                fileManager2.ReadContent();
                Console.WriteLine("Using declaration - no curly braces needed");
            } // fileManager2.Dispose() is called here due to the scope block
            Console.WriteLine("✅ FileManager2 disposed when leaving scope block\n");

            // Clean up our temp files
            File.Delete(tempFile1);
            File.Delete(tempFile2);

            // Pro tip: The compiler transforms this...
            Console.WriteLine("💡 Pro Tip: The compiler transforms the using statement into:");
            Console.WriteLine("   FileManager fm = new FileManager(file);");
            Console.WriteLine("   try {");
            Console.WriteLine("       // your code here");
            Console.WriteLine("   } finally {");
            Console.WriteLine("       if (fm != null) fm.Dispose();");
            Console.WriteLine("   }");
            Console.WriteLine("   This guarantees cleanup even if exceptions occur!");
        }

        /// <summary>
        /// LESSON 2: The three golden rules of disposal that make the .NET world go round.
        /// Follow these and your objects will play nicely with everyone else's.
        /// </summary>
        static void DemonstrateDisposalSemantics()
        {
            Console.WriteLine("Time to learn the three golden rules of disposal:\n");

            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "Disposal semantics demo");

            var fileManager = new FileManager(tempFile);

            Console.WriteLine("🔹 RULE 1: Irreversible Disposal");
            Console.WriteLine("Once disposed, an object is 'dead' - no resurrection allowed!");
            fileManager.ReadContent(); // This works
            fileManager.Dispose();     // Object is now disposed

            try
            {
                fileManager.ReadContent(); // This should throw
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"✅ Caught expected exception: {ex.Message}");
            }

            Console.WriteLine("\n🔹 RULE 2: Idempotent Disposal");
            Console.WriteLine("Calling Dispose() multiple times should be safe - no errors!");
            fileManager.Dispose(); // First call
            fileManager.Dispose(); // Second call - should be safe
            fileManager.Dispose(); // Third call - still safe
            Console.WriteLine("✅ Called Dispose() three times - no problems!");

            Console.WriteLine("\n🔹 RULE 3: Ownership and Chained Disposal");
            Console.WriteLine("If object X owns object Y, then X.Dispose() should call Y.Dispose()");
            Console.WriteLine("We'll see this in action with our CompositeFileManager next!");

            File.Delete(tempFile);
        }

        /// <summary>
        /// LESSON 3: Understanding the difference between Close() and Dispose().
        /// This trips up a lot of developers, so pay attention!
        /// </summary>
        static void DemonstrateCloseVsDispose()
        {
            Console.WriteLine("Close() vs Dispose() - this is where things get interesting...\n");

            var dbConnection = new DatabaseConnection("Server=localhost;Database=TestDB");

            Console.WriteLine("🔹 Close() - Usually means 'pause' or 'hibernate'");
            dbConnection.Open();
            Console.WriteLine($"Connection state: {dbConnection.State}");

            dbConnection.Close(); // Close the connection
            Console.WriteLine($"After Close(): {dbConnection.State}");

            dbConnection.Open(); // We can reopen it!
            Console.WriteLine($"After reopening: {dbConnection.State}");
            Console.WriteLine("✅ Close() allows the object to be reused\n");

            Console.WriteLine("🔹 Dispose() - Means 'permanent shutdown'");
            dbConnection.Dispose(); // Now it's permanently disposed
            Console.WriteLine($"After Dispose(): {dbConnection.State}");

            try
            {
                dbConnection.Open(); // This should fail
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"✅ Cannot reopen after Dispose(): {ex.Message}");
            }

            Console.WriteLine("\n💡 Key Insight:");
            Console.WriteLine("- Close() = Temporary shutdown, can be reopened");
            Console.WriteLine("- Dispose() = Permanent shutdown, object is dead");
            Console.WriteLine("- Some classes make Close() identical to Dispose()");
            Console.WriteLine("- Always check the documentation for the specific class!");
        }

        /// <summary>
        /// LESSON 4: When an object "owns" other disposable objects, it should dispose them too.
        /// This is the third golden rule in action.
        /// </summary>
        static void DemonstrateChainedDisposal()
        {
            Console.WriteLine("Chained disposal - when objects own other objects...\n");

            Console.WriteLine("🔹 Creating a CompositeFileManager that owns two FileManagers:");
            using (var composite = new CompositeFileManager("log1.txt", "log2.txt"))
            {
                composite.WriteToLogs("This is a test log entry");
                Console.WriteLine("CompositeFileManager created and used");
            } // When this disposes, it should dispose both inner FileManagers
            Console.WriteLine("✅ CompositeFileManager disposed - check if inner objects were disposed too!");

            Console.WriteLine("\n💡 This is like a Russian nesting doll:");
            Console.WriteLine("- Outer object disposes");
            Console.WriteLine("- Which disposes inner objects");
            Console.WriteLine("- Which dispose their inner objects");
            Console.WriteLine("- And so on...");
        }

        /// <summary>
        /// LESSON 5: There are times when you should NOT call Dispose().
        /// This goes against the "when in doubt, dispose" rule, but these exceptions matter.
        /// </summary>
        static void DemonstrateWhenNotToDispose()
        {
            Console.WriteLine("When NOT to dispose - the exceptions to the rule...\n");

            Console.WriteLine("🔹 SCENARIO 1: You don't 'own' the object");
            Console.WriteLine("Some objects are shared across the application:");
            Console.WriteLine("Example: System.Drawing.Brushes.Blue is a shared, static resource");
            Console.WriteLine("⚠️  NEVER dispose shared resources like static brushes!");
            Console.WriteLine("✅ Rule: If you got it from a static property, don't dispose it\n");

            Console.WriteLine("🔹 SCENARIO 2: Dispose() does something you want to avoid");
            Console.WriteLine("StreamReader disposes its underlying stream by default:");
            
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            writer.WriteLine("Hello, StreamReader!");
            writer.Flush();
            memoryStream.Position = 0;

            // Use leaveOpen parameter to prevent disposing the stream
            using (var reader = new StreamReader(memoryStream, leaveOpen: true))
            {
                string? content = reader.ReadLine();
                Console.WriteLine($"Read: {content}");
            } // StreamReader disposes, but leaves memoryStream open

            Console.WriteLine($"Stream still usable: {memoryStream.CanRead}");
            memoryStream.Dispose(); // Now we dispose it ourselves
            Console.WriteLine("✅ Used leaveOpen parameter to control disposal\n");

            Console.WriteLine("🔹 SCENARIO 3: The object doesn't really need disposal");
            Console.WriteLine("StringReader/StringWriter don't hold unmanaged resources:");
            var stringReader = new StringReader("Just a string");
            var text = stringReader.ReadToEnd();
            Console.WriteLine($"Read: {text}");
            Console.WriteLine("Could dispose it, but it's not critical - no unmanaged resources");
            stringReader.Dispose(); // We'll dispose it anyway for completeness
            Console.WriteLine("✅ Disposed StringReader - good practice even if not critical");
        }

        /// <summary>
        /// LESSON 6: Dispose() isn't just about unmanaged resources.
        /// Good disposal hygiene includes event unsubscription, flag setting, and data clearing.
        /// </summary>
        static void DemonstrateFieldClearingInDispose()
        {
            Console.WriteLine("Field clearing in Dispose() - the complete cleanup...\n");

            Console.WriteLine("🔹 Event Unsubscription - Preventing Memory Leaks:");
            var eventPublisher = new EventPublisher();
            using (var eventSubscriber = new EventSubscriber(eventPublisher))
            {
                eventPublisher.RaiseTestEvent();
                Console.WriteLine("Subscriber received the event");
            } // Subscriber disposes and unsubscribes
            
            Console.WriteLine("Subscriber disposed - raising event again...");
            eventPublisher.RaiseTestEvent();
            Console.WriteLine("✅ No output from subscriber - it unsubscribed properly\n");

            Console.WriteLine("🔹 Clearing Sensitive Data:");
            using (var secureCache = new SecureCache())
            {
                secureCache.StoreSecret("TopSecretPassword123");
                Console.WriteLine("Secret stored in memory");
            } // Dispose clears the sensitive data
            Console.WriteLine("✅ Sensitive data cleared from memory\n");

            Console.WriteLine("💡 Best Practices for Dispose():");
            Console.WriteLine("1. Unsubscribe from events (prevents memory leaks)");
            Console.WriteLine("2. Set IsDisposed flag (enforces disposal rules)");
            Console.WriteLine("3. Clear sensitive data (security measure)");
            Console.WriteLine("4. Set event handlers to null (prevents unexpected calls)");
            Console.WriteLine("5. Remember: Dispose() is about unmanaged resources first!");
        }

        /// <summary>
        /// LESSON 7: The anonymous disposal pattern - creating IDisposable objects on the fly.
        /// This is incredibly useful for temporary state changes that need guaranteed cleanup.
        /// </summary>
        static void DemonstrateAnonymousDisposal()
        {
            Console.WriteLine("Anonymous disposal pattern - IDisposable on demand...\n");

            var suspendableService = new SuspendableService();
            Console.WriteLine($"Service state: {suspendableService.State}");

            Console.WriteLine("🔹 The old way (error-prone):");
            Console.WriteLine("service.Suspend();");
            Console.WriteLine("try {");
            Console.WriteLine("    // do work");
            Console.WriteLine("} finally {");
            Console.WriteLine("    service.Resume(); // Easy to forget!"); 
            Console.WriteLine("}\n");

            Console.WriteLine("🔹 The new way (bulletproof):");
            Console.WriteLine("Suspending operations with automatic resume...");
            using (suspendableService.SuspendOperations())
            {
                Console.WriteLine($"Inside suspension: {suspendableService.State}");
                Console.WriteLine("Doing some work while suspended...");
                // Even if an exception occurs here, Resume() will be called
            } // Automatically resumes here via Dispose()
            
            Console.WriteLine($"After suspension: {suspendableService.State}");
            Console.WriteLine("✅ Operations resumed automatically!\n");

            Console.WriteLine("💡 The Magic Behind Anonymous Disposal:");
            Console.WriteLine("- Create a helper class that implements IDisposable");
            Console.WriteLine("- Constructor does the 'setup' (like Suspend)");
            Console.WriteLine("- Dispose() does the 'cleanup' (like Resume)");
            Console.WriteLine("- Return it from a method for use in 'using' statements");
            Console.WriteLine("- Bulletproof resource management with minimal code!");
        }
    }
}
