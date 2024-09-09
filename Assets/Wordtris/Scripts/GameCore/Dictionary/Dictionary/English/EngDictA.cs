namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictA : DictionaryBase
    {
        public EngDictA(IDictionaryLoader dictionaryLoader)
            : base(dictionaryLoader)
        {
        }

        protected override string RootLetter =>
@"A";
    }
}

