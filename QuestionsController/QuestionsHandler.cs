using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LoggerUtils;
using QuestionDatabase;
using QuestionEntities;

namespace QuestionsController
{
    public class QuestionsHandler
    {
        private DatabaseController DatabaseController;
        public BindingList<Question> QuestionsList { get; private set; }
        private ListSortDirection CurrentSortDirection;
        private int CurrentSortValueEnum; 
        private enum SortableValueNames
        {
            Type,
            Order,
            Id,
            Text
        };

        public QuestionsHandler()
        {
            try
            {
                QuestionsList = new BindingList<Question>();
                DatabaseController = new DatabaseController();
                CurrentSortDirection = ListSortDirection.Ascending;
                CurrentSortValueEnum = (int) SortableValueNames.Id;
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }
        }

        /// <summary>
        /// Fills the BindingList with data from the database
        /// </summary>
        /// <returns>A response code represnting what happend</returns>
        public int FillQuestionsData()
        {
            int tResultCode = (int) ResultCodesEnum.SUCCESS;

            try
            {
                // Create a new instance of the bindinglist so that data Isn't duplicated 
                BindingList<Question> tQuestionsList = new BindingList<Question>();
                tResultCode = DatabaseController.GetData(tQuestionsList);

                if (tResultCode == (int) ResultCodesEnum.SUCCESS)
                {
                    QuestionsList.Clear();
                    foreach(Question tQuestion in tQuestionsList)
                    {
                        QuestionsList.Add(tQuestion);
                    }
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int) ResultCodesEnum.CODE_FAILUER;
            }

            return tResultCode;
        }

        /// <summary>
        /// Updates the BindingList with data from the database
        /// </summary>
        /// <returns>A response code represnting what happend</returns>
        public int UpdateQuestionsData()
        {
            int tResultCode = (int) ResultCodesEnum.SUCCESS;

            try
            {
                // Sort the array by the Id so that the update operating works correctly
                SortQuestions(SortableValueNames.Id.ToString(), ListSortDirection.Ascending, false);
                BindingList<Question> tQuestionsList = new BindingList<Question>(QuestionsList.ToList());

                tResultCode = DatabaseController.UpdateData(tQuestionsList);

                if (tResultCode == (int) ResultCodesEnum.SUCCESS)
                {
                    QuestionsList.Clear();
                    foreach (Question tQuestion in tQuestionsList)
                    {
                        QuestionsList.Add(tQuestion);
                    }
                }

                // Resort the list using the already selected sort types after refreshing the data
                SortQuestions(Enum.GetName(typeof(SortableValueNames), CurrentSortValueEnum) , CurrentSortDirection, false);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int) ResultCodesEnum.CODE_FAILUER;
            }

            return tResultCode;
        }

        /// <summary>
        /// Gets a specific question from the database and assigns the new value retrieved from the database to the
        /// question instance
        /// </summary>
        /// <param name="pQuestion">The question instance to fill data in</param>
        /// <returns>A response code represnting what happend</returns>
        public int GetQuestion(Question pQuestion)
        {
            int tResultCode = (int)ResultCodesEnum.SUCCESS;

            try
            {
                tResultCode = DatabaseController.GetQuestion(pQuestion);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int)ResultCodesEnum.CODE_FAILUER;
            }

            return tResultCode;
        }

        /// <summary>
        /// Adds a new question to the database, and if that succeeds, add it to the BindingList
        /// </summary>
        /// <param name="pQuestion"></param>
        /// <returns>A response code represnting what happend</returns>
        public int AddQuestion(Question pQuestion)
        {
            int tResultCode = (int) ResultCodesEnum.SUCCESS;

            try
            {
                tResultCode = DatabaseController.AddQuestion(pQuestion);

                // On success, add the current object to the BindingList
                if ((int)ResultCodesEnum.SUCCESS == tResultCode)
                {
                    QuestionsList.Add(pQuestion);
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int) ResultCodesEnum.CODE_FAILUER;
            }

            return tResultCode;
        }

        /// <summary>
        /// Edits a question in the database, and if that succeeds, edit it in the BindingList then notify the UI
        /// </summary>
        /// <param name="pQuestion"></param>
        /// <returns>A response code represnting what happend</returns>
        public int EditQuestion(Question pQuestion)
        {
            int tResultCode = (int) ResultCodesEnum.SUCCESS;

            try
            {
                tResultCode = DatabaseController.EditQuestion(pQuestion);

                if ((int)ResultCodesEnum.SUCCESS == tResultCode)
                {
                    // Get the instance of the question in the BindingList then update It's data with the new instance passed
                    QuestionsList.FirstOrDefault(tQuestion => tQuestion.Id == pQuestion.Id).UpdateQuestion(pQuestion);
                    // Notify the UI that a value was updated so that the UI also updates
                    QuestionsList.ResetBindings();
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int) ResultCodesEnum.CODE_FAILUER;
            }

            return tResultCode;
        }

        /// <summary>
        /// Removes a question in the database, and if that succeeds, remove it from the BindingList
        /// </summary>
        /// <param name="pQuestion"></param>
        /// <returns>A response code represnting what happend</returns>
        public int RemoveQuestion(Question pQuestion)
        {
            int tResultCode = (int) ResultCodesEnum.SUCCESS;

            try
            {
                tResultCode = DatabaseController.DeleteQuestion(pQuestion);

                if ((int)ResultCodesEnum.SUCCESS == tResultCode)
                {
                    QuestionsList.Remove(pQuestion);
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int) ResultCodesEnum.CODE_FAILUER;
            }

            return tResultCode;
        }

        /// <summary>
        /// Tests a ConnectionString if it can access the database/server or not 
        /// </summary>
        /// <param name="pConnectionString"></param>
        /// <returns>A response code represnting what happend</returns>
        public int TestConnection(ConnectionString pConnectionString)
        {
            int tResultCode = (int) ResultCodesEnum.SUCCESS;

            try
            {
                tResultCode = DatabaseController.TestDatabaseConnection(pConnectionString);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int) ResultCodesEnum.CODE_FAILUER;
            }

            return tResultCode;
        }

        /// <summary>
        /// Changes the current ConnectionString instance in the Database object
        /// </summary>
        /// <param name="pConnectionString"></param>
        /// <returns>A response code represnting what happend</returns>
        public int ChangeConnectionString(ConnectionString pConnectionString)
        {
            int tResultCode = (int) ResultCodesEnum.SUCCESS;

            try
            {
                // Changes the values that are present in the app.config file
                pConnectionString.ApplyChanges();

                // Change the current instance of ConnectionString present in the Database to the new instance
                tResultCode = DatabaseController.ChangeConnectionString(pConnectionString);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int) ResultCodesEnum.CODE_FAILUER;
            }

            return tResultCode;
        }

        /// <summary>
        /// Returns the current ConnectionString instance that the Database object currently has
        /// </summary>
        /// <returns>A connectionString instance that is currently present in the database</returns>
        public ConnectionString GetConnectionString()
        {
            ConnectionString tConnectionString = null;

            try
            {
                // Gets the current instance present in the Database object
                tConnectionString = DatabaseController.ConnectionString;
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }

            return tConnectionString;
        }

        /// <summary>
        /// Util function that sorts the questions BindingList
        /// </summary>
        /// <param name="pValueName">The name value to sort by</param>
        /// <param name="pDirection">The sort direction, ASC, DECS</param>
        /// <returns>A response code represnting what happend</returns>
        public int SortQuestions(string pValueName, ListSortDirection pDirection, bool pIsUserAction = true)
        {
            int tResultCode = (int) ResultCodesEnum.SUCCESS;

            try
            {
                // Create a new temporary List
                List<Question> tQuestionsList = new List<Question>();
                Enum.TryParse(pValueName, out SortableValueNames tSortableValueEnum);

                // Based on the valueName sent in the params, pick which proportie to sort by
                switch (tSortableValueEnum)
                {
                    case SortableValueNames.Type:
                        tQuestionsList = (pDirection == ListSortDirection.Descending) ? 
                            QuestionsList.OrderByDescending(tQuestion => tQuestion.Type).ToList() : QuestionsList.OrderBy(tQuestion => tQuestion.Type).ToList();
                        break;
                    case SortableValueNames.Order:
                        tQuestionsList = (pDirection == ListSortDirection.Descending) ?
                            QuestionsList.OrderByDescending(tQuestion => tQuestion.Order).ToList() : QuestionsList.OrderBy(tQuestion => tQuestion.Order).ToList();
                        break;
                    case SortableValueNames.Text:
                        tQuestionsList = (pDirection == ListSortDirection.Descending) ?
                            QuestionsList.OrderByDescending(tQuestion => tQuestion.Text).ToList() : QuestionsList.OrderBy(tQuestion => tQuestion.Text).ToList();
                        break;
                    default:
                        tQuestionsList = (pDirection == ListSortDirection.Descending) ?
                            QuestionsList.OrderByDescending(tQuestion => tQuestion.Id).ToList() : QuestionsList.OrderBy(tQuestion => tQuestion.Id).ToList();
                        break;
                }

                if (pIsUserAction)
                {
                    CurrentSortDirection = pDirection;
                    CurrentSortValueEnum = (int)tSortableValueEnum;
                }

                // Recreate the QuestionsList instance with a new one containing the sorted items
                QuestionsList = new BindingList<Question>(tQuestionsList);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int)ResultCodesEnum.CODE_FAILUER;
            }

            return tResultCode;
        }
    }
}
