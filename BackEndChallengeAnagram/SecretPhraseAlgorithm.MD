 ////Step 1: Fetch User Choices to find anagram.

 ////Step 2: Initialize anagram.

            ////Step 2.1: Convert anagram phrase to character array.
            
            ////Step 2.2: Remove spaces from the above character array. 
            
            ////Step 2.3: Find the length of character array without spaces. 
            
            ////Step 2.4: Group the characters and store the count of each characters as the value.   
            
            ////Step 2.5: Find the distinct characters of the phrase 
            
            ////Step 2.6: Fetch the word list from the file
            
            ////Step 2.7: Assign an integer value for each distinct phrase characters
            
            ////Step 2.8: Removed unwanted words from words list and group the characters and store the count of each characters as the value in all words.  
            
            ////Step 2.9: Store words list as a list
            
            ////Step 2.10: Assign value as Step 7 to each words in the list(step 8)
            
            ////Step 2.11: Store the character length of each words list(step 8)
            
            ////Step 2.12: Calculate the integer value of phrase as Step 7
            
            ////Step 2.13: Store all index values of words
            
            ////Step 2.14: Find all matching words of each words by checking the character limit of words 
            
////Step 3: Take words count choices from User.
		The action to fetch words count choice

////Step 4: Proceed to make permutations to find secret phrases.

            ////Step 1: Check whether find all secret phrases or not 
            if (!IsFoundAllSecretPhrases)
            {
                var newWordsCount = wordsCount + 1;
                if (usedWordsValue < phraseValue && requiredCharacterLength > 0)
                {
                    if (newWordsCount > MaxWordCount)
                    {
                        return;
                    }
                    
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
                                    var max = allowedWords.Count * (taskNumberCopy + 1) / ProcessorDivision;
                                    var startsfrom = allowedWords.Count * taskNumberCopy / ProcessorDivision;
                                    Parallel.For(
                                        startsfrom,
                                        max,
                                        i1 =>
                                            {
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
                else if (usedWordsValue == phraseValue && requiredCharacterLength == 0)
                {
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
                var phrase = string.Join(" ", usedWordsNew.Select(x => stringwordsList[x]));
                var hashValue = Utilities.MD5Hash(phrase);
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
            Console.WriteLine("\n" + string.Join("\n", secretPhrases.Select(x => "The anagram phrase of '" + Utilities.MD5Hash(x) + "' is '" + x + "' - " + timeLog[indexkey++])) + "\n");
            if (!IsFoundAllSecretPhrases)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\nGenerating Permutations to find the next phrase, Please Wait...");
            }
        }
    }
}