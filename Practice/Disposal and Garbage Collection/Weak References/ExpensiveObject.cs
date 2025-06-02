// ExpensiveObject.cs
// This class represents objects that are expensive to create and maintain
// We'll use it to demonstrate weak reference behavior with meaningful objects

using System.Text;

namespace WeakReferences
{
    // This class simulates an expensive object that you might want to cache
    // but also allow to be garbage collected when memory is needed
    public class ExpensiveObject
    {
        public string Data { get; private set; }
        private byte[] _expensiveData;
        private static int _instanceCount = 0;
        private int _instanceId;

        public ExpensiveObject(string data)
        {
            _instanceId = ++_instanceCount;
            Data = data;
            
            // Simulate expensive initialization by allocating significant memory
            _expensiveData = new byte[50000]; // 50KB per object
            
            // Fill with some pattern to make it "real" data
            for (int i = 0; i < _expensiveData.Length; i++)
            {
                _expensiveData[i] = (byte)(i % 256);
            }

            Console.WriteLine($"  ExpensiveObject #{_instanceId} created: {data}");
        }

        // Finalizer to show when objects are actually collected
        ~ExpensiveObject()
        {
            Console.WriteLine($"  ExpensiveObject #{_instanceId} finalized: {Data}");
        }

        public void DoWork()
        {
            Console.WriteLine($"ExpensiveObject #{_instanceId} is working with: {Data}");
            
            // Simulate some expensive work
            var sum = 0;
            for (int i = 0; i < _expensiveData.Length; i += 100)
            {
                sum += _expensiveData[i];
            }
        }

        public override string ToString()
        {
            return $"ExpensiveObject #{_instanceId}: {Data}";
        }
    }
}
