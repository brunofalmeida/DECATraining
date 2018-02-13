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
    /// Interaction logic for SelectionPage.xaml
    /// </summary>
    public partial class SelectionPage : Page
    {
        // Available numbers of rounds
        private static List<int> AVAILABLE_ROUNDS = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 15, 20 };
        // Available numbers of questions
        private static List<int> AVAILABLE_QUESTIONS = new List<int>() { 1, 5, 10 };




        // The window that presented the page as its content
        public Window contentWindow { get; set; }

        // The available categories, test sets, and numbers of questions
        public List<Info.CategoryInfo> availableCategories { get; set; }
        public List<Info.TestSetInfo> availableTestSets { get; set; }
        public List<int> availableRounds { get; set; }
        public List<int> availableQuestions { get; set; }




        /*
         * Constructor.
         */
        public SelectionPage(Window contentWindow)
        {
            InitializeComponent();

            this.contentWindow = contentWindow;

            this.availableCategories = Info.availableCategories;
            this.availableTestSets = new List<Info.TestSetInfo>();
            this.availableRounds = AVAILABLE_ROUNDS;
            this.availableQuestions = AVAILABLE_QUESTIONS;

            updatePage();
        }

        /*
         * Constructor with default category and test.
         */
        public SelectionPage(Window contentWindow, Info.CategoryInfo defaultCategoryInfo, Info.TestSetInfo defaultTestSetInfo)
        {
            InitializeComponent();     

            this.contentWindow = contentWindow;

            this.availableCategories = Info.availableCategories;
            this.availableTestSets = new List<Info.TestSetInfo>();
            this.availableRounds = AVAILABLE_ROUNDS;
            this.availableQuestions = AVAILABLE_QUESTIONS;

            updatePage();

            setDefaults(defaultCategoryInfo, defaultTestSetInfo);
        }




        /*
         * Sets a default category and test set.
         */
        private void setDefaults(Info.CategoryInfo defaultCategoryInfo, Info.TestSetInfo defaultTestSetInfo)
        {
            updatePage();

            enableCategoryComboBox();
            categoryComboBox.SelectedIndex = availableCategories.IndexOf(defaultCategoryInfo);

            enableTestComboBox(defaultCategoryInfo);
            testComboBox.SelectedIndex = availableTestSets.IndexOf(defaultTestSetInfo);

            enableRoundsComboBox();
        }

        /*
         * Sets a default category.
         */
        private void setDefaults(Info.CategoryInfo defaultCategoryInfo)
        {
            updatePage();

            enableCategoryComboBox();
            categoryComboBox.SelectedIndex = availableCategories.IndexOf(defaultCategoryInfo);

            enableTestComboBox(defaultCategoryInfo);
        }




        /*
         * Updates the page.
         */
        private void updatePage()
        {
            disableComboBoxes();
            hideWarning();

            enableCategoryComboBox();

            updateStartButton();
        }

        /*
         * Updates the start button.
         * (Enables it if the category, test, rounds, and questions are selected, or disables it otherwise.)
         */
        private void updateStartButton()
        {
            if (isACategorySelected() && isATestSelected() && areRoundsSelected() && areQuestionsSelected())
                startButton.IsEnabled = true;
            else
                startButton.IsEnabled = false;
        }




        /*
         * Enables the category combo box and sets its items.
         */
        private void enableCategoryComboBox()
        {
            categoryComboBox.IsEnabled = true;

            List<String> categoryStrings = new List<String>();
            foreach (Info.CategoryInfo categoryInfo in availableCategories)
                categoryStrings.Add(categoryInfo.categoryString);

            categoryComboBox.ItemsSource = categoryStrings;
            categoryComboBox.SelectedIndex = -1;

            updateStartButton();
        }

        /*
         * Enables the test combo box and sets its items to match the category.
         */
        private void enableTestComboBox(Info.CategoryInfo categoryInfo)
        {
            testComboBox.IsEnabled = true;

            availableTestSets = Info.availableTestSets[categoryInfo.categoryName];

            List<String> testSetStrings = new List<String>();
            foreach (Info.TestSetInfo testSetInfo in availableTestSets)
                testSetStrings.Add(testSetInfo.testSetStringShort);

            testComboBox.ItemsSource = testSetStrings;
            testComboBox.SelectedIndex = -1;

            updateStartButton();
        }

        /*
         * Enables the rounds combo box and sets its items.
         */
        private void enableRoundsComboBox()
        {
            roundsComboBox.IsEnabled = true;

            List<String> roundStrings = new List<String>();
            foreach (int roundNum in availableRounds)
                roundStrings.Add(roundNum.ToString());

            roundsComboBox.ItemsSource = roundStrings;
            roundsComboBox.SelectedIndex = -1;

            updateStartButton();
        }

        /*
         * Enables the question combo box and sets its items.
         */
        private void enableQuestionsComboBox()
        {
            questionsComboBox.IsEnabled = true;

            List<String> questionStrings = new List<String>();
            foreach (int questionNum in availableQuestions)
                questionStrings.Add(questionNum.ToString());

            questionsComboBox.ItemsSource = questionStrings;
            questionsComboBox.SelectedIndex = -1;

            updateStartButton();
        }

        




        /*
         * Disables all of the combo boxes.
         */
        private void disableComboBoxes()
        {
            categoryComboBox.SelectedIndex = -1;
            testComboBox.SelectedIndex = -1;
            roundsComboBox.SelectedIndex = -1;
            questionsComboBox.SelectedIndex = -1;

            categoryComboBox.IsEnabled = false;
            testComboBox.IsEnabled = false;
            roundsComboBox.IsEnabled = false;
            questionsComboBox.IsEnabled = false;
        }

        /*
         * Hides the warning label.
         */
        private void hideWarning()
        {
            warningLabel.Visibility = Visibility.Hidden;
        }




        /*
         * Returns true if a category is selected in the combo box.
         */
        private bool isACategorySelected()
        {
            if (categoryComboBox.SelectedIndex != -1)
                return true;
            else
                return false;
        }

        /*
         * Returns true if a test is selected in the combo box.
         */
        private bool isATestSelected()
        {
            if (testComboBox.SelectedIndex != -1)
                return true;
            else
                return false;
        }

        /*
         * Returns true if a number of rounds is selected in the combo box.
         */
        private bool areRoundsSelected()
        {
            if (roundsComboBox.SelectedIndex != -1)
                return true;
            else
                return false;
        }

        /*
        * Returns true if a number of questions is selected in the combo box.
        */
        private bool areQuestionsSelected()
        {
            if (questionsComboBox.SelectedIndex != -1)
                return true;
            else
                return false;
        }




        /*
         * Returns the category selected in the combo box.
         */
        private Info.CategoryInfo getSelectedCategory()
        {
            int categoryIndex = categoryComboBox.SelectedIndex;
            return availableCategories[categoryIndex];
        }

        /*
         * Returns the test set selected in the combo box.
         */
        private Info.TestSetInfo getSelectedTest()
        {
            int testIndex = testComboBox.SelectedIndex;
            return availableTestSets[testIndex];
        }

        /*
         * Returns the number of rounds selected in the combo box.
         */
        private int getSelectedRounds()
        {
            int roundsIndex = roundsComboBox.SelectedIndex;
            return availableRounds[roundsIndex];
        }

        /*
         * Returns the number of questions selected in the combo box.
         */
        private int getSelectedQuestions()
        {
            int questionsIndex = questionsComboBox.SelectedIndex;
            return availableQuestions[questionsIndex];
        }




        /*
         * Starts the test set with the given information.
         * If the files are read successfully, starts the test.
         * Otherwise, displays an error dialog.
         */
        private void startTestSet(Info.CategoryInfo categoryInfo, Info.TestSetInfo testSetInfo, int numRounds, int numQuestionsPerRound)
        {
            // Read the test set
            TestSet testSet = TestFileReader.readTestSet(categoryInfo, testSetInfo);

            // If the test set files were read successfully
            if (testSet != null)
            {
                // Remove the bad questions
                testSet.removeIncompleteOrInvalidQuestions();

                // Randomize the questions
                testSet.randomizeQuestions();

                // Calculate the total number of questions
                int numQuestions = numRounds * numQuestionsPerRound;

                // If there are enough questions in the test set
                if (numQuestions <= testSet.numQuestionsAvailable())
                {
                    // Set the number of rounds and questions
                    testSet.numRounds = numRounds;
                    testSet.numQuestionsPerRound = numQuestionsPerRound;

                    // Go to the test start page
                    contentWindow.Content = new TestStartPage(contentWindow, testSet);
                }

                // If there are not enough questions in the test set
                else
                {
                    MessageBox.Show(
                        "Only " + testSet.numQuestionsAvailable() + " questions are available for \"" + testSetInfo.testSetStringLong + "\"." +
                            " You have selected " + numQuestions + " questions.",
                        "Not Enough Questions Available",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                        );

                    updatePage();
                    setDefaults(categoryInfo, testSetInfo);
                }
            }

            // If the test set files were not read
            else
            {
                MessageBox.Show(
                    "The test files for \"" + testSetInfo.testSetStringLong + "\" could not be found." +
                        "\n\nMake sure that the test files are located in the Tests folder." +
                        " If this error persists, redownload the application.",
                    "Test Files Not Found",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );

                updatePage();
                setDefaults(categoryInfo);
            }
        }




        /*
         * Category combo box event handler.
         */
        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isACategorySelected())
                enableTestComboBox(getSelectedCategory());
        }

        /*
         * Test combo box event handler.
         */
        private void testComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isATestSelected())
                enableRoundsComboBox();
        }

        /*
         * Rounds combo box event handler.
         */
        private void roundsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (areRoundsSelected())
                enableQuestionsComboBox();
        }

        /*
         * Questions combo box event handler.
         */
        private void questionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (areQuestionsSelected())
                updateStartButton();
        }

        /*
         * Start button event handler.
         */
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide the warning label
            hideWarning();

            // If a category is selected
            if (isACategorySelected())
            {
                // If a test is selected
                if (isATestSelected())
                {
                    // If a number of rounds is selected
                    if (areRoundsSelected())
                    {
                        // If a number of questions is selected
                        if (areQuestionsSelected())
                        {
                            Info.CategoryInfo categoryInfo = getSelectedCategory();
                            Info.TestSetInfo testSetInfo = getSelectedTest();
                            int numRounds = getSelectedRounds();
                            int numQuestions = getSelectedQuestions();

                            startTestSet(categoryInfo, testSetInfo, numRounds, numQuestions);
                        }

                        // If a number of questions is not selected
                        else
                        {
                            warningLabel.Content = "You must select the questions per round.";
                            warningLabel.Visibility = Visibility.Visible;
                        }
                    }

                    // If a number of rounds is not selected
                    else
                    {
                        warningLabel.Content = "You must select the rounds.";
                        warningLabel.Visibility = Visibility.Visible;
                    } 
                }

                // If a test is not selected
                else
                {
                    warningLabel.Content = "You must select a test.";
                    warningLabel.Visibility = Visibility.Visible;
                }
            }

            // If a category is not selected
            else
            {
                warningLabel.Content = "You must select a category.";
                warningLabel.Visibility = Visibility.Visible;
            }
        }

    }

}
