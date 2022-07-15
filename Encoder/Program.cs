using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encoder
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string s = Console.ReadLine();
                StreamWriter sw = new StreamWriter("log.txt", true);
                
                string s2 = DoubleASCII(s);
                Console.WriteLine(s2);
                sw.WriteLine(s2);

                s2 = HalfASCII(s);
                Console.WriteLine(s2);
                sw.WriteLine(s2);

                sw.WriteLine();
                sw.Close();
            }
        }

        //works best with range [a,z]
        static string DoubleASCII(string s)
        {
            string s2 = "";
            foreach (char c in s)
                s2 += (char)(c * 2);
            return s2;
        }

        //works best with range [a,z]
        static string HalfASCII(string s)
        {
            string s2 = "";
            foreach (char c in s)
                s2 += (char)(c / 2);
            return s2;
        }
    }
}
