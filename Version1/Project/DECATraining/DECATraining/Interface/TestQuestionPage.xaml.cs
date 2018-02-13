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
    /// Interaction logic for TestQuestionPage.xaml
    /// </summary>
    public partial class TestQuestionPage : Page
    {

        // The window that presented the page as its content
        public Window contentWindow { get; set; }

        // The test set to present
        public TestSet testSet { get; set; }

        // The current test question to present
        public TestQuestion currentTestQuestion { get; set; }




        /*
         * Constructor.
         */
        public TestQuestionPage(Window contentWindow, TestSet testSet)
        {
            InitializeComponent();

            this.contentWindow = contentWindow;
            this.testSet = testSet;
            this.currentTestQuestion = null;

            this.testSet.currentRoundQuestionIndex = 0;

            updatePage();
        }




        /*
         * Updates the page.
         */
        private void updatePage()
        {
            // If there are more questions to present
            if (testSet.currentRoundQuestionIndex < testSet.numQuestionsPerRound)
            {
                // If there is no current question reference or the reference needs to be updated, update the page
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

                    // Answer option A
                    if (currentTestQuestion.answerOptions.ContainsKey('a'))
                    {
                        optionARadioButton.IsEnabled = true;
                        optionARadioButton.Content = "a) " + currentTestQuestion.answerOptions['a'];
                    }
                    else
                    {
                        optionARadioButton.IsEnabled = false;
                        optionARadioButton.Content = "";
                    }

                    // Answer option B
                    if (currentTestQuestion.answerOptions.ContainsKey('b'))
                    {
                        optionBRadioButton.IsEnabled = true;
                        optionBRadioButton.Content = "b) " + currentTestQuestion.answerOptions['b'];
                    }
                    else
                    {
                        optionBRadioButton.IsEnabled = false;
                        optionBRadioButton.Content = "";
                    }

                    // Answer option C
                    if (currentTestQuestion.answerOptions.ContainsKey('c'))
                    {
                        optionCRadioButton.IsEnabled = true;
                        optionCRadioButton.Content = "c) " + currentTestQuestion.answerOptions['c'];
                    }
                    else
                    {
                        optionCRadioButton.IsEnabled = false;
                        optionCRadioButton.Content = "";
                    }

                    // Answer option D
                    if (currentTestQuestion.answerOptions.ContainsKey('d'))
                    {
                        optionDRadioButton.IsEnabled = true;
                        optionDRadioButton.Content = "d) " + currentTestQuestion.answerOptions['d'];
                    }
                    else
                    {
                        optionDRadioButton.IsEnabled = false;
                        optionDRadioButton.Content = "";
                    }

                    // Deselect the answer options
                    optionARadioButton.IsChecked = false;
                    optionBRadioButton.IsChecked = false;
                    optionCRadioButton.IsChecked = false;
                    optionDRadioButton.IsChecked = false;

                    // If an answer option was previously selected, select the radio button
                    if      (currentTestQuestion.userAnswer == 'a')
                        optionARadioButton.IsChecked = true;
                    else if (currentTestQuestion.userAnswer == 'b')
                        optionBRadioButton.IsChecked = true;
                    else if (currentTestQuestion.userAnswer == 'c')
                        optionCRadioButton.IsChecked = true;
                    else if (currentTestQuestion.userAnswer == 'd')
                        optionDRadioButton.IsChecked = true;

                    // Warning label
                    warningLabel.Visibility = Visibility.Hidden;

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
            if (testSet.currentRoundQuestionIndex > 0)
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
            if (testSet.currentRoundQuestionIndex < testSet.numQuestionsPerRound - 1)
                nextButton.Content = "Next >";
            else
                nextButton.Content = "Finish Round";

            if (currentTestQuestion.didUserAnswerQuestion())
            {
                nextButton.IsEnabled = true;
                nextButton.IsDefault = true;
            }
            else
            {
                nextButton.IsEnabled = false;
                nextButton.IsDefault = false;
            }
        }




        /*
         * Next button event handler.
         */
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            // If an answer has been selected
            if (currentTestQuestion.didUserAnswerQuestion())
            {
                // Move to the next question
                testSet.currentRoundQuestionIndex++;
                updatePage();
            }

            else
            {
                // Show the warning
                warningLabel.Visibility = Visibility.Visible;
            }
        }

        /*
         * Back button event handler.
         */
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // Move to the previous question
            if (testSet.currentRoundQuestionIndex > 0)
                testSet.currentRoundQuestionIndex--;

            updatePage();
        }

        /*
         * Option radio button event handlers.
         */
        private void optionARadioButton_Checked(object sender, RoutedEventArgs e)
        {
            currentTestQuestion.userAnswer = 'a';
            updateNextButton();
        }
        private void optionBRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            currentTestQuestion.userAnswer = 'b';
            updateNextButton();
        }
        private void optionCRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            currentTestQuestion.userAnswer = 'c';
            updateNextButton();
        }
        private void optionDRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            currentTestQuestion.userAnswer = 'd';
            updateNextButton();
        }

        /*
         * End Round button event handler.
         */
        private void endRoundButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to the test score page
            contentWindow.Content = new TestScorePage(contentWindow, testSet);
        }

    }

}
