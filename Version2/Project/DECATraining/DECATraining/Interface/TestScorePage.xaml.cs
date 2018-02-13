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
    /// Interaction logic for TestScorePage.xaml
    /// </summary>
    public partial class TestScorePage : Page
    {

        // The window that presented the page as its content
        public Window contentWindow { get; set; }

        // The test that was completed
        public TestSet testSet { get; set; }




        /*
         * Constructor.
         */
        public TestScorePage(Window contentWindow, TestSet testSet)
        {
            InitializeComponent();

            this.contentWindow = contentWindow;
            this.testSet = testSet;

            updatePage();
        }




        /*
         * Updates the page.
         */
        private void updatePage()
        {
            // Test name label
            testLabel.Content = testSet.testSetInfo.testSetStringLong;

            // Rounds completed label
            if (testSet.numRounds == 1)
                roundsCompletedLabel.Content = "1/1 Round Complete";
            else
                roundsCompletedLabel.Content = (testSet.currentRoundIndex + 1) + "/" + (testSet.numRounds) + " Rounds Complete";

            // Round label
            roundLabel.Content = "Round " + (testSet.currentRoundIndex + 1);

            // Round - fraction and percent labels
            int roundScore = testSet.userScoreInCurrentRound();
            int roundQuestions = testSet.numQuestionsPerRound;
            roundFractionScoreLabel.Content = roundScore + " / " + roundQuestions;
            roundPercentScoreLabel.Content = Math.Round(
                (double)roundScore / roundQuestions * 100.0,
                System.MidpointRounding.AwayFromZero) + "%";

            // Total - fraction and percent labels
            int totalScore = testSet.userScoreUpToCurrentRound();
            int totalQuestions = (testSet.currentRoundIndex + 1) * testSet.numQuestionsPerRound;
            totalFractionScoreLabel.Content = totalScore + " / " + totalQuestions;
            totalPercentScoreLabel.Content = Math.Round(
                (double)totalScore / totalQuestions * 100.0,
                System.MidpointRounding.AwayFromZero) + "%";

            // View Wrong Answers button
            if (roundScore < roundQuestions)
                viewWrongAnswersButton.IsEnabled = true;
            else
                viewWrongAnswersButton.IsEnabled = false;

            // Next Round (or New Test) button
            if (testSet.currentRoundIndex < testSet.numRounds - 1)
                nextRoundButton.Content = "Next Round";
            else
                nextRoundButton.Content = "New Test";
        }




        /*
         * View Wrong Answers button event handler.
         */
        private void viewWrongAnswersButton_Click(object sender, RoutedEventArgs e)
        {
            // If there are wrong answers to show
            if (testSet.userScoreInCurrentRound() < testSet.numQuestionsPerRound)
            {
                // Start with the first answer in the round
                testSet.currentRoundQuestionIndex = 0;

                // Go to the test answer page
                contentWindow.Content = new TestAnswerPage(contentWindow, testSet, true);
            }

            // If all answers are correct
            else
            {
                MessageBox.Show(
                        "All questions were answered correctly.",
                        "No Wrong Answers",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                        );
            }
        }

        /*
         * View All Answers button event handler.
         */
        private void viewAllAnswersButton_Click(object sender, RoutedEventArgs e)
        {
            // Start with the first answer in the round
            testSet.currentRoundQuestionIndex = 0;

            // Go to the test answer page
            contentWindow.Content = new TestAnswerPage(contentWindow, testSet, false);
        }

        /*
         * Next Round button event handler.
         */
        private void nextRoundButton_Click(object sender, RoutedEventArgs e)
        {
            // If there are more rounds in the test
            if (testSet.currentRoundIndex < testSet.numRounds - 1)
            {
                // Go to the next round and start at the first question
                testSet.currentRoundIndex++;
                testSet.currentRoundQuestionIndex = 0;

                // Go to the test question page
                contentWindow.Content = new TestQuestionPage(contentWindow, testSet);
            }

            // If there are no more rounds
            else
            {
                // Call the End Test button event handler
                endTestButton_Click(this, new RoutedEventArgs());
            }
        }

        /*
         * End Test button event handler.
         */
        private void endTestButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to the selection page
            contentWindow.Content = new SelectionPage(contentWindow);
        }

    }

}
