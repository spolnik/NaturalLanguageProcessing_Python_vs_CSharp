using System;
using System.Collections.Generic;
using NaturalLanguageProcessing;

namespace RW_2010
{
    class Application
    {
        static void Main()
        {
            ITextChecker textChecker = new TextChecker();
            textChecker.Process("pap-100.not", NlpHelper.Iso88592);
            
            Console.WriteLine("===SHORTCUTS===");
            foreach (string shortcut in textChecker.Shortcuts.Keys)
                Console.WriteLine("Shortcut: {0}, count: {1}", shortcut, textChecker.Shortcuts[shortcut]);


            Console.WriteLine("===NEW WORDS===");
            IWordStreammer wordStreammer = new WordStreammer();

            foreach (string word in textChecker.Results.Keys)
            {
                Console.WriteLine("GENEZA: {0}", word);
                List<IEntity> wordEntities = wordStreammer.Stream(word);

                foreach (IEntity wordEntity in wordEntities)
                {
                    Console.WriteLine("BASE: {0}", wordEntity.BaseForm);
                    Console.WriteLine("LABEL: {0}", wordEntity.Label);

                    Console.Write("FORMS: ");
                    foreach (string form in wordEntity.Forms)
                        Console.Write("{0} ", form);
                    Console.WriteLine();
                    Console.WriteLine("ODMIANA WG: {0}", wordEntity.OriginalForm);

                    Console.WriteLine("================================");
                }
            }
        }
    }
}
