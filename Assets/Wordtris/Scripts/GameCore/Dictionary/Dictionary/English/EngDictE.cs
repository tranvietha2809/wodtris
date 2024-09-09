namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictE : DictionaryBase
    {
        // Constructor for EngDictE that passes an IDictionaryLoader instance to the base constructor.
        public EngDictE(IDictionaryLoader dictionaryLoader)
            : base(dictionaryLoader)
        {
        }

        // Optionally override RootLetter if needed
        protected override string RootLetter => @"E"; // Example: assumes 'E' dictionary file is to be loaded
    }
}

