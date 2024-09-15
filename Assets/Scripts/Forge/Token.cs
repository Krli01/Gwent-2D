public class Token
{
    public TokenType Type {get; private set;}
    public string Lexeme {get; private set;}
    public int Line {get; private set;}

    public Token(TokenType type, string lexeme, int line)
    {
        Type = type;
        Line = line;
        Lexeme = lexeme;
    }
}