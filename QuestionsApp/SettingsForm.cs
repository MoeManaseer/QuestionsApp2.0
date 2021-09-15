using System;
using System.Windows.Forms;
using System.Reflection;
using LoggerUtils;
using System.Collections;
using QuestionDatabase;
using QuestionEntities;
using QuestionsController;
using System.Globalization;
using System.Threading;
using System.Configuration;

namespace QuestionsApp
{
    public partial class SettingsForm : Form
    {
        private Control ConnectionValuesContainer;
        private ConnectionString ConnectionString;
        private QuestionsHandler QuestionsHandlerObject;
        private string CurrentLanguage;
        private enum LanguagesEnum
        {
            Arabic,
            English,
        }
        private const string InputControlsWrapperName = "connectionContainer";
        private const string Username = "Username";
        private const string Password = "Password";
        private const string IntegratedSecurityString = "IntegratedSecurity";
        private const string LanguageString = "Language";
        private const string InputLabelString = "input";
        private const string SettingsSaveKey = "settings_save";
        private const string TestKey = "test";
        private const string TextKey = "text";
        private const string SSPIString = "SSPI";
        public SettingsForm(QuestionsHandler pQuestionsHandlerObject)
        {
            try
            {
                CurrentLanguage = ConfigurationManager.AppSettings[LanguageString];
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(CurrentLanguage);
                InitializeComponent();
                QuestionsHandlerObject = pQuestionsHandlerObject;
                ConnectionString = new ConnectionString(QuestionsHandlerObject.GetConnectionString());
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            try
            {
                ConnectionValuesContainer = Controls[InputControlsWrapperName];

                languageComboBox.SelectedValueChanged -= new EventHandler(languageComboBox_SelectedValueChanged);
                languageComboBox.DataSource = Enum.GetValues(typeof(LanguagesEnum));
                languageComboBox.SelectedValueChanged += new EventHandler(languageComboBox_SelectedValueChanged);

                UpdateSettingFields();
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Updates the settings field with the data from the connection string object
        /// </summary>
        private void UpdateSettingFields()
        {
            try
            {
                UpdateConnectionStringFields();
                UpdateLanguageField();
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Updates the connection settings field with the data from the connection string object
        /// </summary>
        private void UpdateConnectionStringFields()
        {
            try
            {
                // Gets the proporty names of the ConnectionString Class
                Type tConnectionStringType = typeof(ConnectionString);
                PropertyInfo[] tConnectionStringProporties = tConnectionStringType.GetProperties();

                // Dynamically loop through the proporties of the connection string class and assigns the values of the input fields with the values in the connection string object
                foreach (PropertyInfo tConnectionStringProporty in tConnectionStringProporties)
                {
                    string tConnectionStringProportyName = tConnectionStringProporty.Name;

                    // Get the input control that contains this specific value
                    Control tCurrentConnectionStringField = ConnectionValuesContainer.Controls[InputLabelString + "_" + tConnectionStringProportyName];
                    // Get the value in the ConnectionString instance
                    string tCurrentConnectionStringValue = tConnectionStringProporty.GetValue(ConnectionString, null).ToString();
                    // Assign the input field value with the value from the ConnectionString instance
                    tCurrentConnectionStringField.Text = tCurrentConnectionStringValue;

                    // When we hit the IntegratedSecurity field, check the value of it and disable/enable the username/password fields
                    if (tConnectionStringProportyName.Equals(IntegratedSecurityString))
                    {
                        CheckIntegratedSecurityValue();
                    }
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Updates the language combobox with the currently selected language
        /// </summary>
        private void UpdateLanguageField()
        {
            try
            {
                languageComboBox.SelectedValueChanged -= new EventHandler(languageComboBox_SelectedValueChanged);
                foreach (string tLanguageText in Enum.GetNames(typeof(LanguagesEnum)))
                {
                    if (tLanguageText.ToLower().StartsWith(CurrentLanguage))
                    {
                        languageComboBox.Text = tLanguageText;
                    }
                }
                languageComboBox.SelectedValueChanged += new EventHandler(languageComboBox_SelectedValueChanged);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Toggles the username and password buttons on and off based on the value of the Security type
        /// </summary>
        private void CheckIntegratedSecurityValue()
        {
            try
            {
                // SSPI is windows authentication, so we don't need a username/password from the user
                if (ConnectionValuesContainer.Controls[InputLabelString + "_" + IntegratedSecurityString].Text.Equals(SSPIString))
                {
                    ToggleUsernamePasswordFields(false);
                }
                else
                {
                    ToggleUsernamePasswordFields(true);
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Helper function that toggles the username/password buttons based on the value given
        /// </summary>
        /// <param name="pValue">whether to show the username/password buttons</param>
        private void ToggleUsernamePasswordFields(bool pValue)
        {
            try
            {
                ConnectionValuesContainer.Controls[InputLabelString + "_" + Username].Enabled = pValue;
                ConnectionValuesContainer.Controls[InputLabelString + "_" + Password].Enabled = pValue;
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
            
        }

        /// <summary>
        /// Helper function that loops thro the props of the connection string class and gets the corosponding input field for it
        /// then assigns the value of the object prop to the value of the input field
        /// </summary>
        private void FillConnectionStringFields()
        {
            try
            {
                // Gets the proporty names of the ConnectionString Class
                Type tConnectionStringType = typeof(ConnectionString);
                PropertyInfo[] tConnectionStringProporties = tConnectionStringType.GetProperties();
                
                // Dynamically loop through the proporties of the connection string class and assigns the values of the connection string object to the values
                // of the input fields
                foreach (PropertyInfo tConnectionStringProporty in tConnectionStringProporties)
                {
                    string tConnectionStringProportyName = tConnectionStringProporty.Name;

                    // Get the input control that contains this specific value
                    Control tCurrentConnectionStringField = ConnectionValuesContainer.Controls[InputLabelString + "_" + tConnectionStringProportyName];
                    // Assign the ConnectionString instance proporty with the value from the coropsonding input field
                    tConnectionStringProporty.SetValue(ConnectionString, tCurrentConnectionStringField.Text.ToString(), null);
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Util function that checks if there is any empty input fields
        /// </summary>
        /// <returns>Whether or not there are any empty fields</returns>
        private bool CheckFormFields()
        {
            ArrayList tControlNames = new ArrayList();

            try
            {
                // loops over the input fields and checks for empty values
                foreach (Control tSettingsInputField in Controls[InputControlsWrapperName].Controls)
                {
                    if (tSettingsInputField.Enabled && string.IsNullOrEmpty(tSettingsInputField.Text))
                        tControlNames.Add(tSettingsInputField.Tag);
                }

                // this means that there are empty fields
                if (tControlNames.Count != 0)
                {
                    MessagesUtility.ShowMessageForm(SettingsSaveKey, (int) ResultCodesEnum.EMPTY_FIELDS);
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }

            return tControlNames.Count == 0;
        }

        /// <summary>
        /// On click listener for the test button to test out the currently inputted connection string data
        /// </summary>
        /// <param name="sender">The control that fired the event</param>
        /// <param name="e">Extra data about the event</param>
        private void testBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // don't check the connection string if there are any empty fields
                if (!CheckFormFields())
                {
                    return;
                }

                // Fill the ConnectionString instance data from the input fields
                FillConnectionStringFields();
                // Test the new ConnectionString instance
                int tResultCode = QuestionsHandlerObject.TestConnection(ConnectionString);

                MessagesUtility.ShowMessageForm(TestKey, tResultCode);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// On click listener for the save button which saves data to the app.config file then changes the connection string in the database object
        /// </summary>
        /// <param name="sender">The control that fired the event</param>
        /// <param name="e">Extra data about the event</param>
        private void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // don't save the connection string if there are any empty fields
                if (!CheckFormFields())
                {
                    return;
                }

                // Fill the ConnectionString instance data from the input fields
                FillConnectionStringFields();
                // Save the ConnectionString instance in the database
                int tResultCode = QuestionsHandlerObject.ChangeConnectionString(ConnectionString);

                if (ResultCodesEnum.SUCCESS == (ResultCodesEnum) tResultCode)
                {
                    MessagesUtility.ShowMessageForm(SettingsSaveKey, tResultCode);
                    Close();
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// On click event that closes the current form
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
        /// Whenever the IntegratedSecurity value changes, check it and see if we should enable/disable the username/password fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void input_IntegratedSecurity_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                // whenever the IntegratedSecuirty value is changed, check what the type of it is and disable/enable certain controls
                CheckIntegratedSecurityValue();
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        private void languageComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                string tNewSelectedLanguage = languageComboBox.Text.Substring(0, 2).ToLower();

                // Only change the language when it actually changes
                if (!tNewSelectedLanguage.Equals(CurrentLanguage))
                {
                    CurrentLanguage = tNewSelectedLanguage;
                    ChangeCurrentLanguage();
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Utility function that changes the current language in the App.config file then restarts the applciation
        /// </summary>
        private void ChangeCurrentLanguage()
        {
            try
            {
                MessagesUtility.ShowMessageForm(LanguageString.ToLower(), (int) ResultCodesEnum.SUCCESS, MessageBoxButtons.OK, TextKey);

                var tConfigurationManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                tConfigurationManager.AppSettings.Settings[LanguageString].Value = CurrentLanguage;
                tConfigurationManager.Save(ConfigurationSaveMode.Modified);

                Application.Restart();
                Environment.Exit(0);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }
    }
}
