using System;
using System.Diagnostics;
using System.Globalization;

namespace TrustPilot
{
    public struct PermutationState
    {
        public int CurrentWordIndex;
        public ulong Value;
        public bool[] ValidIndices;
    }

    public class Program
    {
        private static Stopwatch timer = Stopwatch.StartNew();
        private static IFormatProvider culture = CultureInfo.CreateSpecificCulture("da");

        public static void Main(string[] args)
        {
            timer = Stopwatch.StartNew();

            Console.Write("Analyzing list of words...");
            Cost.GetValidLetters();
            WordList.ReadWords();
            WordList.GetValidWords();
            WordList.GetNextWordIndices();
            Console.WriteLine("Done! (" + timer.ElapsedMilliseconds + " milliseconds)\n");
            
            Console.WriteLine("Select option:");
            Console.WriteLine("1. Find easy secret phrase");
            Console.WriteLine("2. Find \"more difficult\" secret phrase");
            Console.WriteLine("3. Find hard secret phrase");
            Console.WriteLine("4. Exit");

            while (string.IsNullOrWhiteSpace(Anagram.Hash))
            {
                int option;
                if (int.TryParse(Console.ReadKey().KeyChar.ToString(), out option))
                {
                    switch (option)
                    {
                        case 1:
                            Anagram.Hash = "e4820b45d2277f3844eac66c903e84be";
                            Console.WriteLine("\n\nGenerating permutations of words...");
                            break;
                        case 2:
                            Anagram.Hash = "23170acc097c24edb98fc5488ab033fe";
                            Console.WriteLine("\n\nGenerating permutations of words...");
                            break;
                        case 3:
                            Anagram.Hash = "665e5bcb0c20062fe8abaaf4628bb154";
                            Console.WriteLine("\n\nGenerating permutations of words. This takes around 8 seconds on i7-6700K. Please wait...");
                            break;
                        case 4:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("\nInvalid input. Please type 1, 2, 3 or 4:");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\nInvalid input. Please type 1, 2, 3 or 4:");
                }
            }

            timer = Stopwatch.StartNew();            
            string solution = Anagram.FindAnagram(); // here is the actual algorithm
            timer.Stop();

            Console.WriteLine("\nGenerated " + Anagram.NumberOfPermutations.ToString("##,#", culture) + " permutations in " + timer.Elapsed.TotalSeconds.ToString("F4",culture) + " seconds");
            Console.Write("Found solution: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(solution);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadLine();
        }
    }
}