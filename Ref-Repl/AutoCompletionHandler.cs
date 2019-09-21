using Ref.Core;
using System;
using System.Linq;

namespace Ref_Repl
{
    internal class AutoCompletionHandler : IAutoCompleteHandler
    {
        public char[] Separators { get; set; } = new char[] { ' ', ';', '$', '.' };

        public AutoCompletionHandler()
        {
            trie = new Trie<bool>();

            foreach (var ops in Enum.GetNames(typeof(OpCode)))
            {
                trie.Add(ops.ToLower(), false);
            }

            trie.Add(".register", false);
            trie.Add(".clear", false);
            trie.Add(".explain", false);
        }

        public string[] GetSuggestions(string text, int index)
        {
            var res = trie.GetByPrefix(text.ToLower());

            return res.Select(_ => _.Key).ToArray();
        }

        private Trie<bool> trie;
    }
}