using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEndChallengeAnagram
{
    class Program
    {
        static void Main(string[] args)
        {
            var anagramPhrase = "poultry outwits ants";

            anagramPhrase = "pastils turnout towy";
            var encryptedString = Utilities.MD5Hash(anagramPhrase);
            Console.Write(encryptedString);


        }
    }
}
