using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Generics
{
    #region Why Generics Exist - Supporting Classes

    /// <summary>
    /// Object-based stack - demonstrates the problems generics solve
    /// Shows type safety issues and boxing/unboxing performance problems
    /// </summary>
    public class ObjectStack
    {
        private object[] data = new object[100];
        private int position = 0;

        public void Push(object item)
        {
            if (position >= data.Length)
                throw new StackOverflowException("Stack is full");
            data[position++] = item;
        }

        public object Pop()
        {
            if (position == 0)
                throw new InvalidOperationException("Stack is empty");
            return data[--position];
        }

        public int Count => position;
    }

    /// <summary>
    /// Generic stack - demonstrates the solution generics provide
    /// Type-safe, no boxing/unboxing, clean API
    /// This is the Stack<T> example from the course material
    /// </summary>
    public class Stack<T>
    {
        private T[] data = new T[100];
        private int position = 0;

        public void Push(T obj) => data[position++] = obj; // Accepts type T
        public T Pop() => data[--position];                // Returns type T

        public int Count => position;
    }

    #endregion

    #region Type Parameters - Supporting Classes

    /// <summary>
    /// Single type parameter example
    /// Follows T convention for single type parameter
    /// </summary>
    public class SingleTypeParameter<T>
    {
        private T value;

        public SingleTypeParameter(T value)
        {
            this.value = value;
        }

        public void ShowType()
        {
            Console.WriteLine($"Type: {typeof(T).Name}, Value: {value}");
        }
    }

    /// <summary>
    /// Multiple type parameters example
    /// Uses descriptive names with T prefix: TKey, TValue
    /// </summary>
    public class MultipleTypeParameters<TKey, TValue>
    {
        private TKey? key;
        private TValue? value;

        public void SetKeyValue(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public void ShowPair()
        {
            Console.WriteLine($"Key type: {typeof(TKey).Name}, Value type: {typeof(TValue).Name}");
            Console.WriteLine($"Key: {key}, Value: {value}");
        }
    }

    /// <summary>
    /// Demonstrates generic type overloading by arity
    /// ArityOverloadExample, ArityOverloadExample<T>, ArityOverloadExample<T1, T2> are all different types
    /// </summary>
    public class ArityOverloadExample
    {
        public void ShowInfo()
        {
            Console.WriteLine("ArityOverloadExample (no type parameters)");
        }
    }

    public class ArityOverloadExample<T>
    {
        private T value;

        public ArityOverloadExample(T value)
        {
            this.value = value;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"ArityOverloadExample<T> where T is {typeof(T).Name}, value: {value}");
        }
    }

    public class ArityOverloadExample<T1, T2>
    {
        private T1 first;
        private T2 second;

        public ArityOverloadExample(T1 first, T2 second)
        {
            this.first = first;
            this.second = second;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"ArityOverloadExample<T1, T2> where T1 is {typeof(T1).Name}, T2 is {typeof(T2).Name}");
            Console.WriteLine($"Values: {first}, {second}");
        }
    }

    #endregion

    #region Default Keyword - Supporting Classes

    /// <summary>
    /// Demonstrates default(T) behavior with different types
    /// Shows practical usage in generic programming
    /// </summary>
    public static class DefaultValueDemo
    {
        /// <summary>
        /// Shows the default value for any type T
        /// Reference types: null, Value types: zero-initialized
        /// </summary>
        public static void ShowDefaultValue<T>()
        {
            T defaultValue = default(T);
            string typeName = typeof(T).Name;
            
            if (defaultValue == null)
            {
                Console.WriteLine($"default({typeName}) = null");
            }
            else
            {
                Console.WriteLine($"default({typeName}) = {defaultValue}");
            }
        }

        /// <summary>
        /// Practical usage: clearing arrays with default values
        /// This is what the Zap method from the material demonstrates
        /// </summary>
        public static void ClearArray<T>(T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = default(T); // Sets each element to its default value
            }
        }

        /// <summary>
        /// C# 7.1+ simplified default syntax
        /// Can omit (T) when compiler can infer the type
        /// </summary>
        public static void DemonstrateSimplifiedDefault()
        {
            // C# 7.1+ - can omit the type when it can be inferred
            int number = default; // Instead of default(int)
            string text = default; // Instead of default(string)
            DateTime date = default; // Instead of default(DateTime)
            
            Console.WriteLine($"Simplified default int: {number}");
            Console.WriteLine($"Simplified default string: {text ?? "null"}");
            Console.WriteLine($"Simplified default DateTime: {date}");
        }
    }

    #endregion

    #region Subclassing Generic Types - Supporting Classes

    /// <summary>
    /// Pattern 1: Leave base class type parameters open
    /// SpecialStack<T> inherits from CustomStack<T>, keeping T open
    /// </summary>
    public class SpecialStack<T> : CustomStack<T>
    {
        public int OperationCount { get; private set; }

        public new void Push(T item)
        {
            base.Push(item);
            OperationCount++;
        }

        public new T Pop()
        {
            OperationCount++;
            return base.Pop();
        }

        public void PushWithLogging(T item)
        {
            Console.WriteLine($"Pushing {item} to SpecialStack");
            Push(item);
        }
    }

    /// <summary>
    /// Pattern 2: Close generic type parameters with concrete types
    /// IntStack inherits from CustomStack<int>, closing the type parameter
    /// </summary>
    public class IntStack : CustomStack<int>
    {
        public void PushMultiple(params int[] values)
        {
            foreach (int value in values)
            {
                Push(value);
            }
        }

        public int Sum()
        {
            int sum = 0;
            var tempStack = new CustomStack<int>();
            
            // Pop all items to calculate sum, then push them back
            while (!IsEmpty)
            {
                var item = Pop();
                sum += item;
                tempStack.Push(item);
            }
            
            // Restore original order
            while (!tempStack.IsEmpty)
            {
                Push(tempStack.Pop());
            }
            
            return sum;
        }
    }

    /// <summary>
    /// Mixed inheritance - some type parameters open, some closed
    /// </summary>
    public class BaseGeneric<T, U>
    {
        protected T firstValue;
        protected U secondValue;

        public virtual void SetData(T first, U second)
        {
            firstValue = first;
            secondValue = second;
        }

        public virtual void ShowData()
        {
            Console.WriteLine($"First: {firstValue} (Type: {typeof(T).Name})");
            Console.WriteLine($"Second: {secondValue} (Type: {typeof(U).Name})");
        }
    }

    /// <summary>
    /// Mixed inheritance example - keeps T open, closes U to int
    /// </summary>
    public class MixedInheritanceExample<T> : BaseGeneric<T, int>
    {
        public override void ShowData()
        {
            Console.WriteLine("Mixed inheritance example:");
            base.ShowData();
        }
    }

    #endregion

    #region Self-Referencing Generics - Supporting Classes

    /// <summary>
    /// Balloon class implementing IEquatable<Balloon>
    /// This is the self-referencing pattern from the course material
    /// </summary>
    public class Balloon : IEquatable<Balloon>
    {
        public string Color { get; set; } = "";
        public int CC { get; set; }

        public bool Equals(Balloon? other)
        {
            if (other == null) return false;
            return other.Color == Color && other.CC == CC;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Balloon);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Color, CC);
        }
    }

    /// <summary>
    /// Person class implementing IComparable<Person>
    /// Another example of self-referencing generics
    /// </summary>
    public class ComparablePerson : IComparable<ComparablePerson>
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public ComparablePerson(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public int CompareTo(ComparablePerson? other)
        {
            if (other == null) return 1;
            return Age.CompareTo(other.Age); // Compare by age
        }

        public override string ToString()
        {
            return $"{Name} (Age: {Age})";
        }
    }

    /// <summary>
    /// Custom self-referencing interface
    /// Demonstrates the pattern in custom scenarios
    /// </summary>
    public interface ITreeNode<T> where T : ITreeNode<T>
    {
        void AddChild(T child);
        bool CanAccept(T node);
    }

    /// <summary>
    /// Tree node implementing self-referencing interface
    /// </summary>
    public class TreeNode<T> : ITreeNode<TreeNode<T>>
    {
        public T Value { get; set; }
        private List<TreeNode<T>> children = new List<TreeNode<T>>();

        public TreeNode(T value)
        {
            Value = value;
        }

        public void AddChild(TreeNode<T> child)
        {
            children.Add(child);
        }

        public bool CanAccept(TreeNode<T> node)
        {
            // Simple logic - can accept any node
            return node != null;
        }

        public string GetTreeStructure()
        {
            var sb = new StringBuilder();
            BuildTreeString(sb, 0);
            return sb.ToString();
        }

        private void BuildTreeString(StringBuilder sb, int level)
        {
            sb.AppendLine($"{new string(' ', level * 2)}{Value}");
            foreach (var child in children)
            {
                child.BuildTreeString(sb, level + 1);
            }
        }
    }

    #endregion

    #region Static Data in Generic Types - Supporting Classes

    /// <summary>
    /// Demonstrates static data separation in generic types
    /// Each closed type (Bob<int>, Bob<string>, etc.) has its own static data
    /// </summary>
    public class Bob<T>
    {
        public static int Count;
    }

    /// <summary>
    /// Type-specific cache demonstrating practical use of static data in generics
    /// Each type T gets its own cache
    /// </summary>
    public static class TypeSpecificCache<T>
    {
        private static List<T> cache = new List<T>();

        public static void Add(T item)
        {
            cache.Add(item);
        }

        public static int Count => cache.Count;

        public static IEnumerable<T> GetAll()
        {
            return cache.AsReadOnly();
        }

        public static void Clear()
        {
            cache.Clear();
        }
    }

    #endregion

    #region Type Parameter Conversions - Supporting Classes

    /// <summary>
    /// Demonstrates solutions for type parameter conversion ambiguities
    /// Shows the problems and solutions outlined in the course material
    /// </summary>
    public static class ConversionExamples
    {
        /// <summary>
        /// Solution 1: Using the 'as' operator
        /// Safe for reference and nullable conversions only
        /// Never performs custom conversions - unambiguous
        /// </summary>
        public static StringBuilder SafeConvertToStringBuilder<T>(T arg)
        {
            StringBuilder sb = arg as StringBuilder; // OK - unambiguous
            return sb; // Returns null if conversion fails
        }

        /// <summary>
        /// Solution 2: Cast to object first
        /// Conversions to/from object are assumed not to be custom conversions
        /// This resolves the ambiguity
        /// </summary>
        public static TTarget ConvertViaObject<TTarget>(object arg)
        {
            return (TTarget)arg; // OK: First to object, then reference conversion
        }

        /// <summary>
        /// Unboxing example using object cast
        /// Similar pattern for unboxing value types
        /// </summary>
        public static T UnboxValue<T>(object boxedValue)
        {
            return (T)boxedValue; // Unboxing conversion
        }

        /// <summary>
        /// The problematic direct cast - this would cause compile error
        /// Kept as comment to show what NOT to do
        /// </summary>
        /*
        public static StringBuilder ProblematicDirectCast<T>(T arg)
        {
            return (StringBuilder)arg; // Compile-time error: Ambiguous conversion
        }
        */
    }

    #endregion

    #region Variance Supporting Classes

    /// <summary>
    /// Simple covariant interface for producing values
    /// Uses 'out' keyword to allow covariance
    /// </summary>
    public interface IProducer<out T>
    {
        T Produce();
    }

    /// <summary>
    /// Simple contravariant interface for consuming values
    /// Uses 'in' keyword to allow contravariance
    /// </summary>
    public interface IConsumer<in T>
    {
        void Consume(T item);
    }

    /// <summary>
    /// Producer implementation for the variance demonstration
    /// </summary>
    public class Producer<T> : IProducer<T> where T : new()
    {
        public T Produce()
        {
            return new T();
        }
    }

    /// <summary>
    /// Consumer implementation for the variance demonstration
    /// </summary>
    public class Consumer<T> : IConsumer<T>
    {
        public void Consume(T item)
        {
            Console.WriteLine($"  Consuming: {item}");
        }
    }

    /// <summary>
    /// Animal comparer for contravariance demonstration
    /// Compares animals by age
    /// </summary>
    public class AnimalAgeComparer : IComparer<Animal>
    {
        public int Compare(Animal? x, Animal? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return x.Age.CompareTo(y.Age);
        }
    }

    #endregion
}
