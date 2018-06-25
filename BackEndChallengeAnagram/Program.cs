// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="TrustPilot">
//   No CopyRight
// </copyright>
// <summary>
//   The anagram program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BackEndChallengeAnagram
{
    using System;

    /// <summary>The anagram program.</summary>
    public class Program
    {
        /// <summary>The main function.</summary>
        public static void Main()
        {
            //// Step 1: Fetch User Choices to find anagram.
            Anagram.FetchUserChoices();
            //// Step 2: Initialize anagram.
            Anagram.InitializeAnagram();
            //// Step 3: Obtain the word limit from the User.
            Anagram.FetchWordsCountChoice();
            //// Step 4: Proceed to make permutations to find secret phrases.
            Anagram.GeneratePermutationsAndFindPhrases();
            //// Step 5: Log time of end details.
            Console.ForegroundColor = ConsoleColor.White;
            if (!Anagram.IsFoundAllSecretPhrases)
            {
                Console.WriteLine("\nUnfortunately we can't find all secret phrases in generated permutations from 2 to " + Anagram.MaxWordCount + " words.");
            }

            Console.WriteLine("\nPress any key to exit.");
            Console.Write("\nEnd in " + Anagram.ElapsedSeconds());
            Console.ReadLine();
        }
       
    }
}