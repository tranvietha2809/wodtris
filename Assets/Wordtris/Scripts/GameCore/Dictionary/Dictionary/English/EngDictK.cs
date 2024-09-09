namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictK : DictionaryBase
    {
        public EngDictK(IDictionaryLoader dictionaryLoader)
            : base(dictionaryLoader)
        {
        }

        protected override string RootLetter =>
@"K";
    }
}

