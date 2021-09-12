﻿using LoggerUtils;
using QuestionEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace QuestionDatabase
{
    public class DatabaseController
    {
        private SqlConnection SQLConnection;
        public ConnectionString ConnectionString { private set; get; }

        public DatabaseController()
        {
            try
            {
                // Get a new instance of connection string from the app.config
                ConnectionString = new ConnectionString();
            }
            catch (Exception e)
            {
                Logger.WriteExceptionMessage(e);
            }
        }

        /// <summary>
        /// Changes the connectionstring instance with a new one
        /// </summary>
        /// <param name="tNewConnectionString">The new connectionstring instance</param>
        /// <returns>a result code to be used to determine if success or failure</returns>
        public int ChangeConnectionString(ConnectionString tNewConnectionString)
        {
            int tResultCode = (int)ResultCodesEnum.CODE_FAILUER;

            try
            {
                ConnectionString = new ConnectionString(tNewConnectionString);
                tResultCode = (int)ResultCodesEnum.SUCCESS;
            }
            catch (Exception e)
            {
                Logger.WriteExceptionMessage(e);
            }

            return tResultCode;
        }

        /// <summary>
        /// Tests if the connection string given to it successfuly connects to the database
        /// </summary>
        /// <param name="tConnectionString">The connection string to test</param>
        /// <returns>a result code to be used to determine if success or failure</returns>
        public int TestDatabaseConnection(ConnectionString tConnectionString)
        {
            int tResultCode = (int)ResultCodesEnum.SUCCESS;
            SqlCommand tSQLCommand = null;

            try
            {
                string tomato = tConnectionString.ToString();
                SQLConnection = new SqlConnection(tConnectionString.ToString());
                
                // We know that the AllQuestions table will always exist in the database, so test selecting from it
                tSQLCommand = new SqlCommand("SELECT 1 FROM AllQuestions;", SQLConnection);

                SQLConnection.Open();
                tSQLCommand.ExecuteNonQuery();
            }
            catch (SqlException tSQLException)
            {
                Logger.WriteExceptionMessage(tSQLException);
                // Gets the corosponding database error code enum
                tResultCode = QuestionUtilities.GetDatabaseError(tSQLException.Number);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int) ResultCodesEnum.CODE_FAILUER;
            }
            finally
            {
                if (tSQLCommand != null)
                {
                    tSQLCommand.Dispose();
                }

                if (SQLConnection.State == ConnectionState.Open)
                {
                    SQLConnection.Close();
                }
            }

            return tResultCode;
        }

        /// <summary>
        /// Gets data from the database based on the given table names and constructs the DataSet
        /// </summary>
        /// <param name="pQuestionsDataSet">The dataset to be constructed</param>
        /// <param name="pTableNames">The table names to get from the database</param>
        /// <returns>a result code to be used to determine if success or failure</returns>
        public int GetData(BindingList<Question> pQuestionsList)
        {
            SqlCommand tSQLCommand = null;
            SqlDataReader tSQLReader = null;
            int tResultCode = (int)ResultCodesEnum.SUCCESS;

            try
            {
                SQLConnection = new SqlConnection(ConnectionString.ToString());

                tSQLCommand = new SqlCommand("SELECT * FROM AllQuestions;", SQLConnection);
                SQLConnection.Open();

                tSQLReader = tSQLCommand.ExecuteReader(CommandBehavior.KeyInfo);

                // Loop over the records and add them to the questions list
                while (tSQLReader.Read())
                {
                    Question tQuestion = new Question();

                    tQuestion.Id = Convert.ToInt32(tSQLReader["Id"]);
                    tQuestion.Text = Convert.ToString(tSQLReader["Text"]);
                    tQuestion.Type = Convert.ToString(tSQLReader["Type"]);
                    tQuestion.Order = Convert.ToInt32(tSQLReader["Order"]);

                    pQuestionsList.Add(tQuestion);
                }
            }
            catch (SqlException tSQLException)
            {
                Logger.WriteExceptionMessage(tSQLException);
                // Gets the corosponding database error code enum
                tResultCode = QuestionUtilities.GetDatabaseError(tSQLException.Number);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int)ResultCodesEnum.CODE_FAILUER;
            }
            finally
            {
                if (tSQLReader != null)
                {
                    tSQLReader.Close();
                }

                if (tSQLCommand != null)
                {
                    tSQLCommand.Dispose();
                }

                if (SQLConnection.State == ConnectionState.Open)
                {
                    SQLConnection.Close();
                }
            }

            return tResultCode;
        }

        public int UpdateData(BindingList<Question> pQuestionsList)
        {
            SqlCommand tSQLCommand = null;
            SqlDataReader tSQLReader = null;
            int tResultCode = (int)ResultCodesEnum.SUCCESS;

            try
            {
                SQLConnection = new SqlConnection(ConnectionString.ToString());

                tSQLCommand = new SqlCommand("SELECT * FROM AllQuestions", SQLConnection);
                SQLConnection.Open();

                tSQLReader = tSQLCommand.ExecuteReader(CommandBehavior.KeyInfo);
                tSQLReader.Read();

                int tQuestionsListPointer = 0;

                if (tSQLReader.HasRows)
                {
                    while (true)
                    {
                        bool tNextRow = false;
                        int tCurrentId = Convert.ToInt32(tSQLReader["Id"]);

                        if (pQuestionsList[tQuestionsListPointer].Id < tCurrentId)
                        {
                            pQuestionsList.RemoveAt(tQuestionsListPointer);
                        }
                        else
                        {
                            Question tQuestion = pQuestionsList[tQuestionsListPointer].Id > tCurrentId ? new Question() : pQuestionsList[tQuestionsListPointer];

                            tQuestion.Id = tCurrentId;
                            tQuestion.Text = Convert.ToString(tSQLReader["Text"]);
                            tQuestion.Type = Convert.ToString(tSQLReader["Type"]);
                            tQuestion.Order = Convert.ToInt32(tSQLReader["Order"]);

                            if (pQuestionsList[tQuestionsListPointer].Id > tCurrentId)
                            {
                                pQuestionsList.Insert(tQuestionsListPointer, tQuestion);
                            }

                            tNextRow = true;
                            tQuestionsListPointer++;
                        }

                        if (tQuestionsListPointer == pQuestionsList.Count)
                            break;

                        if (tNextRow && !tSQLReader.Read())
                            break;
                    }

                    while (tSQLReader.Read())
                    {
                        Question tQuestion = new Question();

                        tQuestion.Id = Convert.ToInt32(tSQLReader["Id"]);
                        tQuestion.Text = Convert.ToString(tSQLReader["Text"]);
                        tQuestion.Type = Convert.ToString(tSQLReader["Type"]);
                        tQuestion.Order = Convert.ToInt32(tSQLReader["Order"]);

                        pQuestionsList.Add(tQuestion);
                        tQuestionsListPointer++;
                    }

                    for (int i = tQuestionsListPointer; i < pQuestionsList.Count; i++)
                    {
                        pQuestionsList.RemoveAt(pQuestionsList.Count - 1);
                    }
                }
            }
            catch (SqlException tSQLException)
            {
                Logger.WriteExceptionMessage(tSQLException);
                tResultCode = QuestionUtilities.GetDatabaseError(tSQLException.Number);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int)ResultCodesEnum.CODE_FAILUER;
            }
            finally
            {
                // This is to prevent having an error thrown here, if the connection is open then everything else is open
                if (SQLConnection.State == ConnectionState.Open)
                {
                    tSQLReader.Close();
                    tSQLCommand.Dispose();
                    SQLConnection.Close();
                }
            }

            return tResultCode;
        }

        public int GetQuestion(Question pQuestion)
        {
            SqlCommand tSQLCommand = null;
            SqlDataReader tSQLReader = null;
            int tResultCode = (int)ResultCodesEnum.QUESTION_OUT_OF_DATE;

            try
            {
                int tQuestionId = pQuestion.Id;
                string tQuestionTableName = pQuestion.Type + "Questions";

                SQLConnection = new SqlConnection(ConnectionString.ToString());
                SQLConnection.Open();

                // Get the correct procedure based on the tablename
                tSQLCommand = new SqlCommand("Get_" + tQuestionTableName, SQLConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                tSQLCommand.Parameters.Add(new SqlParameter("@Id", tQuestionId));

                tSQLReader = tSQLCommand.ExecuteReader(CommandBehavior.KeyInfo);

                while (tSQLReader.Read())
                {
                    // Create a new data dictionary
                    Dictionary<string, string> tDataDictionary = new Dictionary<string, string>();
                    // Get the current question prop names
                    List<string> tDataParamNames = pQuestion.GetObjectParamNames();

                    // Loop over the prop names and inesrt the key value pairs into the Dictionary
                    foreach (string tDataParamName in tDataParamNames)
                    {
                        tDataDictionary.Add(tDataParamName, Convert.ToString(tSQLReader[tDataParamName]));
                    }

                    // Fill the current question instance with data which is present in the Dictionary
                    tResultCode = pQuestion.FillData(tDataDictionary) ? (int)ResultCodesEnum.SUCCESS : (int)ResultCodesEnum.CODE_FAILUER;
                }
            }
            catch (SqlException tSQLException)
            {
                Logger.WriteExceptionMessage(tSQLException);
                // Gets the corosponding database error code enum
                tResultCode = QuestionUtilities.GetDatabaseError(tSQLException.Number);
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int)ResultCodesEnum.CODE_FAILUER;
            }
            finally
            {
                if (tSQLReader != null)
                {
                    tSQLReader.Close();
                }

                if (tSQLCommand != null)
                {
                    tSQLCommand.Dispose();
                }

                if (SQLConnection.State == ConnectionState.Open)
                {
                    SQLConnection.Close();
                }
            }

            return tResultCode;
        }

        /// <summary>
        /// Adds a new question to the database
        /// </summary>
        /// <param name="pQuestionRow">The new question to be added to the database</param>
        /// <returns>a result code to be used to determine if success or failure</returns>
        public int AddQuestion(Question pQuestion)
        {
            SqlTransaction tSQLTransaction = null;
            SqlCommand tSQLCommand = null;
            int tResultCode = (int)ResultCodesEnum.SUCCESS;

            try
            {
                string tTableName = pQuestion.Type + "Questions";

                SQLConnection = new SqlConnection(ConnectionString.ToString());
                SQLConnection.Open();
                tSQLTransaction = SQLConnection.BeginTransaction();

                // Get the correct procedure based on the tablename
                tSQLCommand = new SqlCommand("Add_" + tTableName, SQLConnection, tSQLTransaction)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Get the current question key value pair object which represent the current question data
                Dictionary<string, string> tQuestionData = pQuestion.GetDataList();

                // Loop over the keys and add the procedure params accordingly
                foreach(string tQuestionDataKey in tQuestionData.Keys)
                {
                    tSQLCommand.Parameters.Add(new SqlParameter(tQuestionDataKey, tQuestionData[tQuestionDataKey]));
                }

                // Add the Id of the question as an output parameter to be used later on
                SqlParameter tNewQuestionId = new SqlParameter("@Id", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                tSQLCommand.Parameters.Add(tNewQuestionId);

                tResultCode = tSQLCommand.ExecuteNonQuery() != 0 ? (int)ResultCodesEnum.SUCCESS : (int)ResultCodesEnum.ADD_FAILURE;

                // If the procedure succeds, get the newly created Id from the database and insert it into the question instance
                pQuestion.Id = Convert.ToInt32(tNewQuestionId.Value);
                tSQLTransaction.Commit();
            }
            catch (SqlException tSQLException)
            {
                if (tSQLTransaction != null)
                {
                    tSQLTransaction.Rollback();
                }
                Logger.WriteExceptionMessage(tSQLException);
                // Gets the corosponding database error code enum
                tResultCode = QuestionUtilities.GetDatabaseError(tSQLException.Number);
            }
            catch (Exception tException)
            {
                tSQLTransaction.Rollback();
                Logger.WriteExceptionMessage(tException);
                tResultCode = (int)ResultCodesEnum.CODE_FAILUER;
            }
            finally
            {
                if (tSQLCommand != null)
                {
                    tSQLCommand.Dispose();
                }

                if (SQLConnection.State == ConnectionState.Open)
                {
                    SQLConnection.Close();
                }
            }

            return tResultCode;
        }

        /// <summary>
        /// Edits a question in the database
        /// </summary>
        /// <param name="pQuestionRow">The question that should be edited in the database</param>
        /// <returns>a result code to be used to determine if success or failure</returns>
        public int EditQuestion(Question pQuestion)
        {
            SqlTransaction tSQLTransaction = null;
            SqlCommand tSQLCommand = null;
            int tResultCode = (int)ResultCodesEnum.SUCCESS;

            try
            {
                string tTableName = pQuestion.Type + "Questions";

                SQLConnection = new SqlConnection(ConnectionString.ToString());
                SQLConnection.Open();
                tSQLTransaction = SQLConnection.BeginTransaction();

                // Get the correct procedure based on the tablename
                tSQLCommand = new SqlCommand("Update_" + tTableName, SQLConnection, tSQLTransaction)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Get the current question key value pair object which represent the current question data
                Dictionary<string, string> tQuestionData = pQuestion.GetDataList();

                // Loop over the keys and add the procedure params accordingly
                foreach (string tQuestionDataKey in tQuestionData.Keys)
                {
                    tSQLCommand.Parameters.Add(new SqlParameter(tQuestionDataKey, tQuestionData[tQuestionDataKey]));
                }

                // Add the question Id individually
                tSQLCommand.Parameters.Add(new SqlParameter("@Id", pQuestion.Id));

                tResultCode = tSQLCommand.ExecuteNonQuery() != 0 ? (int)ResultCodesEnum.SUCCESS : (int)ResultCodesEnum.QUESTION_OUT_OF_DATE;

                tSQLTransaction.Commit();
            }
            catch (SqlException tSQLException)
            {
                if (tSQLTransaction != null)
                {
                    tSQLTransaction.Rollback();
                }
                Logger.WriteExceptionMessage(tSQLException);
                // Gets the corosponding database error code enum
                tResultCode = QuestionUtilities.GetDatabaseError(tSQLException.Number);
            }
            catch (Exception e)
            {
                Logger.WriteExceptionMessage(e);
                tResultCode = (int)ResultCodesEnum.CODE_FAILUER;
            }
            finally
            {
                if (tSQLCommand != null)
                {
                    tSQLCommand.Dispose();
                }

                if (SQLConnection.State == ConnectionState.Open)
                {
                    SQLConnection.Close();
                }
            }

            return tResultCode;
        }

        /// <summary>
        /// Deletes a question from the database
        /// </summary>
        /// <param name="pQuestionRow">The question row that should be removed</param>
        /// <returns>a result code to be used to determine if success or failure</returns>
        public int DeleteQuestion(Question pQuestion)
        {
            SqlTransaction tSQLTransaction = null;
            SqlCommand tSQLCommand = null;
            int tResultCode = (int)ResultCodesEnum.SUCCESS;

            try
            {
                // Get the table name to remove the quesand the question id
                string tTableName = pQuestion.Type + "Questions";
                int tQuestionId = pQuestion.Id;

                SQLConnection = new SqlConnection(ConnectionString.ToString());
                SQLConnection.Open();
                tSQLTransaction = SQLConnection.BeginTransaction();

                // Get the correct procedure based on the tablename
                tSQLCommand = new SqlCommand("Delete_" + tTableName, SQLConnection, tSQLTransaction)
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add the @Id param with the current question Id
                tSQLCommand.Parameters.Add(new SqlParameter("@Id", tQuestionId));

                tResultCode = tSQLCommand.ExecuteNonQuery() != 0 ? (int)ResultCodesEnum.SUCCESS : (int)ResultCodesEnum.QUESTION_OUT_OF_DATE;

                tSQLTransaction.Commit();
            }
            catch (SqlException tSQLException)
            {
                if (tSQLTransaction != null)
                {
                    tSQLTransaction.Rollback();
                }
                Logger.WriteExceptionMessage(tSQLException);
                // Gets the corosponding database error code enum
                tResultCode = QuestionUtilities.GetDatabaseError(tSQLException.Number);
            }
            catch (Exception e)
            {
                tSQLTransaction.Rollback();
                Logger.WriteExceptionMessage(e);
                tResultCode = (int)ResultCodesEnum.CODE_FAILUER;
            }
            finally
            {
                if (tSQLCommand != null)
                {
                    tSQLCommand.Dispose();
                }

                if (SQLConnection.State == ConnectionState.Open)
                {
                    SQLConnection.Close();
                }
            }

            return tResultCode;
        }
    }
}
