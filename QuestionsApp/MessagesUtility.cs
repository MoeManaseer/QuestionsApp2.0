using LoggerUtils;
using QuestionEntities;
using System;
using System.Text;
using System.Windows.Forms;

namespace QuestionsApp
{
    public static class MessagesUtility
    {
        public static DialogResult ShowMessageForm(string pMessageCaption, string pMessageString, int pResultCode, MessageBoxButtons pMessageButtons = MessageBoxButtons.OK, string pCustomMessage = "")
        {
            DialogResult tResult = DialogResult.OK;
            try
            {
                StringBuilder tMessageBuilder = new StringBuilder();
                string tMessageCaption = pMessageCaption;
                MessageBoxIcon tIcon;

                tMessageBuilder.Append(pMessageString);

                if (ResultCodesEnum.SUCCESS == (ResultCodesEnum)pResultCode)
                {
                    // Only add successfully when It's a button of type ok
                    tMessageBuilder.Append(pMessageButtons == MessageBoxButtons.OK ? " successfully" : "");
                    tIcon = MessageBoxIcon.Information;
                }
                else
                {
                    tMessageBuilder.Append(" unsuccessfully\n\n");
                    tMessageBuilder.AppendLine("Error Info:");
                    tMessageBuilder.AppendLine(string.IsNullOrEmpty(pCustomMessage) ? GetErrorMessage(pResultCode) : pCustomMessage);

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
                    case ResultCodesEnum.DATABASE_AUTHENTICATION_FAILUER:
                        tMessage = "User not authenticated";
                        break;
                    case ResultCodesEnum.DATABASE_CONNECTION_DENIED:
                        tMessage = "Connection to database denied";
                        break;
                    case ResultCodesEnum.DATABASE_CONNECTION_FAILURE:
                        tMessage = "Connection to database failed";
                        break;
                    case ResultCodesEnum.SERVER_CONNECTION_FAILURE:
                        tMessage = "Connection to server failed";
                        break;
                    case ResultCodesEnum.SERVER_PAUSED:
                        tMessage = "Connection to server failed, server is paused";
                        break;
                    case ResultCodesEnum.SERVER_NOT_FOUND_OR_DOWN:
                        tMessage = "Connection to server failed, server was not found or down";
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
