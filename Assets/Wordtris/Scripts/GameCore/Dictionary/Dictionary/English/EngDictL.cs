namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictL : DictionaryBase
    {
        public EngDictL(IDictionaryLoader dictionaryLoader)
    : base(dictionaryLoader)
        {
        }

        protected override string RootLetter =>
@"L";
    }
}

