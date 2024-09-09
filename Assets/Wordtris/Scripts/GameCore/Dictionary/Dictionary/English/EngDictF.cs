namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictF : DictionaryBase
    {
        public EngDictF(IDictionaryLoader dictionaryLoader)
            : base(dictionaryLoader)
        {
        }

        protected override string RootLetter =>
@"F";
    }
}

