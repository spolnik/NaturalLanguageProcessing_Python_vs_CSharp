namespace NaturalLanguageProcessing
{
    interface IStreammingEngine
    {
        IEntity LoadEntityFromDatabase(string similarForm);
        IEntity GenerateNewEntity(string word, string similarForm, IEntity similarFormEntities);
    }
}