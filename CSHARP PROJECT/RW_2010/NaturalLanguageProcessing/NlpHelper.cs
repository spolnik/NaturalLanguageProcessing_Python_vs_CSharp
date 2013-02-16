using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NaturalLanguageProcessing
{
    public static class NlpHelper
    {
        public static readonly string DatabaseConnectionString = "data source='RwDb.sdf'";
        public static readonly Encoding Windows1250Encoding = Encoding.GetEncoding("windows-1250");
        public static readonly Encoding Iso88592 = Encoding.GetEncoding("iso-8859-2");

        public static readonly Regex HeaderPapRegex = new Regex("^#\\d{6}");

        public static bool AreAllLettersUpper(string word)
        {
            return word.All(c => !Char.IsLower(c));
        }

        public static int GetCountCommonLettersFromEnd(string word, string form)
        {
            int count = 0;
            for (int i = word.Length - 1, j = form.Length - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (word[i] == form[j])
                    count++;
                else
                    break;
            }

            return count;
        }

        public static List<string> FindSimilarForms(string word)
        {
            int max = 0;

            List<string> similarForms = new List<string>();

            foreach (string form in WordStreammer.AllForms)
            {
                int sameCharsCount = GetCountCommonLettersFromEnd(word, form);
                if (sameCharsCount > max)
                {
                    max = sameCharsCount;
                    similarForms.Clear();
                }

                if (sameCharsCount == max)
                    if (!similarForms.Contains(form))
                        similarForms.Add(form);
            }

            return similarForms;
        }
    }
}