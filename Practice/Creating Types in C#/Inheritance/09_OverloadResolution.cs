using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates overload resolution in the context of inheritance.
    /// Understanding how the compiler resolves method calls when multiple overloads exist
    /// is crucial for writing predictable code with inheritance hierarchies.
    /// 
    /// Key concepts:
    /// 1. Compile-time vs runtime type resolution
    /// 2. Most specific type takes precedence
    /// 3. How inheritance affects method overload selection
    /// 4. Using dynamic to defer resolution to runtime
    /// </summary>

    // Base class for our overload resolution examples
    public class Document
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public Document(string title)
        {
            Title = title;
        }
        
        public virtual void Display()
        {
            Console.WriteLine($"Displaying Document: {Title}");
            Console.WriteLine($"Created: {CreatedDate:yyyy-MM-dd}");
        }
    }

    // First derived class
    public class TextDocument : Document
    {
        public string FontFamily { get; set; } = "Arial";
        public int FontSize { get; set; } = 12;
        
        public TextDocument(string title) : base(title)
        {
        }
        
        public override void Display()
        {
            Console.WriteLine($"Displaying Text Document: {Title}");
            Console.WriteLine($"Font: {FontFamily}, Size: {FontSize}");
            Console.WriteLine($"Content: {Content}");
        }
    }

    // Second derived class
    public class PDFDocument : Document
    {
        public int PageCount { get; set; } = 1;
        public bool IsEncrypted { get; set; } = false;
        
        public PDFDocument(string title) : base(title)
        {
        }
        
        public override void Display()
        {
            Console.WriteLine($"Displaying PDF Document: {Title}");
            Console.WriteLine($"Pages: {PageCount}, Encrypted: {IsEncrypted}");
        }
    }

    // Third derived class
    public class ImageDocument : Document
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; } = "PNG";
        
        public ImageDocument(string title) : base(title)
        {
        }
        
        public override void Display()
        {
            Console.WriteLine($"Displaying Image Document: {Title}");
            Console.WriteLine($"Resolution: {Width}x{Height}, Format: {Format}");
        }
    }

    // Class with overloaded methods for different document types
    public static class DocumentProcessor
    {
        // Method overloads for different document types
        // The compiler will choose the most specific match at compile time
        
        public static void ProcessDocument(Document doc)
        {
            Console.WriteLine("Processing generic Document");
            Console.WriteLine($"  Processing: {doc.Title}");
            doc.Display();
            Console.WriteLine("  Generic processing completed");
        }
        
        public static void ProcessDocument(TextDocument textDoc)
        {
            Console.WriteLine("Processing Text Document with specialized logic");
            Console.WriteLine($"  Processing: {textDoc.Title}");
            Console.WriteLine($"  Checking text formatting (Font: {textDoc.FontFamily})");
            textDoc.Display();
            Console.WriteLine("  Text-specific processing completed");
        }
        
        public static void ProcessDocument(PDFDocument pdfDoc)
        {
            Console.WriteLine("Processing PDF Document with specialized logic");
            Console.WriteLine($"  Processing: {pdfDoc.Title}");
            Console.WriteLine($"  Checking PDF structure ({pdfDoc.PageCount} pages)");
            if (pdfDoc.IsEncrypted)
            {
                Console.WriteLine("  Decrypting PDF...");
            }
            pdfDoc.Display();
            Console.WriteLine("  PDF-specific processing completed");
        }
        
        public static void ProcessDocument(ImageDocument imgDoc)
        {
            Console.WriteLine("Processing Image Document with specialized logic");
            Console.WriteLine($"  Processing: {imgDoc.Title}");
            Console.WriteLine($"  Analyzing image ({imgDoc.Width}x{imgDoc.Height})");
            imgDoc.Display();
            Console.WriteLine("  Image-specific processing completed");
        }
        
        // Overloaded methods with different parameter counts
        public static void ProcessDocuments(Document doc1, Document doc2)
        {
            Console.WriteLine("Processing two documents together");
            ProcessDocument(doc1);
            ProcessDocument(doc2);
            Console.WriteLine("Batch processing completed");
        }
        
        public static void ProcessDocuments(TextDocument textDoc1, TextDocument textDoc2)
        {
            Console.WriteLine("Processing two text documents with text-specific batch logic");
            Console.WriteLine("Comparing text formatting...");
            ProcessDocument(textDoc1);
            ProcessDocument(textDoc2);
            Console.WriteLine("Text batch processing completed");
        }
    }

    // Class demonstrating more complex overload scenarios
    public static class DocumentPrinter
    {
        // Overloads with base and derived types
        public static void Print(Document doc)
        {
            Console.WriteLine($"[GENERIC PRINTER] Printing document: {doc.Title}");
        }
        
        public static void Print(TextDocument textDoc)
        {
            Console.WriteLine($"[TEXT PRINTER] Printing text document: {textDoc.Title} (Font: {textDoc.FontFamily})");
        }
        
        public static void Print(PDFDocument pdfDoc)
        {
            Console.WriteLine($"[PDF PRINTER] Printing PDF document: {pdfDoc.Title} ({pdfDoc.PageCount} pages)");
        }
        
        // Overloads with additional parameters
        public static void Print(Document doc, bool isColorPrint)
        {
            string printType = isColorPrint ? "COLOR" : "BLACK & WHITE";
            Console.WriteLine($"[GENERIC PRINTER] Printing document in {printType}: {doc.Title}");
        }
        
        public static void Print(TextDocument textDoc, bool isColorPrint)
        {
            string printType = isColorPrint ? "COLOR" : "BLACK & WHITE";
            Console.WriteLine($"[TEXT PRINTER] Printing text document in {printType}: {textDoc.Title}");
        }
        
        // Method that accepts multiple document types
        public static void PrintMultiple(params Document[] documents)
        {
            Console.WriteLine($"[BATCH PRINTER] Printing {documents.Length} documents:");
            foreach (Document doc in documents)
            {
                // This will always call Print(Document) because the compile-time type is Document
                Print(doc);
            }
        }
    }

    // Class to demonstrate method resolution with interfaces
    public interface IPrintable
    {
        void Print();
    }

    public class PrintableTextDocument : TextDocument, IPrintable
    {
        public PrintableTextDocument(string title) : base(title) { }
        
        public void Print()
        {
            Console.WriteLine($"IPrintable.Print() called for: {Title}");
        }
    }

    public static class AdvancedProcessor
    {
        // Overloads with interfaces
        public static void Process(Document doc)
        {
            Console.WriteLine("Processing as Document");
        }
        
        public static void Process(IPrintable printable)
        {
            Console.WriteLine("Processing as IPrintable");
            printable.Print();
        }
        
        public static void Process(TextDocument textDoc)
        {
            Console.WriteLine("Processing as TextDocument");
        }
    }

    /// <summary>
    /// Demonstration class for overload resolution with inheritance
    /// </summary>
    public static class OverloadResolutionDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== OVERLOAD RESOLUTION WITH INHERITANCE DEMONSTRATION ===\n");
            
            // Create instances of different document types
            var textDoc = new TextDocument("My Text Document")
            {
                Content = "This is a text document content.",
                FontFamily = "Times New Roman",
                FontSize = 14
            };
            
            var pdfDoc = new PDFDocument("Technical Manual")
            {
                PageCount = 150,
                IsEncrypted = true
            };
            
            var imgDoc = new ImageDocument("Company Logo")
            {
                Width = 1920,
                Height = 1080,
                Format = "PNG"
            };
            
            // 1. DEMONSTRATE COMPILE-TIME TYPE RESOLUTION
            Console.WriteLine("1. COMPILE-TIME TYPE RESOLUTION:");
            Console.WriteLine("Method called depends on the compile-time (declared) type of the variable\n");
            
            Console.WriteLine("--- Direct object references (compile-time type = actual type) ---");
            DocumentProcessor.ProcessDocument(textDoc);  // Calls ProcessDocument(TextDocument)
            Console.WriteLine();
            
            DocumentProcessor.ProcessDocument(pdfDoc);   // Calls ProcessDocument(PDFDocument)
            Console.WriteLine();
            
            DocumentProcessor.ProcessDocument(imgDoc);   // Calls ProcessDocument(ImageDocument)
            Console.WriteLine();
            
            // 2. DEMONSTRATE BASE REFERENCE RESOLUTION
            Console.WriteLine("2. BASE REFERENCE TYPE RESOLUTION:");
            Console.WriteLine("When using base class references, the base class overload is called\n");
            
            Console.WriteLine("--- Base class references (compile-time type = Document) ---");
            Document docRef1 = textDoc;  // Document reference to TextDocument object
            Document docRef2 = pdfDoc;   // Document reference to PDFDocument object
            Document docRef3 = imgDoc;   // Document reference to ImageDocument object
            
            DocumentProcessor.ProcessDocument(docRef1);  // Calls ProcessDocument(Document) !!
            Console.WriteLine();
            
            DocumentProcessor.ProcessDocument(docRef2);  // Calls ProcessDocument(Document) !!
            Console.WriteLine();
            
            DocumentProcessor.ProcessDocument(docRef3);  // Calls ProcessDocument(Document) !!
            Console.WriteLine();
            
            // 3. DEMONSTRATE EXPLICIT CASTING TO GET SPECIFIC OVERLOADS
            Console.WriteLine("3. EXPLICIT CASTING TO ACCESS SPECIFIC OVERLOADS:");
            Console.WriteLine("You can cast to access more specific overloads\n");
            
            if (docRef1 is TextDocument textDocCast)
            {
                DocumentProcessor.ProcessDocument(textDocCast);  // Now calls ProcessDocument(TextDocument)
            }
            Console.WriteLine();
            
            if (docRef2 is PDFDocument pdfDocCast)
            {
                DocumentProcessor.ProcessDocument(pdfDocCast);   // Now calls ProcessDocument(PDFDocument)
            }
            Console.WriteLine();
            
            // 4. DEMONSTRATE BATCH PROCESSING OVERLOADS
            Console.WriteLine("4. BATCH PROCESSING OVERLOAD RESOLUTION:");
            Console.WriteLine("Overloads work with multiple parameters too\n");
            
            Console.WriteLine("--- Two specific TextDocuments ---");
            var textDoc2 = new TextDocument("Second Text Doc") { FontFamily = "Courier New" };
            DocumentProcessor.ProcessDocuments(textDoc, textDoc2);  // Calls ProcessDocuments(TextDocument, TextDocument)
            Console.WriteLine();
            
            Console.WriteLine("--- Mixed document types ---");
            DocumentProcessor.ProcessDocuments(textDoc, pdfDoc);    // Calls ProcessDocuments(Document, Document)
            Console.WriteLine();
            
            // 5. DEMONSTRATE PRINTING OVERLOADS
            Console.WriteLine("5. PRINTING OVERLOAD RESOLUTION:");
            Console.WriteLine("Multiple overloads for the same method name\n");
            
            Console.WriteLine("--- Direct references ---");
            DocumentPrinter.Print(textDoc);              // Calls Print(TextDocument)
            DocumentPrinter.Print(pdfDoc);               // Calls Print(PDFDocument)
            DocumentPrinter.Print(imgDoc);               // Calls Print(Document) - no ImageDocument overload
            Console.WriteLine();
            
            Console.WriteLine("--- With additional parameters ---");
            DocumentPrinter.Print(textDoc, true);        // Calls Print(TextDocument, bool)
            DocumentPrinter.Print(pdfDoc, false);        // Calls Print(Document, bool) - no PDFDocument+bool overload
            Console.WriteLine();
            
            Console.WriteLine("--- Through base references ---");
            DocumentPrinter.Print(docRef1);              // Calls Print(Document)
            DocumentPrinter.Print(docRef2);              // Calls Print(Document)
            Console.WriteLine();
            
            // 6. DEMONSTRATE BATCH PRINTING (ALWAYS USES BASE OVERLOAD)
            Console.WriteLine("6. BATCH PRINTING BEHAVIOR:");
            Console.WriteLine("Array/collection parameters always use the declared type\n");
            
            Document[] allDocs = { textDoc, pdfDoc, imgDoc };
            DocumentPrinter.PrintMultiple(allDocs);      // All calls go to Print(Document)
            Console.WriteLine();
            
            // 7. DEMONSTRATE DYNAMIC RESOLUTION
            Console.WriteLine("7. DYNAMIC RESOLUTION:");
            Console.WriteLine("Using 'dynamic' defers overload resolution until runtime\n");
            
            Console.WriteLine("--- Using dynamic for runtime resolution ---");
            dynamic dynamicTextDoc = textDoc;
            dynamic dynamicPdfDoc = pdfDoc;
            dynamic dynamicImgDoc = imgDoc;
            
            DocumentProcessor.ProcessDocument(dynamicTextDoc);  // Resolves to ProcessDocument(TextDocument) at runtime
            Console.WriteLine();
            
            DocumentProcessor.ProcessDocument(dynamicPdfDoc);   // Resolves to ProcessDocument(PDFDocument) at runtime
            Console.WriteLine();
            
            DocumentProcessor.ProcessDocument(dynamicImgDoc);   // Resolves to ProcessDocument(ImageDocument) at runtime
            Console.WriteLine();
            
            // 8. DEMONSTRATE INTERFACE OVERLOAD RESOLUTION
            Console.WriteLine("8. INTERFACE OVERLOAD RESOLUTION:");
            Console.WriteLine("When multiple overloads match, most specific type wins\n");
            
            var printableTextDoc = new PrintableTextDocument("Printable Document");
            
            // This is interesting - which overload gets called?
            Console.WriteLine("--- PrintableTextDocument implements IPrintable ---");
            // To resolve ambiguity, we'll explicitly cast to show different behaviors
            AdvancedProcessor.Process((TextDocument)printableTextDoc);  // Calls Process(TextDocument)
            Console.WriteLine();
            
            // Let's test different reference types
            Document docRef = printableTextDoc;
            IPrintable printRef = printableTextDoc;
            TextDocument textRef = printableTextDoc;
            
            Console.WriteLine("--- Through different reference types ---");
            Console.Write("Document reference: ");
            AdvancedProcessor.Process(docRef);            // Process(Document)
            
            Console.Write("IPrintable reference: ");
            AdvancedProcessor.Process(printRef);          // Process(IPrintable)
            
            Console.Write("TextDocument reference: ");
            AdvancedProcessor.Process(textRef);           // Process(TextDocument)
            Console.WriteLine();
            
            // 9. SUMMARY OF OVERLOAD RESOLUTION RULES
            ShowOverloadResolutionRules();
        }
        
        private static void ShowOverloadResolutionRules()
        {
            Console.WriteLine("9. OVERLOAD RESOLUTION RULES SUMMARY:");
            Console.WriteLine("=== METHOD OVERLOAD RESOLUTION RULES ===");
            Console.WriteLine();
            
            Console.WriteLine("RULE 1: Compile-time Type Matters");
            Console.WriteLine("  • Method selection is based on the declared (compile-time) type of variables");
            Console.WriteLine("  • Runtime type of the object doesn't affect overload selection");
            Console.WriteLine("  • Example: Document d = new TextDocument(); // Uses Document overloads");
            Console.WriteLine();
            
            Console.WriteLine("RULE 2: Most Specific Type Wins");
            Console.WriteLine("  • When multiple overloads match, the most specific (derived) type is chosen");
            Console.WriteLine("  • TextDocument overload preferred over Document overload for TextDocument objects");
            Console.WriteLine("  • Interfaces are considered less specific than classes");
            Console.WriteLine();
            
            Console.WriteLine("RULE 3: Exact Match Preferred");
            Console.WriteLine("  • Exact type matches are preferred over inheritance-based matches");
            Console.WriteLine("  • No implicit conversions are considered if exact match exists");
            Console.WriteLine();
            
            Console.WriteLine("RULE 4: Collections Use Element's Declared Type");
            Console.WriteLine("  • Document[] array elements are treated as Document type");
            Console.WriteLine("  • Even if actual objects are TextDocument, Document overload is used");
            Console.WriteLine();
            
            Console.WriteLine("RULE 5: Dynamic Defers to Runtime");
            Console.WriteLine("  • 'dynamic' keyword delays overload resolution until runtime");
            Console.WriteLine("  • Runtime type of object determines which overload is called");
            Console.WriteLine("  • Useful for achieving runtime polymorphic overload selection");
            Console.WriteLine();
            
            Console.WriteLine("BEST PRACTICES:");
            Console.WriteLine("  ✓ Use specific types when you want specific overloads");
            Console.WriteLine("  ✓ Use base types when you want generic behavior");
            Console.WriteLine("  ✓ Cast explicitly when you need different overload behavior");
            Console.WriteLine("  ✓ Use 'dynamic' sparingly and only when you need runtime resolution");
            Console.WriteLine("  ✓ Be aware that overload resolution happens at compile time, not runtime");
        }
    }
}
