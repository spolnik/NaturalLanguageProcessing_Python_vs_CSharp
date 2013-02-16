using System.Collections.Generic;

namespace NaturalLanguageProcessing
{
    public interface IWordStreammer
    {
        List<IEntity> Stream(string word);
    }
}