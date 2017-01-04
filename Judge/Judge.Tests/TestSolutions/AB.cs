using System;

namespace Judge.Tests.TestSolutions
{
    class AB
    {
        public static void Main(string[] args)
        {
            var rows = Console.ReadLine().Split(' ');
            Console.WriteLine(int.Parse(rows[0]) + int.Parse(rows[1]));
        }
    }
}
