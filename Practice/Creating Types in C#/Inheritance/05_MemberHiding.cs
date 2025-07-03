using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates member hiding in C# inheritance using the 'new' keyword.
    /// Member hiding is different from overriding - it doesn't involve polymorphism.
    /// Instead, it creates a new member in the derived class that "shadows" the base class member.
    /// 
    /// Key concepts:
    /// 1. Accidental member hiding (compiler warning)
    /// 2. Intentional member hiding with 'new' keyword
    /// 3. Difference between 'new' (hiding) and 'override' (polymorphism)
    /// 4. How hiding affects method resolution based on compile-time type
    /// </summary>

    // Base class for demonstrating member hiding
    public class MediaPlayer
    {
        public string Brand { get; set; } = "Generic";
        public string Model { get; set; } = "Basic";
        
        // Virtual method that can be overridden
        public virtual void Play()
        {
            Console.WriteLine($"{Brand} {Model}: Playing media with basic player...");
        }
        
        // Virtual method for stopping
        public virtual void Stop()
        {
            Console.WriteLine($"{Brand} {Model}: Stopping playback...");
        }
        
        // Non-virtual method - this will be hidden, not overridden
        public void ShowInfo()
        {
            Console.WriteLine($"Basic Media Player - {Brand} {Model}");
        }
        
        // Virtual property
        public virtual string DeviceType => "Generic Media Player";
        
        // Non-virtual field that will be hidden
        public int Volume = 50;
        
        // Virtual method that will be both overridden and hidden in different classes
        public virtual void DisplayControls()
        {
            Console.WriteLine("Basic controls: Play, Stop, Volume");
        }
    }

    // First derived class - demonstrates OVERRIDING (polymorphic behavior)
    public class DVDPlayer : MediaPlayer
    {
        public bool HasDolbySound { get; set; } = true;
        
        // OVERRIDE the virtual Play method - this is polymorphic
        public override void Play()
        {
            Console.WriteLine($"{Brand} {Model} DVD Player: Playing DVD with surround sound!");
        }
        
        // OVERRIDE the virtual Stop method
        public override void Stop()
        {
            Console.WriteLine($"{Brand} {Model} DVD Player: Ejecting DVD and stopping...");
        }
        
        // HIDE the non-virtual ShowInfo method using 'new'
        // This creates a new method, doesn't override the base one
        public new void ShowInfo()
        {
            Console.WriteLine($"DVD Player - {Brand} {Model} with Dolby: {HasDolbySound}");
        }
        
        // OVERRIDE the virtual property
        public override string DeviceType => "DVD Player";
        
        // HIDE the Volume field with a new one
        public new int Volume = 75;
        
        // OVERRIDE the virtual DisplayControls method
        public override void DisplayControls()
        {
            Console.WriteLine("DVD Controls: Play, Stop, Volume, Fast Forward, Rewind, Eject");
        }
        
        // DVD-specific method
        public void Eject()
        {
            Console.WriteLine($"{Brand} {Model}: Ejecting DVD...");
        }
    }

    // Second derived class - demonstrates HIDING (non-polymorphic behavior)
    public class MP3Player : MediaPlayer
    {
        public int StorageGB { get; set; } = 32;
        
        // HIDE the virtual Play method using 'new' - this is NOT polymorphic
        public new void Play()
        {
            Console.WriteLine($"{Brand} {Model} MP3 Player: Playing digital music from {StorageGB}GB storage!");
        }
        
        // OVERRIDE the virtual Stop method - this IS polymorphic
        public override void Stop()
        {
            Console.WriteLine($"{Brand} {Model} MP3 Player: Stopping digital playback...");
        }
        
        // HIDE the non-virtual ShowInfo method
        public new void ShowInfo()
        {
            Console.WriteLine($"MP3 Player - {Brand} {Model} with {StorageGB}GB storage");
        }
        
        // OVERRIDE the virtual property
        public override string DeviceType => "MP3 Player";
        
        // HIDE the Volume field
        public new int Volume = 60;
        
        // HIDE the virtual DisplayControls method with 'new'
        public new void DisplayControls()
        {
            Console.WriteLine("MP3 Controls: Play, Stop, Volume, Next, Previous, Shuffle");
        }
        
        // MP3-specific methods
        public void NextTrack()
        {
            Console.WriteLine($"{Brand} {Model}: Skipping to next track...");
        }
        
        public void Shuffle()
        {
            Console.WriteLine($"{Brand} {Model}: Enabling shuffle mode...");
        }
    }

    // Third derived class - mix of overriding and hiding
    public class StreamingPlayer : MediaPlayer
    {
        public string StreamingService { get; set; } = "Generic";
        public bool IsConnected { get; set; } = true;
        
        // OVERRIDE the virtual Play method
        public override void Play()
        {
            if (IsConnected)
            {
                Console.WriteLine($"{Brand} {Model} Streaming Player: Streaming from {StreamingService}!");
            }
            else
            {
                Console.WriteLine($"{Brand} {Model} Streaming Player: No internet connection!");
            }
        }
        
        // HIDE the virtual Stop method with 'new'
        public new void Stop()
        {
            Console.WriteLine($"{Brand} {Model} Streaming Player: Pausing stream (keeping connection)...");
        }
        
        // HIDE the ShowInfo method
        public new void ShowInfo()
        {
            Console.WriteLine($"Streaming Player - {Brand} {Model} connected to {StreamingService}");
        }
        
        // OVERRIDE the property
        public override string DeviceType => "Streaming Player";
        
        // HIDE the DisplayControls with new version
        public new void DisplayControls()
        {
            Console.WriteLine("Streaming Controls: Play, Pause, Volume, Search, Playlist");
        }
        
        public void ChangeService(string newService)
        {
            StreamingService = newService;
            Console.WriteLine($"Switched to {StreamingService}");
        }
    }

    /// <summary>
    /// Demonstration class showing the differences between hiding and overriding
    /// </summary>
    public static class MemberHidingDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== MEMBER HIDING vs OVERRIDING DEMONSTRATION ===\n");
            
            // Create instances of all player types
            var dvdPlayer = new DVDPlayer
            {
                Brand = "Sony",
                Model = "DVD-X1000",
                HasDolbySound = true
            };
            
            var mp3Player = new MP3Player
            {
                Brand = "Apple",
                Model = "iPod Classic",
                StorageGB = 160
            };
            
            var streamingPlayer = new StreamingPlayer
            {
                Brand = "Roku",
                Model = "Ultra 4K",
                StreamingService = "Netflix",
                IsConnected = true
            };
            
            // 1. DEMONSTRATE OVERRIDING vs HIDING WITH DIRECT REFERENCES
            Console.WriteLine("1. DIRECT OBJECT REFERENCES:");
            Console.WriteLine("Calling methods directly on derived class instances\n");
            
            Console.WriteLine("--- DVD Player (uses override for Play) ---");
            dvdPlayer.Play();      // Calls DVDPlayer.Play() (overridden)
            dvdPlayer.ShowInfo();  // Calls DVDPlayer.ShowInfo() (hidden with new)
            dvdPlayer.DisplayControls(); // Calls DVDPlayer.DisplayControls() (overridden)
            Console.WriteLine($"Volume: {dvdPlayer.Volume}"); // Shows DVDPlayer.Volume (hidden)
            
            Console.WriteLine("\n--- MP3 Player (uses new/hiding for Play) ---");
            mp3Player.Play();      // Calls MP3Player.Play() (hidden with new)
            mp3Player.ShowInfo();  // Calls MP3Player.ShowInfo() (hidden with new)
            mp3Player.DisplayControls(); // Calls MP3Player.DisplayControls() (hidden with new)
            Console.WriteLine($"Volume: {mp3Player.Volume}"); // Shows MP3Player.Volume (hidden)
            
            Console.WriteLine("\n--- Streaming Player (mix of override and hiding) ---");
            streamingPlayer.Play(); // Calls StreamingPlayer.Play() (overridden)
            streamingPlayer.Stop(); // Calls StreamingPlayer.Stop() (hidden with new)
            streamingPlayer.ShowInfo(); // Calls StreamingPlayer.ShowInfo() (hidden with new)
            
            // 2. DEMONSTRATE THE KEY DIFFERENCE - BASE CLASS REFERENCES
            Console.WriteLine("\n2. BASE CLASS REFERENCES:");
            Console.WriteLine("This shows the crucial difference between override and new\n");
            
            // Create base class references to derived objects
            MediaPlayer player1 = dvdPlayer;      // MediaPlayer reference to DVDPlayer
            MediaPlayer player2 = mp3Player;      // MediaPlayer reference to MP3Player  
            MediaPlayer player3 = streamingPlayer; // MediaPlayer reference to StreamingPlayer
            
            Console.WriteLine("--- Through MediaPlayer references ---");
            Console.WriteLine("OVERRIDDEN methods call the derived implementation:");
            Console.WriteLine("HIDDEN methods call the base implementation!");
            
            Console.WriteLine("\nDVD Player through base reference:");
            player1.Play();           // Calls DVDPlayer.Play() - OVERRIDE works polymorphically
            player1.ShowInfo();       // Calls MediaPlayer.ShowInfo() - HIDING doesn't work polymorphically!
            player1.DisplayControls(); // Calls DVDPlayer.DisplayControls() - OVERRIDE works
            Console.WriteLine($"Volume: {player1.Volume}"); // Shows MediaPlayer.Volume - base field
            
            Console.WriteLine("\nMP3 Player through base reference:");
            player2.Play();           // Calls MediaPlayer.Play() - HIDING doesn't work polymorphically!
            player2.Stop();           // Calls MP3Player.Stop() - OVERRIDE works polymorphically
            player2.ShowInfo();       // Calls MediaPlayer.ShowInfo() - HIDING doesn't work
            player2.DisplayControls(); // Calls MediaPlayer.DisplayControls() - HIDING doesn't work
            Console.WriteLine($"Volume: {player2.Volume}"); // Shows MediaPlayer.Volume
            
            Console.WriteLine("\nStreaming Player through base reference:");
            player3.Play();           // Calls StreamingPlayer.Play() - OVERRIDE works
            player3.Stop();           // Calls MediaPlayer.Stop() - HIDING doesn't work!
            player3.ShowInfo();       // Calls MediaPlayer.ShowInfo() - HIDING doesn't work
            player3.DisplayControls(); // Calls MediaPlayer.DisplayControls() - HIDING doesn't work
            
            // 3. DEMONSTRATE POLYMORPHIC ARRAYS
            Console.WriteLine("\n3. POLYMORPHIC BEHAVIOR IN COLLECTIONS:");
            Console.WriteLine("Override methods work polymorphically, hidden methods don't\n");
            
            MediaPlayer[] players = { dvdPlayer, mp3Player, streamingPlayer };
            
            Console.WriteLine("Calling Play() on all players through base references:");
            foreach (MediaPlayer player in players)
            {
                Console.Write($"{player.DeviceType}: ");
                player.Play(); // Only overridden Play() methods work polymorphically
            }
            
            Console.WriteLine("\nCalling Stop() on all players:");
            foreach (MediaPlayer player in players)
            {
                Console.Write($"{player.DeviceType}: ");
                player.Stop(); // Only overridden Stop() methods work polymorphically
            }
            
            Console.WriteLine("\nCalling ShowInfo() on all players (hidden method):");
            foreach (MediaPlayer player in players)
            {
                Console.Write($"{player.DeviceType}: ");
                player.ShowInfo(); // Hidden method - always calls base implementation
            }
            
            // 4. DEMONSTRATE EXPLICIT CASTING TO ACCESS HIDDEN MEMBERS
            Console.WriteLine("\n4. ACCESSING HIDDEN MEMBERS THROUGH CASTING:");
            Console.WriteLine("You can cast to access the hidden implementations\n");
            
            Console.WriteLine("Accessing MP3Player's hidden Play() method:");
            if (player2 is MP3Player mp3)
            {
                mp3.Play(); // This calls the hidden MP3Player.Play() method
            }
            
            Console.WriteLine("\nCompare with base reference call:");
            player2.Play(); // This calls MediaPlayer.Play() because Play() is hidden, not overridden
            
            // 5. SUMMARY COMPARISON
            Console.WriteLine("\n5. SUMMARY - OVERRIDE vs NEW:");
            ShowMethodResolutionSummary();
        }
        
        private static void ShowMethodResolutionSummary()
        {
            Console.WriteLine("\n=== METHOD RESOLUTION SUMMARY ===");
            Console.WriteLine("OVERRIDE (virtual/override):");
            Console.WriteLine("  - Polymorphic behavior");
            Console.WriteLine("  - Method called depends on RUNTIME type of object");
            Console.WriteLine("  - Works through base class references");
            Console.WriteLine("  - Used for true polymorphism");
            
            Console.WriteLine("\nHIDING (new keyword):");
            Console.WriteLine("  - Non-polymorphic behavior");
            Console.WriteLine("  - Method called depends on COMPILE-TIME type of reference");
            Console.WriteLine("  - Hidden methods not accessible through base references");
            Console.WriteLine("  - Used when you want to replace, not extend, base functionality");
            
            Console.WriteLine("\nBest Practice:");
            Console.WriteLine("  - Use OVERRIDE when you want polymorphic behavior");
            Console.WriteLine("  - Use NEW only when you intentionally want to hide base functionality");
            Console.WriteLine("  - Prefer composition over hiding when possible");
        }
    }
}
