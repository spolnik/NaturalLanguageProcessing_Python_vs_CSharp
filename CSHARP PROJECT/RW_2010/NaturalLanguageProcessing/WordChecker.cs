using System;
using System.Data;
using System.Data.SqlServerCe;
using System.Text;

namespace NaturalLanguageProcessing
{
    public class WordChecker : IWordChecker
    {
        #region IWordChecker Members

        public bool Exist(string word)
        {
            return this.Exist(word, Encoding.Unicode);
        }

        public bool Exist(string word, Encoding wordEncoding)
        {
            if (string.IsNullOrEmpty(word))
                return true;

            if (wordEncoding != Encoding.Unicode)
                word = UnicodeConverter.Decode(word, wordEncoding);

            using (var conn = new SqlCeConnection(NlpHelper.DatabaseConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                try
                {
                    var sqlCeCommand = new SqlCeCommand(DatabaseQuery.SelectIdFromFormsWhereForm, conn)
                                           {CommandType = CommandType.Text};
                    sqlCeCommand.Parameters.AddWithValue("@form", word);

                    SqlCeResultSet resultSet =
                        sqlCeCommand.ExecuteResultSet(ResultSetOptions.Scrollable | ResultSetOptions.Insensitive);
                    return resultSet.HasRows;
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

            return false;
        }

        #endregion
    }
}