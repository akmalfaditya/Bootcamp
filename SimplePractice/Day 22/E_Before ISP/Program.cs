using System;

public interface IDevice
{
    void Print(string document);
    void Scan(string document);
    void Fax(string document);
}

public class AllInOnePrinter : IDevice
{
    public void Print(string document)
    {
        Console.WriteLine("Printing: " + document);
    }

    public void Scan(string document)
    {
        Console.WriteLine("Scanning: " + document);
    }

    public void Fax(string document)
    {
        Console.WriteLine("Faxing: " + document);
    }
}

public class SimplePrinter : IDevice
{
    public void Print(string document)
    {
        Console.WriteLine("Printing: " + document);
    }

    public void Scan(string document)
    {
        throw new NotSupportedException("SimplePrinter does not support scanning");
    }

    public void Fax(string document)
    {
        throw new NotSupportedException("SimplePrinter does not support faxing");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        IDevice printer = new AllInOnePrinter();
        printer.Print("Document 1");
        printer.Scan("Document 1");
        printer.Fax("Document 1");

        printer = new SimplePrinter();
        printer.Print("Document 2");
        // printer.Scan("Document 2"); // NotSupportedException
        // printer.Fax("Document 2"); // NotSupportedException
    }
}
