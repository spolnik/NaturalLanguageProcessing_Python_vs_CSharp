using System.Text;

namespace NaturalLanguageProcessing
{
    public static class UnicodeConverter
    {
        public static string Decode(string source, Encoding sourceEncoding)
        {
            byte[] isoBytes = sourceEncoding.GetBytes(source);
            byte[] utfBytes = Encoding.Convert(sourceEncoding, Encoding.Unicode, isoBytes);
            return Encoding.Unicode.GetString(utfBytes);
        }
    }
}