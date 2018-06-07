using System.Collections.Generic;

namespace TrustPilot
{
    public class Anagram
    {
        private const int MAX_WORDS = 18; // 'poultry outwits ants' contains 18 characters
        private static int[] permutation = new int[MAX_WORDS];
        private static ulong[] remainingCost = new ulong[MAX_WORDS];
        private static int currentWordCount = 0;

        public static long NumberOfPermutations = 0;
        public static string Hash;

        public static string FindAnagram()
        {
            int maximumWordCount = 1;
            string solution = null;
                       
            while (solution == null)
            {
                solution = FindAnagram(maximumWordCount);
                maximumWordCount++;
            }

            return solution;
        }

        public static string FindAnagram(int maximumWordCount)
        {
            string anagram;
            ulong difference;
            remainingCost[0] = Cost.PhraseCost;

            //generate permutations of words
            while (true)
            {
                NumberOfPermutations++;

                difference = remainingCost[currentWordCount] - WordList.ValidWords[permutation[currentWordCount]].Cost;
                // if current partial permutation is valid
                if ((difference & Cost.Mask) == 0)
                {
                    // if the current permutation contains all the letters
                    if (difference == 0)
                    {
                        // generate all word arrangements with the words in the current permutation and check their MD5 hashes
                        anagram = GenerateArrangements(maximumWordCount, new Stack<string>());
                                                
                        if (anagram != null)
                        {
                            return anagram; //we found the solution
                        }
                    }

                    // add another word to the permutation
                    currentWordCount++;
                    remainingCost[currentWordCount] = difference;
                    permutation[currentWordCount] = permutation[currentWordCount - 1];
                }
                else
                {
                    // try the next word
                    permutation[currentWordCount] = WordList.ValidWords[permutation[currentWordCount]].NextWordIndex;
                }

                //backtrack
                while (currentWordCount >= maximumWordCount || permutation[currentWordCount] >= WordList.ValidWords.Length)
                {
                    if (currentWordCount > 0)
                    {
                        // the last word in currentPermutation is not valid, so we remove it
                        permutation[currentWordCount] = 0;                        
                        currentWordCount--;
                        permutation[currentWordCount]++;
                    }
                    else
                    {
                        // no solution was found using maximumWordCount words
                        permutation[0] = 0;
                        return null;
                    }
                }
            }
        }

        // recursively generate arrangements for the words in the current permutation
        // if an arrangement matches the required hash return its corresponding anagram, otherwise return null
        private static string GenerateArrangements(int wordCount, Stack<string> arrangement)
        {
            if (arrangement.Count == wordCount)
            {
                NumberOfPermutations++;
                // uncomment this line to print anagrams
                // Console.WriteLine(string.Join(" ", arrangement.ToArray()));
                return string.Join(" ", arrangement.ToArray());
            }

            for (int i = 0; i < wordCount; i++)
            {
                string word = WordList.ValidWords[permutation[i]].Letters;
                if (!arrangement.Contains(word))
                {
                    arrangement.Push(word);
                    string partialResult = GenerateArrangements(wordCount, arrangement);

                    if (partialResult != null && Hash == MD5Hash.Calculate(partialResult))
                    {
                        return partialResult;
                    }

                    arrangement.Pop();
                }
            }

            return null;
        }
    }
}
