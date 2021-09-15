using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections;
using LoggerUtils;
using QuestionsController;
using QuestionEntities;
using QuestionsApp;
using System.Configuration;
using System.Threading;
using System.Globalization;

namespace QuestionsApp
{
    public partial class QuestionForm : Form
    {
        private QuestionsHandler QuestionsHandlerObject;
        private Question CurrentQuestion;
        private int QuestionIndex;
        private bool IsNewQuestion;
        private List<Control> QuestionsInputFieldList;
        private Dictionary<string, string> QuestionTypesDictionary;
        private const string LanguageString = "Language";
        private readonly string CurrentLanguage;
        private const string QuestionInputWrapper = "containerQuestion";
        private const string AddKey = "add";
        private const string UpdateKey = "update";
        private const string TitleKey = "title";
        private const string TextKey = "text";
        private const string FormLoadingKey = "form_loading";
        private const string ContainerString = "container";
        private const string InputString = "input";
        private const string KeyString = "Key";
        private const string ValueString = "Value";

        private enum QuestionTypes
        {
            Smiley, Star, Slider,
        };

        public QuestionForm(QuestionsHandler pQuestionsHandlerObject)
        {
            try
            {
                CurrentLanguage = ConfigurationManager.AppSettings[LanguageString];
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(CurrentLanguage);
                InitializeComponent();
                QuestionsHandlerObject = pQuestionsHandlerObject;
                QuestionIndex = -1;
                CurrentQuestion = QuestionsFactory.GetInstance(QuestionTypes.Smiley.ToString());
                CurrentQuestion.Type = QuestionTypes.Smiley.ToString();
                QuestionTypesDictionary = new Dictionary<string, string>();
                IsNewQuestion = true;
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        public QuestionForm(QuestionsHandler pQuestionsHandlerObject, int pQuestionIndex)
        {
            try
            {
                CurrentLanguage = ConfigurationManager.AppSettings[LanguageString];
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(CurrentLanguage);
                InitializeComponent();
                QuestionsHandlerObject = pQuestionsHandlerObject;
                QuestionIndex = pQuestionIndex;
                string tCurrentQuestionType = QuestionsHandlerObject.QuestionsList[QuestionIndex].Type;
                CurrentQuestion = QuestionsFactory.GetInstance(tCurrentQuestionType);
                CurrentQuestion.Id = QuestionsHandlerObject.QuestionsList[QuestionIndex].Id;
                CurrentQuestion.Type = tCurrentQuestionType;
                QuestionTypesDictionary = new Dictionary<string, string>();
                IsNewQuestion = false;
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        private void QuestionForm_Load(object sender, EventArgs e)
        {
            int tResultCode = (int) ResultCodesEnum.SUCCESS;

            try
            {
                UpdateQuestionsInputFieldList();
                FillQuestionTypes();
                BindComboBox();

                if (IsNewQuestion)
                {
                    Text = MessagesUtility.GetResourceValue(AddKey + "_" + TitleKey + "_" + CurrentLanguage);
                    controlBtn.Text = MessagesUtility.GetResourceValue(AddKey + "_" + TextKey + "_" + CurrentLanguage);
                    ShowExtraQuestionFields();
                }
                else
                {
                    Text = MessagesUtility.GetResourceValue(UpdateKey + "_" + TitleKey + "_" + CurrentLanguage);
                    questionTypeCombo.Enabled = false;
                    controlBtn.Text = MessagesUtility.GetResourceValue(UpdateKey + "_" + TextKey + "_" + CurrentLanguage);
                    tResultCode = QuestionsHandlerObject.GetQuestion(CurrentQuestion);
                    UpdateQuestionFields();
                }

                questionTypeCombo.SelectedValue = CurrentQuestion.Type;

                if (tResultCode != (int) ResultCodesEnum.SUCCESS)
                {
                    MessagesUtility.ShowMessageForm(FormLoadingKey, tResultCode);
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Fills the question types from the string array to the Dictionary and maps them.
        /// </summary>
        private void FillQuestionTypes()
        {
            try
            {
                // loops over the strings in the questionTypes enum and fills up the dictionary
                foreach (QuestionTypes tQuestionType in Enum.GetValues(typeof(QuestionTypes)))
                {
                    string tQuestionTypeString = tQuestionType.ToString();
                    QuestionTypesDictionary.Add(tQuestionTypeString, MessagesUtility.GetResourceValue(tQuestionTypeString + "_" + CurrentLanguage));
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Inserts all the input fields into the QuestionsInputFieldList to be used later
        /// </summary>
        private void UpdateQuestionsInputFieldList()
        {
            try
            {
                QuestionsInputFieldList = new List<Control>();

                string tCurrentQuestionType = CurrentQuestion.Type;

                // Get the parent container of the input fields
                Control tQuestionDataContainer = Controls[QuestionInputWrapper];

                // Get the extra container of the input fields
                Control tQuestionExtraDataContainer = tQuestionDataContainer.Controls[ContainerString + tCurrentQuestionType];

                // loops over the question container and adds the input controls to an array
                foreach (Control tQuestionInputField in tQuestionDataContainer.Controls)
                {
                    if (tQuestionInputField.Name.Contains(InputString))
                    {
                        QuestionsInputFieldList.Add(tQuestionInputField);
                    }
                }

                // loops over the extra question data container and adds the input controls to an array
                foreach (Control tQuestionExtraInputField in tQuestionExtraDataContainer.Controls)
                {
                    if (tQuestionExtraInputField.Name.Contains(InputString))
                    {
                        QuestionsInputFieldList.Add(tQuestionExtraInputField);
                    }
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Binds the ComboBox to the dictionary data
        /// </summary>
        private void BindComboBox()
        {
            try
            {
                // Before adding the BindingSource, remove the ValueChanged event listener, since it fires when adding new items
                questionTypeCombo.SelectedValueChanged -= new EventHandler(questionTypeCombo_SelectedValueChanged);
                questionTypeCombo.DataSource = new BindingSource(QuestionTypesDictionary, null);
                questionTypeCombo.DisplayMember = ValueString;
                questionTypeCombo.ValueMember = KeyString;
                // Reassign the ValueChanged event listener
                questionTypeCombo.SelectedValueChanged += new EventHandler(questionTypeCombo_SelectedValueChanged);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Updates the form fields with data from the current question data
        /// </summary>
        private void UpdateQuestionFields()
        {
            try
            {
                // Show the extra data container specific to the current question
                ShowExtraQuestionFields();

                // Gets the key value pair data from current question instance
                Dictionary<string, string> tQuestionDataDic = CurrentQuestion.GetDataList();

                // Assigns the form fields with data from the current question data row
                foreach (Control tQuestionInputField in QuestionsInputFieldList)
                {
                    string tCurrentFieldName = tQuestionInputField.Name.Split('_')[1];
                    tQuestionInputField.Text = tQuestionDataDic[tCurrentFieldName];
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Picks and shows the current question extra data container and hiding the rest
        /// </summary>
        private void ShowExtraQuestionFields()
        {
            try
            {
                Control QuestionDataContainer = Controls[QuestionInputWrapper];

                // Hides all the extra question data containers
                foreach (var tQuestionExtraDataContainer in QuestionTypesDictionary)
                {
                    QuestionDataContainer.Controls[ContainerString + tQuestionExtraDataContainer.Key].Visible = false;
                }

                string tCurrentQuestionType = CurrentQuestion.Type;
                // Shows the currently selected question type data container
                QuestionDataContainer.Controls[ContainerString + tCurrentQuestionType].Visible = true;
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Checks if any of the question input fields are empty, if so show a user an error message with what is empty
        /// </summary>
        /// <returns>Whether or not there are empty input fields</returns>
        private bool ValidateFields()
        {
            ArrayList tControlNames = new ArrayList();

            try
            {
                // Loops over the form input fields and checks if any of them is empty
                foreach (Control tQuestionInputField in QuestionsInputFieldList)
                {
                    if (string.IsNullOrEmpty(tQuestionInputField.Text))
                        tControlNames.Add(tQuestionInputField.Tag);
                }

                // if there was an empty field
                if (tControlNames.Count != 0)
                {
                    string tOperationName = IsNewQuestion ? AddKey : UpdateKey;
                    MessagesUtility.ShowMessageForm(tOperationName, (int) ResultCodesEnum.EMPTY_FIELDS);
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }

            return tControlNames.Count == 0;
        }

        /// <summary>
        /// Fills the current question datarow with the data from the question input fields
        /// </summary>
        private void FillQuestionRow()
        {
            try
            {
                // Create a new dictionary to fill key value paris from the input fields
                Dictionary<string, string> tQuestionDataDic = new Dictionary<string, string>();
                // Loops over the controls in the controls array and assigns the corosponding field in the question row with the control data
                foreach (Control tQuestionInputField in QuestionsInputFieldList)
                {
                    string curFieldName = tQuestionInputField.Name.Split('_')[1];
                    tQuestionDataDic.Add(curFieldName, tQuestionInputField.Text);
                }

                // After getting the key/value paris, set it to the current instance of the question
                CurrentQuestion.FillData(tQuestionDataDic);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Fires whenever the Add/Update button gets clicked, then calls the corresponding function to Add/Update the question
        /// </summary>
        /// <param name="sender">The control that fired the event</param>
        /// <param name="e">Extra data about the event</param>
        private void controlBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // ValidateFields checks if there is no empty input fields, while checkQuestionFields checks if the data is different from the original or not
                if (ValidateFields())
                {
                    // Fill the current question instance with data
                    FillQuestionRow();

                    // Call the corosponding QuestionsController function
                    int tResultCode = IsNewQuestion ? QuestionsHandlerObject.AddQuestion(CurrentQuestion) : QuestionsHandlerObject.EditQuestion(CurrentQuestion);

                    string tOperationName = IsNewQuestion ? AddKey : UpdateKey;
                    MessagesUtility.ShowMessageForm(tOperationName, tResultCode);

                    // If action success close the form
                    if (tResultCode == (int)ResultCodesEnum.SUCCESS)
                    {
                        Close();
                    }
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }


        /// <summary>
        /// Fires whenever the user hits the exit button, then shows a confirmation message of closing the form
        /// </summary>
        /// <param name="sender">The control that fired the event</param>
        /// <param name="e">Extra data about the event</param>
        private void exitButton_Click(object sender, EventArgs e)
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
        /// Validator function that makes sure that the ending field is always less than the starting field
        /// </summary>
        /// <param name="sender">The control that fired the event</param>
        /// <param name="e">Extra data about the event</param>
        private void input_EndStartValues_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                // This ensures that the end value is always greater or equal to the start value
                input_EndValue.Value = Math.Max(input_EndValue.Value, Math.Min(input_StartValue.Value + 1, 100));
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Event listener that fires whenever the questionTypeCombo changes It's values then updates the form to that corresponding question type
        /// </summary>
        /// <param name="sender">The control that fired the event</param>
        /// <param name="e">Extra data about the event</param>
        private void questionTypeCombo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (QuestionIndex == -1)
                {
                    // Get the new question type
                    CurrentQuestion.Type = questionTypeCombo.SelectedValue.ToString();
                    // Get a new instance of question based on the type
                    CurrentQuestion = QuestionsFactory.GetInstance(CurrentQuestion.Type);
                    // Update the QuestionsInputFieldList with the new extra data input fields
                    UpdateQuestionsInputFieldList();
                    // Show the correct question extra data container
                    ShowExtraQuestionFields();
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }
    }
}
