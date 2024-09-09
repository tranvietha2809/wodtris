namespace Wordtris.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDictY : DictionaryBase
    {
        public EngDictY(IDictionaryLoader dictionaryLoader)
            : base(dictionaryLoader)
        {
        }
        protected override string RootLetter =>
@"Y";
    }
}

