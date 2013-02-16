using System.Text;

namespace NaturalLanguageProcessing
{
    public interface IWordChecker
    {
        bool Exist(string word);
        bool Exist(string word, Encoding wordEncoding);
    }
}