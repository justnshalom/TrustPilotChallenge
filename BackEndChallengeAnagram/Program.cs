using System;

namespace BackEndChallengeAnagram
{
  

    public class Program
    {


        public static void Main(string[] args)
        {
            Anagram.FetchUserChoices();
            Anagram.InitializeAnagram();
            Anagram.FetchWordsCountChoice();
            Anagram.GeneratePermutationsAndFindPhrases();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nPress any key to exit.");
            Console.Write("\nEnd in " +Anagram.ElapsedSeconds());
            Console.ReadLine();
        }
       
    }
}