using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DECATraining
{

    public class TestSet
    {

        public Info.CategoryInfo categoryInfo { get; set; }
        public Info.TestSetInfo testSetInfo { get; set; }

        public List<TestQuestion> questions { get; set; }

        public int numRounds { get; set; }
        public int numQuestionsPerRound { get; set; }

        public int currentRoundIndex { get; set; }
        public int currentRoundQuestionIndex { get; set; }




        /*
         * Constructor
         */
        public TestSet(Info.CategoryInfo categoryInfo, Info.TestSetInfo testSetInfo, List<Test> tests, int numRounds, int numQuestionsPerRound)
        {
            this.categoryInfo = categoryInfo;
            this.testSetInfo = testSetInfo;

            this.questions = new List<TestQuestion>();
            foreach (Test test in tests)
                this.questions.AddRange(test.questions);

            this.numRounds = numRounds;
            this.numQuestionsPerRound = numQuestionsPerRound;

            this.currentRoundIndex = 0;
            this.currentRoundQuestionIndex = 0;
        }




        /*
         * Removes the questions with incomplete or invalid data from the test set.
         */
        public void removeIncompleteOrInvalidQuestions()
        {
            for (int i = questions.Count - 1; i >= 0; i--)
            {
                if (questions[i].isQuestionDataCompleteAndValid() == false)
                    questions.RemoveAt(i);
            }
        }

        /*
         * Randomizes the questions in the test set.
         */
        public void randomizeQuestions()
        {
            List<TestQuestion> newQuestions = new List<TestQuestion>();

            // Create a list of numbers, each representing a question index
            List<int> questionIndices = new List<int>();
            for (int i = 0; i < questions.Count; i++)
                questionIndices.Add(i);

            // Randomly choose and add the questions to the new questions list
            Random random = new Random();
            for (int i = 0; i < questions.Count; i++)
            {
                // Generate the question index
                int questionIndicesIndex = random.Next(0, questionIndices.Count);

                // Add the question to the new questions list
                newQuestions.Add(questions[questionIndices[questionIndicesIndex]]);

                // Remove the question index from the list
                questionIndices.RemoveAt(questionIndicesIndex);
            }

            questions = newQuestions;
        }




        /*
         * Returns the question index for the specified round and question within that round.
         */
        public int getQuestionIndex(int roundIndex, int roundQuestionIndex)
        {
            return (roundIndex * numQuestionsPerRound) + roundQuestionIndex;
        }

        /*
         * Returns the question for the specified round and question within that round.
         */
        public TestQuestion getQuestion(int roundIndex, int roundQuestionIndex)
        {
            int questionIndex = getQuestionIndex(roundIndex, roundQuestionIndex);

            if (questionIndex >= 0 && questionIndex < questions.Count)
                return questions[questionIndex];
            else
                return null;
        }

        /*
         * Returns the question for the current round and question within that round.
         */
        public TestQuestion getCurrentQuestion()
        {
            return getQuestion(currentRoundIndex, currentRoundQuestionIndex);
        }




        /*
         * Returns the number of questions to be answered.
         */
        public int numQuestions()
        {
            return numRounds * numQuestionsPerRound;
        }

        /*
         * Returns the number of questions available to use.
         */
        public int numQuestionsAvailable()
        {
            return questions.Count;
        }

        /*
         * Returns the number of questions the user answered.
         */
        public int numQuestionsAnswered()
        {
            int numAnswered = 0;

            foreach (TestQuestion question in questions)
            {
                if (question.didUserAnswerQuestion())
                    numAnswered++;
            }

            return numAnswered;
        }




        /*
         * Returns the user's score (number of questions answered correctly) in the specified round.
         */
        public int userScoreInRound(int roundIndex)
        {
            int startQuestionIndex = getQuestionIndex(roundIndex, 0);
            int endQuestionIndex = getQuestionIndex(roundIndex + 1, 0);

            int score = 0;

            foreach (TestQuestion question in questions.GetRange(startQuestionIndex, endQuestionIndex - startQuestionIndex))
            {
                if (question.isUserAnswerCorrect())
                    score++;
            }

            return score;
        }

        /*
         * Returns the user's score (number of questions answered correctly) in the current round.
         */
        public int userScoreInCurrentRound()
        {
            return userScoreInRound(currentRoundIndex);
        }

        /*
         * Returns the user's score (number of questions answered correctly) up to and including the specified round.
         */
        public int userScoreUpToRound(int roundIndex)
        {
            int score = 0;

            for (int i = 0; i <= roundIndex; i++)
                score += userScoreInRound(i);

            return score;
        }

        /*
         * Returns the user's score (number of questions answered correctly) up to and including the current round.
         */
        public int userScoreUpToCurrentRound()
        {
            return userScoreUpToRound(currentRoundIndex);
        }

        /*
         * Returns the user's score (number of questions answered correctly) in the entire test set.
         */
        public int userScore()
        {
            int score = 0;

            for (int i = 0; i < numRounds; i++)
                score += userScoreInRound(i);

            return score;
        }

    }

}
