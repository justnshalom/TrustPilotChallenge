// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Anagram.cs" company="TrustPilot">
//   No CopyRight
// </copyright>
// <summary>
//  The library to find secret phrases
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BackEndChallengeAnagram
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>The library to find secret phrases.</summary>
    public class Anagram
    {
        /// <summary>The processor division.</summary>
        private static readonly int ProcessorDivision;

        /// <summary>The filtered word list.</summary>
        private static Dictionary<string, Dictionary<char, int>> filteredWordList;

        /// <summary>The anagram phrase.</summary>
        private static string anagramPhrase = "poultry outwits ants";

        /// <summary>The time log of each founded secret phrases.</summary>
        private static List<string> timeLog;

        /// <summary>The log index.</summary>
        private static int logIndex;

        /// <summary>Gets or sets the generated permutations.</summary>
        private static List<List<int>> generatedPermutations;

        /// <summary>The founded secret phrases.</summary>
        private static List<string> secretPhrases;

        /// <summary>The alphabets in anagram.</summary>
        private static char[] alphabetsInAnagram;

        /// <summary>The filtered words index.</summary>
        private static List<int> filteredWordsIndex;

        /// <summary>The integer character length list.</summary>
        private static int[] integerCharacterLengthList;

        /// <summary>The integer words list.</summary>
        private static double[] integerWordsList;

        /// <summary>The phrase value.</summary>
        private static double phraseValue;

        /// <summary>The secret phrases hash values.</summary>
        private static string[] secretPhrasesHashValues = new string[3] { "e4820b45d2277f3844eac66c903e84be", "23170acc097c24edb98fc5488ab033fe", "665e5bcb0c20062fe8abaaf4628bb154" };

        /// <summary>The string words list.</summary>
        private static string[] stringWordsList;

        /// <summary>The timer.</summary>
        private static Stopwatch timer = Stopwatch.StartNew();

        /// <summary>The words allowed to follow.</summary>
        private static Dictionary<int, List<int>> wordsAllowedToFollow;

        /// <summary>The anagram character length.</summary>
        private static int anagramCharacterLength;

        /// <summary>The anagram characters group.</summary>
        private static Dictionary<char, int> anagramCharactersGroup;

        /// <summary>Initializes static members of the <see cref="Anagram"/> class.</summary>
        static Anagram()
        {
            ProcessorDivision = Environment.ProcessorCount / 2;
            MaxWordCount = 4;
        }

        /// <summary>Gets or sets the maximum words count allowed to generate permutations.</summary>
        internal static int MaxWordCount { get; set; }

        /// <summary>Gets or sets a value indicating whether is found all words.</summary>
        internal static bool IsFoundAllSecretPhrases { get; set; }

        /// <summary>Fetch user choices.</summary>
        public static void FetchUserChoices()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nDo you want to continue with default Anagram Phrase '" + anagramPhrase + "'? (y/n)");
            Console.ForegroundColor = ConsoleColor.White;
            string key = Console.ReadKey().Key.ToString();
            Console.WriteLine("\n--------------------------");
            if (key.ToUpper() == "Y")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nOk, you can continue with the default Anagram Phrase '" + anagramPhrase + "'.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nPlease Enter your Anagram Phrase");
                Console.ForegroundColor = ConsoleColor.White;
                anagramPhrase = Console.ReadLine();
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

        /// <summary>Initialize to find secret phrases.</summary>
        public static void InitializeAnagram()
        {
            //// Step 2.1: Convert anagram phrase to character array.
            var anagramPhraseCharacters = anagramPhrase.ToCharArray().ToList();
            //// Step 2.2: Remove spaces from the above character array. 
            var alphabetsCharacters = anagramPhraseCharacters.Where(x => x != ' ').ToArray();
            //// Step 2.3: Find the length of character array without spaces. 
            anagramCharacterLength = alphabetsCharacters.Count();
            //// Step 2.4: Group the characters and store the count of each character as the value.   
            anagramCharactersGroup = alphabetsCharacters.GroupBy(x => x).ToList().ToDictionary(x => x.FirstOrDefault(), x => x.Count());
            //// Step 2.5: Find the distinct characters of the phrase .
            alphabetsInAnagram = alphabetsCharacters.Distinct().ToArray();
            //// Step 2.6: Fetch the word list from the file.
            var wordList = File.ReadLines("../../wordList.text").ToArray();
            //// Step 2.7: Assign an integer value for each distinct phrase characters.
            var alphabetsList = alphabetsInAnagram.Select((x, i) => new { Item = x, Index = i }).ToDictionary(x => x.Item, x => Math.Pow(10, x.Index));
            //// Step 2.8: Remove unwanted words from words list, group the characters and store the count of each character as the value, in all words.  
            filteredWordList = wordList.Distinct().OrderByDescending(l => l.Length).ToList().ToDictionary(x => x, x => x.ToCharArray().Select(c => c).GroupBy(g => g).ToList().ToDictionary(d => d.FirstOrDefault(), d => d.Count())).Where(x => x.Key.Length <= anagramCharacterLength && x.Value.All(y => alphabetsInAnagram.Contains(y.Key)) && !x.Value.Any(z => z.Value > anagramCharactersGroup[z.Key])).ToDictionary(x => x.Key, x => x.Value);
            //// Step 2.9: Store words list as a list.
            stringWordsList = filteredWordList.Select(x => x.Key).ToArray();
            //// Step 2.10: Assign value to each words in the list as per the integer value assigned in Step 2.7.
            integerWordsList = filteredWordList.Select(x => x.Value.Sum(y => alphabetsList[y.Key] * y.Value)).ToArray();
            //// Step 2.11: Store the character length of each words list(step 2.8).
            integerCharacterLengthList = filteredWordList.Select(x => x.Key.Length).ToArray();
            //// Step 2.12: Calculate the integer value of phrase as per the integer value assigned in Step 2.7.
            phraseValue = anagramCharactersGroup.Sum(x => alphabetsList[x.Key] * x.Value);
            //// Step 2.13: Store all index values of words.
            filteredWordsIndex = integerWordsList.Select((x, i) => i).ToList();
            //// Step 2.14: Find all matching words of each word by checking the character limit of words.
            wordsAllowedToFollow = filteredWordsIndex.ToDictionary(x => x, x => filteredWordsIndex.Where(y => y >= x && IsAllowThisWord(filteredWordList[stringWordsList[x]], filteredWordList[stringWordsList[y]])).ToList());
        }

        /// <summary>The action to fetch words count choice.</summary>
        public static void FetchWordsCountChoice()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nBy default it will generate 2 to " + MaxWordCount + " words, Do you want to continue(y) or would you like to increase the count of words(n) ?");
            Console.ForegroundColor = ConsoleColor.White;
            string key3 = Console.ReadKey().Key.ToString();
            Console.WriteLine("\n--------------------------");
            if (key3.ToUpper() != "Y")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nPlease Enter maximum generating words\n");
                Console.ForegroundColor = ConsoleColor.White;
                var wordCountByUser = Console.ReadLine();
                int countArgument = 0;
                if (wordCountByUser != null && int.TryParse(wordCountByUser, out countArgument))
                {
                    MaxWordCount = countArgument;
                }

                Console.WriteLine("\n--------------------------");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nOk, the process will generate 2 to " + MaxWordCount + " words to find phrases");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nGenerating Permutations to find the first phrase, Please Wait...");
        }

        /// <summary>Proceed to generate permutations and find phrases.</summary>
        internal static void GeneratePermutationsAndFindPhrases()
        {
            secretPhrases = new List<string>();
            timeLog = new List<string>();
            generatedPermutations = new List<List<int>>();
            timer = Stopwatch.StartNew();
            //// Step 4.1: Loop through all filtered words
            for (int i1 = 0; i1 < integerWordsList.Length; i1++)
            {
                //// Step 4.1.1: Call recursive function with following arguments:
                //// Words Count        :   Count of words used to generate permutation in this section.
                //// Used Words         :   Words that are used in this section to  generate permutation.
                //// Words Value        :   Calculated value of used words in this section as per the integer value assigned in Step 2.7.
                //// Word Index         :    Index of current word.
                //// Required Length    :    Number of characters remaining required to  generate permutations.
                RecursivePhraseFinder(1, new List<int>() { i1 }, integerWordsList[i1], i1, anagramCharacterLength - integerCharacterLengthList[i1]);
            }
        }

        /// <summary>The recursive phrase finder.</summary>
        /// <param name="wordsCount">Count of words used to generate permutation in this section.</param>
        /// <param name="usedWords">Words that are used in this section to  generate permutation.</param>
        /// <param name="usedWordsValue"> Calculated value of used words in this section.</param>
        /// <param name="wordIndex">Index of current word.</param>
        /// <param name="requiredCharacterLength">Number of characters remaining required to  generate permutations.</param>
        internal static void RecursivePhraseFinder(int wordsCount, List<int> usedWords, double usedWordsValue, int wordIndex, int requiredCharacterLength)
        {
            //// Step 4.1.1.1: Check if all secret phrases has been found out.
            if (!IsFoundAllSecretPhrases)
            {
                var newWordsCount = wordsCount + 1;
                //// Step 4.1.1.1.1: Check if the combined word length exceeds the limit or not as per Step 3. Also check if the remaining character length of the generated permutation is greater than 0 to combine more words.
                if (newWordsCount <= MaxWordCount && usedWordsValue < phraseValue && requiredCharacterLength > 0)
                {
                    //// Step 4.1.1.1.1.1: Find allowed words that matches to the combined words to generate permutations.
                    var allowedWords = wordsAllowedToFollow[wordIndex];
                    Parallel.For(
                        0,
                        wordsCount - 1,
                        i =>
                        {
                            allowedWords = allowedWords.Intersect(wordsAllowedToFollow[usedWords[i]]).ToList();
                        });
                    allowedWords = allowedWords.Where(x => integerCharacterLengthList[x] <= requiredCharacterLength && integerCharacterLengthList[x] + usedWordsValue <= phraseValue).ToList();

                    var tasks = new Task[ProcessorDivision];

                    for (var taskNumber = 0; taskNumber < ProcessorDivision; taskNumber++)
                    {
                        var taskNumberCopy = taskNumber;
                        tasks[taskNumber] = Task.Factory.StartNew(
                            () =>
                            {
                                //// Step 4.1.1.1.1.2: Loop through all allowed words to join more words to generate permutation.
                                Parallel.For(
                                    allowedWords.Count * taskNumberCopy / ProcessorDivision,
                                    allowedWords.Count * (taskNumberCopy + 1) / ProcessorDivision,
                                    i1 =>
                                    {
                                        //// Step 4.1.1.1.2.3: Go to Step 4.1.1 with generated values in the current section.
                                        var i = allowedWords[i1];
                                        var usedWordsNow = new List<int>();
                                        usedWordsNow.AddRange(usedWords);
                                        usedWordsNow.Add(i);
                                        RecursivePhraseFinder(newWordsCount, usedWordsNow, integerWordsList[i] + usedWordsValue, i, requiredCharacterLength - integerCharacterLengthList[i]);
                                    });
                            });
                    }

                    Task.WaitAll(tasks);
                }
                else if (requiredCharacterLength == 0&& usedWordsValue == phraseValue)
                {
                    //// Step 4.1.1.2: Check if the character length of the generated permutation is equal to the phrase character length.
                    //// Step 4.1.1.2.1: Generate permutations from the combined words, and check if it is matching with hash value.
                    GeneratePermutations(usedWords);
                }
            }
        }

        /// <summary>The elapsed seconds.</summary>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string ElapsedSeconds()
        {
            return decimal.Round(Convert.ToDecimal(timer.Elapsed.TotalSeconds), 2) + " Seconds";
        }

        /// <summary>The elapsed minutes.</summary>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string ElapsedMinutes()
        {
            return decimal.Round(Convert.ToDecimal(timer.Elapsed.TotalMinutes), 2) + " Minutes";
        }

        /// <summary>Calculate generated permutations per seconds.</summary>
        /// <param name="count">The count.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string CalculatePerSeconds(int count)
        {
            return Convert.ToInt32(count / timer.Elapsed.TotalSeconds) + " words per Second";
        }

        /// <summary>Calculate generated permutations per minutes.</summary>
        /// <param name="count">The count.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string CalculatePerMinutes(int count)
        {
            return Convert.ToInt32(count / timer.Elapsed.TotalMinutes) + " words per minutes";
        }

        /// <summary>Check whether the word matching together to generate a phrase.</summary>
        /// <param name="firstArray">The first word character array.</param>
        /// <param name="secondArray">The second word character array.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        private static bool IsAllowThisWord(Dictionary<char, int> firstArray, Dictionary<char, int> secondArray)
        {
            var result = new Dictionary<char, int>(firstArray);
            secondArray.ToList()
                .ForEach(
                    x =>
                    {
                        if (result.ContainsKey(x.Key))
                        {
                            result[x.Key] += x.Value;
                        }
                        else
                        {
                            result.Add(x.Key, x.Value);
                        }
                    });
            return result.All(x => x.Value <= anagramCharactersGroup[x.Key]);
        }

        /// <summary>The generate permutations.</summary>
        /// <param name="usedWords">The used words now.</param>
        private static void GeneratePermutations(List<int> usedWords)
        {
            var isFoundEncryptedKey = false;
            Parallel.ForEach(
                Get(usedWords).ToList(),
                (usedWordsNew) =>
                {
                    generatedPermutations.Add(usedWordsNew);
                    var phrase = string.Join(" ", usedWordsNew.Select(x => stringWordsList[x]));
                    var hashValue = Utilities.Md5Hash(phrase);
                    if (secretPhrasesHashValues.Contains(hashValue))
                    {
                        isFoundEncryptedKey = true;
                        timeLog.Add("Elapsed " + ElapsedSeconds());
                        secretPhrases.Add(phrase);
                        if (secretPhrases.Count() == secretPhrasesHashValues.Count())
                        {
                            IsFoundAllSecretPhrases = true;
                        }
                    }
                });
            if (isFoundEncryptedKey || logIndex % 100 == 0)
            {
                PrintSecretPhrases(isFoundEncryptedKey);
            }

            logIndex++;
        }

        /// <summary>The get.</summary>
        /// <param name="set">The set.</param>
        /// <param name="subset">The subset.</param>
        /// <returns>The <see cref="IEnumerable"/>.</returns>
        private static IEnumerable<List<int>> Get(List<int> set, List<int> subset = null)
        {
            if (subset == null)
            {
                subset = new List<int>();
            }

            if (!set.Any())
            {
                yield return subset.ToList();
            }

            for (var i = 0; i < set.Count(); i++)
            {
                var newSubset = set.Take(i).Concat(set.Skip(i + 1)).ToList();
                foreach (var permutation in Get(newSubset, subset.Concat(set.Skip(i).Take(1)).ToList()))
                {
                    yield return permutation.ToList();
                }
            }
        }

        /// <summary>Print founded secret phrases.</summary>
        /// <param name="isFoundEncryptedKey">The is found encrypted key.</param>
        private static void PrintSecretPhrases(bool isFoundEncryptedKey)
        {
            var totalcount = generatedPermutations.Count;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("|");
            var indexkey = 0;
            if (!isFoundEncryptedKey)
            {
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nGenerated " + CalculatePerSeconds(totalcount) + " - (" + ElapsedSeconds() + ")\n" + CalculatePerMinutes(totalcount) + " - (" + ElapsedMinutes() + ")");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n" + string.Join("\n", secretPhrases.Select(x => "The anagram phrase of '" + Utilities.Md5Hash(x) + "' is '" + x + "' - " + timeLog[indexkey++])) + "\n");
            if (!IsFoundAllSecretPhrases)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nGenerating Permutations to find the next phrase, Please Wait...");
            }
        }
    }
}