using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DECATraining
{

    /*
     * A class used to store the data from one test question.
     */
    public class TestQuestion
    {

        public String text { get; set; }
        public Dictionary<char, String> answerOptions { get; set; } // lowercase letters a-d

        public char correctAnswer { get; set; }                     // lowercase letters a-d
        public String correctAnswerExplanation { get; set; }

        public char userAnswer { get; set; }                        // lowercase letters a-d




        /*
         * Constructor.
         */
        public TestQuestion()
        {
            this.text = "";
            this.answerOptions = new Dictionary<char, string>();
            this.correctAnswer = ' ';
            this.correctAnswerExplanation = "";
            this.userAnswer = ' ';
        }




        /*
         * Adds answer data (correct answer and explanation) to the question.
         */
        public void addAnswer(char correctAnswer, String correctAnswerExplanation)
        {
            this.correctAnswer = correctAnswer;
            this.correctAnswerExplanation = correctAnswerExplanation;
        }




        /*
         * Returns false if any of the question data is incomplete or invalid.
         * (Does not check the user answer.)
         */
        public bool isQuestionDataCompleteAndValid()
        {
            // Text
            if (TestFileReader.IsNullOrWhiteSpace(text))
                return false;
            if (text.Contains('ó'))
                return false;

            // Answer options - keys
            foreach (char answerOption in "abcd")
            {
                if (answerOptions.ContainsKey(answerOption) == false)
                    return false;
            }

            // Answer options - values
            foreach (String answerOption in answerOptions.Values)
            {
                if (TestFileReader.IsNullOrWhiteSpace(answerOption))
                    return false;
                if (answerOption.Contains('ó'))
                    return false;
            }

            // Correct answer
            if (answerOptions.Keys.Contains(correctAnswer) == false)
                return false;

            // Correct answer explanation
            if (TestFileReader.IsNullOrWhiteSpace(correctAnswerExplanation))
                return false;
            if (correctAnswerExplanation.Contains('ó'))
                return false;

            return true;
        }

        /*
         * Returns true if the user's answer is the same as the correct answer.
         */
        public bool isUserAnswerCorrect()
        {
            if (userAnswer == correctAnswer)
                return true;
            else
                return false;
        }

        /*
         * Returns true if the user answered the question.
         */
        public bool didUserAnswerQuestion()
        {
            if ("abcd".Contains(userAnswer))
                return true;
            else
                return false;
        }

    }

}
