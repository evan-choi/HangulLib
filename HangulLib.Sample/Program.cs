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
            Console.WriteLine(Contains("규환", "규호"));
            Console.WriteLine(Contains("규환", "규화"));
            Console.WriteLine(Contains("규환", "규환"));
            Console.WriteLine(Contains("규환", "규환!"));
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
            if (a.Length < b.Length)
                return false;

            var c_a = Hangul.Disassemble(a, false).ToArray();
            var c_b = Hangul.Disassemble(b, false).ToArray();

            char[] cho_a = c_a.Select(cc => ((cc.Chars.Count() > 0 ? (char)cc[0] : cc.Completion))).ToArray();
            char[] cho_b = c_b.Select(cc => ((cc.Chars.Count() > 0 ? (char)cc[0] : cc.Completion))).ToArray();

            char[] jung_a = c_a.Select(cc => ((cc.Chars.Count() == 2 ? (char)cc[1] : cc.Completion))).ToArray();
            char[] jung_b = c_b.Select(cc => ((cc.Chars.Count() == 2 ? (char)cc[1] : cc.Completion))).ToArray();

            int index = -1;

            while ((index = Array.IndexOf(cho_a, cho_b[0], index + 1)) != -1)
            {
                bool result = true;

                for (int i = 0; i < b.Length; i++)
                {
                    int a_i = index + i;

                    if (a_i < a.Length)
                    {
                        char t_a = a[a_i];
                        char t_b = b[i];

                        if (Hangul.IsChosung(b[i]))
                        {
                            t_a = cho_a[a_i];
                        }

                        if (Hangul.IsJungsung(jung_b[i]))
                        {
                            var aChars = c_a[a_i].Chars.Take(2).ToArray();
                            var bChars = c_b[i].Chars.Take(2).ToArray();

                            if (bChars.Length > 1 && bChars[1].Chars.Length == 0 &&
                                aChars.Length > 1 && aChars[1].Chars.Length > 0)
                                aChars[1] = aChars[1][0];

                            t_a = Hangul.Assemble(new ComplexChar[] { aChars })[0];
                            t_b = Hangul.Assemble(new ComplexChar[] { bChars })[0];
                        }

                        if (t_a != t_b)
                        {
                            result = false;
                            break;
                        }
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                }

                if (result)
                    return true;
            }

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
