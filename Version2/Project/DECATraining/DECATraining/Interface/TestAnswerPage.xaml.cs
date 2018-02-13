using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DECATraining
{

    /// <summary>
    /// Interaction logic for TestAnswerPage.xaml
    /// </summary>
    public partial class TestAnswerPage : Page
    {

        // The window that presented the page as its content
        public Window contentWindow { get; set; }

        // The test set to present
        public TestSet testSet { get; set; }

        // The current test question to present
        public TestQuestion currentTestQuestion { get; set; }

        // Show only wrong answers (true), or show all answers (false)
        public bool onlyShowWrongAnswers { get; set; }




        /*
         * Constructor.
         */
        public TestAnswerPage(Window contentWindow, TestSet testSet, bool onlyShowWrongAnswers)
        {
            InitializeComponent();

            this.contentWindow = contentWindow;
            this.testSet = testSet;
            this.currentTestQuestion = null;
            this.onlyShowWrongAnswers = onlyShowWrongAnswers;

            this.testSet.currentRoundQuestionIndex = 0;

            updatePage();
        }




        /*
         * Updates the page.
         */
        private void updatePage()
        {
            if (shouldShowAnswer(testSet.currentRoundQuestionIndex) == false)
                goToNextAnswer();

            // If there are more questions to present
            if (testSet.currentRoundQuestionIndex < testSet.numQuestionsPerRound)
            {
                // If there is no current test question reference or the reference needs to be updated, update the page
                if (currentTestQuestion == null || currentTestQuestion != testSet.getCurrentQuestion())
                {
                    // Update the current question reference
                    currentTestQuestion = testSet.getCurrentQuestion();

                    // Test label
                    testLabel.Content = testSet.testSetInfo.testSetStringLong;

                    // Round number
                    roundNumberLabel.Content = "Round " + (testSet.currentRoundIndex + 1) + " / " + testSet.numRounds;

                    // Question number
                    questionNumberLabel.Content = "Question " + (testSet.currentRoundQuestionIndex + 1) + " / " + testSet.numQuestionsPerRound;

                    // Question text block
                    questionTextBlock.Text = currentTestQuestion.text;

                    // User answer - text
                    if (currentTestQuestion.didUserAnswerQuestion())
                        userAnswerLabel.Content =
                            currentTestQuestion.userAnswer + ") " + currentTestQuestion.answerOptions[currentTestQuestion.userAnswer];
                    else
                        userAnswerLabel.Content = "No answer";

                    // User answer - colour
                    if (currentTestQuestion.isUserAnswerCorrect())
                        userAnswerLabel.Foreground = new SolidColorBrush(Colors.Green);
                    else
                        userAnswerLabel.Foreground = new SolidColorBrush(Colors.Red);

                    // Correct answer
                    correctAnswerLabel.Content =
                        "Answer: " + currentTestQuestion.correctAnswer + ") " + currentTestQuestion.answerOptions[currentTestQuestion.correctAnswer];

                    // Correct answer explanation
                    correctAnswerExplanationTextBlock.Text = currentTestQuestion.correctAnswerExplanation;

                    // Back button
                    updateBackButton();

                    // Next button
                    updateNextButton();
                }
            }

            else
            {
                // Go to the test score page
                contentWindow.Content = new TestScorePage(contentWindow, testSet);
            }
        }

        /*
         * Updates the Back button.
         */
        private void updateBackButton()
        {
            if (isFirstAnswerToShow() == false)
                backButton.IsEnabled = true;
            else
                backButton.IsEnabled = false;

            backButton.IsDefault = false;
        }

        /*
         * Updates the Next button.
         */
        private void updateNextButton()
        {
            if (isLastAnswerToShow() == false)
                nextButton.Content = "Next >";
            else
                nextButton.Content = "Finish";

            nextButton.IsEnabled = true;
            nextButton.IsDefault = true;
        }




        /*
         * Returns true if the given answer number in the round should be shown.
         */
        private bool shouldShowAnswer(int roundQuestionIndex)
        {
            if (onlyShowWrongAnswers &&
                roundQuestionIndex >= 0 &&
                roundQuestionIndex < testSet.numQuestionsPerRound &&
                testSet.getQuestion(testSet.currentRoundIndex, roundQuestionIndex).isUserAnswerCorrect())
                return false;
            else
                return true;
        }

        /*
         * Returns true if there were no answers shown before the current answer.
         */
        private bool isFirstAnswerToShow()
        {
            for (int i = testSet.currentRoundQuestionIndex - 1; i >= 0; i--)
            {
                if (shouldShowAnswer(i))
                    return false;
            }

            return true;
        }

        /*
         * Returns true if there are no more answers to show after the current answer.
         */
        private bool isLastAnswerToShow()
        {
            for (int i = testSet.currentRoundQuestionIndex + 1; i < testSet.numQuestionsPerRound; i++)
            {
                if (shouldShowAnswer(i))
                    return false;
            }

            return true;
        }




        /*
         * Changes the current round question index to the previous answer to show.
         */
        private void goToPreviousAnswer()
        {
            if (testSet.currentRoundQuestionIndex > 0 && isFirstAnswerToShow() == false)
            {
                testSet.currentRoundQuestionIndex--;

                while (testSet.currentRoundQuestionIndex > 0 && shouldShowAnswer(testSet.currentRoundQuestionIndex) == false)
                    testSet.currentRoundQuestionIndex--;
            }

            updatePage();
        }

        /*
         * Changes the current round question index to the next answer to show.
         */
        private void goToNextAnswer()
        {
            if (testSet.currentRoundQuestionIndex < testSet.numQuestionsPerRound)
            {
                testSet.currentRoundQuestionIndex++;

                while (testSet.currentRoundQuestionIndex < testSet.numQuestionsPerRound && shouldShowAnswer(testSet.currentRoundQuestionIndex) == false)
                    testSet.currentRoundQuestionIndex++;
            }

            updatePage();
        }




        /*
         * Next button event handler.
         */
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            goToNextAnswer();
        }

        /*
         * Back button event handler.
         */
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            goToPreviousAnswer();
        }

        /*
         * Back to Score button event handler.
         */
        private void scoreButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to the score page
            contentWindow.Content = new TestScorePage(contentWindow, testSet);
        }

    }

}
