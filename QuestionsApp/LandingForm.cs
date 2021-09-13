﻿using LoggerUtils;
using QuestionEntities;
using QuestionsController;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace QuestionsApp
{
    public partial class LandingForm : Form
    {
        private QuestionsHandler QuestionsHandlerObject;
        private System.Windows.Forms.Timer UpdateTimer;
        private string CurrentLanguage;
        private const string LanguageString = "Language";
        private const string FormLoadingKey = "form_loading";
        private const string IdKey = "Id";
        private const string OrderKey = "Order";
        private const string TypeKey = "Type";
        private const string TextKey = "Text";
        private const string ArabicKey = "ar";
        private const string SettingsPanelString = "settingsPanel";
        private const string RemoveKey = "remove";
        private const string ConfirmationKey = "confirmation";

        public LandingForm()
        {
            try
            {
                CurrentLanguage = ConfigurationManager.AppSettings[LanguageString];
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(CurrentLanguage);
                InitializeComponent();
                QuestionsHandlerObject = new QuestionsHandler();
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
                UpdateTimer = new System.Windows.Forms.Timer();
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
                tResultCode = pIsUpdate ? QuestionsHandlerObject.UpdateQuestionsData() : QuestionsHandlerObject.FillQuestionsData();

                if (tResultCode == (int) ResultCodesEnum.SUCCESS)
                {
                    // Assign the datasource
                    allQuestionsGrid.DataSource = QuestionsHandlerObject.QuestionsList;
                    // Enable the form controls
                    ToggleFormControls(true);
                    // Check the list for being empty or not
                    CheckList();
                    // Updates the columns of the DataGridView
                    UpdateQuestionsListTable();
                }
                else
                {
                    MessagesUtility.ShowMessageForm(FormLoadingKey, tResultCode);
                    ToggleFormControls(false);
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
                allQuestionsGrid.Columns[IdKey].Visible = false;
                allQuestionsGrid.Columns[OrderKey].Width = 80;
                allQuestionsGrid.Columns[TypeKey].Width = 80;
                allQuestionsGrid.Columns[TextKey].Width = 298;
                allQuestionsGrid.Columns[TextKey].DisplayIndex = 3;

                allQuestionsGrid.Columns[OrderKey].HeaderText = MessagesUtility.GetResourceValue(OrderKey + "_" + CurrentLanguage);
                allQuestionsGrid.Columns[TypeKey].HeaderText = MessagesUtility.GetResourceValue(TypeKey + "_" + CurrentLanguage);
                allQuestionsGrid.Columns[TextKey].HeaderText = MessagesUtility.GetResourceValue(TextKey + "_" + CurrentLanguage);

                if (CurrentLanguage.ToLower().Equals(ArabicKey))
                {
                    allQuestionsGrid.Columns[OrderKey].DisplayIndex = 3;
                    allQuestionsGrid.Columns[TextKey].DisplayIndex = 1;
                    allQuestionsGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    foreach(DataGridViewColumn tGridColumn in allQuestionsGrid.Columns)
                    {
                        tGridColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
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
                    if (!tFormControl.Name.Equals(SettingsPanelString))
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
                QuestionForm tQuestionForm = new QuestionForm(QuestionsHandlerObject);
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
                QuestionForm tQuestionForm = new QuestionForm(QuestionsHandlerObject, tCurrentIndex);
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
                MessageBoxButtons tMessageButtons = MessageBoxButtons.YesNo;
                DialogResult tResult = MessagesUtility.ShowMessageForm(RemoveKey, (int) ResultCodesEnum.SUCCESS, tMessageButtons, ConfirmationKey);

                if (tResult == DialogResult.Yes)
                {
                    // Get the current selected question index
                    int tCurrentSelectedIndex = allQuestionsGrid.CurrentRow.Index;
                    // Get the current question to be removed
                    Question tSelectedQuestion = QuestionsHandlerObject.QuestionsList[tCurrentSelectedIndex];
                    // Call the questionsController remove function and get a response
                    int tResultCode = QuestionsHandlerObject.RemoveQuestion(tSelectedQuestion);

                    MessagesUtility.ShowMessageForm(RemoveKey, tResultCode);
                    CheckList();
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
                SettingsForm tSettingsForm = new SettingsForm(QuestionsHandlerObject);
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
                QuestionsHandlerObject.SortQuestions(tTempCurrentClickedCloumn.Name, direction);
                // Reset the DataSource with the new sorted QuestionsList
                allQuestionsGrid.DataSource = QuestionsHandlerObject.QuestionsList;

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
                LoadUpdateForm(true);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }
    }
}
