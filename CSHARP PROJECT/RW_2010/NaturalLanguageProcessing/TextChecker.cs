using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NaturalLanguageProcessing
{
    public class TextChecker : ITextChecker
    {
        private readonly Dictionary<string, int> _results;
        private readonly Dictionary<string, int> _shortcuts;
        private readonly WordChecker _wordChecker;

        public TextChecker()
        {
            this._results = new Dictionary<string, int>();
            this._shortcuts = new Dictionary<string, int>();
            this._wordChecker = new WordChecker();
        }

        #region ITextChecker Members

        public void Process(string fileName)
        {
            this.Process(fileName, Encoding.Unicode);
        }

        public void Process(string fileName, Encoding fileEncoding)
        {
            this._results.Clear();

            var reader = new StreamReader(fileName, fileEncoding);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (NlpHelper.HeaderPapRegex.IsMatch(line))
                    continue;

                if (fileEncoding != Encoding.Unicode)
                    line = UnicodeConverter.Decode(line, fileEncoding);

                string[] words = Regex.Split(line.Trim(), "[[]|[]]|[_]|[|]|\\d+|\\s+|[-&#;!.:?,\")(\'\\/]");

                foreach (string word in words.Where(word => word.Length > 2 && !this._wordChecker.Exist(word)))
                    this.ProcessWord(word);
            }
        }

        public Dictionary<string, int> Results
        {
            get { return this._results; }
        }


        public Dictionary<string, int> Shortcuts
        {
            get { return this._shortcuts; }
        }

        #endregion

        private void ProcessWord(string word)
        {
            if (word.Length == 3 || NlpHelper.AreAllLettersUpper(word))
            {
                if (this._shortcuts.ContainsKey(word))
                    this._shortcuts[word]++;
                else
                    this._shortcuts[word] = 1;
            }
            else if (this._results.ContainsKey(word))
                this._results[word]++;
            else
                this._results[word] = 1;
        }
    }
}