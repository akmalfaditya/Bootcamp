﻿using System;

/// <summary>
/// Comprehensive demonstration of operator overloading in C#.
/// 
/// This project showcases all major operator overloading concepts:
/// 1. Basic arithmetic operators (+, -, *, /)
/// 2. Compound assignment operators (+=, -=, *=, /=) - automatic when basic operators are defined
/// 3. Expression-bodied operator syntax for cleaner code
/// 4. Checked operators (C# 11+) for overflow detection
/// 5. Equality operators (==, !=) with proper Equals/GetHashCode overrides
/// 6. Comparison operators (<, >, <=, >=) with IComparable implementation
/// 7. Implicit and explicit conversion operators
/// 8. Unary operators (+, -, !, ++, --)
/// 9. Bitwise operators (&, |, ^, ~, <<, >>)
/// 10. True/false operators for three-valued logic (like SQL NULL handling)
/// 11. Real-world applications in financial and mathematical domains
/// 
/// Key takeaways for developers:
/// - Operator overloading makes custom types feel like built-in types
/// - Always implement operators in logical pairs (== with !=, < with >, etc.)
/// - Use structs for value-type semantics in mathematical types
/// - Implicit conversions should be safe; explicit conversions may lose data
/// - True/false operators enable custom types to work with if statements and logical operators
/// - Checked operators provide overflow safety for mission-critical calculations
/// </summary>

namespace OperatorOverloadingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Operator Overloading in C# - Complete Demonstration ===\n");

            // Run all demonstrations
            BasicArithmeticOverloadingDemo();
            CompoundAssignmentDemo();
            ExpressionBodiedOperatorsDemo();
            CheckedOperatorsDemo();
            EqualityOperatorsDemo();
            ComparisonOperatorsDemo();
            ImplicitExplicitConversionsDemo();
            UnaryOperatorsDemo();
            IncrementDecrementDemo();
            BitwiseOperatorsDemo();
            TrueFalseOperatorsDemo();
            RealWorldScenarioDemo();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        #region Basic Arithmetic Overloading

        static void BasicArithmeticOverloadingDemo()
        {
            Console.WriteLine("1. BASIC ARITHMETIC OPERATOR OVERLOADING");
            Console.WriteLine("=========================================");

            // Create some musical notes
            Note noteA = new Note(0);    // A note (reference point)
            Note noteB = new Note(2);    // B note (2 semitones from A)
            Note noteC = new Note(3);    // C note (3 semitones from A)

            Console.WriteLine($"Note A: {noteA.SemitonesFromA} semitones from A");
            Console.WriteLine($"Note B: {noteB.SemitonesFromA} semitones from A");
            Console.WriteLine($"Note C: {noteC.SemitonesFromA} semitones from A");

            // Use overloaded + operator to transpose notes
            Note cSharp = noteC + 1;     // Add 1 semitone to C to get C#
            Note fSharp = noteA + 9;     // Add 9 semitones to A to get F#

            Console.WriteLine($"\nAfter transposition:");
            Console.WriteLine($"C + 1 semitone = C# ({cSharp.SemitonesFromA} semitones)");
            Console.WriteLine($"A + 9 semitones = F# ({fSharp.SemitonesFromA} semitones)");

            // Use overloaded - operator to find intervals
            int intervalBC = noteC - noteB;  // Distance between B and C
            Console.WriteLine($"\nInterval between B and C: {intervalBC} semitones");

            // Use overloaded * operator for octave multiplication
            Note highC = noteC * 2;      // C note, 2 octaves higher
            Console.WriteLine($"C note 2 octaves higher: {highC.SemitonesFromA} semitones");

            Console.WriteLine();
        }

        #endregion

        #region Compound Assignment

        static void CompoundAssignmentDemo()
        {
            Console.WriteLine("2. COMPOUND ASSIGNMENT OPERATORS");
            Console.WriteLine("=================================");

            Note currentNote = new Note(5);  // F note
            Console.WriteLine($"Starting note: {currentNote.SemitonesFromA} semitones (F)");

            // These work automatically when you overload the basic operators
            currentNote += 2;  // Move up 2 semitones (F to G)
            Console.WriteLine($"After += 2: {currentNote.SemitonesFromA} semitones (G)");

            currentNote -= 1;  // Move down 1 semitone (G to F#)
            Console.WriteLine($"After -= 1: {currentNote.SemitonesFromA} semitones (F#)");

            currentNote *= 2;  // Double the octave
            Console.WriteLine($"After *= 2: {currentNote.SemitonesFromA} semitones (F# high octave)");

            Console.WriteLine();
        }

        #endregion

        #region Checked Operators (C# 11+)

        static void CheckedOperatorsDemo()
        {
            Console.WriteLine("3.5. CHECKED OPERATORS (C# 11+)");
            Console.WriteLine("=================================");

            Console.WriteLine("Starting with SafeNumber demonstration:");
            
            SafeNumber safeNum1 = new SafeNumber(int.MaxValue - 1);
            SafeNumber safeNum2 = new SafeNumber(5);

            Console.WriteLine($"SafeNumber1: {safeNum1.Value}");
            Console.WriteLine($"SafeNumber2: {safeNum2.Value}");

            // Unchecked addition (default behavior)
            try
            {
                SafeNumber uncheckedResult = safeNum1 + safeNum2;
                Console.WriteLine($"Unchecked addition result: {uncheckedResult.Value}");
            }
            catch (OverflowException ex)
            {
                Console.WriteLine($"Unchecked addition failed: {ex.Message}");
            }

            // Checked addition (explicit overflow checking)
            try
            {
                SafeNumber checkedResult = checked(safeNum1 + safeNum2);
                Console.WriteLine($"Checked addition result: {checkedResult.Value}");
            }
            catch (OverflowException ex)
            {
                Console.WriteLine($"Checked addition detected overflow: {ex.Message}");
            }

            Console.WriteLine("\nNote: The checked operator provides overflow detection for safer arithmetic operations.");
            Console.WriteLine();
        }

        #endregion

        #region Expression-Bodied Operators

        static void ExpressionBodiedOperatorsDemo()
        {
            Console.WriteLine("3. EXPRESSION-BODIED OPERATORS");
            Console.WriteLine("===============================");

            Console.WriteLine("The operators in our Note struct use expression-bodied syntax:");
            Console.WriteLine("public static Note operator +(Note x, int semitones) => new Note(x.value + semitones);");
            Console.WriteLine("\nThis makes the code more concise and readable for simple operations.");

            // Demonstrate the clean syntax in action
            Vector2D vector1 = new Vector2D(3, 4);
            Vector2D vector2 = new Vector2D(1, 2);

            Console.WriteLine($"\nVector addition example:");
            Console.WriteLine($"Vector1: {vector1}");
            Console.WriteLine($"Vector2: {vector2}");

            Vector2D sum = vector1 + vector2;
            Console.WriteLine($"Sum: {sum}");

            Console.WriteLine();
        }

        #endregion

        #region Equality Operators

        static void EqualityOperatorsDemo()
        {
            Console.WriteLine("5. EQUALITY OPERATORS DEMONSTRATION");
            Console.WriteLine("===================================");

            Note note1 = new Note(5);   // F note
            Note note2 = new Note(5);   // Another F note
            Note note3 = new Note(7);   // G note

            Console.WriteLine($"Note1: {note1.SemitonesFromA} semitones");
            Console.WriteLine($"Note2: {note2.SemitonesFromA} semitones");
            Console.WriteLine($"Note3: {note3.SemitonesFromA} semitones");

            // Test equality operators
            Console.WriteLine($"\nEquality tests:");
            Console.WriteLine($"note1 == note2: {note1 == note2}");  // True
            Console.WriteLine($"note1 == note3: {note1 == note3}");  // False
            Console.WriteLine($"note1 != note3: {note1 != note3}");  // True

            // Test Equals method (overridden)
            Console.WriteLine($"\nEquals method tests:");
            Console.WriteLine($"note1.Equals(note2): {note1.Equals(note2)}");
            Console.WriteLine($"note1.Equals(note3): {note1.Equals(note3)}");

            // Test GetHashCode (same values should have same hash)
            Console.WriteLine($"\nHash codes:");
            Console.WriteLine($"note1.GetHashCode(): {note1.GetHashCode()}");
            Console.WriteLine($"note2.GetHashCode(): {note2.GetHashCode()}");
            Console.WriteLine($"note3.GetHashCode(): {note3.GetHashCode()}");

            Console.WriteLine();
        }

        #endregion

        #region Comparison Operators

        static void ComparisonOperatorsDemo()
        {
            Console.WriteLine("6. COMPARISON OPERATORS DEMONSTRATION");
            Console.WriteLine("=====================================");

            Note[] notes = {
                new Note(0),   // A
                new Note(2),   // B
                new Note(5),   // D
                new Note(7),   // E
                new Note(9)    // F#
            };

            Console.WriteLine("Original notes:");
            for (int i = 0; i < notes.Length; i++)
            {
                Console.WriteLine($"Note {i}: {notes[i].SemitonesFromA} semitones");
            }

            // Test comparison operators
            Console.WriteLine($"\nComparison tests:");
            Console.WriteLine($"notes[0] < notes[1]: {notes[0] < notes[1]}");  // True
            Console.WriteLine($"notes[2] > notes[1]: {notes[2] > notes[1]}");  // True
            Console.WriteLine($"notes[0] <= notes[0]: {notes[0] <= notes[0]}");  // True
            Console.WriteLine($"notes[4] >= notes[3]: {notes[4] >= notes[3]}");  // True

            // Sort the array using IComparable implementation
            Array.Sort(notes);
            Console.WriteLine($"\nSorted notes (using IComparable):");
            for (int i = 0; i < notes.Length; i++)
            {
                Console.WriteLine($"Note {i}: {notes[i].SemitonesFromA} semitones");
            }

            Console.WriteLine();
        }

        #endregion

        #region Implicit and Explicit Conversions

        static void ImplicitExplicitConversionsDemo()
        {
            Console.WriteLine("7. IMPLICIT AND EXPLICIT CONVERSIONS");
            Console.WriteLine("====================================");

            Note noteA = new Note(0);     // A note
            Note noteC = new Note(3);     // C note

            Console.WriteLine($"Note A: {noteA.SemitonesFromA} semitones");
            Console.WriteLine($"Note C: {noteC.SemitonesFromA} semitones");

            // Implicit conversion to frequency (double)
            double freqA = noteA;  // No cast needed - implicit conversion
            double freqC = noteC;

            Console.WriteLine($"\nImplicit conversion to frequency:");
            Console.WriteLine($"A note frequency: {freqA:F2} Hz");
            Console.WriteLine($"C note frequency: {freqC:F2} Hz");

            // Explicit conversion back to Note
            double someFrequency = 523.25;  // C note frequency
            Note reconstructedNote = (Note)someFrequency;  // Explicit cast required

            Console.WriteLine($"\nExplicit conversion from frequency:");
            Console.WriteLine($"Frequency {someFrequency} Hz converts to: {reconstructedNote.SemitonesFromA} semitones");

            // Test conversion accuracy
            Console.WriteLine($"\nRound-trip conversion test:");
            Note original = new Note(7);  // E note
            double freq = original;       // To frequency
            Note backToNote = (Note)freq; // Back to note
            Console.WriteLine($"Original: {original.SemitonesFromA} semitones");
            Console.WriteLine($"Frequency: {freq:F2} Hz");
            Console.WriteLine($"Back to note: {backToNote.SemitonesFromA} semitones");

            Console.WriteLine();
        }

        #endregion

        #region Unary Operators

        static void UnaryOperatorsDemo()
        {
            Console.WriteLine("8. UNARY OPERATORS DEMONSTRATION");
            Console.WriteLine("=================================");

            Vector2D vector = new Vector2D(3, -4);
            Console.WriteLine($"Original vector: {vector}");
            Console.WriteLine($"Magnitude: {vector.Magnitude:F2}");

            // Unary minus (negation)
            Vector2D negated = -vector;
            Console.WriteLine($"Negated vector: {negated}");

            // Unary plus (identity, but could normalize)
            Vector2D normalized = +vector;
            Console.WriteLine($"Normalized vector: {normalized}");

            // Logical not (custom behavior)
            Vector2D perpendicular = !vector;
            Console.WriteLine($"Perpendicular vector: {perpendicular}");

            Console.WriteLine();
        }

        #endregion

        #region Increment and Decrement

        static void IncrementDecrementDemo()
        {
            Console.WriteLine("9. INCREMENT AND DECREMENT OPERATORS");
            Console.WriteLine("====================================");

            Counter counter = new Counter(5);
            Console.WriteLine($"Initial counter: {counter.Value}");

            // Pre-increment
            Counter preInc = ++counter;
            Console.WriteLine($"After pre-increment (++counter): counter={counter.Value}, returned={preInc.Value}");

            // Post-increment
            Counter postInc = counter++;
            Console.WriteLine($"After post-increment (counter++): counter={counter.Value}, returned={postInc.Value}");

            // Pre-decrement
            Counter preDec = --counter;
            Console.WriteLine($"After pre-decrement (--counter): counter={counter.Value}, returned={preDec.Value}");

            // Post-decrement
            Counter postDec = counter--;
            Console.WriteLine($"After post-decrement (counter--): counter={counter.Value}, returned={postDec.Value}");

            Console.WriteLine();
        }

        #endregion

        #region Bitwise Operators

        static void BitwiseOperatorsDemo()
        {
            Console.WriteLine("10. BITWISE OPERATORS DEMONSTRATION");
            Console.WriteLine("==================================");

            BitSet set1 = new BitSet(0b1010);  // Binary: 1010
            BitSet set2 = new BitSet(0b1100);  // Binary: 1100

            Console.WriteLine($"Set1: {set1}");
            Console.WriteLine($"Set2: {set2}");

            // Bitwise AND
            BitSet andResult = set1 & set2;
            Console.WriteLine($"set1 & set2: {andResult}");

            // Bitwise OR
            BitSet orResult = set1 | set2;
            Console.WriteLine($"set1 | set2: {orResult}");

            // Bitwise XOR
            BitSet xorResult = set1 ^ set2;
            Console.WriteLine($"set1 ^ set2: {xorResult}");

            // Bitwise NOT
            BitSet notResult = ~set1;
            Console.WriteLine($"~set1: {notResult}");

            // Left shift
            BitSet leftShift = set1 << 2;
            Console.WriteLine($"set1 << 2: {leftShift}");

            // Right shift
            BitSet rightShift = set1 >> 1;
            Console.WriteLine($"set1 >> 1: {rightShift}");

            Console.WriteLine();
        }

        #endregion

        #region True/False Operators

        static void TrueFalseOperatorsDemo()
        {
            Console.WriteLine("11. TRUE/FALSE OPERATORS DEMONSTRATION");
            Console.WriteLine("======================================");

            Console.WriteLine("SqlBoolean demonstrates three-valued logic (True, False, Null)");
            Console.WriteLine("This is useful for database operations where NULL values are common.");

            SqlBoolean trueValue = SqlBoolean.True;
            SqlBoolean falseValue = SqlBoolean.False;
            SqlBoolean nullValue = SqlBoolean.Null;

            Console.WriteLine($"\nBasic values:");
            Console.WriteLine($"True: {trueValue}");
            Console.WriteLine($"False: {falseValue}");
            Console.WriteLine($"Null: {nullValue}");

            // Test in conditional statements
            Console.WriteLine($"\nConditional statement tests:");
            
            Console.WriteLine("Testing 'if (trueValue)':");
            if (trueValue)
                Console.WriteLine("  Entered true branch");
            else
                Console.WriteLine("  Entered false branch");

            Console.WriteLine("Testing 'if (falseValue)':");
            if (falseValue)
                Console.WriteLine("  Entered true branch");
            else
                Console.WriteLine("  Entered false branch");

            Console.WriteLine("Testing 'if (nullValue)':");
            if (nullValue)
                Console.WriteLine("  Entered true branch");
            else
                Console.WriteLine("  Entered false branch");

            // Test logical operations with bitwise operators
            Console.WriteLine($"\nLogical operations:");
            Console.WriteLine($"trueValue & falseValue: {trueValue & falseValue}");
            Console.WriteLine($"trueValue & nullValue: {trueValue & nullValue}");
            Console.WriteLine($"falseValue & nullValue: {falseValue & nullValue}");

            Console.WriteLine($"trueValue | falseValue: {trueValue | falseValue}");
            Console.WriteLine($"trueValue | nullValue: {trueValue | nullValue}");
            Console.WriteLine($"falseValue | nullValue: {falseValue | nullValue}");

            // Test negation
            Console.WriteLine($"\nNegation operations:");
            Console.WriteLine($"!trueValue: {!trueValue}");
            Console.WriteLine($"!falseValue: {!falseValue}");
            Console.WriteLine($"!nullValue: {!nullValue}");

            Console.WriteLine();
        }

        #endregion

        #region Real World Scenario

        static void RealWorldScenarioDemo()
        {
            Console.WriteLine("12. REAL WORLD SCENARIO - MONEY CALCULATION SYSTEM");
            Console.WriteLine("==================================================");

            // Create some money amounts
            Money salary = new Money(5000.00m, "USD");
            Money bonus = new Money(1000.00m, "USD");
            Money tax = new Money(900.00m, "USD");

            Console.WriteLine($"Monthly salary: {salary}");
            Console.WriteLine($"Performance bonus: {bonus}");
            Console.WriteLine($"Tax deduction: {tax}");

            // Use overloaded operators for calculations
            Money totalIncome = salary + bonus;
            Money netIncome = totalIncome - tax;

            Console.WriteLine($"\nCalculations:");
            Console.WriteLine($"Total income: {totalIncome}");
            Console.WriteLine($"Net income after tax: {netIncome}");

            // Comparison operations
            Console.WriteLine($"\nComparisons:");
            Console.WriteLine($"Salary > Tax: {salary > tax}");
            Console.WriteLine($"Bonus == Tax: {bonus == tax}");

            // Multiplication for projections
            Money yearlyProjection = netIncome * 12;
            Console.WriteLine($"Yearly projection: {yearlyProjection}");            // Percentage calculations
            Money raise = salary * 0.1m;  // 10% raise
            Money newSalary = salary + raise;
            Console.WriteLine($"After 10% raise: {newSalary}");

            // Currency conversion using method (better than operator overloading for this)
            Money eurAmount = netIncome.ConvertTo("EUR");
            Console.WriteLine($"Converted to EUR: {eurAmount}");

            // Conversion operators demonstration
            decimal rawAmount = (decimal)salary;  // Explicit conversion to decimal
            Money fromDecimal = 2500.00m;         // Implicit conversion from decimal
            Console.WriteLine($"Extracted amount: {rawAmount:C}");
            Console.WriteLine($"Created from decimal: {fromDecimal}");

            Console.WriteLine();
        }

        #endregion
    }

    #region Musical Note Structure

    /// <summary>
    /// Represents a musical note as semitones from A.
    /// This is a perfect example of operator overloading for a domain-specific type.
    /// Musicians think in terms of transposition (adding semitones), intervals (subtracting notes),
    /// and octaves (multiplying), so operators make the code read naturally.
    /// </summary>
    public struct Note : IComparable<Note>
    {
        private readonly int value;  // Semitones from A

        public int SemitonesFromA => value;

        public Note(int semitonesFromA)
        {
            value = semitonesFromA;
        }

        // Arithmetic operators with expression-bodied syntax (C# 6+)
        // This is much cleaner than traditional method syntax for simple operations
        public static Note operator +(Note x, int semitones) => new Note(x.value + semitones);
        public static Note operator -(Note x, int semitones) => new Note(x.value - semitones);
        public static int operator -(Note x, Note y) => x.value - y.value;  // Returns interval
        public static Note operator *(Note x, int octaves) => new Note(x.value + (octaves - 1) * 12);

        // Equality operators - MUST implement both == and != as a pair
        // The C# compiler enforces this rule
        public static bool operator ==(Note x, Note y) => x.value == y.value;
        public static bool operator !=(Note x, Note y) => x.value != y.value;

        // Comparison operators - implement all four or none
        // This enables sorting, comparisons, and use with generic collections
        public static bool operator <(Note x, Note y) => x.value < y.value;
        public static bool operator >(Note x, Note y) => x.value > y.value;
        public static bool operator <=(Note x, Note y) => x.value <= y.value;
        public static bool operator >=(Note x, Note y) => x.value >= y.value;

        // Implicit conversion to frequency (safe - no data loss, commonly used)
        // Users can write: double freq = noteA; instead of noteA.ToFrequency()
        public static implicit operator double(Note note)
        {
            // Convert semitones to frequency using A4 = 440 Hz as reference
            return 440.0 * Math.Pow(2.0, (double)note.value / 12.0);
        }

        // Explicit conversion from frequency (potentially lossy - requires casting)
        // Users must write: Note note = (Note)440.0; to show intent
        public static explicit operator Note(double frequency)
        {
            // Convert frequency back to nearest semitone
            int semitones = (int)Math.Round(12.0 * Math.Log2(frequency / 440.0));
            return new Note(semitones);
        }

        // IComparable implementation - required when overloading comparison operators
        // This enables Array.Sort(), List.Sort(), and other framework sorting methods
        public int CompareTo(Note other) => this.value.CompareTo(other.value);

        // Object overrides - ALWAYS override these when implementing == and !=
        // This ensures consistency between operators and framework methods
        public override bool Equals(object? obj)
        {
            if (obj is Note note)
                return this == note;
            return false;
        }

        public override int GetHashCode() => value.GetHashCode();

        public override string ToString()
        {
            string[] noteNames = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
            int noteIndex = ((value % 12) + 12) % 12;  // Handle negative values
            int octave = 4 + (value / 12);
            return $"{noteNames[noteIndex]}{octave}";
        }
    }

    #endregion

    #region Vector2D Structure

    public struct Vector2D
    {
        public double X { get; }
        public double Y { get; }

        public double Magnitude => Math.Sqrt(X * X + Y * Y);

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        // Arithmetic operators
        public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.X + b.X, a.Y + b.Y);
        public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator *(Vector2D v, double scalar) => new Vector2D(v.X * scalar, v.Y * scalar);

        // Unary operators
        public static Vector2D operator -(Vector2D v) => new Vector2D(-v.X, -v.Y);
        public static Vector2D operator +(Vector2D v) => v.Magnitude == 0 ? v : new Vector2D(v.X / v.Magnitude, v.Y / v.Magnitude);
        public static Vector2D operator !(Vector2D v) => new Vector2D(-v.Y, v.X);  // 90-degree rotation

        public override string ToString() => $"({X:F1}, {Y:F1})";
    }

    #endregion

    #region Counter Class

    public class Counter
    {
        public int Value { get; private set; }

        public Counter(int initialValue)
        {
            Value = initialValue;
        }

        // Pre-increment: returns the incremented value
        public static Counter operator ++(Counter c)
        {
            c.Value++;
            return c;
        }

        // Pre-decrement: returns the decremented value
        public static Counter operator --(Counter c)
        {
            c.Value--;
            return c;
        }

        // Note: C# doesn't allow separate post-increment operators
        // The compiler automatically handles the difference between ++c and c++
    }

    #endregion

    #region BitSet Structure

    public struct BitSet
    {
        private readonly int bits;

        public BitSet(int value)
        {
            bits = value;
        }

        // Bitwise operators
        public static BitSet operator &(BitSet a, BitSet b) => new BitSet(a.bits & b.bits);
        public static BitSet operator |(BitSet a, BitSet b) => new BitSet(a.bits | b.bits);
        public static BitSet operator ^(BitSet a, BitSet b) => new BitSet(a.bits ^ b.bits);
        public static BitSet operator ~(BitSet a) => new BitSet(~a.bits);
        public static BitSet operator <<(BitSet a, int shift) => new BitSet(a.bits << shift);
        public static BitSet operator >>(BitSet a, int shift) => new BitSet(a.bits >> shift);

        public override string ToString() => Convert.ToString(bits, 2).PadLeft(8, '0');
    }

    #endregion

    #region Money Class

    public class Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        // Arithmetic operators
        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot add different currencies");
            return new Money(a.Amount + b.Amount, a.Currency);
        }

        public static Money operator -(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot subtract different currencies");
            return new Money(a.Amount - b.Amount, a.Currency);
        }

        public static Money operator *(Money money, decimal multiplier)
            => new Money(money.Amount * multiplier, money.Currency);

        public static Money operator *(decimal multiplier, Money money)
            => money * multiplier;

        // Comparison operators
        public static bool operator ==(Money a, Money b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.Currency == b.Currency && a.Amount == b.Amount;
        }

        public static bool operator !=(Money a, Money b) => !(a == b);

        public static bool operator >(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot compare different currencies");
            return a.Amount > b.Amount;
        }

        public static bool operator <(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot compare different currencies");
            return a.Amount < b.Amount;
        }        public static bool operator >=(Money a, Money b) => a > b || a == b;
        public static bool operator <=(Money a, Money b) => a < b || a == b;

        // Explicit conversion to decimal (extract amount)
        public static explicit operator decimal(Money money) => money.Amount;

        // Implicit conversion from decimal (assumes USD)
        public static implicit operator Money(decimal amount) => new Money(amount, "USD");

        // Method for currency conversion (more appropriate than operator overloading)
        public Money ConvertTo(string targetCurrency)
        {
            // Simulated conversion rates from USD
            decimal rate = targetCurrency.ToUpper() switch
            {
                "USD" => 1.0m,
                "EUR" => 0.85m,
                "GBP" => 0.73m,
                "JPY" => 110.0m,
                _ => throw new NotSupportedException($"Currency {targetCurrency} not supported")
            };

            // Convert to USD first if needed, then to target currency
            decimal usdAmount = Currency.ToUpper() == "USD" ? Amount : Amount / GetRateFromUSD(Currency);
            return new Money(usdAmount * rate, targetCurrency.ToUpper());
        }

        private decimal GetRateFromUSD(string currency)
        {
            return currency.ToUpper() switch
            {
                "USD" => 1.0m,
                "EUR" => 0.85m,
                "GBP" => 0.73m,
                "JPY" => 110.0m,
                _ => 1.0m
            };
        }        public override bool Equals(object? obj)
        {
            if (obj is Money money)
                return this == money;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }

        public override string ToString() => $"{Amount:C} {Currency}";
    }

    #endregion

    #region SafeNumber Structure (C# 11+ Checked Operators)

    public struct SafeNumber
    {
        public int Value { get; }

        public SafeNumber(int value)
        {
            Value = value;
        }

        // Regular (unchecked) addition operator
        public static SafeNumber operator +(SafeNumber x, SafeNumber y)
            => new SafeNumber(x.Value + y.Value);

        // Checked addition operator (C# 11+)
        // This version is called when the operation is in a checked context
        public static SafeNumber operator checked +(SafeNumber x, SafeNumber y)
            => new SafeNumber(checked(x.Value + y.Value));

        // Regular subtraction
        public static SafeNumber operator -(SafeNumber x, SafeNumber y)
            => new SafeNumber(x.Value - y.Value);

        // Checked subtraction
        public static SafeNumber operator checked -(SafeNumber x, SafeNumber y)
            => new SafeNumber(checked(x.Value - y.Value));

        // Regular multiplication
        public static SafeNumber operator *(SafeNumber x, SafeNumber y)
            => new SafeNumber(x.Value * y.Value);

        // Checked multiplication
        public static SafeNumber operator checked *(SafeNumber x, SafeNumber y)
            => new SafeNumber(checked(x.Value * y.Value));

        public override string ToString() => Value.ToString();
    }

    #endregion

    #region SqlBoolean Structure (True/False Operators)

    /// <summary>
    /// Reimplementation of SqlBoolean-like functionality to demonstrate true/false operators.
    /// This type supports three-valued logic: True, False, and Null.
    /// </summary>
    public struct SqlBoolean
    {
        private readonly byte m_value;

        // Static instances for the three possible values
        public static readonly SqlBoolean Null = new SqlBoolean(0);
        public static readonly SqlBoolean False = new SqlBoolean(1);
        public static readonly SqlBoolean True = new SqlBoolean(2);

        private SqlBoolean(byte value)
        {
            m_value = value;
        }

        // The true operator - determines when the value should be considered "true" in conditionals
        public static bool operator true(SqlBoolean x)
            => x.m_value == True.m_value;

        // The false operator - determines when the value should be considered "false" in conditionals
        public static bool operator false(SqlBoolean x)
            => x.m_value == False.m_value;

        // Logical NOT operator
        public static SqlBoolean operator !(SqlBoolean x)
        {
            if (x.m_value == Null.m_value) return Null;
            if (x.m_value == False.m_value) return True;
            return False;
        }

        // Bitwise AND operator (works with && when true/false operators are defined)
        public static SqlBoolean operator &(SqlBoolean x, SqlBoolean y)
        {
            // SQL three-valued logic for AND
            if (x.m_value == False.m_value || y.m_value == False.m_value) return False;
            if (x.m_value == Null.m_value || y.m_value == Null.m_value) return Null;
            return True;
        }

        // Bitwise OR operator (works with || when true/false operators are defined)
        public static SqlBoolean operator |(SqlBoolean x, SqlBoolean y)
        {
            // SQL three-valued logic for OR
            if (x.m_value == True.m_value || y.m_value == True.m_value) return True;
            if (x.m_value == Null.m_value || y.m_value == Null.m_value) return Null;
            return False;
        }

        // Equality operators
        public static bool operator ==(SqlBoolean x, SqlBoolean y)
            => x.m_value == y.m_value;

        public static bool operator !=(SqlBoolean x, SqlBoolean y)
            => x.m_value != y.m_value;

        // Explicit conversion to bool (may throw for Null)
        public static explicit operator bool(SqlBoolean x)
        {
            if (x.m_value == Null.m_value)
                throw new InvalidOperationException("Cannot convert SqlBoolean.Null to bool");
            return x.m_value == True.m_value;
        }

        // Implicit conversion from bool
        public static implicit operator SqlBoolean(bool value)
            => value ? True : False;

        public override bool Equals(object? obj)
        {
            if (obj is SqlBoolean other)
                return this == other;
            return false;
        }

        public override int GetHashCode() => m_value.GetHashCode();

        public override string ToString()
        {
            if (m_value == Null.m_value) return "Null";
            if (m_value == False.m_value) return "False";
            return "True";
        }
    }

    #endregion
}
