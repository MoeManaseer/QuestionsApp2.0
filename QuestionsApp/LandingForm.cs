using LoggerUtils;
using QuestionEntities;
using QuestionsApp;
using QuestionsController;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace QuestionsFormsTest
{
    public partial class LandingForm : Form
    {
        private Controller QuestionsControllerObject;
        private Timer UpdateTimer;

        public LandingForm()
        {
            try
            {
                InitializeComponent();
                QuestionsControllerObject = new Controller();
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        private void LandingFrom_Load(object sender, EventArgs e)
        {
            try
            {
                InitTimer();
                LoadUpdateForm(false);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Initializes the UpdateTimer
        /// </summary>
        private void InitTimer()
        {
            try
            {
                UpdateTimer = new Timer();
                UpdateTimer.Tick += new EventHandler(UpdateTimer_Tick);
                // 20 secs
                UpdateTimer.Interval = 20000;
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Event listener for the UpdateTimer object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                LoadUpdateForm(true);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Util function that loads/updates the data in the QuestionsControllerObject
        /// </summary>
        /// <param name="pIsUpdate">Whether to update or load the data from the database</param>
        public void LoadUpdateForm(bool pIsUpdate)
        {
            int tResultCode = (int)ResultCodesEnum.SUCCESS;

            try
            {
                tResultCode = pIsUpdate ? QuestionsControllerObject.UpdateQuestionsData() : QuestionsControllerObject.FillQuestionsData();

                if (tResultCode == (int) ResultCodesEnum.SUCCESS)
                {
                    UpdateTimer.Start();
                    // Assign the datasource
                    allQuestionsGrid.DataSource = QuestionsControllerObject.QuestionsList;
                    // Enable the form controls
                    ToggleFormControls(true);
                    // Check the list for being empty or not
                    CheckList();
                    // Updates the columns of the DataGridView
                    UpdateQuestionsListTable();
                }
                else
                {
                    MessagesUtility.ShowMessageForm("Form Loading", "The form was loaded", tResultCode);
                    ToggleFormControls(false);
                    allQuestionsGrid.DataSource = null;
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Updates the table columns data in the DataGridView
        /// </summary>
        private void UpdateQuestionsListTable()
        {
            try
            {
                allQuestionsGrid.Columns["Id"].Visible = false;
                allQuestionsGrid.Columns["Order"].Width = 80;
                allQuestionsGrid.Columns["Type"].Width = 80;
                allQuestionsGrid.Columns["Text"].Width = 298;
                allQuestionsGrid.Columns["Text"].DisplayIndex = 3;
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Helper function that disables all controls in the form
        /// </summary>
        private void ToggleFormControls(bool pValue)
        {
            try
            {
                // Disable/Enable every control in the form except for the exit/settings buttons
                foreach (Control tFormControl in Controls)
                {
                    if (!tFormControl.Name.Equals("settingsPanel"))
                    {
                        tFormControl.Enabled = pValue;
                    }
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Helper function that checks whether the questiosn data grid is empty or not, if it is disable the edit/delete buttons
        /// </summary>
        private void CheckList()
        {
            try
            {
                // If there are no items in the DataGridView, disable the Edit/Delete buttons
                ToggleButtons(allQuestionsGrid.RowCount != 0);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Helper function that disables/enabels the edit/add buttons
        /// </summary>
        /// <param name="pValue">bool value to enable/disable the form add/edit buttons</param>
        private void ToggleButtons(bool pValue)
        {
            try
            {
                editBtn.Enabled = pValue;
                removeBtn.Enabled = pValue;
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Event listener that fires whenever a row is selected, when it is enable the Edit/Delete buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allQuestionsGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ToggleButtons(true);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// On click function that launches the question form to add a new question
        /// </summary>
        /// <param name="sender">The control that fired the event</param>
        /// <param name="e">Extra data about the event</param>
        private void addBtn_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateTimer.Stop();
                // Show the QuestionForm and give it the current instance of the QuestionsController
                QuestionForm tQuestionForm = new QuestionForm(QuestionsControllerObject);
                tQuestionForm.ShowDialog();
                UpdateTimer.Start();
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// On click function that launches the question form to edit the selected question
        /// </summary>
        /// <param name="sender">The control that fired the event</param>
        /// <param name="e">Extra data about the event</param>
        private void editBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current selectedRow index
                int tCurrentIndex = allQuestionsGrid.SelectedRows[0].Index;
                UpdateTimer.Stop();
                // Open the questionsForm and pass it the current selected question index
                QuestionForm tQuestionForm = new QuestionForm(QuestionsControllerObject, tCurrentIndex);
                tQuestionForm.ShowDialog();
                UpdateTimer.Start();
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// On click function that removes the currently selected question after getting confirmation from the user
        /// </summary>
        /// <param name="sender">The control that fired the event</param>
        /// <param name="e">Extra data about the event</param>
        private void removeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string tMessage = "Are you sure you want to delete this question? deleted questions are lost forever...";
                string tCaption = "Delete question";
                MessageBoxButtons tMessageButtons = MessageBoxButtons.YesNo;
                DialogResult tResult = MessagesUtility.ShowMessageForm(tCaption, tMessage, (int) ResultCodesEnum.SUCCESS, tMessageButtons);

                if (tResult == DialogResult.Yes)
                {
                    // Get the current selected question index
                    int tCurrentSelectedIndex = allQuestionsGrid.CurrentRow.Index;
                    // Get the current question to be removed
                    Question tSelectedQuestion = QuestionsControllerObject.QuestionsList[tCurrentSelectedIndex];
                    // Call the questionsController remove function and get a response
                    int tResultCode = QuestionsControllerObject.RemoveQuestion(tSelectedQuestion);

                    MessagesUtility.ShowMessageForm("Question Remove", "The question was removed", tResultCode);
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// On click event listener that closes the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// On click event listener that launches the settings form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateTimer.Stop();
                SettingsForm tSettingsForm = new SettingsForm(QuestionsControllerObject);
                tSettingsForm.ShowDialog();
                LoadUpdateForm(false);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// On click event listener on the DataGridView header cells that enables sorting the items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allQuestionsGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // Get the current clicked column
                DataGridViewColumn tTempCurrentClickedCloumn = allQuestionsGrid.Columns[e.ColumnIndex];
                ListSortDirection direction;

                // Decide what SortOrder the list should be sorted with
                if (tTempCurrentClickedCloumn.HeaderCell.SortGlyphDirection == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    direction = ListSortDirection.Ascending;
                }

                // Sort the items in the QuestionsList
                QuestionsControllerObject.SortQuestions(tTempCurrentClickedCloumn.Name, direction);
                // Reset the DataSource with the new sorted QuestionsList
                allQuestionsGrid.DataSource = QuestionsControllerObject.QuestionsList;

                // Reupdate the column headers and reformat the table
                UpdateQuestionsListTable();

                // Reselect the new column that was generated after the new DataSource binding
                DataGridViewColumn tCurrentClickedCloumn = allQuestionsGrid.Columns[e.ColumnIndex];

                // Reset the SortGlyphDirection after the column was re generated
                tCurrentClickedCloumn.HeaderCell.SortGlyphDirection = direction == ListSortDirection.Ascending ?
                    tCurrentClickedCloumn.HeaderCell.SortGlyphDirection = SortOrder.Ascending :
                    tCurrentClickedCloumn.HeaderCell.SortGlyphDirection = SortOrder.Descending;
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// On click event listener on the refresh button that reloads the data in the questions list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                LoadUpdateForm(false);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }
    }
}
