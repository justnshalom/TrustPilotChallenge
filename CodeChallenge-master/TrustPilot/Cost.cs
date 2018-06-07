using System;
using System.Linq;

namespace TrustPilot
{
    public static class Cost
    {
        private static byte[] characterCount = new byte[256];

        public static char[] ValidLetters;
        public static string Phrase = "poultry outwits ants".Replace(" ", "");
        public static ulong PhraseCost;
        public static ulong Mask = 0x8888888888888888;

        // get the letters & cost of the original phrase
        public static void GetValidLetters()
        {
            ValidLetters = Phrase.Distinct().OrderBy(letter => letter).ToArray();
            TryGetCost(Phrase, out PhraseCost);
        }


        // calculate a 64bit cost for a string, which will be used for fast string comparisons        
        public static bool TryGetCost(string text, out ulong cost)
        {
            if(ValidLetters.Length > 16)
            {
                throw new ArgumentException("Encoding.ValidLetters should not contain more than 16 elements");
            }

            cost = 0;
            Array.Clear(characterCount, 0, characterCount.Length);

            foreach (char c in text.ToLower())
            {
                if(!ValidLetters.Contains(c))
                {
                    return false;
                }

                characterCount[c]++;
            }

            // see "2. An O(1) string comparison algorithm" in ALGORITHM.MD
            for (int index = 0; index < ValidLetters.Length; index++)
            {
                cost += (ulong) (characterCount[ValidLetters[index]]) << 4 * (15 - index);
            }

            return true;
        }
    }
}
