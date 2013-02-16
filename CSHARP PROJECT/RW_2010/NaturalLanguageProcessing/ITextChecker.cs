using System.Collections.Generic;
using System.Text;

namespace NaturalLanguageProcessing
{
    public interface ITextChecker
    {
        Dictionary<string, int> Results { get; }
        Dictionary<string, int> Shortcuts { get; }
        void Process(string fileName);
        void Process(string fileName, Encoding fileEncoding);
    }
}