using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DECATraining
{

    /*
     * Used to store the data for a single test.
     */
    public class Test
    {

        public Info.TestName testName { get; set; }
        public List<TestQuestion> questions { get; set; }


        /*
         * Constructor.
         */
        public Test(Info.TestName testName)
        {
            this.testName = testName;
            this.questions = new List<TestQuestion>();
        }


        /*
         * If a question exists for the given index, adds the answer to the test question.
         * Returns true if the operation succeeded, or false if it failed.
         */
        public bool addAnswer(int questionIndex, char correctAnswer, String correctAnswerExplanation)
        {
            if (questionIndex < questions.Count)
            {
                questions[questionIndex].addAnswer(correctAnswer, correctAnswerExplanation);
                return true;
            }
            else
            {
                Console.WriteLine("Problem: Invalid input to addAnswer().");
                return false;
            }
        }

    }

}
