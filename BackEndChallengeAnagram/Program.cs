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

        public static List<List<int>> permutations = new List<List<int>>();

        private static List<char> allAlphabetsInAnagram;

        private static Dictionary<string, Dictionary<char, int>> allowedWordList;

        private static int anagramLength;

        private static int characterLength;

        private static Dictionary<char, int> chracterGroup;

        private static readonly IFormatProvider culture = CultureInfo.CreateSpecificCulture("en");

        private static List<int> integercharacterLengthList;

        private static List<double> integerwordsList;

        private static readonly int maxLevel = 4;

        private static double phraseValue;

        private static readonly List<string> secretphases = new List<string> { "e4820b45d2277f3844eac66c903e84be", "23170acc097c24edb98fc5488ab033fe", "665e5bcb0c20062fe8abaaf4628bb154" };

        private static List<int> singleCharactersStarting;

        private static List<string> stringwordsList;

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
            var anagramPhrase = "poultry outwits ants";

            ////anagramPhrase = "printout stout yawls";
            var anagramPhraseCharacters = anagramPhrase.ToCharArray().ToList();
            anagramLength = anagramPhraseCharacters.Count();
            var alphabetsCharacters = anagramPhraseCharacters.Where(x => x != ' ').ToArray();
            characterLength = alphabetsCharacters.Count();
            wordsLength = anagramPhrase.Split(' ').Length;
            chracterGroup = alphabetsCharacters.GroupBy(x => x).ToList().ToDictionary(x => x.FirstOrDefault(), x => x.Count());
            allAlphabetsInAnagram = anagramPhraseCharacters.Where(x => x != ' ').Distinct().ToList();

            ////var mutationstring = GetPermutations(anagramPhrase);
            var wordList = File.ReadLines("../../wordlist.text").ToArray();
            var expectedStringLength = characterLength - 7;

            var alphabetsList = allAlphabetsInAnagram.Select((x, i) => new { Item = x, Index = i }).ToDictionary(x => x.Item, x => Math.Pow(10, x.Index));

            allowedWordList = wordList.Distinct().OrderBy(l => l).ToList().ToDictionary(x => x, x => x.ToCharArray().Select(c => c).GroupBy(g => g).ToList().ToDictionary(d => d.FirstOrDefault(), d => d.Count())).Where(x => x.Key.Length <= expectedStringLength && x.Value.All(y => allAlphabetsInAnagram.Contains(y.Key)) && !x.Value.Any(z => z.Value > chracterGroup[z.Key])).ToDictionary(x => x.Key, x => x.Value);
            stringwordsList = allowedWordList.Select(x => x.Key).ToList();
            integerwordsList = allowedWordList.Select(x => x.Value.Sum(y => alphabetsList[y.Key] * y.Value)).ToList();
            integercharacterLengthList = allowedWordList.Select(x => x.Key.Length).ToList();
            phraseValue = chracterGroup.Sum(x => alphabetsList[x.Key] * x.Value);
            singleCharactersStarting = allAlphabetsInAnagram.OrderBy(x => x).Select(x => FirstIndexMatch(x.ToString())).ToList();

            var allwordsintegertoallow = integerwordsList.Select((x, i) => i).ToList();

            WordsAllowedToFollow = allwordsintegertoallow.ToDictionary(x => x, x => allwordsintegertoallow.Where(y => isAllowThisWord(allowedWordList[stringwordsList[x]], allowedWordList[stringwordsList[y]])).ToList());

            ////var validwords=integerwordsList.SelectMany(x => x == phraseValue?x: x < phraseValue?integerwordsList.Where(y => x+y< phraseValue && &integerwordsList.Where(z => x + z < phraseValue);
            timer = Stopwatch.StartNew();

            ////RecursivePhraseFinder(string.Empty, 0);
            Console.WriteLine("started finding first one");
            for (int i1 = 0; i1 < integerwordsList.Count; i1++)
            {
                RecursivePhraseFinder(1, new List<int>() { i1 }, integerwordsList[i1], i1, characterLength-integercharacterLengthList[i1]);
            }
            Console.WriteLine("\nPress any key to exit.");
            Console.Write("\nENd in " + timer.Elapsed.TotalSeconds.ToString("F4", culture) + " seconds");
            Console.ReadLine();
        }

        private static void RecursivePhraseFinder(int level, List<int> usedwords, double currentphraseValue, int currenti, int newrequiredcharacterlength)
        {
            if (level == 1&&currenti==2)
            {

            }
            if (!FoundAllWords)
                if (currentphraseValue < phraseValue)
                {
                    var newLevel = level + 1;
                    if (level == 1)
                    {
                        
                    }
                    
                    if (level == 2)
                    {

                    }
                    if (newLevel <= maxLevel)
                    {
                        var degreeOfParallelism = Environment.ProcessorCount / 2;
                        var allowedWords = WordsAllowedToFollow[currenti];
                        if (level != 0)
                        {
                            /////allowedwordstothis = allowedwordstothis.Intersect(allowedWords).ToList();
                        }
                        ////var currentCharacterArray = allowedWordList[stringwordsList[i]];
                        var startsfrom = 0;
                        var max = allowedWords.Count;
                        ////var tasks = new Task[degreeOfParallelism];

                        ////for (var taskNumber = 0; taskNumber < degreeOfParallelism; taskNumber++)
                        ////{
                        ////    var taskNumberCopy = taskNumber;
                        ////    tasks[taskNumber] = Task.Factory.StartNew(
                        ////        () =>
                        ////            {
                        ////                var max = allowedWords.Count * (taskNumberCopy + 1) / degreeOfParallelism;
                        ////                var startsfrom = allowedWords.Count * taskNumberCopy / degreeOfParallelism;
                        ////                if (currenti > startsfrom)
                        ////                {
                        ////                    startsfrom = currenti;
                        ////                }

                                        ////var lastdisabledword = "";
                                        ////Parallel.For(
                                        ////    startsfrom,
                                        ////    max,
                                        ////    i1 =>
                                        ////        {
                                        for (int i1 = startsfrom; i1 < max; i1++)
                                        {
                                            
                                                    var i = allowedWords[i1];
                                                    ////var disqualifiedobjects = new List<int>();

                                                    var currentvalue = integerwordsList[i] + currentphraseValue;
                                                    var currentcharacterLength = integercharacterLengthList[i];
                                                    if (currentcharacterLength <= newrequiredcharacterlength && currentvalue <= phraseValue)
                                                    {
                                                        if (level == 1)
                                                        {

                                                        }

                                                        if (level == 2)
                                                        {

                                                        }
                                                        if (level == 3)
                                                        {

                                                        }

                                                        if (level == 4)
                                                        {

                                                        }
                                                        var usedWordsNow = new List<int>();
                                                        usedWordsNow.AddRange(usedwords);
                                                        usedWordsNow.Add(i);
                                                        ////var requiredcharacters = new Dictionary<char, int>();
                                                        var requiredCharacterLength = newrequiredcharacterlength - integercharacterLengthList[i];
                                ////foreach (var x in chracterGroup)
                                ////    if (currentCharacterArray.ContainsKey(x.Key)) requiredcharacters[x.Key] = x.Value - currentCharacterArray[x.Key];
                                ////    else requiredcharacters.Add(x.Key, x.Value);
                                ////var phrase = string.Join(" ", usedwords.Select(x => stringwordsList[x]));
                                ////Console.Write("\n" + phrase);
                                if (requiredCharacterLength > 0)
                                {
                                    RecursivePhraseFinder(newLevel, usedWordsNow, currentvalue, i, requiredCharacterLength);
                                }else 
                                if(currentvalue == phraseValue&&requiredCharacterLength == 0)
                                {
                                    var phrase = string.Join(" ", usedwords.Select(x => stringwordsList[x]));
                                    var encryptedString = Utilities.MD5Hash(phrase);
                                    permutations.Add(usedwords);
                                    if (permutations.Count % 100 == 0)
                                    {
                                        Console.WriteLine("\n" + phrase + " " + permutations.Count + " " + timer.Elapsed.TotalSeconds.ToString("F4", culture) + " seconds");
                                    }

                                    ////if("e4820b45d2277f3844eac66c903e84be"==encryptedString)
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

                                                    ////else if (integercharacterLengthList[i] == 1)
                                                    ////{
                                                    ////    var newindex = singleCharactersStarting.FindIndex(x => x == i) + 1;
                                                    ////    i = singleCharactersStarting.Contains(newindex) ? singleCharactersStarting[newindex] : i;
                                                    ////}
                                                    //// i++;
                                                ////});

                        }
                    ////});
                    ////    }

                    ////    Task.WaitAll(tasks);
                    }
                }
                ////else if (usedwords.Any() && currentphraseValue == phraseValue)
                ////{
                ////    var phrase = string.Join(" ", usedwords.Select(x => stringwordsList[x]));
                ////    var encryptedString = Utilities.MD5Hash(phrase);
                ////    permutations.Add(usedwords);
                ////    if (permutations.Count % 100 == 0)
                ////    {
                ////        Console.WriteLine("\n" + phrase + " " + permutations.Count + " " + timer.Elapsed.TotalSeconds.ToString("F4", culture) + " seconds");
                ////    }

                ////    ////if("e4820b45d2277f3844eac66c903e84be"==encryptedString)
                ////    if (secretphases.Contains(encryptedString))
                ////    {
                ////        Console.Write("\nFound " + encryptedString + " of " + phrase + " in " + timer.Elapsed.TotalSeconds.ToString("F4", culture) + " seconds");
                ////        List.Add(phrase);
                ////        if (List.Count() < secretphases.Count())
                ////        {
                ////            Console.WriteLine("\nstarted finding next one");
                ////        }
                ////        else
                ////        {
                ////            Console.WriteLine("\nFounded All phases");
                ////            Console.WriteLine("\nPress any key to exit.");
                ////            Console.ReadLine();
                ////        }
                ////    }
                ////}
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