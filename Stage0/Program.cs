using System;

namespace Stage0;

internal class Program
{
    static void Main(string[] args)
    {
        //Student student = new Student("Yair", 90);
        //Student student2 = new Student("Anna", 85);
        //Student student3 = new Student("Bob", 78);

        List<Student> students = new List<Student> { 
            new Student { Id = 1, Name = "Yair", Grade = 90 },
            new Student { Name = "Anna", Grade = 85 },
            new Student { Id = 3, Name = "Bob", Grade = 78 },
            new Student { Id = 4, Name = "Cathy", Grade = 92 },
            new Student { Id = 5, Name = "David", Grade = 88 }
        };

        students.Sort();

        //IEnumerable<Student> topStudents = students.FindAll(student => student.Grade >= 85);
        // IEnumerable<Student> orderedTopStudents = topStudents.OrderByDescending(student => student.Grade);
        IEnumerable<Student> topStudents = students.Where(s => s.Grade >= 85).OrderByDescending(s => s.Grade);//.Take(3);


        foreach (var student in students)
        {
            Console.WriteLine(student);
        }

        var lecturer = new { ID = 29, Name = "Dani" };


        Greeting("Yair", 30);
        Greeting("Anna", 25);
        Console.ReadLine();
    }

    private static void Greeting(string name, int grade)
    {
        Console.WriteLine($"Hello {name} grade : {grade}");
    }
}