using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenDatabase : MonoBehaviour
{
    public static TokenDatabase Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public readonly string[] KeywordLexemes = new string[]
    {
        //"if",
        //"else",
        "for",
        "in",
        "while",
        "Number",
        "String",
        "Boolean",
    };
    public readonly Dictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>()
    {
        //{"if", TokenType.IF},
        //{"else", TokenType.ELSE},
        {"Number", TokenType.KW_NUMBER},
        {"String", TokenType.KW_STRING},
        {"Boolean", TokenType.KW_BOOLEAN},
        {"for", TokenType.FOR},
        {"in", TokenType.IN},
        {"while", TokenType.WHILE},
    };
    public readonly string[] GameObjectLexemes = new string[]
    {
        "card",
        // "Type",
        // "Name",
        // "Faction",
        // "Power",
        // "Range",
        // "Image",
        "OnActivation",
        "Effect",
        "Selector",
        "Source",
        "Single",
        "Predicate",
        "PostAction",
        "effect",
        "Params",
        "Action", 
    };
    public readonly Dictionary<string, TokenType> GameObjects = new Dictionary<string, TokenType>()
    {
        {"card", TokenType.CARD},
        //{"Type", TokenType.TYPE},
        //{"Name", TokenType.NAME},
        //{"Faction", TokenType.FACTION},
        //{"Power", TokenType.POWER},
        //{"Range", TokenType.RANGE},
        //{"Image", TokenType.IMAGE},
        {"OnActivation", TokenType.ON_ACTIVATION},
        {"Effect", TokenType.EFFECT_PROP},
        {"Selector", TokenType.SELECTOR},
        {"Source", TokenType.SOURCE},
        {"Single", TokenType.SINGLE},
        {"Predicate", TokenType.PREDICATE},
        {"PostAction", TokenType.POST_ACTION},
        {"effect", TokenType.EFFECT_DEF},
        {"Params", TokenType.PARAMS},
        {"Action", TokenType.ACTION}
    };
    public readonly Dictionary<string, TokenType> Operators = new Dictionary<string, TokenType>()
    {
        {"@", TokenType.CONCAT},
        {"@@", TokenType.SPACE_CONCAT},
        {"+", TokenType.PLUS},
        {"-", TokenType.MINUS},
        {"*", TokenType.MULT},
        {"/", TokenType.DIV},
        {"^", TokenType.POW},
        {"&&", TokenType.AND},
        {"||", TokenType.OR},
        {"!", TokenType.NOT},
        {"++", TokenType.U_INCREMENT},
        {"--", TokenType.U_DECREMENT},
    };
    public readonly Dictionary<string, TokenType> Comparators = new Dictionary<string, TokenType>()
    {
        {"==", TokenType.EQUALS},
        {"!=", TokenType.DIFFERENT},
        {"<", TokenType.LESSER},
        {">", TokenType.GREATER},
        {"<=", TokenType.LESSER_EQUAL},
        {">=", TokenType.GREATER_EQUAL},
    };
    public readonly Dictionary<string, TokenType> Symbols = new Dictionary<string, TokenType>()
    {
        {"(", TokenType.L_PAREN},
        {")", TokenType.R_PAREN},
        {"{", TokenType.L_BRACE},
        {"}", TokenType.R_BRACE},
        {"[", TokenType.L_BRACKET},
        {"]", TokenType.R_BRACKET},
        {",", TokenType.COMMA},
        {".", TokenType.DOT},
        {":", TokenType.COLON},
        {";", TokenType.SEMICOLON},
        {"=", TokenType.ASSIGN},
        {"+=", TokenType.PLUS_ASSIGN},
        {"-=", TokenType.MINUS_ASSIGN},
        {"=>", TokenType.PREDICATE_ASSIGN},
    };
}
