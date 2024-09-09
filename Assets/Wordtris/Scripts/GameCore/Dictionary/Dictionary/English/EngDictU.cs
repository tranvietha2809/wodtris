namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictU : DictionaryBase
    {
        public EngDictU(IDictionaryLoader dictionaryLoader)
            : base(dictionaryLoader)
        {
        }
        protected override string RootLetter =>
@"U";
    }
}

