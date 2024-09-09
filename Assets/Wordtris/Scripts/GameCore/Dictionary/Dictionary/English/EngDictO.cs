namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictO : DictionaryBase
    {
        public EngDictO(IDictionaryLoader dictionaryLoader)
            : base(dictionaryLoader)
        {
        }
        protected override string RootLetter =>
@"O";
    }
}

