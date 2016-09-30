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
            Console.WriteLine(Contains("김소은", "김ㅅㅇ"));

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

        static bool Contains(string a, string b)
        {
            var d_a = Hangul.Disassemble(a);
            var d_b = Hangul.Disassemble(b);

            string[] uChars = d_a
                .Select(cc => cc.Chars[0].ToString())
                .Take(b.Length)
                .ToArray();

            

            return false;
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
