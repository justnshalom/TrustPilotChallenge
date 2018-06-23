**Step 1**: Fetch User Choices to find anagram.
------------
 **Step 2**: Initialize anagram.
------------

- **Step 2.1**: Convert anagram phrase to character array.
- **Step 2.2**: Remove spaces from the above character array. 
- **Step 2.3** Find the length of character array without spaces.    
- **Step 2.4**: Group the characters and store the count of each character as the value.   
- **Step 2.5**: Find the distinct characters of the phrase.
- **Step 2.6**: Fetch the word list from the file.
- **Step 2.7**: Assign an integer value for each distinct phrase characters.
- **Step 2.8**: Removed unwanted words from words list and group the characters and store the count of each character as the value in all words.  
- **Step 2.9**: Store words list as a list.
- **Step 2.10**: Assign value as Step 7 to each words in the list(step 8).
- **Step 2.11**: Store the character length of each words list(step 8).
- **Step 2.12**: Calculate the integer value of phrase as Step 7.
- **Step 2.13**: Store all index values of words.
- **Step 2.14**: Find all matching words of each word by checking the character limit of words.

------------
**Step 3**: Pick words limit from User.
------------

**Step 4**: Proceed to make permutations to find secret phrases.
------------

**Step 4.1**: Loop through all filtered words.

**Step 4.1.1**: Call recursive function with following arguments:
 > Words Count        :   Count of words using to generate permutation in this section.
 Used Words         :   Words that used to in this section to  generate permutation.
 Words Value        :   Calculated value of used words in this section as Step 2.7.
 Word Index         :   Index of current word.
 Required Length    :   Remaining character Length to  generate permutation.

- **Step 4.1.1.1**: Check whether not found all secret phrases.
If yes go to Step 4.1.1.1.1 else go to Step 4.1.1.1

 - **Step 4.1.1.1.1**: Check the words length exceeds the limit or not as Step 3 choice.

   - **Step 4.1.1.1.1.1**: Stop the current section.
                
  - **Step 4.1.1.1.2**: Check the remaining character length of generated permutation is greater than 0 to concatenate more words.

   - **Step 4.1.1.1.2.1**: Find allowed words that matching to the used words to generate permutations.

   - **Step 4.1.1.1.2.2**: Loop through all allowed words to join more words to generate permutation.

   - **Step 4.1.1.1.2.3**: Go to Step 4.1.1 with generated values in the current section.
  
- **Step 4.1.1.2**: Check the character length of generated permutation is equal to the phrase character length.
  - **Step 4.1.1.2.1**: Generate permutations from the allowed list of words matching to the phrase.