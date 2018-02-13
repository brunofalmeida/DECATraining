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
    /// Interaction logic for TestStartPage.xaml
    /// </summary>
    public partial class TestStartPage : Page
    {

        // The window that presented the page as its content
        public Window contentWindow { get; set; }

        // The test set to present
        public TestSet testSet { get; set; }




        /*
         * Constructor.
         */
        public TestStartPage(Window contentWindow, TestSet testSet)
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

            // Number of rounds label
            if (testSet.numRounds == 1)
                numRoundsLabel.Content = "1 Round";
            else
                numRoundsLabel.Content = testSet.numRounds + " Rounds";

            // Number of questions per round label
            if (testSet.numQuestionsPerRound == 1)
                numQuestionsPerRoundLabel.Content = "1 Question Per Round";
            else
                numQuestionsPerRoundLabel.Content = testSet.numQuestionsPerRound + " Questions Per Round";

            // Number of questions label
            if (testSet.numQuestions() == 1)
                numQuestionsLabel.Content = "(1 Question)";
            else
                numQuestionsLabel.Content = "(" + testSet.numQuestions() + " Questions)";
        }


        /*
         * Start button event handler.
         */
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            // Start with the first round and question
            testSet.currentRoundIndex = 0;
            testSet.currentRoundQuestionIndex = 0;

            // Go to the test question page
            contentWindow.Content = new TestQuestionPage(contentWindow, testSet);
        }

        /*
         * Back button event handler.
         */
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to the selection page
            contentWindow.Content = new SelectionPage(contentWindow, testSet.categoryInfo, testSet.testSetInfo);
        }

    }

}
