using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndChallengeAnagram
{
    class Program
    {
        private static List<string> allowedWordList;
        private static int wordsLength;
        private static int anagramLength;
        private static Dictionary<string,int> chracterGroup;
        static void Main(string[] args)
        {
            var anagramPhrase = "poultry outwits ants";
            ////anagramPhrase = "printout stout yawls";
            var encryptedString = Utilities.MD5Hash(anagramPhrase);
            var anagramPhraseCharacters = anagramPhrase.ToCharArray().Select(c => c.ToString()).ToList();
            anagramLength = anagramPhraseCharacters.Count();
            wordsLength = anagramPhrase.Split(' ').Length;
            chracterGroup = anagramPhraseCharacters.GroupBy(x => x).ToList().ToDictionary(x => x.FirstOrDefault(), x => x.Count());
            var allAlphabetsInAnagram = anagramPhraseCharacters.Where(x => x != " ").Distinct().ToList();
            ////var mutationstring = GetPermutations(anagramPhrase);
            var wordList= File.ReadLines("../../wordlist.text").ToArray();
            var expectedStringLength = anagramLength - ((wordsLength - 1) * 2);
            allowedWordList = wordList.Where(x => x.Length <= expectedStringLength && !x.ToCharArray().Select(c => c.ToString()).ToList().Any(y=> !allAlphabetsInAnagram.Contains(y))&&!x.ToCharArray().Select(c => c.ToString()).GroupBy(g => g).ToList().ToDictionary(d => d.FirstOrDefault(), d => d.Count()).Any(z => z.Value > chracterGroup[z.Key])).Distinct().OrderByDescending(l=>l.Length).ToList();


            RecursivePhraseFinder("", 0);

            Console.Write(encryptedString);

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadLine();
        }
        private static List<string> secretphases = new List<string> { "e4820b45d2277f3844eac66c903e84be", "23170acc097c24edb98fc5488ab033fe", "665e5bcb0c20062fe8abaaf4628bb154" };
       
        public static List<string> List=new List<string>();
        private static void RecursivePhraseFinder(string phrase, int level,List<string> usedWords=null)
        {
            if (level == wordsLength)
            {
                var encryptedString = Utilities.MD5Hash(phrase);
                if (secretphases.Contains(encryptedString))
                {
                    List.Add(phrase);
                }
            }
            else
            {
                var newLevel = level + 1;
                foreach (var word in allowedWordList)
                {
                    ////"printout stout yawls";
                    if (phrase== "printout stout" && word == "yawls")
                    {

                    }
                    if (usedWords==null|| !usedWords.Contains(word))
                    {
                        var usedWordsNow= new List<string>();
                        if (usedWords != null)
                        {
                            usedWordsNow.AddRange(usedWords);
                        }

                        usedWordsNow.Add(word);
                        var newPhrase = level == 0 ? word : phrase + " " + word;
                        if (newLevel == 3&& newPhrase.Length==20)
                        {
                            Console.WriteLine("Checking -> "+newPhrase);
                        }
                        if(newPhrase.StartsWith("printout") && newPhrase.Length == 20)
                        {

                        }
                        if (newPhrase=="printout stout yawls")
                        {

                        }
                        if (!newPhrase.StartsWith("printout"))
                        {

                        }
                        if (phrase == "printout stout" && word == "yawls")
                        {

                        }
                        ////Avoided not match length characters
                        if ((newLevel != wordsLength && newPhrase.Length <= anagramLength)||(newLevel == wordsLength&& newPhrase.Length == anagramLength))
                        {
                            if (phrase == "printout" && word == "stout")
                            {

                            }
                            var newPhraseCharacters = newPhrase.ToCharArray().Select(c => c.ToString()).ToList();
                            var newPhraseChracterGroup = newPhraseCharacters.GroupBy(x => x).ToList().ToDictionary(x => x.FirstOrDefault(), x => x.Count());
                            if (!newPhraseChracterGroup.Any(x => x.Value > chracterGroup[x.Key]))
                            {
                                if (phrase == "printout stout" && word == "yawls")
                                {

                                }
                                RecursivePhraseFinder(newPhrase, newLevel, usedWordsNow);
                            }
                        }
                    }else
                    {
                        if (phrase == "printout stout" && word == "yawls")
                        {

                        }
                    }
                }
            }
        }
    }
}
