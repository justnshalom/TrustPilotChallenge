using System;
using System.Diagnostics;
using System.Globalization;
using TrustPilot;

namespace Analysis
{
    // This program is used to calculate the average time of finding the solution for all 3 difficulty levels
    class Program
    {
        private static IFormatProvider culture = CultureInfo.CreateSpecificCulture("da");

        public static void Main(string[] args)
        {
            Cost.GetValidLetters();
            WordList.GetValidWords();
            WordList.GetNextWordIndices();

            Console.Write("Calculating average time to find easy secret phrase:");
            PrintAverageTime("e4820b45d2277f3844eac66c903e84be");

            Console.Write("Calculating average time to find more difficult secret phrase:");
            PrintAverageTime("23170acc097c24edb98fc5488ab033fe");

            Console.Write("Calculating average time to find hard secret phrase:");
            PrintAverageTime("665e5bcb0c20062fe8abaaf4628bb154");

            Console.ReadLine();
        }

        private static void PrintAverageTime(string hash)
        {
            Anagram.Hash = hash;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            
            for (int i = 0; i < 100; i++)
            {
                Anagram.FindAnagram();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine((timer.Elapsed.TotalSeconds / 100).ToString("F4", culture) + " seconds");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
