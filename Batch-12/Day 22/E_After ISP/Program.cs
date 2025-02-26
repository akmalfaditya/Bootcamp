using System;

public interface IPrinter
{
    void Print(string document);
}

public interface IScanner
{
    void Scan(string document);
}

public interface IFax
{
    void Fax(string document);
}

public class AllInOnePrinter : IPrinter, IScanner, IFax
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

public class SimplePrinter : IPrinter
{
    public void Print(string document)
    {
        Console.WriteLine("Printing: " + document);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        AllInOnePrinter allInOnePrinter = new AllInOnePrinter();
        allInOnePrinter.Print("Document 1");
        allInOnePrinter.Scan("Document 1");
        allInOnePrinter.Fax("Document 1");

        SimplePrinter simplePrinter = new SimplePrinter();
        simplePrinter.Print("Document 2");
    }
}
