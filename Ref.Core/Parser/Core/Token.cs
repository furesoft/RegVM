namespace Ref.Core.Parser.Core
{
    public class Token
    {
        public TokenType TokenType { get; set; }

        public string Value { get; set; }

        public Token(TokenType tokenType)
        {
            TokenType = tokenType;
            Value = string.Empty;
        }

        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public Token Clone()
        {
            return new Token(TokenType, Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}