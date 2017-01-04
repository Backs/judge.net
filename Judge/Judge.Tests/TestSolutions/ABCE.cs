using System;

namespace Judge.Tests.TestSolutions
{
    class ABCE
    {
        public static void Main(string[] args)
        {
            var rows = Console.ReadLine().Split(' ') // compilation error
            Console.WriteLine(int.Parse(rows[0]) + int.Parse(rows[1]));
        }
    }
}
