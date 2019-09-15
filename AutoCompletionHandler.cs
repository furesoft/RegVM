using System;
using System.Linq;

namespace RefVM
{
    internal class AutoCompletionHandler : IAutoCompleteHandler
    {
        public char[] Separators { get; set; } = new char[] { ' ', ';', '$' };

        public string[] GetSuggestions(string text, int index)
        {
            if (text.EndsWith('$'))
            {
                var registers = Enum.GetNames(typeof(Registers));

                return registers.ToArray();
            }

            return Enum.GetNames(typeof(OpCode));
        }
    }
}