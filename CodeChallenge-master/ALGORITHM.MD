# A fast solution to the TrustPilot Code Challenge - Backend Developer


## The problem

Find an anagram of the phrase 'poultry outwits ants', that has a given MD5 hash and contains only words from a given list

## The solution

Finding anagrams involves generating permutations using a backtracking algorithm. However, the given word list contains **99175 words**, so if **N** is the number of words needed for the anagram, we will have to generate **99175^N** permutations (worst case scenario). 
This is an exponential algorithm, so even for a small **N** this can take a long time. Therefore, my focus for this challenge has been on performance, and below are the optimizations I made in order to significantly reduce the time it takes to find a solution.


### 1. Removing invalid words

Many words in the word list contain characters that are not in 'poultry outwits ants'. This means that they cannot be part of the solution, and should therefore be removed before we start generating permutations of words.
We also remove the words that have the same characters as the phrase but in greater number. For example a word containing two **a**'s could never be part of the solution since 'poultry outwits ants' only has one **a**.

After removing the invalid words we are left with **1658 words**, almost **60 times** fewer than the starting number.
The algorithm to remove invalid words visits each word in the original word list just once, so it's complexity is **O(N)**.


### 2. An O(1) string comparison algorithm

Each permutations of words that we generate, has to be compared to the original phrase to see if it's an anagram.
In other words, for each permutation we perform a string comparison... That's a lot of string comparisons! So we have to make sure our comparisons are fast.

Assume we have two strings of length **N1** and **N2** that we want to compare. A naive way of comparing the two strings would be to take each character from the first string and compare it to each character from the second string. As a quick approximation it would be an **O(min(N1, N2)^2)** algorithm.
By knowing that we will always compare words to the phrase 'poultry outwits ants' and by preprocessing our list of words, we can implement a very fast **O(1)** string comparison algorithm.

Let's count how many times each character appears in the phrase 'poultry outwits ants' and assign 4 bits per character:

|   -|   a|   i|   l|   n|   o|   p|   r|   s|   t|   u|   w|   y|    |    |    |    |
| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:|
|HEX:|   1|   1|   1|   1|   2|   1|   1|   2|   4|   2|   1|   1|   0|   0|   0|   0|
|BIN:|0001|0001|0001|0001|0010|0001|0001|0010|0100|0010|0001|0001|0000|0000|0000|0000|

We notice that the phrase has 12 distinct characters and 't' is the character that appears most times, 4. 
As seen above, 64 bits are enough to store how many times each character appears in the given phrase, so we could use an unsigned long variable to store this value. We call this value the **cost** of the string.

This representation is sufficient for all the permutations we generate, because we removed the words containing other letters in section 1.

Now let's compare the costs of 'poultry outwits ants' and 'trust piiilooot':

'poultry outwits ants':

|   -|   a|   i|   l|   n|   o|   p|   r|   s|   t|   u|   w|   y|    |    |    |    |
| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:|
|HEX:|   1|   1|   1|   1|   2|   1|   1|   2|   4|   2|   1|   1|   0|   0|   0|   0|
|BIN:|0001|0001|0001|0001|0010|0001|0001|0010|0100|0010|0001|0001|0000|0000|0000|0000|

'trust piiilooot':

|   -|   a|   i|   l|   n|   o|   p|   r|   s|   t|   u|   w|   y|    |    |    |    |
| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:|
|HEX:|   0|   3|   1|   0|   3|   1|   1|   1|   3|   1|   0|   0|   0|   0|   0|   0|
|BIN:|0000|0011|0001|0000|0011|0001|0001|0001|0011|0001|0000|0000|0000|0000|0000|0000|

By subtracting the 64 bit cost('trust piiilooot') from the 64 bit cost('poultry outwits ants') we obtain the following 64 bit number:

|   -|   a|   i|   l|   n|   o|   p|   r|   s|   t|   u|   w|   y|    |    |    |    |
| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:|
|BIN:|0000|1110|0000|0001|1111|0000|0000|0001|0001|0001|0001|0001|0000|0000|0000|0000|

Notice that the most significant bit is 1 only for 'i' and 'o', the letters that appeared more times in 'trust piiilooot' than in 'poultry outwits ants'. In fact, this will always happen when subtracting the bigger 4 bit value from the smaller, as long as we don't use this representation for strings that have the same character appearing more than 7 (0111) times.

By performing a binary AND between the cost difference calculated above and the following mask:

0x8888888888888888

|   -|   a|   i|   l|   n|   o|   p|   r|   s|   t|   u|   w|   y|    |    |    |    |
| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:|
|HEX:|   8|   8|   8|   8|   8|   8|   8|   8|   8|   8|   8|   8|   8|   8|   8|   8|
|BIN:|1000|1000|1000|1000|1000|1000|1000|1000|1000|1000|1000|1000|1000|1000|1000|1000|

we obtain the value

|   -|   a|   i|   l|   n|   o|   p|   r|   s|   t|   u|   w|   y|    |    |    |    |
| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:| --:|
|BIN:|0000|1000|0000|0000|1000|0000|0000|0000|0000|0000|0000|0000|0000|0000|0000|0000|

Notice that this value contains 0000 for the letters that appeared at most the same number of time in 'trust piiilooot' as in 'poultry outwits ants', and 1000 otherwise.

Using this representation we can say that given two strings s1 and s2:

1. s2 is an anagram of s1 if and only if
	cost(s1) - cost(s2) == 0

2. s2 can be part of an anagram of s1 if and only if
	(cost(s1) - cost(s2)) & 0x8888888888888888 == 0

3. s2 cannot be part of an anagram of s1 if and only if
	(cost(s1) - cost(s2)) & 0x8888888888888888 > 0
	
If we calculate the cost of each word from the list of words ahead of time, comparing two strings will only consist of one integer subtraction, one binary AND operation, and one integer comparison.
This string comparison does not depend on the length of the strings, so as stated earlier, we have an O(1) algorithm. It doesn't get any faster than this ☺

### 3. Generating fewer permutations

Let's assume our list of words contains 3 words "a", "ab" and "b", and we are looking for anagrams containing 2 words. The 9 possible permutations are:

|  1|  2|  3|  4|  5|  6|  7|  8|  9|
|--:|--:|--:|--:|--:|--:|--:|--:|--:|
|  a|  a|  a| ab| ab| ab|  b|  b|  b|
|  a| ab|  b|  a| ab|  b|  a| ab|  b|

We would have to check each permutation if it's an anagram of our given phrase. If our list of words has **N** words and we are generating permutations of **M** words, then in the worst case scenario we would have to check **N^M** permutations. The algorithm complexity would therefore be **O(N^M)**

We notice however that permutations 2 and 4 have the same words, as well as permutations 3 and 7, and 6 and 9.
This means if one of them is an anagram of our phrase, so is the other. So instead, we can generate only the permutations that are unique combinations of words, and when we find an anagram of our phrase, then we generate all the arrangements using those words.
Thus, the permutations we would generate would be:

|  1|  2|  3|  4|  5|  6|
|--:|--:|--:|--:|--:|--:|
|  a|  a|  a| ab| ab|  b|
|  a| ab|  b| ab|  b|  b|

together with the arrangements of the permutations that are anagrams of our given phrase.

This way we "only" generate **(N + M - 1)! / ((N - 1)! * M!)** permutations (an **O((N^M) / M!)** operation). With the word list cleaned in section 1, using this technique we reduce the maximum number of permutations from 1658^3 = 4.557.782.312 to 761.005.420, when generating permutations of 3 words. 

The "more difficult" phrase, "ty outlaws printouts", is discovered faster than the easy phrase, "printout stout yawls", because the permutation "outlaws printout ty" is generated before the permutation "printout stout yawls" (the word list is sorted alphabetically), and as soon as we generated "outlaws printout ty" we generate all the arrangements containing the same words and find "ty outlaws printouts".

### 4. Generating even fewer permutations

Let's have another look at the permutations from the previous section

|  1|  2|  3|  4|  5|  6|
|--:|--:|--:|--:|--:|--:|
|  a|  a|  a| ab| ab|  b|
|  a| ab|  b| ab|  b|  b|

Let's assume the solution to our problem is "b b". After generating permutation 1, we can see that the first word is "a", so permutation 1 cannot be our solution. We can then skip directly to permutation 4, because permutations 2 and 3 start with "a" as well. At permutation 4 we notice again that the first word "ab" cannot be part of the solution, so we skip to permutation 6.

However, we can do better than this, if we notice the similarities between words. For example, "a" is part of "ab", so it is safe to say that if "a" is not part of the solution, then neither can "ab". So in the previous example, after we notice that permutation 1 is not valid, we can skip directly to permutation 6.

If we would have another word "ba" it also would not be part of a solution, but for performance and in order to keep a low memory footprint, we only address the cases where one word starts with another, as in "ab" starts with "a".

So we need a way of determining what word to try next after we generated a permutation with an invalid word. This can be done by analyzing the list of words before we start generating permutations and determining the next word to try for each word.

Below is an example of a list of words and what the next word index would be for each word:

|         -|  a| ab|abc| ad|  b| ba|
|---------:|--:|--:|--:|--:|--:|--:|
|Word Index|  0|  1|  2|  3|  4|  5|
|Next Index|  4|  3|  3|  4|  5|  0|

So with this in mind, in the example at the beginning of this section, when we determine at step 1 that the permutation is invalid we can skip directly to step 6.


## Conclusion

By implementing the optimizations described above we obtain the following results on an i7-6700K:

|Phrase			|Avg. time	|Result               |
|---------------|----------:|--------------------:|
|Easy			|     0,32 s|printout stout yawls |
|More difficult*|	  0,25 s|ty outlaws printouts |
|Hard			|	   8,4 s|wu lisp not statutory|

* see section 3 for an explanation of why finding the more difficult phrase is faster than finding the easy phrase

Copyright (C) 2017, MSc. Stefan Mihai Stanescu , all rights reserved.