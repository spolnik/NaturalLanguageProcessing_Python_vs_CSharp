using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;

namespace NaturalLanguageProcessing
{
    public class WordStreammer : IWordStreammer
    {
        private static readonly List<string> Forms;
        private readonly List<IEntity> _entities;
        private readonly IStreammingEngine _streammingEngine;

        static WordStreammer()
        {
            Forms = new List<string>();

            using (SqlCeConnection conn = new SqlCeConnection(NlpHelper.DatabaseConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                try
                {
                    var sqlCeCommand = new SqlCeCommand(DatabaseQuery.SelectFormFromForms, conn) { CommandType = CommandType.Text };
                    
                    SqlCeResultSet resultSet =
                        sqlCeCommand.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Insensitive);

                    if (resultSet.HasRows)
                    {
                        int ordForm = resultSet.GetOrdinal("Form");
                        resultSet.ReadFirst();
                        Forms.Add(resultSet.GetString(ordForm));

                        
                        while (resultSet.Read())
                        {
                            string form = resultSet.GetString(ordForm);
//                            if (!Forms.Contains(form))
                                Forms.Add(form);
                        }
                    }
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
        }

        public WordStreammer()
        {
            this._entities = new List<IEntity>();
            this._streammingEngine = new StreammingEngine();
        }

        public static List<string> AllForms
        {
            get { return Forms; }
        }

        public List<IEntity> Stream(string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentNullException("word");

            this._entities.Clear();
            List<string> similarForms = NlpHelper.FindSimilarForms(word);

            this.CreateEntities(word, similarForms);

            return this._entities;
        }

        private void CreateEntities(string word, IEnumerable<string> similarForms)
        {
            List<IEntity> entities = new List<IEntity>();

            foreach (string similarForm in similarForms)
            {
                IEntity similarFormEntity = this._streammingEngine.LoadEntityFromDatabase(similarForm);

                if (similarFormEntity.BaseForm == null ||
                    similarFormEntity.Label == null ||
                    similarFormEntity.OriginalForm == null ||
                    similarFormEntity.Prefix == null)
                {
                    continue;
                }

                bool duplicateLabel = entities.Any(entity => string.Equals(entity.Label, similarFormEntity.Label));

                if (duplicateLabel) 
                    continue;

                entities.Add(similarFormEntity);
                this._entities.Add(this._streammingEngine.GenerateNewEntity(word, similarForm, similarFormEntity));
            }
        }
    }
}