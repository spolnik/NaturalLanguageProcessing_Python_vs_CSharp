using System.Collections.Generic;

namespace NaturalLanguageProcessing
{
    public interface IEntity
    {
        string BaseForm { get; set; }
        List<string> Forms { get; }
        double Probability { get; set; }
        string Label { get; set; }
        string Prefix { get; set; }
        string OriginalForm { get; set; }
    }
}