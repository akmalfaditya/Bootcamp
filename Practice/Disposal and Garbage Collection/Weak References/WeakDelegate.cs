using System;
using System.Collections.Generic;
using System.Reflection;

namespace WeakReferences
{
    // WeakDelegate allows event publishers to hold weak references to subscribers
    // This prevents memory leaks where event publishers keep subscribers alive
    public class WeakDelegate<TDelegate> where TDelegate : Delegate
    {
        // Inner class to hold the weak reference to the target and method info
        class MethodTarget
        {
            public readonly WeakReference? Reference; // Weak reference to instance target
            public readonly MethodInfo Method;        // Method to invoke

            public MethodTarget(Delegate d)
            {
                // For static methods, d.Target is null, so Reference remains null
                // For instance methods, we create a weak reference to the target
                if (d.Target != null) 
                    Reference = new WeakReference(d.Target);
                Method = d.Method;
            }
        }

        List<MethodTarget> _targets = new List<MethodTarget>();

        // Like Delegate.Combine - adds subscribers
        public void Combine(TDelegate target)
        {
            if (target == null) return;
            
            // Handle multicast delegates by iterating through invocation list
            foreach (Delegate d in (target as Delegate)!.GetInvocationList())
                _targets.Add(new MethodTarget(d));
        }

        // Like Delegate.Remove - removes subscribers
        public void Remove(TDelegate target)
        {
            if (target == null) return;
            
            foreach (Delegate d in (target as Delegate)!.GetInvocationList())
            {
                // Find the MethodTarget that matches the delegate's target and method
                MethodTarget? mt = _targets.Find(w =>
                    Equals(d.Target, w.Reference?.Target) &&
                    Equals(d.Method.MethodHandle, w.Method.MethodHandle));
                if (mt != null) _targets.Remove(mt);
            }
        }

        // Returns a multicast delegate of currently alive targets
        // This is where the magic happens - dead references are automatically cleaned up
        public TDelegate? Target
        {
            get
            {
                Delegate? combinedTarget = null;
                
                // Iterate over a copy to allow modification during iteration
                foreach (MethodTarget mt in _targets.ToArray())
                {
                    WeakReference? wr = mt.Reference;
                    
                    // If it's a static method target (wr == null) OR an alive instance target
                    if (wr == null || wr.Target != null)
                    {
                        // Re-create the delegate for alive targets
                        var newDelegate = Delegate.CreateDelegate(
                            typeof(TDelegate), wr?.Target, mt.Method);
                        combinedTarget = Delegate.Combine(combinedTarget, newDelegate);
                    }
                    else
                    {
                        // Target was collected, remove the dead reference
                        _targets.Remove(mt);
                    }
                }
                return combinedTarget as TDelegate;
            }
            set
            {
                _targets.Clear();
                if (value != null)
                    Combine(value);
            }
        }
    }
}
