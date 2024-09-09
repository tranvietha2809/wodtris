namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictD : DictionaryBase
    {
        public EngDictD(IDictionaryLoader dictionaryLoader)
            : base(dictionaryLoader)
        {
        }

        protected override string RootLetter =>
@"D";
    }
}

