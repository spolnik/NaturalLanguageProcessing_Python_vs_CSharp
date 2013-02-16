using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;

namespace NaturalLanguageProcessing
{
    class StreammingEngine : IStreammingEngine
    {
        public IEntity LoadEntityFromDatabase(string similarForm)
        {
            IEntity entity = new StreamingEntity();

            using (var conn = new SqlCeConnection(NlpHelper.DatabaseConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                try
                {
                    var sqlCeCommand = new SqlCeCommand(DatabaseQuery.SelectIdFromFormsWhereForm, conn) { CommandType = CommandType.Text };
                    sqlCeCommand.Parameters.AddWithValue("@form", similarForm);

                    SqlCeResultSet resultSet = sqlCeCommand.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Insensitive);
                    List<int> ids = new List<int>();

                    if (resultSet.HasRows)
                    {
                        int ordId = resultSet.GetOrdinal("ID");
                        resultSet.ReadFirst();
                        ids.Add(resultSet.GetInt32(ordId));

                        while (resultSet.Read())
                            ids.Add(resultSet.GetInt32(ordId));
                    }

                    int id = ids[0];
//                    foreach (int id in ids)
                    {
                        sqlCeCommand = new SqlCeCommand(DatabaseQuery.SelectAllFromBaseWordWhereId, conn);
                        sqlCeCommand.Parameters.AddWithValue("@id", id);

                        resultSet =
                            sqlCeCommand.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Insensitive);

                        if (resultSet.HasRows)
                        {
                            int ordBaseForm = resultSet.GetOrdinal("BaseForm");
                            int ordLabel = resultSet.GetOrdinal("Label");
                            int ordPrefix= resultSet.GetOrdinal("Prefix");
                            resultSet.ReadFirst();

                            entity.BaseForm = resultSet.GetString(ordBaseForm);
                            entity.Label = resultSet.GetString(ordLabel);
                            entity.Prefix = resultSet.GetString(ordPrefix);
                        }

                        sqlCeCommand = new SqlCeCommand(DatabaseQuery.SelectFormFromFormsWhereId, conn);
                        sqlCeCommand.Parameters.AddWithValue("@id", id);

                        resultSet =
                            sqlCeCommand.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Insensitive);

                        if (resultSet.HasRows)
                        {
                            int ordForm = resultSet.GetOrdinal("Form");
                            resultSet.ReadFirst();

                            entity.Forms.Add(resultSet.GetString(ordForm));

                            while (resultSet.Read())
                                entity.Forms.Add(resultSet.GetString(ordForm));
                        }
                    }

                    return entity;
                }
                catch (SqlCeException sqlexception)
                {
                    Console.WriteLine("Error form: {0}", sqlexception.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error form: {0}", ex.Message);
                }
            }

            return null;
        }

        public IEntity GenerateNewEntity(string word, string similarForm, IEntity similarFormEntities)
        {
            IEntity wordEntity = new StreamingEntity(); 

            string postfix = similarForm.Substring(similarFormEntities.Prefix.Length);

            wordEntity.Prefix = string.IsNullOrEmpty(postfix) ? word : word.Remove(word.Length - postfix.Length);
            
            string appendBaseForm = similarFormEntities.BaseForm.Substring(similarFormEntities.Prefix.Length);

            wordEntity.BaseForm = string.Concat(wordEntity.Prefix, appendBaseForm);

            foreach (string form in similarFormEntities.Forms)
            {
                string appendForm = form.Substring(similarFormEntities.Prefix.Length);
                wordEntity.Forms.Add(string.Concat(wordEntity.Prefix, appendForm));
            }

            wordEntity.Label = similarFormEntities.Label;
            wordEntity.Probability = 100.0;
            wordEntity.OriginalForm = similarFormEntities.BaseForm;

            return wordEntity;
        }
    }
}