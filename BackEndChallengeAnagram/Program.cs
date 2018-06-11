using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackEndChallengeAnagram
{
    class Program
    {
        private static Dictionary<string, Dictionary<char,int >> allowedWordList;
        private static int wordsLength;
        private static int anagramLength;
        private static int characterLength;
        private static int maxLevel=3;
        private static double phraseValue;
        private static Dictionary<char,int> chracterGroup;
        private static List<double> integerwordsList;
        private static List<string> stringwordsList;
        private static Stopwatch timer = Stopwatch.StartNew();
        private static IFormatProvider culture = CultureInfo.CreateSpecificCulture("en");
        static void Main(string[] args)
        {
            var anagramPhrase = "poultry outwits ants";
            ////anagramPhrase = "printout stout yawls";
            var encryptedString = Utilities.MD5Hash(anagramPhrase);
            var anagramPhraseCharacters = anagramPhrase.ToCharArray().ToList();
            anagramLength = anagramPhraseCharacters.Count();
            var alphabetsCharacters = anagramPhraseCharacters.Where(x => x != ' ');
            characterLength = alphabetsCharacters.Count();
            wordsLength = anagramPhrase.Split(' ').Length;
            chracterGroup = alphabetsCharacters.GroupBy(x => x).ToList().ToDictionary(x => x.FirstOrDefault(), x => x.Count());
            var allAlphabetsInAnagram = anagramPhraseCharacters.Where(x => x != ' ').Distinct().ToList();
            ////var mutationstring = GetPermutations(anagramPhrase);
            var wordList= File.ReadLines("../../wordlist.text").ToArray();
            var expectedStringLength = anagramLength - ((wordsLength - 1) * 2);
            
            var alphabetsList = allAlphabetsInAnagram.Select((x, i) => new { Item = x, Index = i }).ToDictionary(x => x.Item, x => (Math.Pow(10,x.Index)));

            allowedWordList = wordList.Distinct().OrderByDescending(l => l.Length).ToList().ToDictionary(x => x, x => x.ToCharArray().Select(c => c).GroupBy(g => g).ToList().ToDictionary(d => d.FirstOrDefault(), d => d.Count())).Where(x => x.Key.Length <= expectedStringLength && !x.Value.Any(y => !allAlphabetsInAnagram.Contains(y.Key)) && !x.Value.Any(z => z.Value > chracterGroup[z.Key])).ToDictionary(x=>x.Key,x=>x.Value);
            stringwordsList= allowedWordList.Select(x => x.Key).ToList();
            integerwordsList = allowedWordList.Select(x => x.Value.Sum(y => alphabetsList[y.Key] * y.Value)).ToList();

            phraseValue = chracterGroup.Sum(x => alphabetsList[x.Key] * x.Value);

            ////var validwords=integerwordsList.SelectMany(x => x == phraseValue?x: x < phraseValue?integerwordsList.Where(y => x+y< phraseValue && &integerwordsList.Where(z => x + z < phraseValue);

            timer = Stopwatch.StartNew();
            ////RecursivePhraseFinder(string.Empty, 0);
            Console.WriteLine("started finding first one");



            RecursivePhraseFinder(0, new List<int>(),0);
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadLine();

        }

        private static List<string> secretphases = new List<string> { "e4820b45d2277f3844eac66c903e84be", "23170acc097c24edb98fc5488ab033fe", "665e5bcb0c20062fe8abaaf4628bb154" };
        public static bool FoundAllWords = false;
        public static List<string> List=new List<string>();

        public static List<List<int>> permutations = new List<List<int>>();

        private static void RecursivePhraseFinder(int level, List<int> usedwords = null, double currentphraseValue = 0, int currenti = 0)
        {
            if (!FoundAllWords)
            {
                if (currentphraseValue < phraseValue)
                {
                    var newLevel = level + 1;
                    if (newLevel <= maxLevel)
                    {
                        var degreeOfParallelism = Environment.ProcessorCount / 2;

                        var tasks = new Task[degreeOfParallelism];
                        
                        for (int taskNumber = 0; taskNumber < degreeOfParallelism; taskNumber++)
                        {
                            int taskNumberCopy = taskNumber;
                            tasks[taskNumber] = Task.Factory.StartNew(
                                () =>
                                    {
                                       
                                    var max = integerwordsList.Count * (taskNumberCopy + 1) / degreeOfParallelism;
                                        var startsfrom = (integerwordsList.Count * taskNumberCopy / degreeOfParallelism);
                                        if (currenti > startsfrom)
                                        {
                                            startsfrom = currenti;
                                        }
                                    for (int i = startsfrom;
                                        i < max;
                                        i++)
                                    {  
                                        var currentvalue = integerwordsList[i] + currentphraseValue;
                                        if (currentvalue <= phraseValue)
                                        {
                                            var usedWordsNow = new List<int>();
                                            usedWordsNow.AddRange(usedwords);
                                            usedWordsNow.Add(i);
                                            if (usedwords.Contains(1639) && usedwords.Contains(1639)&& i == 127)
                                            {

                                            }
                                            
                                                RecursivePhraseFinder(newLevel, usedWordsNow, currentvalue,i);

                                        }

                                    }
                                });
                        }

                        Task.WaitAll(tasks);
                    }
                }
                else if (usedwords.Any() && currentphraseValue == phraseValue)
                {
                    var phrase = string.Join(" ", usedwords.Select(x => stringwordsList[x]));
                    var encryptedString = Utilities.MD5Hash(phrase);
                    ////Console.WriteLine("\n" + phrase);
                    if (secretphases.Contains(encryptedString))
                    {
                        Console.Write("\nFound " + encryptedString + " of " + phrase + " in " + timer.Elapsed.TotalSeconds.ToString("F4", culture) + " seconds");
                        List.Add(phrase);
                        if (List.Count() < secretphases.Count())
                        {
                            Console.WriteLine("\nstarted finding next one");
                        }
                        else
                        {
                            Console.WriteLine("\nFounded All phases");
                            Console.WriteLine("\nPress any key to exit.");
                            Console.ReadLine();
                        }
                    }
                }
            }

        }
        ////private static void RecursivePhraseFinder(int level, List<int> usedwords = null, double currentphraseValue = 0)
        ////{
        ////    if (!FoundAllWords)
        ////    {
        ////        if (currentphraseValue < phraseValue)
        ////        {
        ////            var newLevel = level + 1;
        ////            if (newLevel <= maxLevel)
        ////            {
        ////                var degreeOfParallelism = Environment.ProcessorCount / 2;

        ////                var tasks = new Task[degreeOfParallelism];

        ////                for (int taskNumber = 0; taskNumber < degreeOfParallelism; taskNumber++)
        ////                {
        ////                    int taskNumberCopy = taskNumber;

        ////                    tasks[taskNumber] = Task.Factory.StartNew(
        ////                        () =>
        ////                        {
        ////                            var max = integerwordsList.Count * (taskNumberCopy + 1) / degreeOfParallelism;
        ////                            for (int i = integerwordsList.Count * taskNumberCopy / degreeOfParallelism;
        ////                                i < max;
        ////                                i++)
        ////                            {
        ////                                var currentvalue = integerwordsList[i] + currentphraseValue;
        ////                                if (currentvalue <= phraseValue)
        ////                                {
        ////                                    var usedWordsNow = new List<int>();
        ////                                    usedWordsNow.AddRange(usedwords);
        ////                                    usedWordsNow.Add(i);
        ////                                    if (usedwords.Contains(1639)&&i==127)
        ////                                    {

        ////                                    }
        ////                                        RecursivePhraseFinder(newLevel, usedWordsNow, currentvalue);
                                            
        ////                                }

        ////                            }
        ////                        });
        ////                }

        ////                Task.WaitAll(tasks);
        ////            }
        ////        }
        ////        else if (usedwords.Any() && currentphraseValue == phraseValue)
        ////        {
        ////            var phrase = string.Join(" ", usedwords.Select(x => stringwordsList[x]));
        ////            var encryptedString = Utilities.MD5Hash(phrase);
        ////            if (secretphases.Contains(encryptedString))
        ////            {
        ////                Console.Write("\nFound " + encryptedString + " of " + phrase + " in " + timer.Elapsed.TotalSeconds.ToString("F4", culture) + " seconds");
        ////                List.Add(phrase);
        ////                if (List.Count() < secretphases.Count())
        ////                {
        ////                    Console.WriteLine("started finding next one");
        ////                }
        ////                else
        ////                {
        ////                    Console.WriteLine("Founded All phases");
        ////                    Console.WriteLine("\nPress any key to exit.");
        ////                    Console.ReadLine();
        ////                }
        ////            }
        ////        }
        ////    }
            
        ////}
    }
}
