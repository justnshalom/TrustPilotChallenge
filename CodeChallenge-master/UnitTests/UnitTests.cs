using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrustPilot;
using System.Linq;

namespace UnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestInitialize()]
        public void Startup()
        {
            Cost.GetValidLetters();
        }

        [TestMethod]
        public void GetValidLettersTest()
        {
            Assert.AreEqual(Cost.ValidLetters.Count(), 12, "'poultry outwits ants' should have 12 distinct letters");
            Assert.AreEqual(Cost.ValidLetters[0], 'a', "Encoding.ValidLetters should be sorted alphabetically");
        }

        [TestMethod]
        public void DifferenceTest()
        {
            Cost.TryGetCost("trustpilot", out ulong cost);
            ulong difference1 = Cost.PhraseCost - cost;
            ulong masked1 = difference1 & Cost.Mask;            
            Assert.AreEqual(masked1, (ulong)0, "'trustpilot' should only have letters from 'poultry outwits ants'");

            Cost.TryGetCost("trustpilooot", out cost);
            ulong difference2 = Cost.PhraseCost - cost;
            ulong masked2 = difference2 & Cost.Mask;
            Assert.AreEqual(masked2, (ulong)0x800000000000, "'trustpilooot' should not have the same letters as 'poultry outwits ants'");
        }

        [TestMethod]
        public void GetHashTest()
        {
            bool valid1 = Cost.TryGetCost(Cost.Phrase, out ulong cost1);
            bool valid2 = Cost.TryGetCost("invalid", out ulong cost2);
            bool valid3 = Cost.TryGetCost("trustpilot", out ulong cost3);

            Assert.AreEqual(string.Format("{0:X}", cost1), "1111211242110000", "'poultry outwits ants' should have the hex cost 0x1111211242110000");
            Assert.IsTrue(valid1, "'poultry outwits ants' should be valid");
            Assert.IsFalse(valid2, "'invalid' should not be valid because it contains the letters 'v' and 'd'");
            Assert.IsTrue(valid3, "'trustpilot' should be valid");
        }

        [TestMethod]
        public void GetValidWordsTest()
        {
            WordList.ReadWords();
            WordList.GetValidWords();

            int wordCount = WordList.ValidWords.Count();
            for (int i = 0; i < wordCount; i++)
            {
                bool valid = Cost.TryGetCost(WordList.ValidWords[i].Letters, out ulong cost);

                Assert.IsTrue(valid, WordList.ValidWords[i] + " should be a valid word");
                Assert.AreEqual(cost, WordList.ValidWords[i].Cost, WordList.ValidWords[i] + " should have the hex cost 0x" + cost);
            }
        }

        [TestMethod]
        public void SkipPermutationsTest()
        {
            Cost.Phrase = "b b ab";
            Cost.GetValidLetters();

            WordList.AllWords = new string[] { "a", "ab", "b" };
            WordList.GetValidWords();
            WordList.GetNextWordIndices();

            Anagram.FindAnagram(3);

            Assert.AreEqual(Anagram.NumberOfPermutations, 11, "it should not generate all 3^3 permutations");
            /*
             The algorithm should not generate all 27 permutations of "a", "ab", "b", but only the following 11 permutations:
             a
             a a
             a b
             a b b
             ab
             ab a
             ab b
             ab b b
             b
             b b
             b b b
            */
        }
    }
}