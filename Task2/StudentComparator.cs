using System;
using System.Collections.Generic;

namespace Task2
{
    public class StudentComparator : IEqualityComparer<Student>
    {
        public bool Equals(Student m, Student n)
        {
            return StringComparer
                .InvariantCultureIgnoreCase
                .Equals($"{m.index} {m.name} {m.surname}",
                    $"{n.index} {n.name} {n.surname}");
        }
        

        public int GetHashCode(Student x)
        {
            // throw new System.NotImplementedException();
            return StringComparer
                .CurrentCultureIgnoreCase
                .GetHashCode(
                    $"{x.index} {x.name} {x.surname}");
        }
    }
}