using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ref.Core.Parser.Core
{
    public class BaseParser
    {
        public static AsmFile Parse(string src)
        {
            var p = new BaseParser();

            var tokenizer = new Tokenizer();
            p._tokens = tokenizer.Tokenize(src).ToList();

            var tokens = new List<Token>();
            Token t;

            do
            {
                t = p.NextToken();
                System.Console.WriteLine(t);
                tokens.Add(t);
            }
            while (t.TokenType != TokenType.EOF);

            return null;
        }

        public Token NextToken()
        {
            return _tokens[_tokenIndex++];
        }

        private int _tokenIndex;
        private List<Token> _tokens = new List<Token>();
    }
}