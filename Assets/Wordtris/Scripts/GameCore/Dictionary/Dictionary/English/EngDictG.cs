namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictG : DictionaryBase
    {
        public EngDictG(IDictionaryLoader dictionaryLoader)
            : base(dictionaryLoader)
        {
        }

        protected override string RootLetter =>
@"G";
    }
}

