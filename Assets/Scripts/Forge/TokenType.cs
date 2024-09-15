public enum TokenType
{
    #region Data Types
        NUMBER,
        STRING,
        BOOLEAN,
    #endregion

    #region Keywords
        IF,
        ELSE,
        KW_BOOLEAN,
        KW_STRING,
        KW_NUMBER,
        FOR,
        WHILE,
        
    #endregion

    #region Game Objects
        CARD,
        TYPE,
        NAME,
        FACTION,
        POWER,
        RANGE,
        ON_ACTIVATION,
        EFFECT_PROP,
        SELECTOR,
        SOURCE,
        SINGLE,
        PREDICATE,
        POST_ACTION,
        EFFECT_DEF,
        PARAMS,
        ACTION,
    #endregion

    #region Binary Operators
        CONCAT,
        SPACE_CONCAT,
        PLUS,
        MINUS,
        MULT,
        DIV,
        POW,
        AND,
        OR,
    #endregion

    #region Unary Operators
        NOT,
        U_INCREMENT,
        U_DECREMENT,
    #endregion

    #region Comparators
        EQUALS,
        DIFFERENT,
        LESSER,
        GREATER,
        LESSER_EQUAL,
        GREATER_EQUAL,
    #endregion

    #region Symbols

        L_PAREN,
        R_PAREN,
        L_BRACE,
        R_BRACE,
        L_BRACKET,
        R_BRACKET,
        COMMA,
        DOT,
        COLON,
        SEMICOLON,
        ASSIGN,
        PREDICATE_ASSIGN,

    #endregion

    #region Identifiers
        IDENTIFIER,
    #endregion

    #region End of File
    EOF,
    #endregion
}
