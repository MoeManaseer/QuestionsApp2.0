using LoggerUtils;
using QuestionEntities;
using System;
using System.Configuration;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace QuestionsApp
{
    public static class MessagesUtility
    {
        private static readonly string CurrentLanguage = ConfigurationManager.AppSettings["Language"];
        private static ResourceManager ResourcesManager = new ResourceManager("QuestionsApp.Messages", typeof(Messages).Assembly);
        private const string FailureString = "failure";
        private const string SuccessString = "success";
        private const string SpecificErrorKey = "specific_error";
        private const string DatabaseFailureKey = "database_failure";
        private const string SuccessKey = "success";
        private const string QuestionOutOfDateKey = "question_out_of_date";
        private const string CodeFailureKey = "code_failure";
        private const string NothingToUpdateKey = "nothing_to_update";
        private const string EmptyFieldKey = "empty_fields";
        private const string DatabaseAuthenticationFailureKey = "database_authentication_failure";
        private const string DatabaseConnectionDeniedKey = "database_connection_denied";
        private const string DatabaseConnectionFailureKey = "database_connection_failure";
        private const string ServerConnectionFailureKey = "server_connection_failure";
        private const string ServerPausedKey = "server_paused";
        private const string ServerNotFoundKey = "server_not_found_or_down";
        private const string DefaultMessageKey = "default_message";

        /// <summary>
        /// Utility function that shows a message box
        /// </summary>
        /// <param name="pMessageCaption">The message caption</param>
        /// <param name="pMessageString">The message text</param>
        /// <param name="pResultCode">The resultCode number</param>
        /// <param name="pMessageButtons">The message custom buttons</param>
        /// <param name="pCustomMessage">The message custom text</param>
        /// <returns>The result of the message</returns>
        public static DialogResult ShowMessageForm(string pMessageKey, int pResultCode, MessageBoxButtons pMessageButtons = MessageBoxButtons.OK, string pCustomMessageKey = "")
        {
            DialogResult tResult = DialogResult.OK;
            try
            {
                string tCurrentKey = pMessageKey + "_" + CurrentLanguage;
                StringBuilder tMessageBuilder = new StringBuilder();
                string tMessageCaption = ResourcesManager.GetString(tCurrentKey);
                MessageBoxIcon tIcon;

                if (ResultCodesEnum.SUCCESS == (ResultCodesEnum) pResultCode)
                {
                    tMessageBuilder.AppendLine(ResourcesManager.GetString(tCurrentKey + "_" + (string.IsNullOrEmpty(pCustomMessageKey) ? SuccessString : pCustomMessageKey)));
                    tIcon = MessageBoxIcon.Information;
                }
                else
                {
                    tMessageBuilder.AppendLine(ResourcesManager.GetString(tCurrentKey + "_" + FailureString));
                    tMessageBuilder.AppendLine("\n\n" + GetResourceValue(SpecificErrorKey + "_" + CurrentLanguage) + "\n");
                    tMessageBuilder.AppendLine(GetErrorMessage(pResultCode));
                    tIcon = MessageBoxIcon.Error;
                }

                tResult = MessageBox.Show(tMessageBuilder.ToString(), tMessageCaption, pMessageButtons, tIcon);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }

            return tResult;
        }

        /// <summary>
        /// Utility function that returns a resource string from the Messages resource file
        /// </summary>
        /// <param name="pResourceKey">The key of the string that should be brought back</param>
        /// <returns>A value from the Messages resource file</returns>
        public static string GetResourceValue(string pResourceKey)
        {
            string tResourceValue = "";

            try
            {
                tResourceValue = ResourcesManager.GetString(pResourceKey);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }

            return tResourceValue;
        }

        /// <summary>
        /// Utility function that returns a string based on the resultCode provided to it
        /// </summary>
        /// <param name="pResultCode">The resultCode number</param>
        /// <returns>The message corosponding to the result code</returns>
        private static string GetErrorMessage(int pResultCode)
        {
            string tMessage = "";

            try
            {
                switch ((ResultCodesEnum) pResultCode)
                {
                    case ResultCodesEnum.DATABASE_FAILURE:
                        tMessage = ResourcesManager.GetString(DatabaseFailureKey + "_" + CurrentLanguage);
                        break;
                    case ResultCodesEnum.SUCCESS:
                        tMessage = ResourcesManager.GetString(SuccessKey + "_" + CurrentLanguage);
                        break;
                    case ResultCodesEnum.QUESTION_OUT_OF_DATE:
                        tMessage = ResourcesManager.GetString(QuestionOutOfDateKey + "_" + CurrentLanguage);
                        break;
                    case ResultCodesEnum.CODE_FAILUER:
                        tMessage = ResourcesManager.GetString(CodeFailureKey + "_" + CurrentLanguage);
                        break;
                    case ResultCodesEnum.NOTHING_TO_UPDATE:
                        tMessage = ResourcesManager.GetString(NothingToUpdateKey + "_" + CurrentLanguage);
                        break;
                    case ResultCodesEnum.EMPTY_FIELDS:
                        tMessage = ResourcesManager.GetString(EmptyFieldKey + "_" + CurrentLanguage);
                        break;
                    case ResultCodesEnum.DATABASE_AUTHENTICATION_FAILUER:
                        tMessage = ResourcesManager.GetString(DatabaseAuthenticationFailureKey + "_" + CurrentLanguage);
                        break;
                    case ResultCodesEnum.DATABASE_CONNECTION_DENIED:
                        tMessage = ResourcesManager.GetString(DatabaseConnectionDeniedKey + "_" + CurrentLanguage);
                        break;
                    case ResultCodesEnum.DATABASE_CONNECTION_FAILURE:
                        tMessage = ResourcesManager.GetString(DatabaseConnectionFailureKey + "_" + CurrentLanguage);
                        break;
                    case ResultCodesEnum.SERVER_CONNECTION_FAILURE:
                        tMessage = ResourcesManager.GetString(ServerConnectionFailureKey + "_" + CurrentLanguage);
                        break;
                    case ResultCodesEnum.SERVER_PAUSED:
                        tMessage = ResourcesManager.GetString(ServerPausedKey + "_" + CurrentLanguage);
                        break;
                    case ResultCodesEnum.SERVER_NOT_FOUND_OR_DOWN:
                        tMessage = ResourcesManager.GetString(ServerNotFoundKey + "_" + CurrentLanguage);
                        break;
                    default:
                        tMessage = ResourcesManager.GetString(DefaultMessageKey + "_" + CurrentLanguage);
                        break;
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }

            return tMessage;
        }
    }
}
