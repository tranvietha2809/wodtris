namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictP : DictionaryBase
    {
        public EngDictP(IDictionaryLoader dictionaryLoader)
            : base(dictionaryLoader)
        {
        }
        protected override string RootLetter =>
@"P";
    }
}

