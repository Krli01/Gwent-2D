using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result {}

public class ProgramResult : Result
{
    public (List<Card>, List<Effect>) value {get; private set;}

    //public bool Success {get; private set;}

    public ProgramResult (List<Card> c, List<Effect> e)
    {
        value = (c, e);
    }

}

public class CardResult : Result
{
    public Card value {get; private set;}

    public CardResult (Card c)
    {
        value = c;
    }
}

public class EffectResult : Result
{
    public Effect value {get; private set;}

    public EffectResult (Effect e)
    {
        value = e;
    }
}

public class ParamDecResult : Result
{
    public string name {get; private set;}
    public string type {get; private set;}

    public ParamDecResult (string n, string t)
    {
        name = n;
        type = t;
    }
}

public class ActionResult : Result
{
    public AST_Action value {get; private set;}

    public ActionResult (AST_Action act)
    {
        value = act;
    }
}

public class StatementResult : Result
{
    public AST_Statement value {get; private set;}

    public StatementResult (AST_Statement stmt)
    {
        value = stmt;
    }
}

public class ForLoopResult : Result
{
    public AST_ForLoop value {get; private set;}

    public ForLoopResult (AST_ForLoop loop)
    {
        value = loop;
    }
}

public class ExpressionResult : Result
{
    public string type {get; private set;}
    public ExpressionResult (string t)
    {
        type = t;
    }
}

public class IdentifierResult : Result
{
    public string value {get; private set;}

    public IdentifierResult (string s)
    {
        value = s;
    }
}

public class FunctionResult : Result
{
    public string returnType {get; set;}

    public FunctionResult (string t)
    {
        returnType = t;
    }
}

public class ParamAssignResult : Result
{
    public string Name {get; set;}
    public string Value {get; set;}

    public ParamAssignResult (string name, string value)
    {
        Name = name;
        Value = value;
    }
}