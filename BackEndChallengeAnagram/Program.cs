namespace BackEndChallengeAnagram
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    internal class Program
    {
        public static bool FoundAllWords = false;

        public static List<string> List = new List<string>();
        public static List<string> ListTimeLog = new List<string>();

        public static List<List<int>> permutations = new List<List<int>>();

        private static char[] allAlphabetsInAnagram;

        private static Dictionary<string, Dictionary<char, int>> allowedWordList;

        private static int anagramLength;

        private static int characterLength;

        private static Dictionary<char, int> chracterGroup;

        private static readonly IFormatProvider culture = CultureInfo.CreateSpecificCulture("en");
        private static List<int> allwordsintegertoallow;
        private static int[] integercharacterLengthList;

        private static double[] integerwordsList;

        private static readonly int maxLevel = 4;

        private static double phraseValue;

        private static readonly string[] secretphases = new string[3] { "e4820b45d2277f3844eac66c903e84be", "23170acc097c24edb98fc5488ab033fe", "665e5bcb0c20062fe8abaaf4628bb154" };

        private static int[] singleCharactersStarting;

        private static string[] stringwordsList;

        private static Stopwatch timer = Stopwatch.StartNew();

        private static Dictionary<int, List<int>> WordsAllowedToFollow;

        private static int wordsLength;

        public static int FirstIndexMatch(string x)
        {
            var index = 0;
            foreach (var item in stringwordsList)
            {
                if (item.StartsWith(x)) return index;
                index++;
            }

            return -1;
        }

        private static bool isAllowThisWord(Dictionary<char, int> firstArray, Dictionary<char, int> secondArray)
        {
            var result = new Dictionary<char, int>(firstArray);
            secondArray.ToList()
                .ForEach(
                    x =>
                        {
                            if (result.ContainsKey(x.Key)) result[x.Key] += x.Value;
                            else result.Add(x.Key, x.Value);
                        });
            foreach (var x in result) if (x.Value > chracterGroup[x.Key]) return false;

            return true;
        }

        private static void Main(string[] args)
        {
            timer = Stopwatch.StartNew();
            var anagramPhrase = "poultry outwits ants";
            var anagramPhraseCharacters = anagramPhrase.ToCharArray().ToList();
            anagramLength = anagramPhraseCharacters.Count();
            var alphabetsCharacters = anagramPhraseCharacters.Where(x => x != ' ').ToArray();
            characterLength = alphabetsCharacters.Count();
            wordsLength = anagramPhrase.Split(' ').Length;
            chracterGroup = alphabetsCharacters.GroupBy(x => x).ToList().ToDictionary(x => x.FirstOrDefault(), x => x.Count());
            allAlphabetsInAnagram = anagramPhraseCharacters.Where(x => x != ' ').Distinct().ToArray();
            var wordList = File.ReadLines("../../wordlist.text").ToArray();
            var expectedStringLength = characterLength;
            var alphabetsList = allAlphabetsInAnagram.Select((x, i) => new { Item = x, Index = i }).ToDictionary(x => x.Item, x => Math.Pow(10, x.Index));
            allowedWordList = wordList.Distinct().OrderByDescending(l => l.Length).ToList().ToDictionary(x => x, x => x.ToCharArray().Select(c => c).GroupBy(g => g).ToList().ToDictionary(d => d.FirstOrDefault(), d => d.Count())).Where(x => x.Key.Length <= expectedStringLength && x.Value.All(y => allAlphabetsInAnagram.Contains(y.Key)) && !x.Value.Any(z => z.Value > chracterGroup[z.Key])).ToDictionary(x => x.Key, x => x.Value);
            stringwordsList = allowedWordList.Select(x => x.Key).ToArray();
            integerwordsList = allowedWordList.Select(x => x.Value.Sum(y => alphabetsList[y.Key] * y.Value)).ToArray();
            integercharacterLengthList = allowedWordList.Select(x => x.Key.Length).ToArray();
            phraseValue = chracterGroup.Sum(x => alphabetsList[x.Key] * x.Value);
            singleCharactersStarting = allAlphabetsInAnagram.OrderBy(x => x).Select(x => FirstIndexMatch(x.ToString())).ToArray();
            allwordsintegertoallow = integerwordsList.Select((x, i) => i).ToList();
            WordsAllowedToFollow = allwordsintegertoallow.ToDictionary(x => x, x => allwordsintegertoallow.Where(y => y >= x && isAllowThisWord(allowedWordList[stringwordsList[x]], allowedWordList[stringwordsList[y]])).ToList());
            

            for (int i1 = 0; i1 < integerwordsList.Length; i1++)
            {
                RecursivePhraseFinder(1, new List<int>() { i1 }, integerwordsList[i1], i1, characterLength - integercharacterLengthList[i1]);
            }

            Console.WriteLine("\nPress any key to exit.");
            Console.Write("\nENd in " + timer.Elapsed.TotalSeconds.ToString("F4", culture) + " seconds");
            Console.ReadLine();
        }
        public static IEnumerable<List<int>> Get(IEnumerable<int> set, IEnumerable<int> subset = null)
        {
            if (subset == null) subset = new List<int>();
            if (!set.Any()) yield return subset.ToList();

            for (var i = 0; i < set.Count(); i++)
            {
                var newSubset = set.Take(i).Concat(set.Skip(i + 1));
                foreach (var permutation in Get(newSubset, subset.Concat(set.Skip(i).Take(1))))
                {
                    yield return permutation.ToList();
                }
            }
        }

        private static void RecursivePhraseFinder(int level, List<int> usedwords, double currentphraseValue, int currenti, int newrequiredcharacterlength)
        {
            if (!FoundAllWords)
            {
                var newLevel = level + 1;
                if (newLevel <= maxLevel)
                {
                    var degreeOfParallelism = Environment.ProcessorCount / 2;

                    var allowedWords = (currenti != -1) ? WordsAllowedToFollow[currenti] : allwordsintegertoallow;
                    Parallel.For(0, level - 1, i =>
                    {
                        allowedWords = allowedWords.Intersect(WordsAllowedToFollow[usedwords[i]]).ToList();
                    });
                    allowedWords = allowedWords.Where(x => integercharacterLengthList[x] <= newrequiredcharacterlength).ToList();
                    var tasks = new Task[degreeOfParallelism];

                    for (var taskNumber = 0; taskNumber < degreeOfParallelism; taskNumber++)
                    {
                        var taskNumberCopy = taskNumber;
                        tasks[taskNumber] = Task.Factory.StartNew(
                            () =>
                                {
                                    var max = allowedWords.Count * (taskNumberCopy + 1) / degreeOfParallelism;
                                    var startsfrom = allowedWords.Count * taskNumberCopy / degreeOfParallelism;
                                    Parallel.For(
                                        startsfrom,
                                        max,
                                        i1 =>
                                            {
                                                var i = allowedWords[i1];
                                                var currentvalue = integerwordsList[i] + currentphraseValue;
                                                var currentcharacterLength = integercharacterLengthList[i];
                                                if (currentcharacterLength <= newrequiredcharacterlength && currentvalue <= phraseValue)
                                                {
                                                    var usedWordsNow = new List<int>();
                                                    usedWordsNow.AddRange(usedwords);
                                                    usedWordsNow.Add(i);
                                                    var requiredCharacterLength = newrequiredcharacterlength - integercharacterLengthList[i];
                                                    if (requiredCharacterLength > 0)
                                                    {
                                                        RecursivePhraseFinder(newLevel, usedWordsNow, currentvalue, i, requiredCharacterLength);
                                                    }
                                                    else
                                                        if (currentvalue == phraseValue && requiredCharacterLength == 0)
                                                    {
                                                        Parallel.ForEach(Get(usedWordsNow).ToList(), (usedWordsNew) =>
                                                        {
                                                            var phrase = string.Join(" ", usedWordsNew.Select(x => stringwordsList[x]));
                                                            var encryptedString = Utilities.MD5Hash(phrase);
                                                            var foundencryptedkey = false;
                                                            if (secretphases.Contains(encryptedString))
                                                            {
                                                                foundencryptedkey = true;
                                                                ListTimeLog.Add("Found in "+Convert.ToInt32(timer.Elapsed.TotalSeconds).ToString()+" seconds");
                                                                List.Add(phrase);
                                                                if (List.Count() == secretphases.Count())
                                                                {
                                                                    FoundAllWords = true;
                                                                }
                                                            }
                                                            permutations.Add(usedWordsNew);
                                                            if (foundencryptedkey||permutations.Count % 5000 == 0)
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine(Convert.ToInt32(permutations.Count / timer.Elapsed.TotalSeconds)+ " words per seconds(" + Convert.ToInt32(timer.Elapsed.TotalSeconds) + ")\n" + Convert.ToInt32(permutations.Count / timer.Elapsed.TotalMinutes) + " words per minutes(" + Convert.ToInt32(timer.Elapsed.TotalMinutes) + ")");
                                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                                var indexkey = 0;
                                                                    Console.WriteLine("\n" + string.Join("\n", List.Select(x => x+" -> "+ Utilities.MD5Hash(x)+" - "+ ListTimeLog[indexkey++])));
                                                                    Console.ResetColor();
                                                            }
                                                        });
                                                    }
                                                }
                                            });
                                });
                    }

                    Task.WaitAll(tasks);
                }


            }

        }
    }
}