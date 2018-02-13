using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DECATraining
{

    /*
     * Reads the test data from text files.
     */
    public static class TestFileReader
    {

        /*
         * Returns a test set, as read and combined from the files for the tests in the set.
         * May return null if none of the test files are opened correctly.
         */
        public static TestSet readTestSet(Info.CategoryInfo categoryInfo, Info.TestSetInfo testSetInfo)
        {
            List<Test> tests = new List<Test>();

            // Go through each of the tests in the test set
            foreach (Info.TestName testName in testSetInfo.testNames)
            {
                // Read the test
                Test test = readTest(testName);

                // If the test was read successfully, add it to the list
                if (test != null)
                    tests.Add(test);
            }

            // If any tests were read successfully, create the test set
            if (tests.Count > 0)
                return new TestSet(categoryInfo, testSetInfo, tests, 0, 0);
            else
                return null;
        }

        /*
         * Returns a test, as read from the test's file.
         * May return null if the file is not opened correctly.
         */
        public static Test readTest(Info.TestName testName)
        {
            List<String> lines = readLines(@"Tests\" + testName.ToString() + ".txt");

            if (lines != null)
                return parseTest(lines, testName);
            else
                return null;
        }




        /*
         * Reads the data from the file and returns a list of the lines in the file.
         * May return null if the file is not opened correctly.

         * (All lines are stripped of leading and trailing whitespace and newline characters.)
         * (No blank lines are included.)
         * (No page headers, footers, or numbers are included.)
         */
        private static List<String> readLines(String fileName)
        {
            StreamReader file = null;
            List<String> lines = null;

            try
            {
                // Open the file
                file = new StreamReader(fileName);

                // Initialize the list of lines (assuming the file opened correctly)
                lines = new List<String>();

                // Read each line into the list until the end of the file is reached
                String line = "";
                while (line != null)
                {
                    line = file.ReadLine();

                    if (line != null)
                    {
                        line = line.Trim();

                        if (!IsNullOrWhiteSpace(line) &&
                            !isAPageHeader(line) &&
                            !isAPageFooter(line) &&
                            !isAPageNumber(line))
                        {
                            line.Replace("’", "'");
                            line.Replace("“", "\"");
                            line.Replace("”", "\"");
                            
                            lines.Add(line);
                        }      
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Exception: The file directory could not be found.");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Exception: The file could not be found.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: Unknown exception.");
            }
            finally
            {
                // Close the file
                if (file != null)
                    file.Close();
            }

            return lines;
        }

        /*
         * Converts a list of string lines into a single string.
         * (Adds a space between lines.)
         */
        private static String convertLinesToString(List<String> lines)
        {
            String newString = "";

            if (lines.Count > 0)
                newString += lines[0];

            for (int i = 1; i < lines.Count; i++)
                newString += " " + lines[i];

            return newString;
        }




        /*
         * Parses and returns a test.
         */
        private static Test parseTest(List<String> lines, Info.TestName testName)
        {
            Test test = new Test(testName);

            // Current line index
            int linesIndex = 0;

            // The question and answer numbers expected next
            int expectedQuestionNumber = 1;
            int expectedAnswerNumber = 1;

            while (linesIndex < lines.Count)
            {
                // If the current line starts a question
                if (isText(lines[linesIndex]) && parseQuestionNumber(lines[linesIndex]) == expectedQuestionNumber)
                {
                    // Secondary line index
                    int secondaryLinesIndex = linesIndex + 1;

                    // Find the end of the question
                    while (secondaryLinesIndex < lines.Count && beginsWithAQuestionNumber(lines[secondaryLinesIndex]) == false)
                        secondaryLinesIndex++;

                    // Parse the question and add it to the test
                    test.questions.Add(parseQuestion(
                        convertLinesToString(lines.GetRange(linesIndex, secondaryLinesIndex - linesIndex)) ));

                    linesIndex = secondaryLinesIndex;

                    expectedQuestionNumber++;
                }

                // If the current line starts an answer
                else if (isACorrectAnswer(lines[linesIndex]) && parseQuestionNumber(lines[linesIndex]) == expectedAnswerNumber)
                {
                    // Secondary line index
                    int secondaryLinesIndex = linesIndex + 1;

                    // Find the end of the answer
                    while (secondaryLinesIndex < lines.Count && beginsWithAQuestionNumber(lines[secondaryLinesIndex]) == false)
                        secondaryLinesIndex++;

                    // Parse the answer and add it to the test
                    KeyValuePair<char, String> answer = parseAnswer(
                        convertLinesToString(lines.GetRange(linesIndex, secondaryLinesIndex - linesIndex)) );
                    test.addAnswer(expectedAnswerNumber - 1, answer.Key, answer.Value);

                    linesIndex = secondaryLinesIndex;

                    expectedAnswerNumber++;
                }

                else
                {
                    Console.WriteLine("Problem: Invalid input to parseTest().");
                    linesIndex++;
                }
            }

            return test;
        }




        /*
         * Parses and returns a question.
         */
        private static TestQuestion parseQuestion(String line)
        {
            TestQuestion question = new TestQuestion();

            // If the first line is question text
            if (isText(line))
            {
                // Current character index
                int characterIndex = 0;

                // Secondary character index (beginning of answer options)
                int secondaryCharacterIndex = line.Length;
                for (int i = 0; i < line.Length; i++)
                {
                    if (isAnAnswerOption(line.Substring(i).Trim()))
                    {
                        secondaryCharacterIndex = i;
                        break;
                    }
                }

                // Parse the question text
                question.text = parseText(line.Substring(characterIndex, secondaryCharacterIndex - characterIndex).Trim());

                // Parse the answer options
                if ( isAnAnswerOption(line.Substring(secondaryCharacterIndex).Trim()) )
                    question.answerOptions = parseAnswerOptions(line.Substring(secondaryCharacterIndex).Trim());
                else
                    Console.WriteLine("Problem: Invalid input to parseQuestion().");
            }

            else
            {
                Console.WriteLine("Problem: Invalid input to parseQuestion().");
            }

            return question;
        }

        /*
         * Parses and returns the answer.
         * (correct answer, correct answer explanation)
         */
        private static KeyValuePair<char, String> parseAnswer(String line)
        {
            char correctAnswer = ' ';
            String correctAnswerExplanation = "";

            // If the first line is a correct answer
            if (isACorrectAnswer(line))
            {
                // Current character index
                int characterIndex = 0;

                // Secondary character index (beginning of correct answer explanation)
                int secondaryCharacterIndex = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    if (isACapitalLetterFromAToD(line[i]))
                    {
                        secondaryCharacterIndex = i + 1;
                        break;
                    }
                }

                // Parse the correct answer
                correctAnswer = parseCorrectAnswer(line.Substring(characterIndex, secondaryCharacterIndex - characterIndex).Trim());

                // Parse the correct answer explanation
                if ( isACorrectAnswerExplanation(line.Substring(secondaryCharacterIndex).Trim()) )
                    correctAnswerExplanation = parseCorrectAnswerExplanation(line.Substring(secondaryCharacterIndex).Trim());
                else
                    Console.WriteLine("Problem: Invalid input to parseAnswer().");
            }

            else
            {
                Console.WriteLine("Problem: Invalid input to parseAnswer().");
            }

            return new KeyValuePair<char, string>(correctAnswer, correctAnswerExplanation);
        }




        /*
         * Parses the returns a question number.
         */
        private static int parseQuestionNumber(String line)
        {
            if (beginsWithAQuestionNumber(line))
            {
                return Convert.ToInt32( line.Substring(0, line.IndexOf('.')) );
            }
            else
            {
                Console.WriteLine("Problem: Invalid input to parseQuestionNumber().");
                return -1;
            }
        }

        /*
         * Parses and returns a question text.
         */
        private static String parseText(String line)
        {
            String text = "";

            // If the line is question text
            if (isText(line))
            {
                // Current character index
                int characterIndex = line.IndexOf('.') + 1;

                // Skip space characters
                while ( characterIndex < line.Length && IsNullOrWhiteSpace(line[characterIndex].ToString()) )
                    characterIndex++;

                // Parse the rest of the line as the question text
                text += line.Substring(characterIndex).Trim();
            }

            else
            {
                Console.WriteLine("Problem: Invalid input to parseText().");
            }

            return text;
        }

        /*
         * Parses and returns a single answer option.
         */
        private static KeyValuePair<char, String> parseAnswerOption(String line)
        {
            char letter = ' ';
            String text = "";

            // Parse the letter
            letter = Convert.ToChar(line[0].ToString().ToLower());

            // Current character index
            int characterIndex = 2;

            // Skip space characters
            while ( characterIndex < line.Length && IsNullOrWhiteSpace(line[characterIndex].ToString()) )
                characterIndex++;

            // Parse the text from the rest of the line
            text = line.Substring(characterIndex);

            return new KeyValuePair<char, string>(letter, text);
        }

        /*
         * Parses and returns multiple answer options.
         */
        private static Dictionary<char, String> parseAnswerOptions(String line)
        {
            Dictionary<char, String> answerOptions = new Dictionary<char, string>();

            // If the line begins an answer option
            if (isAnAnswerOption(line))
            {
                // Find the beginning of each answer option
                List<int> answerOptionIndices = new List<int>();    // Letter indices (e.g. index of 'a')
                for (int i = 0; i < line.Length - 1; i++)
                {
                    if (isAnAnswerOption(line.Substring(i, 2)))
                        answerOptionIndices.Add(i);
                }
                answerOptionIndices.Add(line.Length);   // Add an index marker just past the end of the list

                // Parse each answer option
                for (int i = 0; i < answerOptionIndices.Count - 1; i++)
                {
                    int index1 = answerOptionIndices[i];
                    int index2 = answerOptionIndices[i + 1];

                    // Parse the answer option
                    KeyValuePair<char, String> answerOption = parseAnswerOption(
                        line.Substring(index1, index2 - index1).Trim());
                    char letter = answerOption.Key;
                    String text = answerOption.Value;

                    // Add the answer option to the question
                    if (answerOptions.ContainsKey(letter))
                        Console.WriteLine("Problem: Duplicate answer option in parseAnswerOptions().");
                    answerOptions[letter] = text;
                }
            }

            else
            {
                Console.WriteLine("Problem: Invalid input to parseAnswerOptions()");
            }

            return answerOptions;
        }

        /*
         * Parses and returns a correct answer.
         */
        private static char parseCorrectAnswer(String line)
        {
            // If the line begins a correct answer
            if (isACorrectAnswer(line))
            {
                // Parse and return the capital letter from A-D
                for (int i = line.IndexOf('.') + 1; i < line.Length; i++)
                {
                    if (isACapitalLetterFromAToD(line[i]))
                        return Convert.ToChar(line[i].ToString().ToLower());
                }
            }

            else
            {
                Console.WriteLine("Problem: Invalid input to parseCorrectAnswer().");
            }

            return ' ';
        }

        /*
         * Parses and returns a correct answer explanation.
         */
        private static String parseCorrectAnswerExplanation(String line)
        {
            String correctAnswerExplanation = "";

            // If the line begins a correct answer explanation
            if (isACorrectAnswerExplanation(line))
            {
                // Add the line to the explanation (ignore source information)
                if (line.Contains("SOURCE:"))
                    correctAnswerExplanation += line.Substring(0, line.IndexOf("SOURCE:")).Trim();
                else
                    correctAnswerExplanation += line.Trim();
            }

            else
            {
                Console.WriteLine("Problem: Invalid input to parseCorrectAnswerExplanation()");
            }

            return correctAnswerExplanation;
        }




        /*
         * Returns true if the line is a page header.
         * (The line contains "EXAM",
         * or starts with "Test " and a digit,
         * or starts with a 4-digit sequence and contains "DECA".)
         */
        private static bool isAPageHeader(String line)
        {
            if (line.Contains("EXAM"))
                return true;
            else if (line.Length > 5 && line.StartsWith("Test ") && isADigit(line[5]))
                return true;
            else if (line.Length >= 4 && isADigitSequence(line.Substring(0, 4)) && line.Contains("DECA"))
                return true;
            else if (line.Length >= 4 && isADigitSequence(line.Substring(0, 4)) && line.Contains("HS"))
                return true;
            else if (line.Contains("Exam Key"))
                return true;
            else
                return false;
        }

        /*
         * Returns true if the line is a page footer.
         * (The line starts with "Copyright",
         * or contains "Students ‘Demonstrating Excellence Celebrating Achievement’".)
         */
        private static bool isAPageFooter(String line)
        {
            if (line.StartsWith("Copyright"))
                return true;
            else if (line.Contains("Columbus, Ohio"))
                return true;
            else if (line.Contains("Students ‘Demonstrating Excellence Celebrating Achievement’"))
                return true;
            else
                return false;
        }

        /*
         * Returns true if the line is a page number.
         * (The entire line is a digit sequence.)
         */
        private static bool isAPageNumber(String line)
        {
            if (isADigitSequence(line))
                return true;
            else
                return false;
        }




        /*
         * Returns true if the line begins a question text.
         * (The line begins with a question number and is followed by multiple non-space characters before any answer options).
         */
        private static bool isText(String line)
        {
            // If the line begins with a question number
            if (beginsWithAQuestionNumber(line))
            {
                // Current character index
                int characterIndex = line.IndexOf('.') + 1;

                // Find where the question text stops (an answer option may begin)
                int secondaryCharacterIndex = characterIndex;
                for (; secondaryCharacterIndex < line.Length; secondaryCharacterIndex++)
                {
                    if ( isAnAnswerOption(line.Substring(secondaryCharacterIndex)) )
                        break;
                }

                // Count the number of non-spaces in the question text
                int numberOfNonSpaces = 0;
                for (int i = characterIndex; i < secondaryCharacterIndex; i++)
                {
                    if (IsNullOrWhiteSpace(line[i].ToString()) == false)
                        numberOfNonSpaces++;
                }

                // Return true if there are multiple non-space characters
                if (numberOfNonSpaces > 1)
                    return true;
            }

            return false;
        }

        /*
         * Returns true if the line begins an answer option.
         * (The first character is a capital letter from A-D and the second character is a period).
         */
        private static bool isAnAnswerOption(String line)
        {
            if (line.Length > 1 && isACapitalLetterFromAToD(line[0]) && line[1] == '.')
                return true;
            else
                return false;
        }

        /*
         * Returns true if the line begins a correct answer.
         * (Begins with a question number, followed by whitespace and a single letter from A-D).
         */
        private static bool isACorrectAnswer(String line)
        {
            // If the line begins with a question number
            if (beginsWithAQuestionNumber(line))
            {
                // Return true if a character is a capital letter from A-D and is followed by a space, or is the last letter
                for (int i = line.IndexOf('.') + 1; i < line.Length; i++)
                {
                    if (isACapitalLetterFromAToD(line[i]))
                    {
                        if (i == (line.Length - 1) || line[i + 1] == ' ')
                            return true;
                        else
                            break;
                    }

                }
            }

            return false;
        }

        /*
         * Returns true if the line is a question correct answer explanation.
         * (The line does not begin with a question number and is not an answer option).
         */
        private static bool isACorrectAnswerExplanation(String line)
        {
            if (beginsWithAQuestionNumber(line) == false && isAnAnswerOption(line) == false)
                return true;
            else
                return false;
        }


        

        /*
         * Returns true if the string is null, empty, or consists only of white-space characters.
         */
        public static bool IsNullOrWhiteSpace(String str)
        {
            return (String.IsNullOrEmpty(str) || str.Trim().Length == 0);
        }

        /*
         * Returns true if the character is a digit (from 0-9).
         */
        private static bool isADigit(char character)
        {
            if ("0123456789".Contains(character))
                return true;
            else
                return false;
        }

        /*
         * Returns true if the string is a sequence of only digits.
         */
        public static bool isADigitSequence(String sequence)
        {
            if (sequence.Length == 0)
                return false;

            foreach (char character in sequence)
            {
                if (isADigit(character) == false)
                    return false;
            }

            return true;
        }

        /*
         * Returns true if the character is a capital letter from A-D.
         */
        private static bool isACapitalLetterFromAToD(char character)
        {
            if ("ABCD".Contains(character))
                return true;
            else
                return false;
        }

        /*
         * Returns true if the character is a letter from a-z (ignoring case).
         */
        private static bool isALetter(char character)
        {
            if ( "abcdefghijklmnopqrstuvwxyz".Contains(character.ToString().ToLower()) )
                return true;
            else
                return false;
        }

        /*
         * Returns true if the line begins with a question number (a digit sequence and a period).
         */
        private static bool beginsWithAQuestionNumber(String line)
        {
            // If the line contains a period before index 5 and the characters before it are a digit sequence
            if ( line.Contains('.') && line.IndexOf('.') < 5 && isADigitSequence(line.Substring(0, line.IndexOf('.'))) )
                return true;
            else
                return false;
        }

    }

}
