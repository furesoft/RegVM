namespace Ref.Core.Parser.Core
{
    public enum TokenType
    {
        Mnemonic,
        Label,
        Number,
        Register,
        Data,
        String,
        Tab,
        EOF,
        Comma,
        Invalid,
        HexNumber,
        NewLine,
    }
}