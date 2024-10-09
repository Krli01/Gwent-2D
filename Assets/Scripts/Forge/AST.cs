using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class AST : IVisitable
{
    public abstract Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    public virtual AST Type()
    {
        return this;
    }
}

public class AST_Action : AST
{
    public List<AST_Identifier> ParamNames {get; private set;}
    public AST_StatementBlock Actions {get; private set;}

    public AST_Action(List<AST_Identifier> paramNames, AST_StatementBlock actions)
    {
        ParamNames = paramNames;
        Actions = actions;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_ArithmeticExpression : AST_Expression
{
    public AST_Term Left {get; private set;}
    public AST_Term Right {get; private set;}
    public string Op {get; private set;}

    public AST_ArithmeticExpression (AST_Term exp)
    {
        Left = exp;
    }
    public AST_ArithmeticExpression(AST_Term left, string op, AST_Term right)
    {
        Left = left;
        Op = op;
        Right = right;
    }

    public override AST Type()
    {
        return Left.Type();
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_BinaryAssignStatement : AST_Statement
{
    public AST_Identifier ID {get; private set;}
    public AST_PropertyAccess Prop {get; private set;}
    public string Op {get; private set;}
    public AST_Expression Exp {get; private set;}
    public AST_FunctionCall Func {get; private set;}

    public AST_BinaryAssignStatement (AST_Identifier id, string op, AST_Expression exp)
    {
        ID = id;
        Op = op;
        Exp = exp;
    }

    public AST_BinaryAssignStatement (AST_PropertyAccess prop, string op, AST_Expression exp)
    {
        Prop = prop;
        Op = op;
        Exp = exp;
    }

    public AST_BinaryAssignStatement (AST_Identifier id, string op, AST_FunctionCall func)
    {
        ID = id;
        Op = op;
        Func = func;
    }

    public AST_BinaryAssignStatement (AST_PropertyAccess prop, string op, AST_FunctionCall func)
    {
        Prop = prop;
        Op = op;
        Func = func;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_BooleanExpression : AST_Expression
{
    public AST_ComparisonExpression Left {get; private set;}
    public AST_ComparisonExpression Right {get; private set;}
    public string Op {get; private set;}
    
    public AST_BooleanExpression (AST_ComparisonExpression exp)
    {
        Left = exp;
    }
    public AST_BooleanExpression(AST_ComparisonExpression left, string op, AST_ComparisonExpression right)
    {
        Left = left;
        Op = op;
        Right = right;
    }

    public override AST Type()
    {
        return Left.Type();
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_Boolean : AST_Expression
{
    public bool Value {get; private set;}

    public AST_Boolean (bool content)
    {
        Value = content;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_CardDefinition : AST
{
    public AST_CardBody Body {get; private set;}

    public AST_CardDefinition (AST_CardBody body)
    {
        Body = body;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_CardBody : AST
{
    public string type {get; private set;}
    public string Name {get; private set;}
    public string Faction {get; private set;}
    public int Power {get; private set;}
    public List<string> Range {get; private set;}
    public List<AST_EffectActivation> Effect {get; private set;}
    public AST_CardBody(string _type, string name, string faction, int power, List<string> range, List<AST_EffectActivation> effect)
    {
        type = _type;
        Name = name;
        Faction = faction;
        Power = power;
        Range = range;
        Effect = effect;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_ComparisonExpression : AST_Expression
{
    public AST_ArithmeticExpression Left {get; private set;}
    public AST_ArithmeticExpression Right {get; private set;}
    public string Op {get; private set;}

    public AST_ComparisonExpression (AST_ArithmeticExpression exp)
    {
        Left = exp;
    }
    public AST_ComparisonExpression(AST_ArithmeticExpression left, string op, AST_ArithmeticExpression right)
    {
        Left = left;
        Op = op;
        Right = right;
    }

    public override AST Type()
    {
        return Left.Type();
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_EffectActivation : AST
{
    public AST_EffectInvocation Effect {get; private set;}

    public AST_EffectActivation ( AST_EffectInvocation effect)
    {
        Effect = effect;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_EffectBody : AST
{
    public string Name {get; private set;}
    public List<AST_ParamDeclaration> Params {get; private set;}
    public AST_Action Action {get; private set;}

    public AST_EffectBody(string name, List<AST_ParamDeclaration> _params, AST_Action action)
    {
        Name = name;
        Params = _params;
        Action = action;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_EffectDefinition : AST
{
    public AST_EffectBody Body {get; private set;}

    public AST_EffectDefinition(AST_EffectBody body)
    {
        Body = body;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_EffectInvocation : AST
{
    public string Name {get; private set;}
    public List<AST_ParamAssignment> Params {get; private set;}
    public AST_Selector Selector {get; private set;}
    public AST_PostAction PostAction {get; private set;}

    public AST_EffectInvocation(string effect, List<AST_ParamAssignment> _params, AST_Selector selector, AST_PostAction postAction)
    {
        Name = effect;
        Params = _params;
        Selector = selector;
        PostAction = postAction;
    }

    public AST_EffectInvocation(string effect, List<AST_ParamAssignment> _params, AST_Selector selector)
    {
        Name = effect;
        Params = _params;
        Selector = selector;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public abstract class AST_Expression : AST 
{   

}

public class AST_Factor : AST_Expression
{
    public AST_Expression Expression {get; private set;}
    //public AST_UnaryAssignFactor UnaryOp {get; private set;}

    public AST_Factor(AST_Expression expression)
    {
        Expression = expression;
    }
    // public AST_Factor(AST_UnaryAssignFactor unaryOp)
    // {
    //     UnaryOp = unaryOp;
    // }

    public override AST Type()
    {
        //if (UnaryOp != null) return UnaryOp.Type();
        return Expression.Type();
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_ForLoop : AST_Statement
{
    public AST_Identifier Element {get; private set;}
    public AST_Identifier Identifier {get; private set;}
    public AST_PropertyAccess Collection {get; private set;}
    public AST_FunctionCall Function {get; private set;}
    public AST_StatementBlock Statements {get; private set;}

    public AST_ForLoop (AST_Identifier element, AST_Identifier identifier, AST_StatementBlock statements)
    {
        Element = element;
        Identifier = identifier;
        Statements = statements;
    }
    public AST_ForLoop (AST_Identifier element, AST_PropertyAccess collection, AST_StatementBlock statements)
    {
        Element = element;
        Collection = collection;
        Statements = statements;
    }
    public AST_ForLoop (AST_Identifier element, AST_FunctionCall function, AST_StatementBlock statements)
    {
        Element = element;
        Function = function;
        Statements = statements;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_FunctionCall : AST_Statement
{
    public AST_PropertyAccess Prop {get; private set;}
    public AST_Expression Param {get; private set;}
    public AST_Predicate Predicate {get; private set;}

    public AST_FunctionCall (AST_PropertyAccess prop)
    {
        Prop = prop;
    }
    public AST_FunctionCall (AST_PropertyAccess prop, AST_Expression param)
    {
        Prop = prop;
        Param = param;
    }
    public AST_FunctionCall (AST_PropertyAccess prop, AST_Predicate predicate)
    {
        Prop = prop;
        Predicate = predicate;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_Identifier : AST_Expression
{
    public string ID {get; private set;}

    public AST_Identifier (string id)
    {
        ID = id;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_Number : AST_Expression
{
    public float Value {get; private set;}

    public AST_Number (float num)
    {
        Value = num;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_ParamAssignment : AST
{
    public string Name {get; private set;}
    public AST_Expression Value {get; private set;}

    public AST_ParamAssignment (string name, AST_Expression value)
    {
        Name = name;
        Value = value;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_ParamDeclaration : AST
{
    public string Name {get; private set;}
    public string type {get; private set;}

    public AST_ParamDeclaration(string name, string _type)
    {
        Name = name;
        type = _type;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_PostAction : AST
{
    public string type {get; private set;}
    public List<AST_ParamAssignment> Params {get; private set;}
    public AST_Selector Selector {get; private set;}
    public AST_PostAction PostAction {get; private set;}
    
    public AST_PostAction (string _type, List<AST_ParamAssignment> _params, AST_Selector selector, AST_PostAction postAction)
    {
        type = _type;
        Params = _params;
        Selector = selector;
        PostAction = postAction;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_Predicate : AST
{
    public AST_Identifier ID {get; private set;}
    public AST_BooleanExpression Condition {get; private set;}

    public AST_Predicate (AST_Identifier id, AST_BooleanExpression condition)
    {
        ID = id;
        Condition = condition;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_Program : AST
{
    //public List<Card> Cards {get; private set;}
    //public List<Effect> Effects {get; private set;}
    public List<AST> Items = new List<AST>();

    public AST_Program(List<AST> items)
    {
        Items = items;
        // Cards = new List<Card>();
        // Effects = new List<Effect>();
       // (List<Card>, List<Effect>) Lists = (Cards, Effects);
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_ProgramVoid : AST
{
    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_PropertyAccess : AST_Expression
{
    public List<AST_Identifier> Prop {get; private set;}

    public AST_PropertyAccess (List<AST_Identifier> props)
    {
        Prop = props;
    }

    public override string ToString()
    {
        string s = "";
        for (int i = 0; i < Prop.Count; i++)
        {
            if (s == "") s = Prop[i].ID;
            else s += "." + Prop[i].ID;
        }
        return s;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_Selector : AST
{
    public string Source {get; private set;}
    public AST_Expression Single {get; private set;}
    public AST_Predicate Predicate {get; private set;}

    public AST_Selector (string source, AST_Expression single, AST_Predicate predicate)
    {
        Source = source;
        Single = single;
        Predicate = predicate;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public abstract class AST_Statement : AST
{
    //ForLoop | WhileLoop | UnaryAssignment | BinaryAssignment | FunctionCall 
}

public class AST_StatementBlock : AST
{
    public List<AST_Statement> Statements {get; private set;}

    public AST_StatementBlock(List<AST_Statement> statements)
    {
        Statements = statements;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }



    // public override dynamic Visit(Dictionary<string, Effect> GlobalScope, Dictionary<string, dynamic> LocalScope)
    // {
    //     List <dynamic> result = new List<dynamic>();
    //     foreach (var statement in Statements)
    //     {
    //         var nextStatement = statement.Visit(GlobalScope, LocalScope);
    //         result.Add(nextStatement);
    //     }
    //     return result;
    // }
}

public class AST_String : AST_Expression
{
    public string Value {get; private set;}

    public AST_String (string content)
    {
        Value = content;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_Term : AST_Expression
{
    public AST_Factor Left {get; private set;}
    public AST_Factor Right {get; private set;}
    public string Op {get; private set;}

    public AST_Term (AST_Factor exp)
    {
        Left = exp;
    }
    public AST_Term(AST_Factor left, string op, AST_Factor right)
    {
        Left = left;
        Op = op;
        Right = right;
    }

    public override AST Type()
    {
        return Left.Type();
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_UnaryAssignFactor : AST_Expression
{
    public string Op {get; private set;}
    public AST_Identifier ID {get; private set;}
    public AST_PropertyAccess Prop {get; private set;}

    public AST_UnaryAssignFactor (string op, AST_Identifier id)
    {
        ID = id;
        Op = op;
    }

    public AST_UnaryAssignFactor (string op, AST_PropertyAccess prop)
    {
        Prop = prop;
        Op = op;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }

    public override AST Type()
    {
        if (Op == "++" || Op == "--") return new AST_Number(0); // ???
        else return new AST_Boolean(true); // ???
    }
}

public class AST_UnaryAssignStatement : AST_Statement
{
    public string Op {get; private set;}
    public AST_Identifier ID {get; private set;}
    public AST_PropertyAccess Prop {get; private set;}

    public AST_UnaryAssignStatement (string op, AST_Identifier id)
    {
        ID = id;
        Op = op;
    }

    public AST_UnaryAssignStatement (string op, AST_PropertyAccess prop)
    {
        Prop = prop;
        Op = op;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}

public class AST_WhileLoop : AST_Statement
{
    public AST_BooleanExpression Condition {get; private set;}
    public AST_StatementBlock Actions {get; private set;}

    public AST_WhileLoop (AST_BooleanExpression condition, AST_StatementBlock actions)
    {
        Condition = condition;
        Actions = actions;
    }

    public override Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        return v.Visit(this, globalScope, LocalScope);
    }
}