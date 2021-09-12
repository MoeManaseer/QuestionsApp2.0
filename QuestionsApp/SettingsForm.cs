﻿using System;
using System.Windows.Forms;
using System.Reflection;
using LoggerUtils;
using System.Collections;
using QuestionDatabase;
using QuestionEntities;
using QuestionsController;
using QuestionsApp;
using System.Globalization;
using System.Threading;
using System.Configuration;

namespace QuestionsApp
{
    public partial class SettingsForm : Form
    {
        private Control ConnectionValuesContainer;
        private ConnectionString ConnectionString;
        private Controller QuestiosnControllerObject;
        private string CurrentLanguage;
        private readonly string InputControlsWrapperName = "connectionContainer";
        private readonly string UsernameInput = "input_Username";
        private readonly string PasswordInput = "input_Password";
        private readonly string IntegratedSecurityString = "IntegratedSecurity";
        public SettingsForm(Controller pQuestionsController)
        {
            try
            {
                CurrentLanguage = ConfigurationManager.AppSettings["Language"];
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(CurrentLanguage);
                InitializeComponent();
                QuestiosnControllerObject = pQuestionsController;
                ConnectionString = new ConnectionString(QuestiosnControllerObject.GetConnectionString());
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
                // Gets the proporty names of the ConnectionString Class
                Type tConnectionStringType = typeof(ConnectionString);
                PropertyInfo[] tConnectionStringProporties = tConnectionStringType.GetProperties();

                // Dynamically loop through the proporties of the connection string class and assigns the values of the input fields with the values in the connection string object
                foreach (PropertyInfo tConnectionStringProporty in tConnectionStringProporties)
                {
                    string tConnectionStringProportyName = tConnectionStringProporty.Name;

                    // Get the input control that contains this specific value
                    Control tCurrentConnectionStringField = ConnectionValuesContainer.Controls["input_" + tConnectionStringProportyName];
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
        /// Toggles the username and password buttons on and off based on the value of the Security type
        /// </summary>
        private void CheckIntegratedSecurityValue()
        {
            try
            {
                // SSPI is windows authentication, so we don't need a username/password from the user
                if (ConnectionValuesContainer.Controls["input_IntegratedSecurity"].Text.Equals("SSPI"))
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
                ConnectionValuesContainer.Controls[UsernameInput].Enabled = pValue;
                ConnectionValuesContainer.Controls[PasswordInput].Enabled = pValue;
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
                    Control tCurrentConnectionStringField = ConnectionValuesContainer.Controls["input_" + tConnectionStringProportyName];
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
                    MessagesUtility.ShowMessageForm("settings_save", (int) ResultCodesEnum.EMPTY_FIELDS);
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
                int tResultCode = QuestiosnControllerObject.TestConnection(ConnectionString);

                MessagesUtility.ShowMessageForm("test", tResultCode);
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
                int tResultCode = QuestiosnControllerObject.ChangeConnectionString(ConnectionString);

                if (ResultCodesEnum.SUCCESS == (ResultCodesEnum) tResultCode)
                {
                    MessagesUtility.ShowMessageForm("settings_save", tResultCode);
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

        private void ChangeCurrentLanguage()
        {
            try
            {
                MessagesUtility.ShowMessageForm("language", (int) ResultCodesEnum.SUCCESS, MessageBoxButtons.OK, "text");

                var tConfigurationManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                tConfigurationManager.AppSettings.Settings["Language"].Value = CurrentLanguage;
                tConfigurationManager.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }
    }
}
