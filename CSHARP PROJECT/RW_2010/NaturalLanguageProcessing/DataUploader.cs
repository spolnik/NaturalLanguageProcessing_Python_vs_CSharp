using System;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;

namespace NaturalLanguageProcessing
{
    public static class DataUploader
    {
        static int _counter;

        public static void UploadData()
        {
            string firstLine, secondLine;

            StreamReader reader = new StreamReader("base.iso", NlpHelper.Windows1250Encoding);
            
            using (SqlCeConnection conn = new SqlCeConnection(NlpHelper.DatabaseConnectionString))
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                while ((firstLine = reader.ReadLine()) != null)
                {
                    _counter++;

                    firstLine = UnicodeConverter.Decode(firstLine, NlpHelper.Windows1250Encoding);
                    secondLine = UnicodeConverter.Decode(reader.ReadLine(), NlpHelper.Windows1250Encoding);

                    string[] baseFormAttributes = firstLine.Split(' ');
                    secondLine = secondLine.TrimStart(new[] { '[', '\'' });
                    secondLine = secondLine.TrimEnd(new[] { ']', '\'' });
                    string[] forms = secondLine.Split(new[] { @"', '" }, StringSplitOptions.RemoveEmptyEntries);

                    string prefix = baseFormAttributes[1];

                    bool lookingForPrefix = true;
                    while (lookingForPrefix)
                    {
                        lookingForPrefix = false;
                        foreach (string form in forms)
                        {
                            if (form.StartsWith(prefix))
                                continue;
                            lookingForPrefix = true;
                            break;
                        }

                        if (lookingForPrefix)
                            prefix = prefix.Substring(0, prefix.Length - 1);
                    }

                    int id = Int32.Parse(baseFormAttributes[0]);
                    AddBaseWordRow(conn, id,
                        baseFormAttributes[1],
                        baseFormAttributes[2],
                        prefix);

                    foreach (string form in forms)
                        AddFormsRow(conn, id, form);
                }
            }
        }

        static void AddFormsRow(SqlCeConnection conn, int id, string form)
        {
            string sql = string.Concat("INSERT INTO Forms (ID, Form) ",
                "VALUES (@id, @form)");

            try
            {
                SqlCeCommand sqlCeCommand = new SqlCeCommand(sql, conn) { CommandType = CommandType.Text };

                sqlCeCommand.Parameters.AddWithValue("@id", id);
                sqlCeCommand.Parameters.AddWithValue("@form", form);
                sqlCeCommand.ExecuteNonQuery();
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

        static void AddBaseWordRow(SqlCeConnection conn, int id, string baseForm, string label, string prefix)
        {
            string sql = string.Concat("INSERT INTO BaseWord ",
                "(ID, BaseForm, Label, Prefix) ",
                "VALUES (@id, @baseForm, @label, @prefix)");

            try
            {
                SqlCeCommand sqlCeCommand = new SqlCeCommand(sql, conn) { CommandType = CommandType.Text };
                sqlCeCommand.Parameters.AddWithValue("@id", id);
                sqlCeCommand.Parameters.AddWithValue("@baseForm", baseForm);
                sqlCeCommand.Parameters.AddWithValue("@label", label);
                sqlCeCommand.Parameters.AddWithValue("@prefix", prefix);
                sqlCeCommand.ExecuteNonQuery();
                Console.WriteLine("Base word row added, coutner: {0}", _counter);
            }
            catch (SqlCeException sqlexception)
            {
                Console.WriteLine("Error base: {0}", sqlexception.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error base: {0}", ex.Message);
            }
        }
    }
}