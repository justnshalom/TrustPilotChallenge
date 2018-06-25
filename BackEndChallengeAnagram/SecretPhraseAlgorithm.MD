

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
- **Step 2.8**: Remove unwanted words from words list, group the characters and store the count of each character as the value, in all words.  
- **Step 2.9**: Store the allowed words list as a string list.
- **Step 2.10**: Assign value to each words in the list as per the integer value assigned in Step 2.7.
- **Step 2.11**: Store the character length of each word in the word list.
- **Step 2.12**: Calculate the integer value of phrase as per the integer value assigned in Step 2.7.
- **Step 2.13**: Store all index values of words.
- **Step 2.14**: Find all matching words of each word by checking the character limit of words.

------------
**Step 3**: Obtain the word limit from the User.
------------
By default it will generate 2 to 4 words, User has an option to change the word limit to 'n'.

**Step 4**: Proceed to make permutations to find secret phrases.
------------

**Step 4.1**: Loop through all filtered words.

**Step 4.1.1**: Call recursive function with following arguments:
 > Word Count        :   Count of words used to generate permutation in this section.
 Used Words         :   Words that are used in this section to  generate permutation.
 Words Value        :   Calculated value of used words in this section as per the integer value assigned in Step 2.7.
 Word Index         :   Index of current word.
 Required Length    :   Number of characters remaining required to  generate permutations.

- **Step 4.1.1.1**: Check if all secret phrases has been found out.
If no go to Step 4.1.1.1.1 else stop process.

   - **Step 4.1.1.1.1**: Check if the combined word length exceeds the limit or not as per Step 3. Also check if the remaining character length of the generated permutation is greater than 0 to combine more words.
If yes go to Step 4.1.1.1.1.1 else go to Step 4.1.1.2

     - **Step 4.1.1.1.1.1**: Find allowed words that matches to the combined words to generate permutations.

     - **Step 4.1.1.1.1.2**: Loop through all allowed words to join more words to generate permutation.

     - **Step 4.1.1.1.1.3**: Go to Step 4.1.1 with generated values in the current section.
  
- **Step 4.1.1.2**: Check if the character length of the generated permutation is equal to the phrase character length.
  - **Step 4.1.1.2.1**: Generate permutations from the combined words, and check if it is matching with hash value.
If yes go to step Step 4.1.1.2.1.1
    -  **Step 4.1.1.2.1.1**: Display phrases as output.
 