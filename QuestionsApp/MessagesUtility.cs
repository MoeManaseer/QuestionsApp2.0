using LoggerUtils;
using QuestionEntities;
using System;
using System.Text;
using System.Windows.Forms;

namespace QuestionsApp
{
    public static class MessagesUtility
    {
        public static void ShowMessageForm(string pOperationName, string pMessageCaption, int pResultCode, string pMessageString = "")
        {
            try
            {
                StringBuilder tMessageBuilder = new StringBuilder();
                string tMessageCaption = pOperationName;
                MessageBoxButtons tMessageButtons = MessageBoxButtons.OK;
                MessageBoxIcon tIcon;

                tMessageBuilder.Append("The " + pOperationName + " operationg was a ");

                if (ResultCodesEnum.SUCCESS == (ResultCodesEnum)pResultCode)
                {
                    tMessageBuilder.Append("success");
                    tIcon = MessageBoxIcon.Information;
                }
                else
                {
                    tMessageBuilder.Append("failure\n");
                    tMessageBuilder.AppendLine("Error Info:");
                    
                    tMessageBuilder.AppendLine(string.IsNullOrEmpty(pMessageString) ? GetErrorMessage(pResultCode) : pMessageString);
                    tIcon = MessageBoxIcon.Error;
                }

                MessageBox.Show(tMessageBuilder.ToString(), tMessageCaption, tMessageButtons, tIcon);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        private static string GetErrorMessage(int pResultCode)
        {
            string tMessage = "";

            try
            {
                switch ((ResultCodesEnum) pResultCode)
                {
                    case ResultCodesEnum.DATABASE_FAILURE:
                        tMessage = "Database failure, please contact administrator\n";
                        break;
                    case ResultCodesEnum.SUCCESS:
                        tMessage = "Successful\n";
                        break;
                    case ResultCodesEnum.QUESTION_OUT_OF_DATE:
                        tMessage = "The question is either deleted or updated, please refresh data and try again\n";
                        break;
                    case ResultCodesEnum.CODE_FAILUER:
                        tMessage = "Something wrong happend while executing.. please try again or contact an administrator\n";
                        break;
                    case ResultCodesEnum.NOTHING_TO_UPDATE:
                        tMessage = "Nothing to update\n";
                        break;
                    case ResultCodesEnum.EMPTY_FIELDS:
                        tMessage = "There are empty fields, please fill them.\n";
                        break;
                    default:
                        tMessage = "No Message is here for the current resultCode";
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
