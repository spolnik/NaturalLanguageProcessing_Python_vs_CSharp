using System.Collections.Generic;

namespace NaturalLanguageProcessing
{
    public class StreamingEntity : IEntity
    {
        private readonly List<string> _forms = new List<string>();

        #region IEntity Members

        public string BaseForm { get; set; }

        public List<string> Forms
        {
            get { return this._forms; }
        }

        public double Probability { get; set; }
        public string Label { get; set; }
        public string Prefix { get; set; }
        public string OriginalForm { get; set; }
        
        #endregion
    }
}