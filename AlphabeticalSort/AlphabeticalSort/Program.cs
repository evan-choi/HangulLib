using HangulLib;
using HangulLib.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"E:\음악\보컬로이드 불러보았다";

            //string[] fs = Directory.GetFiles(path)
            //    .Select(f => Path.GetFileNameWithoutExtension(f))
            //    .ToArray();
            string[] fs = File.ReadAllLines("List.txt");

            var group = fs
                .GroupBy(f => GetHeader(f))
                .OrderBy(f => f.Key + (IsEng(f.Key) ? 55203 : 0))
                .ToArray();

            foreach (var g in group)
            {
                Console.WriteLine($"[{g.Key}]");

                foreach (var item in g.OrderBy(v => v))
                {
                    Console.WriteLine($"\t{item}");
                }
            }
        }

        static char GetHeader(string data)
        {
            var cc = Hangul.Disassemble(data[0]);

            if (IsEng(cc))
                return char.ToUpper(cc);

            if (!ComplexChar.IsSolo(cc))
                return cc[0];

            return '#';
        }

        static bool IsEng(char ch)
        {
            return (97 <= ch && ch <= 122) || 
                   (65 <= ch && ch <= 90);
        }
    }
}
