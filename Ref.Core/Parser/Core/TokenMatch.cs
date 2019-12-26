namespace Ref.Core.Parser.Core
{
    public class TokenMatch
    {
        public bool IsMatch { get; set; }
        public string RemainingText { get; set; }
        public TokenType TokenType { get; set; }
        public string Value { get; set; }
    }
}