using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Lexer : MonoBehaviour
{
    private string inputText {get; set;}
    public int currentLine { get; private set; }
    public string Error { get; private set; }
    public bool Success { get; private set;}
    private List<Token> Tokens = new List<Token>();

    public List<Token> Run(string input)
    {
        Success = false;
        currentLine = 1;
        Error = "";
        inputText = input;
        Tokens.Clear();
        
        Token nextToken;
        do
        {
            nextToken = GetNextToken();
            Tokens.Add(nextToken);
        }
        while (nextToken != null && nextToken.Type != TokenType.EOF);

        if (nextToken == null) Tokens.Clear();

        return Tokens;
    }

    Token GetNextToken()
    {
        if (inputText == "")
        {
            Success = true;
            return new Token(TokenType.EOF, "", currentLine);
        } 
            

        var firstSeparator = Regex.Match(inputText, @"\s+|\n|\""|[\[\](){},\.;:]|(=>)|(!=)|(==)|=|[*/^!|&]|@@|@|
            \+\+|\+|(\-\-)|\-|[<>]|(>=)|(<=)");
        if (firstSeparator.Success)
        {
            if (firstSeparator.Index != 0)
            {
                string data = inputText.Substring(0, firstSeparator.Index);
                
                if (TokenDatabase.Instance.KeywordLexemes.Contains(data))
                {
                    inputText = inputText.Substring(data.Length);
                    return CreateKeywordToken(data);
                }

                if (TokenDatabase.Instance.GameObjectLexemes.Contains(data))
                {
                    inputText = inputText.Substring(data.Length);
                    return CreateGameObjectToken(data);
                }

                var possibleNum = Regex.Match (data, @"\d+(\.\d+)*");
                if (possibleNum.Success && possibleNum.Index == 0)
                {
                    inputText = inputText.Substring(possibleNum.Length);
                    return CreateNumberToken(possibleNum);
                }

                if (data == "true" || data == "false")
                {
                    inputText = inputText.Substring(data.Length);
                    return CreateBooleanToken(data);
                }

                inputText = inputText.Substring(data.Length);
                return CreateIdentifierToken(data);
            }
            else
            {
                if (firstSeparator.Value == "\"")
                {
                    inputText = inputText.Substring(1);
                    var nextQuotation = Regex.Match(inputText, "\"");
                    if (nextQuotation.Success)
                    {
                        string stringToken = inputText.Substring(0,nextQuotation.Index);
                        inputText = inputText.Substring(nextQuotation.Index+1);
                        return CreateStringToken(stringToken);
                    }
                    else
                    {
                        Error = $"Line {currentLine}: Invalid token";
                        return null;
                    }
                }
                if (Regex.Match(firstSeparator.Value, @"\s+").Success)
                {
                    inputText = inputText.Substring(firstSeparator.Length);
                    return GetNextToken();
                }
                if (firstSeparator.Value == "\n")
                {
                    inputText = inputText.Substring(1);
                    currentLine ++;
                    return GetNextToken();
                }
                if (Regex.Match(firstSeparator.Value, @"[*/^!|&]|@@|@|\+\+|\+|(\-\-)|\-").Success)
                {    
                    //Debug.Log(firstSeparator.Value);
                    inputText = inputText.Substring(firstSeparator.Length);
                    return CreateOperatorToken(firstSeparator);
                }

                if (Regex.Match(firstSeparator.Value, @"[<>]|(>=)|(<=)|(==)").Success)
                {
                    inputText = inputText.Substring(firstSeparator.Length);
                    return CreateComparatorToken(firstSeparator);
                }

                if (Regex.Match(firstSeparator.Value, @"[\[\](){},\.;:]|(=>)|(!=)|=").Success)
                {
                    inputText = inputText.Substring(firstSeparator.Length);
                    return CreateSymbolToken(firstSeparator);
                }
            }
        }

        Error = $"Line {currentLine}: Separator (symbol) expected";
        return null;
    }    
    
    private Token CreateBooleanToken(string value)
    {
        return new Token(TokenType.BOOLEAN, value, currentLine);
    }
    private Token CreateComparatorToken(Match match)
    {
        TokenType type = TokenDatabase.Instance.Comparators[match.Value];
        return new Token(type, match.Value, currentLine);
    }
    private Token CreateGameObjectToken(string data)
    {
        TokenType type = TokenDatabase.Instance.GameObjects[data];
        return new Token(type, data, currentLine);
    }
    private Token CreateIdentifierToken(string data)
    {
        if(Regex.Match(data, @"^[A-Za-z][0-9A-Za-z_]*").Success)
            return new Token(TokenType.IDENTIFIER, data, currentLine);
        else
        {
            Error = $"Line {currentLine} : Invalid token '{data}'";
            return null;
        }
    }
    private Token CreateKeywordToken(string data)
    {
        TokenType type = TokenDatabase.Instance.Keywords[data];
        return new Token(type, data, currentLine);
    }
    private Token CreateNumberToken(Match match)
    {
        return new Token(TokenType.NUMBER, match.Value, currentLine);
    }
    private Token CreateOperatorToken(Match match)
    {
        if(inputText[0] == '+')
        {
            inputText = inputText.Substring(1);
            return new Token(TokenType.U_INCREMENT, match.Value, currentLine);
        }
        TokenType type = TokenDatabase.Instance.Operators[match.Value];
        return new Token(type, match.Value, currentLine);
    }
    private Token CreateStringToken(string data)
    {
        return new Token(TokenType.STRING, data, currentLine);
    }
    private Token CreateSymbolToken(Match match)
    {
        TokenType type = TokenDatabase.Instance.Symbols[match.Value];
        return new Token(type, match.Value, currentLine);
    }
}