using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BackEndChallengeAnagram
{
    public class Anagram
    {
        public static bool FoundAllWords = false;
        public static string AnagramPhrase = "poultry outwits ants";
        public static int degreeOfParallelism = Environment.ProcessorCount / 2;
        public static List<string> SecretPhrases = new List<string>();
        public static List<string> TimeLog = new List<string>();
        public static List<List<int>> GeneratedPermutations = new List<List<int>>();
        private static char[] AlphabetsInAnagram;
        private static Dictionary<string, Dictionary<char, int>> FilteredWordList;
        private static int AnagramCharacterLength;
        private static Dictionary<char, int> AnagramChractersGroup;
        private static List<int> FilteredWordsIndex;
        private static int[] IntegerCharacterLengthList;
        private static double[] IntegerWordsList;
        private static int maxLevel = 4;
        private static double phraseValue;
        internal static string[] secretPhrasesHashValues = new string[3] { "e4820b45d2277f3844eac66c903e84be", "23170acc097c24edb98fc5488ab033fe", "665e5bcb0c20062fe8abaaf4628bb154" };
        private static string[] stringwordsList;
        private static Stopwatch timer = Stopwatch.StartNew();
        private static Dictionary<int, List<int>> WordsAllowedToFollow;
        public static void FetchUserChoices()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nDo you want to continue with default Anagram Phrase '" + AnagramPhrase + "'? (y/n)");
            Console.ForegroundColor = ConsoleColor.White;
            string key = Console.ReadKey().Key.ToString();
            Console.WriteLine("\n--------------------------");
            if (key.ToUpper() == "Y")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nOk, you can continue with the default Anagram Phrase '" + AnagramPhrase + "'.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nPlease Enter your Anagram Phrase");
                Console.ForegroundColor = ConsoleColor.White;
                AnagramPhrase = Console.ReadLine();
                Console.WriteLine("\n--------------------------");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nDo you want to continue to find the Anagram Phrase of these default MD5 Hash values \n'" + string.Join("'\n'", secretPhrasesHashValues) + "'     ? (y/n)");
            Console.ForegroundColor = ConsoleColor.White;
            string key2 = Console.ReadKey().Key.ToString();
            Console.WriteLine("\n--------------------------");
            if (key2.ToUpper() != "Y")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nPlease Enter your MD5 Hash Value\n");
                Console.ForegroundColor = ConsoleColor.White;
                secretPhrasesHashValues = new string[1];
                secretPhrasesHashValues[0] = Console.ReadLine();
                Console.WriteLine("\n--------------------------");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nOk, the process is starting to find phrases of \n'" + string.Join("'\n'", secretPhrasesHashValues) + "'.");
        }
        public static void FetchWordsCountChoice()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nBy default it will generate 2 to " + maxLevel + " words, Do you continue(y) or need to increase the count of words(n) ?");
            Console.ForegroundColor = ConsoleColor.White;
            string key3 = Console.ReadKey().Key.ToString();
            Console.WriteLine("\n--------------------------");
            if (key3.ToUpper() != "Y")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nPlease Enter maximum generating words\n");
                Console.ForegroundColor = ConsoleColor.White;
                maxLevel = int.Parse(Console.ReadLine());
                Console.WriteLine("\n--------------------------");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nOk, the process will generate 2 to " + maxLevel + " words to find phrases");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nGenerating Permutations to find the first phrase, Please Wait...");
        }
        public static void InitializeAnagram()
        {
            ////Step 1: Convert anagram phrase to character array.
            var anagramPhraseCharacters = AnagramPhrase.ToCharArray().ToList();
            ////Step 2: Remove spaces from the above character array. 
            var alphabetsCharacters = anagramPhraseCharacters.Where(x => x != ' ').ToArray();
            ////Step 3: Find the length of character array without spaces. 
            AnagramCharacterLength = alphabetsCharacters.Count();
            ////Step 4: Group the characters and store the count of each characters as the value.   
            AnagramChractersGroup = alphabetsCharacters.GroupBy(x => x).ToList().ToDictionary(x => x.FirstOrDefault(), x => x.Count());
            ////Step 5: Find the distinct characters of the phrase 
            AlphabetsInAnagram = alphabetsCharacters.Distinct().ToArray();
            ////Step 6: Fetch the word list from the file
            var wordList = File.ReadLines("../../wordlist.text").ToArray();
            ////Step 7: Assign an integer value for each distinct phrase characters
            var alphabetsList = AlphabetsInAnagram.Select((x, i) => new { Item = x, Index = i }).ToDictionary(x => x.Item, x => Math.Pow(10, x.Index));
            ////Step 8: Removed unwanted words from words list and group the characters and store the count of each characters as the value in all words.  
            FilteredWordList = wordList.Distinct().OrderByDescending(l => l.Length).ToList().ToDictionary(x => x, x => x.ToCharArray().Select(c => c).GroupBy(g => g).ToList().ToDictionary(d => d.FirstOrDefault(), d => d.Count())).Where(x => x.Key.Length <= AnagramCharacterLength && x.Value.All(y => AlphabetsInAnagram.Contains(y.Key)) && !x.Value.Any(z => z.Value > AnagramChractersGroup[z.Key])).ToDictionary(x => x.Key, x => x.Value);
            ////Step 9: Store words list as a list
            stringwordsList = FilteredWordList.Select(x => x.Key).ToArray();
            ////Step 10: Assign value as Step 7 to each words in the list(step 8)
            IntegerWordsList = FilteredWordList.Select(x => x.Value.Sum(y => alphabetsList[y.Key] * y.Value)).ToArray();
            ////Step 11: Store the character length of each words list(step 8)
            IntegerCharacterLengthList = FilteredWordList.Select(x => x.Key.Length).ToArray();
            ////Step 12: Calculate the integer value of phrase as Step 7
            phraseValue = AnagramChractersGroup.Sum(x => alphabetsList[x.Key] * x.Value);
            ////Step 13: Store all index values of words
            FilteredWordsIndex = IntegerWordsList.Select((x, i) => i).ToList();
            ////Step 14: Find all matching words of each words by checking the character limit of words 
            WordsAllowedToFollow = FilteredWordsIndex.ToDictionary(x => x, x => FilteredWordsIndex.Where(y => y >= x && isAllowThisWord(FilteredWordList[stringwordsList[x]], FilteredWordList[stringwordsList[y]])).ToList());
        }
        internal static void GeneratePermutationsAndFindPhrases()
        {
            timer = Stopwatch.StartNew();
            for (int i1 = 0; i1 < IntegerWordsList.Length; i1++)
            {
                RecursivePhraseFinder(1, new List<int>() { i1 }, IntegerWordsList[i1], i1, AnagramCharacterLength - IntegerCharacterLengthList[i1]);
            }
        }
        internal static void RecursivePhraseFinder(int level, List<int> usedwords, double currentphraseValue, int currenti, int newrequiredcharacterlength)
        {
            if (!FoundAllWords)
            {
                var newLevel = level + 1;
                if (currentphraseValue < phraseValue && newrequiredcharacterlength > 0)
                {
                    if (newLevel <= maxLevel)
                    {
                        var allowedWords = WordsAllowedToFollow[currenti];
                        Parallel.For(0, level - 1, i =>
                        {
                            allowedWords = allowedWords.Intersect(WordsAllowedToFollow[usedwords[i]]).ToList();
                        });
                        allowedWords = allowedWords.Where(x => IntegerCharacterLengthList[x] <= newrequiredcharacterlength && IntegerCharacterLengthList[x] + currentphraseValue <= phraseValue).ToList();
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
                                            var currentvalue = IntegerWordsList[i] + currentphraseValue;
                                            var usedWordsNow = new List<int>();
                                            usedWordsNow.AddRange(usedwords);
                                            usedWordsNow.Add(i);
                                            var requiredCharacterLength = newrequiredcharacterlength - IntegerCharacterLengthList[i];
                                            RecursivePhraseFinder(newLevel, usedWordsNow, currentvalue, i, requiredCharacterLength);
                                        });
                                });
                        }
                        Task.WaitAll(tasks);
                    }
                }
                else if (currentphraseValue == phraseValue && newrequiredcharacterlength == 0)
                {
                    GeneratePermutations(usedwords);
                }
            }
        }
        
        public static string ElapsedSeconds()
        {
            return decimal.Round(Convert.ToDecimal(timer.Elapsed.TotalSeconds), 2).ToString() + " Seconds";
        }
        private static string ElapsedMinutes()
        {
            return decimal.Round(Convert.ToDecimal(timer.Elapsed.TotalMinutes), 2).ToString() + " Minutes";
        }
        private static string CalculatePerSeconds(int count)
        {
            return Convert.ToInt32(count / timer.Elapsed.TotalSeconds).ToString() + " words per Second";
        }
        private static string CalculatePerMinutes(int count)
        {
            return Convert.ToInt32(count / timer.Elapsed.TotalMinutes).ToString() + " words per minutes"; ;
        }

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
            foreach (var x in result) if (x.Value > AnagramChractersGroup[x.Key]) return false;

            return true;
        }
        internal static IEnumerable<List<int>> Get(IEnumerable<int> set, IEnumerable<int> subset = null)
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

        private static int logIndex = 0;
        internal static void GeneratePermutations(List<int> usedWordsNow)
        {
            var foundencryptedkey = false;
            Parallel.ForEach(Get(usedWordsNow).ToList(), (usedWordsNew) =>
            {
                GeneratedPermutations.Add(usedWordsNew);
                var phrase = string.Join(" ", usedWordsNew.Select(x => stringwordsList[x]));
                var hashValue = Utilities.MD5Hash(phrase);
                if (secretPhrasesHashValues.Contains(hashValue))
                {
                    foundencryptedkey = true;
                    TimeLog.Add("Elapsed " + ElapsedSeconds());
                    SecretPhrases.Add(phrase);
                    if (SecretPhrases.Count() == secretPhrasesHashValues.Count())
                    {
                        FoundAllWords = true;
                    }
                }
            });
            if (foundencryptedkey || logIndex % 100 == 0)
            {
                PrintSecretPhrases(foundencryptedkey);
            }
            logIndex++;
        }

        private static void PrintSecretPhrases(bool foundencryptedkey)
        {
            var totalcount = GeneratedPermutations.Count;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("|");
            var indexkey = 0;
            if (foundencryptedkey)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nGenerated " + CalculatePerSeconds(totalcount) + " - (" + ElapsedSeconds() + ")\n" + CalculatePerMinutes(totalcount) + " - (" + ElapsedMinutes() + ")");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n" + string.Join("\n", SecretPhrases.Select(x => "The anagram phrase of '" + Utilities.MD5Hash(x) + "' is '" + x + "' - " + TimeLog[indexkey++])) + "\n");
                if (!FoundAllWords)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("\nGenerating Permutations to find the next phrase, Please Wait...");
                }
            }
        }
    }
}