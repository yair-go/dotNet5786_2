using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stage0
{
    internal class Student : IComparable<Student>
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public int Grade { get; set; }

        public int CompareTo(Student? other)
        {
            return this.Name.CompareTo(other?.Name);
        }

        override public string ToString()
        {
            return $"Student Id: {Id}, Name: {Name}, Grade: {Grade}";
        }
    }
}
