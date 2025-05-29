using System;

namespace Classes
{
    /// <summary>
    /// Demonstrates indexers - allows objects to be accessed like arrays
    /// Indexers let you use square bracket notation: sentence[0], sentence[1], etc.
    /// </summary>
    public class Sentence
    {
        // Private array to store the words
        // This is what our indexer will access
        private string[] words;

        /// <summary>
        /// Constructor initializes with a default sentence
        /// </summary>
        public Sentence()
        {
            words = "The quick brown fox".Split(' ');
            Console.WriteLine($"Sentence created with {words.Length} words");
        }

        /// <summary>
        /// Constructor that takes a custom sentence
        /// </summary>
        /// <param name="sentence">The sentence to split into words</param>
        public Sentence(string sentence)
        {
            words = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Console.WriteLine($"Custom sentence created with {words.Length} words");
        }

        /// <summary>
        /// This is the indexer! The magic happens here.
        /// 'this' keyword with parameters in square brackets creates an indexer
        /// Now you can use sentence[0], sentence[1], etc.
        /// </summary>
        /// <param name="wordNum">Index of the word to get/set</param>
        /// <returns>The word at the specified index</returns>
        public string this[int wordNum]
        {
            get 
            { 
                // Add bounds checking - good practice!
                if (wordNum < 0 || wordNum >= words.Length)
                    throw new IndexOutOfRangeException($"Index {wordNum} is out of range. Valid range: 0-{words.Length - 1}");
                
                return words[wordNum]; 
            }
            set 
            { 
                // Same bounds checking for setter
                if (wordNum < 0 || wordNum >= words.Length)
                    throw new IndexOutOfRangeException($"Index {wordNum} is out of range. Valid range: 0-{words.Length - 1}");
                
                Console.WriteLine($"Changing word {wordNum} from '{words[wordNum]}' to '{value}'");
                words[wordNum] = value; 
            }
        }

        /// <summary>
        /// You can have multiple indexers with different parameter types!
        /// This one finds a word by its content
        /// </summary>
        /// <param name="word">The word to find</param>
        /// <returns>Index of the word, or -1 if not found</returns>
        public int this[string word]
        {
            get
            {
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Equals(word, StringComparison.OrdinalIgnoreCase))
                        return i;
                }
                return -1; // Not found
            }
        }

        /// <summary>
        /// Property to get the number of words
        /// </summary>
        public int WordCount => words.Length;

        /// <summary>
        /// Method to get the full sentence back
        /// </summary>
        /// <returns>Complete sentence as a string</returns>
        public string GetFullSentence()
        {
            return string.Join(" ", words);
        }

        /// <summary>
        /// Method to add a word to the end
        /// </summary>
        /// <param name="word">Word to add</param>
        public void AddWord(string word)
        {
            // Create a new array with one more element
            string[] newWords = new string[words.Length + 1];
            Array.Copy(words, newWords, words.Length);
            newWords[words.Length] = word;
            words = newWords;
            
            Console.WriteLine($"Added word '{word}'. Sentence now has {words.Length} words.");
        }
    }
}
