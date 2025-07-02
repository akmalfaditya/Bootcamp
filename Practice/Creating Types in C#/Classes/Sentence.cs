using System;
using System.Collections.Generic;
using System.Linq;

namespace Classes
{
    /// <summary>
    /// Sentence class demonstrating indexers - making objects behave like arrays
    /// Indexers let you access object data using square bracket notation: obj[index]
    /// Think of it like creating a custom array or dictionary interface for your object
    /// </summary>
    public class Sentence
    {
        private readonly List<string> _words;

        /// <summary>
        /// Constructor that takes a sentence and splits it into words
        /// </summary>
        /// <param name="sentence">The sentence to work with</param>
        public Sentence(string sentence = "Hello world from C# indexers!")
        {
            _words = sentence?.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList() 
                     ?? new List<string>();
            Console.WriteLine($"  📝 Created sentence with {_words.Count} words: \"{sentence}\"");
        }

        /// <summary>
        /// Integer indexer - access words by position (like an array)
        /// This is the most common type of indexer
        /// </summary>
        /// <param name="index">Position of the word (0-based)</param>
        /// <returns>The word at that position</returns>
        public string this[int index]
        {
            get
            {
                if (index < 0 || index >= _words.Count)
                    throw new IndexOutOfRangeException($"Index {index} is out of range (0-{_words.Count - 1})");
                return _words[index];
            }
            set
            {
                if (index < 0 || index >= _words.Count)
                    throw new IndexOutOfRangeException($"Index {index} is out of range (0-{_words.Count - 1})");
                
                string oldWord = _words[index];
                _words[index] = value ?? "";
                Console.WriteLine($"  ✏️ Changed word {index}: '{oldWord}' → '{value}'");
            }
        }

        /// <summary>
        /// Range indexer - access multiple words at once (C# 8+ feature)
        /// Demonstrates how indexers can work with ranges, not just single values
        /// </summary>
        /// <param name="range">Range of words to get</param>
        /// <returns>Array of words in the specified range</returns>
        public string[] this[Range range]
        {
            get
            {
                var (start, length) = range.GetOffsetAndLength(_words.Count);
                return _words.Skip(start).Take(length).ToArray();
            }
        }

        /// <summary>
        /// String indexer - find word by prefix or exact match
        /// Shows how indexers don't have to be numeric - they can use any type!
        /// </summary>
        /// <param name="prefix">Prefix to search for</param>
        /// <returns>First word starting with the prefix, or null if not found</returns>
        public string? this[string prefix]
        {
            get
            {
                return _words.FirstOrDefault(word => 
                    word.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Multi-parameter indexer - get words within a specific length range
        /// Indexers can take multiple parameters, just like methods!
        /// </summary>
        /// <param name="minLength">Minimum word length</param>
        /// <param name="maxLength">Maximum word length</param>
        /// <returns>All words within the length range</returns>
        public string[] this[int minLength, int maxLength]
        {
            get
            {
                return _words.Where(word => word.Length >= minLength && word.Length <= maxLength)
                            .ToArray();
            }
        }

        /// <summary>
        /// Property to get the number of words
        /// </summary>
        public int WordCount => _words.Count;

        /// <summary>
        /// Method to add a word to the sentence
        /// </summary>
        /// <param name="word">Word to add</param>
        public void AddWord(string word)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                _words.Add(word);
                Console.WriteLine($"  ➕ Added word: '{word}' (total words: {_words.Count})");
            }
        }

        /// <summary>
        /// Method to remove a word at a specific position
        /// </summary>
        /// <param name="index">Index of word to remove</param>
        public void RemoveWordAt(int index)
        {
            if (index >= 0 && index < _words.Count)
            {
                string removedWord = _words[index];
                _words.RemoveAt(index);
                Console.WriteLine($"  ❌ Removed word at index {index}: '{removedWord}'");
            }
            else
            {
                Console.WriteLine($"  ⚠️ Cannot remove word at index {index} - out of range!");
            }
        }

        /// <summary>
        /// Override ToString to display the sentence
        /// </summary>
        /// <returns>The complete sentence</returns>
        public override string ToString()
        {
            return string.Join(" ", _words);
        }

        /// <summary>
        /// Method to display all indexer examples
        /// </summary>
        public void DemonstrateIndexers()
        {
            Console.WriteLine($"  📖 Current sentence: \"{ToString()}\"");
            Console.WriteLine($"  📊 Word count: {WordCount}");
            
            if (_words.Count > 0)
            {
                Console.WriteLine($"  🔢 First word (index 0): '{this[0]}'");
                if (_words.Count > 1)
                {
                    Console.WriteLine($"  🔢 Second word (index 1): '{this[1]}'");
                }
                
                // Range indexer example
                if (_words.Count >= 3)
                {
                    var firstThree = this[0..3];
                    Console.WriteLine($"  📐 First 3 words (range 0..3): [{string.Join(", ", firstThree.Select(w => $"'{w}'"))}]");
                }
                
                // String indexer example
                var wordStartingWithC = this["C"];
                if (wordStartingWithC != null)
                {
                    Console.WriteLine($"  🔍 Word starting with 'C': '{wordStartingWithC}'");
                }
                
                // Multi-parameter indexer example
                var mediumWords = this[3, 6];
                if (mediumWords.Length > 0)
                {
                    Console.WriteLine($"  📏 Words 3-6 chars long: [{string.Join(", ", mediumWords.Select(w => $"'{w}'"))}]");
                }
            }
        }
    }
}
