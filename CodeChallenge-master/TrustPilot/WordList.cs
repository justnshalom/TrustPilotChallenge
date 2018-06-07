using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrustPilot
{
    public struct Word
    {
        public string Letters;
        public ulong Cost;
        public int NextWordIndex;
    }

    public static class WordList
    {
        public static string[] AllWords;
        public static Word[] ValidWords;

        public static void ReadWords()
        {
            try
            {
                AllWords = File.ReadLines("wordlist").ToArray();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Make sure the 'worldlist' file is in the same directory as the program. Press any key to exit.");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        // see "1. Removing invalid words" in ALGORITHM.MD
        public static void GetValidWords()
        {
            var validWords = new List<Word>();
            ulong difference = 0;
            string cleanedWord;

            foreach (string word in AllWords)
            {
                cleanedWord = word.Replace(" ", "");

                if (Cost.TryGetCost(cleanedWord, out ulong value))
                {
                    difference = Cost.PhraseCost - value;
                    if ((difference & Cost.Mask) == 0)
                    {
                        validWords.Add(new Word() { Letters = cleanedWord, Cost = value });
                    }
                }
            }
            
            ValidWords = validWords.Distinct().ToArray();            
        }

        // see "4. Generating even fewer permutations" in ALGORITHM.MD
        public static void GetNextWordIndices()
        {
            for(int i = 0; i < ValidWords.Length - 1; i++)
            {
                bool found = false;
                for (int j = i + 1; j < ValidWords.Length; j++)
                {
                    if (!ValidWords[j].Letters.StartsWith(ValidWords[i].Letters))
                    {
                        found = true;
                        ValidWords[i].NextWordIndex = j;
                        break;
                    }
                }

                if (!found)
                {
                    ValidWords[i].NextWordIndex =  ValidWords.Length;
                }
            }

            ValidWords[ValidWords.Length - 1].NextWordIndex =  ValidWords.Length;            
        }
    }
}
