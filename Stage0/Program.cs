using System;

namespace Stage0;

internal class Program
{
    static void Main(string[] args)
    {
        Greeting("Yair", 30);
        Greeting("Anna", 25);
        Console.ReadLine();
    }

    private static void Greeting(string name, int grade)
    {
        Console.WriteLine($"Hello {name} age : {grade}");
    }
}