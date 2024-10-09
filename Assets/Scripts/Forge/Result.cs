using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result {}

public class ActionResult : Result
{
    public AST_Action value {get; private set;}

    public ActionResult (AST_Action act)
    {
        value = act;
    }
}

public class BooleanExpResult : Result
{
    public bool value {get; private set;}

    public BooleanExpResult (bool b)
    {
        value = b;
    }
}

public class BooleanResult : Result
{
    public bool value {get; private set;}

    public BooleanResult (bool b)
    {
        value = b;
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

public class ComparisonResult : Result
{
    public bool value {get; private set;}

    public ComparisonResult (bool c)
    {
        value = c;
    }
}

public class ContextResult : Result {} 

public class EffectResult : Result
{
    public Effect value {get; private set;}

    public EffectResult (Effect e)
    {
        value = e;
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

public class ForLoopResult : Result
{
    public AST_ForLoop value {get; private set;}

    public ForLoopResult (AST_ForLoop loop)
    {
        value = loop;
    }
}

public class FunctionAccessResult : Result
{
    public List<AST_Identifier> value {get; private set;}

    public FunctionAccessResult (List<AST_Identifier> p)
    {
        value = p;
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

public class GameCardResult : Result
{
    public GameCard value {get; private set;}

    public GameCardResult (GameCard c)
    {
        value = c;
    }
    public GameCardResult ()
    {
        
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

public class ListResult : Result
{
    public List<GameCard> value {get; private set;}

    public ListResult (List<GameCard> v)
    {
        value = v;
    }
}

public class NumberResult : Result
{
    public float value {get; private set;}

    public NumberResult (float f)
    {
        value = f;
    }
}

public class OnActivationResult : Result
{
    public AST_EffectInvocation value {get; private set;}

    public OnActivationResult (AST_EffectInvocation v)
    {
        value = v;
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

public class PlayerResult : Result
{
    public Player value {get; private set;}

    public PlayerResult(Player p)
    {
        value = p;
    }
}

public class PredicateParamResult : Result
{
    public GameCard value {get; private set;}

    public PredicateParamResult (GameCard v)
    {
        value = v;
    }
}

public class PredicateResult : Result
{
    public  Func<GameCard, bool> value {get; private set;}

    public PredicateResult ( Func<GameCard, bool> v)
    {
        value = v;
    }
}

public class ProgramResult : Result
{
    public (List<Card>, List<Effect>) value {get; private set;}

    //public bool Success {get; private set;}

    public ProgramResult (List<Card> c, List<Effect> e)
    {
        value = (c, e);
    }
}

/*public class SelectorResult : Result
{
    public List<GameCard> targets {get; private set;}
    public List<Card> graveyard {get; private set;}
    public Stack<Card> deck {get; private set;}

    public SelectorResult (List<GameCard> v)
    {
        targets = v;
    }
    public SelectorResult (Stack<Card> v)
    {
        deck = v;
    }
    public SelectorResult (List<Card> v)
    {
        graveyard = v;
    }
}*/

public class StackResult : Result
{
    public Stack<Card> value {get; private set;}

    public StackResult (Stack<Card> deck)
    {
        value = deck;
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

public class StringResult : Result
{
    public string value {get; private set;}

    public StringResult (string s)
    {
        value = s;
    }
}