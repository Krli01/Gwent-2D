using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class Parser
{
    public Token currentToken {get; private set;}
    public string Error {get; private set;}
    private List<Token> Tokens;

    public AST Run(List<Token> tokens)
    {
        Error = "";
        if (tokens == null || tokens.Count == 0) return new AST_ProgramVoid();
        Tokens = tokens;
        currentToken = Tokens[0];
        return Program();
    }

    private bool Eat(TokenType type)
    {
            if (currentToken.Type == type)
            {
                Tokens.RemoveAt(0);
                if (Tokens.Count > 0) currentToken = Tokens[0];
                return true;
            }

            Error = $"Syntax error. Line {currentToken.Line} : '{type}' expected. CurrentToken: '{currentToken.Lexeme}'";
            return false;
    }

    private bool CheckProp(string prop)
    {
        if (currentToken.Lexeme != prop)
        {
            Error = $"Syntax error. Line {currentToken.Line} : property '{prop}' expected";
            return false;
        }

        return true;
    }

    public AST Program()
    {   
        //Debug.Log($"Program: {currentToken.Type}, {currentToken.Lexeme}, {currentToken.Line}");
        if(currentToken.Type == TokenType.EOF) return new AST_ProgramVoid();

        List<AST> Items = new List<AST>();
        while (currentToken.Type != TokenType.EOF)
        {
            if (currentToken.Type == TokenType.EFFECT_DEF)
            {
                AST_EffectDefinition effectDef = EffectDefinition();
                if (effectDef == null) 
                {
                    //Debug.Log("null eff");
                    return null;
                }
                Items.Add(effectDef);
            }

            if (currentToken.Type == TokenType.CARD)
            {
                AST_CardDefinition card = CardDefinition();
                if (card == null)
                {
                    //Debug.Log("null card");
                    return null;
                }
                Items.Add(card);
            }
        }

        return new AST_Program(Items);
    }

    private AST_EffectDefinition EffectDefinition()
    {
        //Debug.Log($"EffectDefinition: {currentToken.Type}, {currentToken.Lexeme}, {currentToken.Line}");
        Eat(TokenType.EFFECT_DEF);
        //Debug.Log(currentToken.Type);
        if(!Eat(TokenType.L_BRACE)) return null;

        AST_EffectBody Body = EffectBody();
        if (Body == null) return null;
        if (!Eat(TokenType.R_BRACE)) return null;

        return new AST_EffectDefinition(Body);
    }

    private AST_EffectBody EffectBody()
    {
        if (!CheckProp("Name")) return null;
        Eat(TokenType.IDENTIFIER);
        if (!Eat(TokenType.COLON)) return null;
        
        string Name = currentToken.Lexeme;
        if (!Eat(TokenType.STRING)) return null;

        if(!Eat(TokenType.COMMA)) return null;

        List<AST_ParamDeclaration> Params = new List<AST_ParamDeclaration>();
        if (currentToken.Type == TokenType.PARAMS)
        {
            Eat(TokenType.PARAMS);
            if (!Eat(TokenType.COLON)) return null;
            if(!Eat(TokenType.L_BRACE)) return null;
            Params = EffectParams();
            if (Params == null) return null;
            if(!Eat(TokenType.R_BRACE)) return null; 
            if(!Eat(TokenType.COMMA)) return null;
        }

        if(!Eat(TokenType.ACTION)) return null;
        if(!Eat(TokenType.COLON)) return null;

        AST_Action Action = EffectAction();
        if (Action == null) return null;
        
        return new AST_EffectBody(Name, Params, Action);
    }

    private List<AST_ParamDeclaration> EffectParams()
    {
        List<AST_ParamDeclaration> _params = new List<AST_ParamDeclaration>();
        while (currentToken.Type != TokenType.R_BRACE)
        {
            AST_ParamDeclaration newParam = ParamDeclaration();
            if (newParam == null) return null;
            else _params.Add(newParam);
            //Debug.Log($"Param created: {newParam.Name}, {newParam.Type}");

            if(currentToken.Type != TokenType.COMMA) break;
            Eat(TokenType.COMMA);
        }

        return _params;
    }

    private AST_ParamDeclaration ParamDeclaration()
    {
        string name = currentToken.Lexeme;
        if (!Eat(TokenType.IDENTIFIER)) return null;
        if (!Eat(TokenType.COLON)) return null;

        if (currentToken.Type == TokenType.KW_NUMBER)
        {
            Eat(TokenType.KW_NUMBER);
            return new AST_ParamDeclaration(name, "number");
        }
        if (currentToken.Type == TokenType.KW_STRING)
        {
            Eat(TokenType.KW_STRING);
            return new AST_ParamDeclaration(name, "string");
        }
        if (currentToken.Type == TokenType.KW_BOOLEAN)
        {
            Eat(TokenType.KW_BOOLEAN);
            return new AST_ParamDeclaration(name, "boolean");
        }

        return null;
    }

    private AST_Action EffectAction()
    {
        if(!Eat(TokenType.L_PAREN)) return null;

        List<AST_Identifier> actionParams = new List<AST_Identifier>();
        while (currentToken.Type != TokenType.R_PAREN)
        {
            AST_Identifier name = new AST_Identifier(currentToken.Lexeme);
            if (!Eat(TokenType.IDENTIFIER)) return null;
            actionParams.Add(name);

            if(currentToken.Type != TokenType.COMMA) break;
            Eat(TokenType.COMMA);
        }

        if (actionParams. Count != 2)
        {
            Error = $"Syntax error. Line {currentToken.Line} : No definition of Actions takes {actionParams.Count} arguments";
            return null;
        }

        if(!Eat(TokenType.R_PAREN)) return null;
        if(!Eat(TokenType.PREDICATE_ASSIGN)) return null;

        //int l = currentToken.Line;
        AST_StatementBlock Body = StatementBlock();
        if (Body == null) return null;

        return new AST_Action(actionParams, Body);
    }

    private AST_StatementBlock StatementBlock()
    {
        if(!Eat(TokenType.L_BRACE)) return null;
        
        List<AST_Statement> Actions = new List<AST_Statement>();
        while (currentToken.Type != TokenType.R_BRACE)
        {
            if (currentToken.Type == TokenType.FOR)
            {
                AST_ForLoop forLoop = ForLoop();
                if (forLoop == null) return null;
                Actions.Add(forLoop); 
            }

            else if (currentToken.Type == TokenType.WHILE)
            {
                AST_WhileLoop whileLoop = WhileLoop();
                if (whileLoop == null) return null;
                Actions.Add(whileLoop);
            }
            
            else if (currentToken.Type == TokenType.IDENTIFIER)
            {   
                AST_Identifier id = new AST_Identifier(currentToken.Lexeme);
                Eat(TokenType.IDENTIFIER);
                
                if (currentToken.Type == TokenType.DOT)
                {
                    //Debug.Log("entra al assign prop");
                    AST_PropertyAccess prop = PropertyAccess(id);
                    if(prop == null)
                    {
                        //Debug.Log("prop null");
                        return null;
                    } 

                    if (currentToken.Type == TokenType.L_PAREN)
                    {
                        AST_FunctionCall functionCall = FunctionCall(prop);
                        if (functionCall == null) return null;
                        Actions.Add(functionCall);
                    } 

                    else if(currentToken.Type == TokenType.ASSIGN || currentToken.Type == TokenType.PLUS_ASSIGN || currentToken.Type == TokenType.MINUS_ASSIGN)
                    {
                        AST_AssignStatement assignStatement = AssignStatement(prop);
                        if (assignStatement == null)
                        {
                        //Debug.Log("assign null");
                        return null;
                        } 
                        Actions.Add(assignStatement); 
                    }

                    else
                    {
                        Error = $"Syntax error. Line {currentToken.Line} : Invalid token {currentToken.Lexeme}";
                        return null;
                    }
                }
                
                else if(currentToken.Type == TokenType.ASSIGN || currentToken.Type == TokenType.PLUS_ASSIGN || currentToken.Type == TokenType.MINUS_ASSIGN)
                {
                    AST_AssignStatement assignStatement = AssignStatement(id);
                    if (assignStatement == null) return null;
                    Actions.Add(assignStatement); 
                }

                else
                    {
                        Error = $"Syntax error. Line {currentToken.Line} : Invalid token {currentToken.Lexeme}";
                        return null;
                    }

            }

            if (!Eat(TokenType.SEMICOLON)) return null;
        }
        
        //Debug.Log("no actions: " + (Actions.Count == 0));
        if (Actions.Count == 0)
        {
            Error = $"Syntax error. Statement block must contain at least one statement";
            return null;
        } 
        if (!Eat(TokenType.R_BRACE)) return null;
        
        return new AST_StatementBlock(Actions);
    }

    private AST_FunctionCall FunctionCall(AST_PropertyAccess prop)
    {
        if (!Eat(TokenType.L_PAREN)) return null;
        
        if (currentToken.Type == TokenType.L_PAREN)
        {
            AST_Predicate predicate = Predicate();
            if (predicate == null) return null;
            if (!Eat(TokenType.R_PAREN)) return null;
            return new AST_FunctionCall(prop, predicate);
        }

        if (currentToken.Type != TokenType.R_PAREN)
        {
            AST_Expression newExp = Expression();
            if (newExp == null) return null;
            if (!Eat(TokenType.R_PAREN)) return null;
            return new AST_FunctionCall(prop, newExp);
        }

        if (!Eat(TokenType.R_PAREN)) return null;
        return new AST_FunctionCall(prop);
    }

    private AST_PropertyAccess PropertyAccess(AST_Identifier id)
    {
        List<AST_Identifier> prop = new List<AST_Identifier> {id};
        while (currentToken.Type == TokenType.DOT)
        {
            if(!Eat(TokenType.DOT)) return null;
            AST_Identifier newID = new AST_Identifier(currentToken.Lexeme);
            if(!Eat(TokenType.IDENTIFIER)) return null;
            prop.Add(newID);
        }
        
        return new AST_PropertyAccess(prop);
    }

    private AST_AssignStatement AssignStatement(AST_Identifier id)
    {   
        if (currentToken.Type == TokenType.ASSIGN)
        {
            Eat(TokenType.ASSIGN);
            AST_Expression value = Expression();
            if (value == null) return null;

            if(value.Type() is AST_PropertyAccess && currentToken.Type == TokenType.L_PAREN)
            {
                AST_FunctionCall func = FunctionCall((AST_PropertyAccess)value.Type());
                if (func == null) return null;
                return new AST_AssignStatement(id, "=", func);
            }
            return new AST_AssignStatement(id, "=", value);
        }
        if (currentToken.Type == TokenType.PLUS_ASSIGN)
        {
            Eat(TokenType.PLUS_ASSIGN);
            AST_Expression value = Expression();
            if (value == null) return null;
            if(value.Type() is AST_PropertyAccess && currentToken.Type == TokenType.L_PAREN)
            {
                AST_FunctionCall func = FunctionCall((AST_PropertyAccess)value.Type());
                if (func == null) return null;
                return new AST_AssignStatement(id, "+=", func);
            }
            return new AST_AssignStatement(id, "+=", value);
        }
        if (currentToken.Type == TokenType.MINUS_ASSIGN)
        {
            Eat(TokenType.MINUS_ASSIGN);
            AST_Expression value = Expression();
            if (value == null) return null;
            if(value.Type() is AST_PropertyAccess && currentToken.Type == TokenType.L_PAREN)
            {
                AST_FunctionCall func = FunctionCall((AST_PropertyAccess)value.Type());
                if (func == null) return null;
                return new AST_AssignStatement(id, "-=", func);
            }
            return new AST_AssignStatement(id, "-=", value);
        }
        
        return null;
    }

     private AST_AssignStatement AssignStatement(AST_PropertyAccess id)
    {   
        //Debug.Log("assign stmt prop");
        if (currentToken.Type == TokenType.ASSIGN)
        {
            Eat(TokenType.ASSIGN);
            AST_Expression value = Expression();
            if (value == null) return null;
            if(value.Type() is AST_PropertyAccess && currentToken.Type == TokenType.L_PAREN)
            {
                AST_FunctionCall func = FunctionCall((AST_PropertyAccess)value.Type());
                if (func == null) return null;
                return new AST_AssignStatement(id, "=", func);
            }
            return new AST_AssignStatement(id, "=", value);
        }
        if (currentToken.Type == TokenType.PLUS_ASSIGN)
        {
            Eat(TokenType.PLUS_ASSIGN);
            AST_Expression value = Expression();
            if (value == null) return null;
            if(value.Type() is AST_PropertyAccess && currentToken.Type == TokenType.L_PAREN)
            {
                AST_FunctionCall func = FunctionCall((AST_PropertyAccess)value.Type());
                if (func == null) return null;
                return new AST_AssignStatement(id, "+=", func);
            }
            return new AST_AssignStatement(id, "+=", value);
        }
        if (currentToken.Type == TokenType.MINUS_ASSIGN)
        {
            //Debug.Log("-=");
            Eat(TokenType.MINUS_ASSIGN);
            //Debug.Log(currentToken.Type);
            AST_Expression value = Expression();
            if (value == null) return null;
            if(value.Type() is AST_PropertyAccess && currentToken.Type == TokenType.L_PAREN)
            {
                AST_FunctionCall func = FunctionCall((AST_PropertyAccess)value.Type());
                if (func == null) return null;
                return new AST_AssignStatement(id, "-=", func);
            }
            return new AST_AssignStatement(id, "-=", value);
        }
  
        return null;
    }

    private AST_ForLoop ForLoop()
    {
        Eat(TokenType.FOR);
        AST_Identifier element = new AST_Identifier(currentToken.Lexeme);
        if (!Eat(TokenType.IDENTIFIER)) return null;
        if (!Eat(TokenType.IN)) return null;

        AST_Identifier id = new AST_Identifier(currentToken.Lexeme);
        if (!Eat(TokenType.IDENTIFIER)) return null;

        if (currentToken.Type == TokenType.DOT)
        {
            AST_PropertyAccess collection = PropertyAccess(id);
            if (collection == null) 
            {
                //Debug.Log("coll null");
                return null;
            }

            AST_StatementBlock statementss = StatementBlock();
            if (statementss == null)
            {
                //Debug.Log("stmts 1 null");
                return null;
            }

            return new AST_ForLoop(element, collection, statementss);
        }

        AST_StatementBlock statements = StatementBlock();
        if (statements == null)
        {
                //Debug.Log("stmts 2 null");
                return null;
        }

        return new AST_ForLoop(element, id, statements);
    }

    private AST_WhileLoop WhileLoop()
    {
        Eat(TokenType.WHILE);
        if (!Eat(TokenType.L_PAREN)) return null;

        AST_BooleanExpression condition = BooleanExpression();
        if (condition == null)
        {
            Error = $"Syntax error. Line {currentToken.Line} : 'while' loop must declare a condition";
            return null;
        } 

        if (!Eat(TokenType.R_PAREN)) return null;

        AST_StatementBlock actions = StatementBlock();
        if (actions == null)
        {
            //Debug.Log("while stmt null");
            return null;
        }

        return new AST_WhileLoop(condition, actions);
    }

    private AST_Expression Expression()
    {
        return BooleanExpression();
    }

    private AST_BooleanExpression BooleanExpression()
    {
        AST_ComparisonExpression left = ComparisonExpression();

        if(currentToken.Type == TokenType.AND)
        {
            Eat(TokenType.AND);
            AST_ComparisonExpression right = ComparisonExpression();
            if (right == null) return null;
            return new AST_BooleanExpression(left, "&&", right);
        }

        if(currentToken.Type == TokenType.OR)
        {
            Eat(TokenType.OR);
            AST_ComparisonExpression right = ComparisonExpression();
            if (right == null) return null;
            return new AST_BooleanExpression(left, "||", right);
        }

        return new AST_BooleanExpression(left);
    }

    private AST_ComparisonExpression ComparisonExpression()
    {
        AST_ArithmeticExpression left = ArithmeticExpression();

        if(currentToken.Type == TokenType.GREATER)
        {
            Eat(TokenType.GREATER);
            AST_ArithmeticExpression right = ArithmeticExpression();
            if (right == null) return null;
            return new AST_ComparisonExpression(left, ">", right);
        }

        if(currentToken.Type == TokenType.GREATER_EQUAL)
        {
            Eat(TokenType.GREATER_EQUAL);
            AST_ArithmeticExpression right = ArithmeticExpression();
            if (right == null) return null;
            return new AST_ComparisonExpression(left, ">=", right);
        }
        if(currentToken.Type == TokenType.LESSER)
        {
            Eat(TokenType.LESSER);
            AST_ArithmeticExpression right = ArithmeticExpression();
            if (right == null) return null;
            return new AST_ComparisonExpression(left, "<", right);
        }
        if(currentToken.Type == TokenType.LESSER_EQUAL)
        {
            Eat(TokenType.LESSER_EQUAL);
            AST_ArithmeticExpression right = ArithmeticExpression();
            if (right == null) return null;
            return new AST_ComparisonExpression(left, "<=", right);
        }
        if(currentToken.Type == TokenType.EQUALS)
        {
            Eat(TokenType.EQUALS);
            AST_ArithmeticExpression right = ArithmeticExpression();
            if (right == null) return null;
            return new AST_ComparisonExpression(left, "==", right);
        }

        return new AST_ComparisonExpression(left);
    }

    private AST_ArithmeticExpression ArithmeticExpression()
    {
        AST_Term left = Term();

        if(currentToken.Type == TokenType.PLUS)
        {
            Eat(TokenType.PLUS);
            AST_Term right = Term();
            if (right == null) return null;
            return new AST_ArithmeticExpression(left, "+", right);
        }

        if(currentToken.Type == TokenType.MINUS)
        {
            Eat(TokenType.MINUS);
            AST_Term right = Term();
            if (right == null) return null;
            return new AST_ArithmeticExpression(left, "-", right);
        }

        return new AST_ArithmeticExpression(left);
    }

    private AST_Term Term()
    {
        AST_Factor left = Factor();

        if(currentToken.Type == TokenType.MULT)
        {
            Eat(TokenType.MULT);
            AST_Factor right = Factor();
            if (right == null) return null;
            return new AST_Term(left, "*", right);
        }

        if(currentToken.Type == TokenType.DIV)
        {
            Eat(TokenType.DIV);
            AST_Factor right = Factor();
            if (right == null) return null;
            return new AST_Term(left, "/", right);
        }

        if(currentToken.Type == TokenType.POW)
        {
            Eat(TokenType.POW);
            AST_Factor right = Factor();
            if (right == null) return null;
            return new AST_Term(left, "^", right);
        }

        if(currentToken.Type == TokenType.CONCAT)
        {
            Eat(TokenType.CONCAT);
            AST_Factor right = Factor();
            if (right == null) return null;
            return new AST_Term(left, "@", right);
        }

        if(currentToken.Type == TokenType.SPACE_CONCAT)
        {
            Eat(TokenType.SPACE_CONCAT);
            AST_Factor right = Factor();
            if (right == null) return null;
            return new AST_Term(left, "@@", right);
        }

        return new AST_Term(left);
    }

    public AST_Factor Factor()
    {
        if (currentToken.Type == TokenType.NOT)
        {
            Eat(TokenType.NOT);
            AST_Factor exp = Factor();
            return new AST_Factor("!", exp);
        }
        if (currentToken.Type == TokenType.U_DECREMENT)
        {
            Eat(TokenType.U_DECREMENT);
            AST_Factor exp = Factor();
            return new AST_Factor("--", exp);
        }
        if (currentToken.Type == TokenType.U_INCREMENT)
        {
            Eat(TokenType.U_INCREMENT);
            AST_Factor exp = Factor();
            return new AST_Factor("++", exp);
        }
        if (currentToken.Type == TokenType.NUMBER)
        {
            float num = float.Parse(currentToken.Lexeme);
            Eat(TokenType.NUMBER);
            AST_Number numFactor = new AST_Number(num);
            return new AST_Factor(numFactor);
        }
        if (currentToken.Type == TokenType.STRING)
        {
            string content = currentToken.Lexeme;
            Eat(TokenType.STRING);
            AST_String stringFactor = new AST_String(content);
            return new AST_Factor(stringFactor);
        }
        if (currentToken.Type == TokenType.BOOLEAN)
        {
            bool value = bool.Parse(currentToken.Lexeme);
            Eat(TokenType.BOOLEAN);
            AST_Expression boolFactor = new AST_Boolean(value);
            return new AST_Factor(boolFactor);
        }
        if (currentToken.Type == TokenType.IDENTIFIER)
        {
            AST_Identifier id = new AST_Identifier(currentToken.Lexeme);
            Eat(TokenType.IDENTIFIER);
            
            if(currentToken.Type == TokenType.DOT)
            {
                AST_PropertyAccess prop = PropertyAccess(id);
                if (prop == null) return null;
                return new AST_Factor(prop);
            }

            return new AST_Factor(id);    
        }
        if (currentToken.Type == TokenType.L_PAREN)
        {
            Eat(TokenType.L_PAREN);
            AST_Expression exp = Expression();
            if (exp == null) return null;
            if (!Eat(TokenType.R_PAREN)) return null;

            return new AST_Factor(exp);
        }
        Error = $"Syntax error. Line {currentToken.Line} Invalid token {currentToken.Lexeme}";
        return null;
    }

    public AST_CardDefinition CardDefinition()
    {
        Eat(TokenType.CARD);
        if(!Eat(TokenType.L_BRACE)) return null;

        AST_CardBody body = CardBody();
        if (body == null) return null;

        if(!Eat(TokenType.R_BRACE)) return null;

        return new AST_CardDefinition(body);
    }

    public AST_CardBody CardBody()
    { 
        if (!CheckProp("Type")) return null;
        Eat(TokenType.IDENTIFIER);
        if(!Eat(TokenType.COLON)) return null;
        string Type = currentToken.Lexeme;
        if(!Eat(TokenType.STRING)) return null;
        
        if(!Eat(TokenType.COMMA)) return null;

        if (!CheckProp("Name")) return null;
        Eat(TokenType.IDENTIFIER);
        if(!Eat(TokenType.COLON)) return null;
        string Name = currentToken.Lexeme;
        if(!Eat(TokenType.STRING)) return null;
        
        if(!Eat(TokenType.COMMA)) return null;

        if (!CheckProp("Faction")) return null;
        Eat(TokenType.IDENTIFIER);
        if(!Eat(TokenType.COLON)) return null;
        string Faction = currentToken.Lexeme;
        if(!Eat(TokenType.STRING)) return null;
        
        if(!Eat(TokenType.COMMA)) return null;

        int Power = 0;
        if(currentToken.Lexeme == "Power")
        {
            Eat(TokenType.IDENTIFIER);
            if(!Eat(TokenType.COLON)) return null;
            Power = int.Parse(currentToken.Lexeme);
            if(!Eat(TokenType.NUMBER)) return null;
            if(!Eat(TokenType.COMMA)) return null;
        }
        
        List<string> Range = new List<string>();
        if (currentToken.Lexeme == "Range")
        {
            Eat(TokenType.IDENTIFIER);
            if(!Eat(TokenType.COLON)) return null;
            if(!Eat(TokenType.L_BRACKET)) return null;
            while(currentToken.Type != TokenType.R_BRACKET)
            {
                string s = currentToken.Lexeme;
                if(!Eat(TokenType.STRING)) return null;
                Range.Add(s);
                if (currentToken.Type == TokenType.COMMA) Eat(TokenType.COMMA);
                else if (currentToken.Type != TokenType.R_BRACKET)
                {
                    Error = $"Syntax error. Line {currentToken.Line} : TokenType.COMMA expected";
                    return null;
                }
            }
            if(!Eat(TokenType.R_BRACKET)) return null;
            if(!Eat(TokenType.COMMA)) return null;
        }
        
        if(!Eat(TokenType.ON_ACTIVATION)) return null;
        if(!Eat(TokenType.COLON)) return null;
        if(!Eat(TokenType.L_BRACKET)) return null;

        List<AST_EffectActivation> OnActivation = new List<AST_EffectActivation>();
        while (currentToken.Type != TokenType.R_BRACKET)
        {
            AST_EffectActivation Effect = EffectActivation();
            if (Effect == null) return null;
            //Debug.Log("addin effect " + Effect.Effect.Name);
            OnActivation.Add(Effect);
            if (currentToken.Type == TokenType.COMMA) Eat(TokenType.COMMA);
            else if (currentToken.Type != TokenType.R_BRACKET)
            {
                Error = $"Syntax error. Line {currentToken.Line} : TokenType.COMMA expected";
                return null;
            }
        }

        if(!Eat(TokenType.R_BRACKET)) return null;

        return new AST_CardBody(Type, Name, Faction, Power, Range, OnActivation);
    }

    private AST_EffectActivation EffectActivation()
    {
        if(!Eat(TokenType.L_BRACE)) return null;

        AST_EffectInvocation Effect = EffectInvocation();
        if (Effect == null) return null;

        if(!Eat(TokenType.R_BRACE)) return null;

        return new AST_EffectActivation(Effect);
    }

    public AST_EffectInvocation EffectInvocation()
    {
        if(!Eat(TokenType.EFFECT_PROP)) return null;
        if(!Eat(TokenType.COLON)) return null;

        if (currentToken.Type != TokenType.L_BRACE)
        {
            string NameOnly = currentToken.Lexeme;
            if(!Eat(TokenType.STRING)) return null;
            if(!Eat(TokenType.COMMA)) return null;
            return new AST_EffectInvocation(NameOnly, new List<AST_ParamAssignment>(), null);
        }

        Eat(TokenType.L_BRACE);
        if (!CheckProp("Name")) return null;
        Eat(TokenType.IDENTIFIER);
        if(!Eat(TokenType.COLON)) return null;

        string Name = currentToken.Lexeme;
        if(!Eat(TokenType.STRING)) return null;
        if(!Eat(TokenType.COMMA)) return null;

        List<AST_ParamAssignment> Params = new List<AST_ParamAssignment>();  
        while (currentToken.Type != TokenType.R_BRACE)
        {
            AST_ParamAssignment nextParam = ParamAssignment();
            if(nextParam == null) return null;
            Params.Add(nextParam);
        }

        if(!Eat(TokenType.R_BRACE)) return null;
        if(!Eat(TokenType.COMMA)) return null;

        AST_Selector selector = Selector();
        if (selector == null) return null;
        
        if(!Eat(TokenType.COMMA)) return null;
        
        if (currentToken.Type == TokenType.POST_ACTION)
        {
            AST_PostAction postAction = PostAction();
            if (postAction == null) return null;

            return new AST_EffectInvocation (Name, Params, selector, postAction);;
        }
        
        return new AST_EffectInvocation (Name, Params, selector);
    }

    public AST_ParamAssignment ParamAssignment()
    {
        string Name = currentToken.Lexeme;
        if (!Eat(TokenType.IDENTIFIER)) return null;
        if (!Eat(TokenType.COLON)) return null;

        AST_Expression Value = Expression();
        if (Value == null) return null;

        if (!Eat(TokenType.COMMA)) return null;
        
        return new AST_ParamAssignment(Name, Value);
    }

    public AST_Selector Selector()
    {
        if (!Eat(TokenType.SELECTOR)) return null;
        if (!Eat(TokenType.COLON)) return null;
        if (!Eat(TokenType.L_BRACE)) return null;

        string Source = "parent";
        if (currentToken.Type == TokenType.SOURCE)
        {
            Eat(TokenType.SOURCE);
            if (!Eat(TokenType.COLON)) return null;
            Source = currentToken.Lexeme;
            if (!Eat(TokenType.STRING)) return null;
            if (!Eat(TokenType.COMMA)) return null;
        }

        bool Single = false;
        if (currentToken.Type == TokenType.SINGLE)
        {
            Eat(TokenType.SINGLE);
            if (!Eat(TokenType.COLON)) return null;
            Single = bool.Parse(currentToken.Lexeme);
            if(!Eat(TokenType.BOOLEAN)) return null;
            if (!Eat(TokenType.COMMA)) return null;
        }

        if (!Eat(TokenType.PREDICATE)) return null;
        if (!Eat(TokenType.COLON)) return null;
        AST_Predicate predicate = Predicate();
        if (predicate == null) return null;

        //Debug.Log($"Line {currentToken.Line} : {currentToken.Type}");
        if (!Eat(TokenType.R_BRACE)) return null;

        return new AST_Selector(Source, Single, predicate);
    }

    public AST_Predicate Predicate()
    {
        if (!Eat(TokenType.L_PAREN)) return null;

        AST_Identifier id = new AST_Identifier(currentToken.Lexeme);
        if (!Eat(TokenType.IDENTIFIER)) return null;

        if (!Eat(TokenType.R_PAREN)) return null;
        if (!Eat(TokenType.PREDICATE_ASSIGN)) return null;

        AST_BooleanExpression condition = BooleanExpression();
        if (condition == null) return null;

        return new AST_Predicate(id, condition);
    }

    public AST_PostAction PostAction()
    {
        Eat(TokenType.POST_ACTION);
        if(!Eat(TokenType.COLON)) return null;
        if(!Eat(TokenType.L_BRACE)) return null;
        if (!CheckProp("Type")) return null;
        Eat(TokenType.IDENTIFIER);
        if(!Eat(TokenType.COLON)) return null;

        string type = currentToken.Lexeme;
        if(!Eat(TokenType.STRING)) return null;
        if(!Eat(TokenType.COMMA)) return null;

        List<AST_ParamAssignment> _params = new List<AST_ParamAssignment>();
        if (currentToken.Type == TokenType.PARAMS)
        {
            Eat(TokenType.PARAMS);
            if (!Eat(TokenType.COLON)) return null;
            if (!Eat(TokenType.L_BRACE)) return null;

            while (currentToken.Type != TokenType.R_BRACE)
            {
                AST_ParamAssignment p = ParamAssignment();
                if (p == null) return null;
                _params.Add(p);
            }

            Eat(TokenType.R_BRACE);
            if (!Eat(TokenType.COMMA)) return null;
        }

        AST_Selector selector = null;
        if (currentToken.Type == TokenType.SELECTOR)
        {
            selector = Selector();
            if(selector == null) return null;
        }

        if (!Eat(TokenType.COMMA)) return null;

        AST_PostAction postAction = null;
        if (currentToken.Type == TokenType.POST_ACTION)
        {
            postAction = PostAction();
            if(postAction == null) return null;
        }

        if(!Eat(TokenType.R_BRACE)) return null;

        return new AST_PostAction(type, _params, selector, postAction);
    }

}
