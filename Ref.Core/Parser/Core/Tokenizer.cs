using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ref.Core.Parser.Core
{
    public class Tokenizer
    {
        public Tokenizer()
        {
            _tokenDefinitions = new List<TokenDefinition>();

            _tokenDefinitions.Add(new TokenDefinition(TokenType.String, "^\"[^\"]*\""));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.HexNumber, @"^0x[0-9a-zA-Z]*"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Number, "^\\d+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Data, @"^\.[a-zA-Z][a-zA-Z0-9]+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Label, @"^[a-zA-Z][a-zA-Z0-9]+:"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Mnemonic, @"^[a-zA-Z][a-zA-Z0-9]+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Register, @"^\$[a-zA-Z]+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Tab, @"^\t"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Comma, @"^,"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.NewLine, @"^\n"));
        }

        public IEnumerable<Token> Tokenize(string src)
        {
            var tokens = new List<Token>();

            string remainingText = src;

            while (!string.IsNullOrWhiteSpace(remainingText))
            {
                var match = FindMatch(remainingText);
                if (match.IsMatch)
                {
                    tokens.Add(new Token(match.TokenType, match.Value));
                    remainingText = match.RemainingText;
                }
                else
                {
                    if (IsWhitespace(remainingText))
                    {
                        remainingText = remainingText.Substring(1);
                    }
                    else
                    {
                        var invalidTokenMatch = CreateInvalidTokenMatch(remainingText);
                        tokens.Add(new Token(invalidTokenMatch.TokenType, invalidTokenMatch.Value));
                        remainingText = invalidTokenMatch.RemainingText;
                    }
                }
            }

            tokens.Add(new Token(TokenType.EOF, string.Empty));

            return tokens;
        }

        private List<TokenDefinition> _tokenDefinitions;

        private TokenMatch CreateInvalidTokenMatch(string lqlText)
        {
            var match = Regex.Match(lqlText, "(^\\S+\\s)|^\\S+");
            if (match.Success)
            {
                return new TokenMatch()
                {
                    IsMatch = true,
                    RemainingText = lqlText.Substring(match.Length),
                    TokenType = TokenType.Invalid,
                    Value = match.Value.Trim()
                };
            }

            throw new Exception("Failed to generate invalid token");
        }

        private TokenMatch FindMatch(string lqlText)
        {
            foreach (var tokenDefinition in _tokenDefinitions)
            {
                var match = tokenDefinition.Match(lqlText);
                if (match.IsMatch)
                    return match;
            }

            return new TokenMatch() { IsMatch = false };
        }

        private bool IsWhitespace(string lqlText)
        {
            return Regex.IsMatch(lqlText, "^\\s+");
        }
    }
}