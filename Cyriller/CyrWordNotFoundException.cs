using System;

namespace Cyriller
{
    public class CyrWordNotFoundException : Exception
    {
        public CyrWordNotFoundException(string Word) : base("The word was not found in the collection. Word: [" + Word + "].")
        {
            this.Word = Word;
        }

        public string Word { get; protected set; }
    }
}
