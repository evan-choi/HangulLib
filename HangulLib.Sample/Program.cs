using HangulLib.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HangulLib.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Data: ");
                string data = Console.ReadLine();

                foreach (ComplexChar cc in Hangul.Disassemble(data))
                {
                    // ToString2 - 디버깅용
                    Console.WriteLine("Disassemble: " + cc.ToString2());
                    Console.WriteLine("Assemble: " + Hangul.Assemble(cc));
                }

                Console.WriteLine();
            }
        }

        static void Benchmark(string data)
        {
            int test = 100000;
            List<double> lst = new List<double>();

            for (int i = 0; i < test; i++)
            {
                var sw = new Stopwatch();

                sw.Start();
                var arr = Hangul.Disassemble(data).ToArray();
                sw.Stop();

                lst.Add(sw.ElapsedTicks);
            }

            Console.WriteLine($"Test {test} - {lst.Sum() / test}/ticks");
        }
    }
}
