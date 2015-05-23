using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemporaryTest
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine(IdGenerator.NewId());
            }

            Console.Read();
        }
    }
}
