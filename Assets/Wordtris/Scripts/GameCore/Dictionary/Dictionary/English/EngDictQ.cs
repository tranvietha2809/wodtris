namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictQ : DictionaryBase
    {
        public EngDictQ(IDictionaryLoader dictionaryLoader)
            : base(dictionaryLoader)
        {
        }
        protected override string RootLetter =>
@"Q";
    }
}

