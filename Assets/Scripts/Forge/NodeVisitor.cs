using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class NodeVisitor : IVisitor
{
    public Result Visit(AST node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("visit ast type : " + node.GetType());
        UnityEngine.Debug.Log("visit ast");
//        AST_Program a = (AST_Program) node;
        return node.Accept(this, globalScope, LocalScope);
    }

    public Result Visit(AST_Action node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("action");
        if (node.ParamNames.Count != 2) throw new Exception ($"Error : No definition of 'Action' takes {node.ParamNames.Count} arguments");
        if (node.ParamNames[0].ID == node.ParamNames[1].ID) throw new Exception($"Error : Both parameters of 'Action' cannot have the same name");

        var newLocalScope = new Dictionary<string, Result>(LocalScope)
        {
            { node.ParamNames[0].ID, new ExpressionResult("targets") },
            { node.ParamNames[1].ID, new ExpressionResult("context") }
        };

        node.Actions.Accept(this, globalScope, newLocalScope);
        
        return new ActionResult(node);
    }

    public Result Visit(AST_ArithmeticExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("arithmetic exp");

        AST l = node.Left.Type();
        if (l is AST_Identifier)
        {
            AST_Identifier l2 = (AST_Identifier) l;
            if(!LocalScope.ContainsKey(l2.ID)) throw new Exception ($"Error : Variable {l2.ID} does not exist in current context");
        }

        AST r = node.Left.Type();
        if (r is AST_Identifier)
        {
            AST_Identifier r2 = (AST_Identifier) l;
            if(!LocalScope.ContainsKey(r2.ID)) throw new Exception ($"Error : Variable {r2.ID} does not exist in current context");
        }

        ExpressionResult left = (ExpressionResult) node.Left.Accept(this, globalScope, LocalScope);
        if (node.Op != null)
        {
            ExpressionResult right = (ExpressionResult) node.Right.Accept(this, globalScope, LocalScope);

            if (!(left.type == "number" && right.type == "number")) throw new Exception($"Error : Operator {node.Op} can only be applied between number types");
        }
        return left;
    }

    public Result Visit(AST_AssignStatement node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("assign stmt");
        switch (node.Op)
        {
            case "=":
                if(node.ID != null)
                {
                    ExpressionResult er;
                    if (node.Exp != null)
                    {
                        er = (ExpressionResult) node.Exp.Accept(this, globalScope, LocalScope);
                    }
                    else
                    {
                        er = (ExpressionResult) node.Func.Accept(this, globalScope, LocalScope);
                    }

                    if (LocalScope.ContainsKey(node.ID.ID))
                    {
                        LocalScope[node.ID.ID] = er;
                    }
                    else LocalScope.Add(node.ID.ID, er);
                }
                else // assign to 'PropertyAccess'
                {
                    ExpressionResult r = (ExpressionResult) node.Prop.Accept(this, globalScope, LocalScope);
                    if (r.type != "number") throw new Exception ($"Error : Unable to assign value to property {node.Prop.Prop}");
                    
                    if (node.Exp != null)
                    {
                        ExpressionResult e = (ExpressionResult) node.Exp.Accept(this, globalScope, LocalScope);
                        if (e.type != "number") throw new Exception ($"Error : Property '{node.Prop.Prop}' must have a numerical value");
                    }
                    else
                    {
                        throw new Exception ($"Error :  Unable to assign value '{node.Func.Prop.Prop}' to property {node.Prop.Prop}");
                    }
                }
                break;

            case "+=":
            case "-=":
                if(node.ID != null)
                {
                    if (!LocalScope.ContainsKey(node.ID.ID)) throw new Exception ($"Error : Idedntifier '{node.ID.ID}' does not exist in current context");

                    ExpressionResult type = (ExpressionResult) LocalScope[node.ID.ID];
                    if (type.type != "number") throw new Exception ($"Error : Operator '{node.Op}' cannot be applied on non-numeric types");

                    ExpressionResult r = (ExpressionResult) node.Exp.Accept(this, globalScope, LocalScope);
                    if (r.type != "number") throw new Exception ($"Error : Operator '{node.Op}' can only be applied between numeric types");
                }
                else
                {
                    ExpressionResult r = (ExpressionResult) node.Prop.Accept(this, globalScope, LocalScope);
                    if (r.type != "number") throw new Exception ($"Error : Unable to assign value to property {node.Prop.Prop}");

                    if (node.Exp != null)
                    {
                        ExpressionResult e = (ExpressionResult) node.Exp.Accept(this, globalScope, LocalScope);
                        if (e.type != "number") throw new Exception ($"Error : Operator '{node.Op}' can only be applied numeric types");
                    }
                    if (node.Func != null)
                    {
                        throw new Exception ($"Error :  Unable to assign value '{node.Func.Prop.Prop}' to property {node.Prop.Prop}");
                    }
                }
                break;
        }

        return new Result();
    }

    public Result Visit(AST_Boolean node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("bool");
        return new ExpressionResult("boolean");
    }

    public Result Visit(AST_BooleanExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("boolean exp");

        AST l = node.Left.Type();
        if (l is AST_Identifier)
        {
            AST_Identifier l2 = (AST_Identifier) l;
            if(!LocalScope.ContainsKey(l2.ID)) throw new Exception ($"Error : Variable {l2.ID} does not exist in current context");
        }

        AST r = node.Left.Type();
        if (r is AST_Identifier)
        {
            AST_Identifier r2 = (AST_Identifier) l;
            if(!LocalScope.ContainsKey(r2.ID)) throw new Exception ($"Error : Variable {r2.ID} does not exist in current context");
        }

        ExpressionResult left = (ExpressionResult) node.Left.Accept(this, globalScope, LocalScope);
        if (node.Op != null)
        {
            ExpressionResult right = (ExpressionResult) node.Right.Accept(this, globalScope, LocalScope);

            if (!(left.type == "boolean" && right.type == "boolean")) throw new Exception($"Error : Operator {node.Op} can only be applied between objects of type Boolean");

            return new ExpressionResult("boolean");
        }
        return left;
    }

    public Result Visit(AST_CardBody node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("card body");
        List<Effect> effects = new List<Effect>();
        for (int i = 0; i < node.Effect.Count; i++)
        {
            Result eff = node.Effect[i].Accept(this, globalScope, LocalScope);
            EffectResult effectResult = (EffectResult) eff;
            Effect cardEffect = effectResult.value;
            effects.Add(cardEffect);
        }

        switch (node.type)
        {
            case "Lider":
                if (node.Power != 0) throw new Exception ("Error: Leader card declaration must not contain the property 'Power'");
                if (node.Range.Count > 0) throw new Exception ("Error: Leader card declaration must not contain the property 'Range'");
                Card newCard = new LeaderCard(CardDatabase.Instance.id++, node.Name, "This is a leader card", effects, node.Faction, "img 1");
                return new CardResult(newCard);

            case "Oro" or "Plata":
                if(node.Range.Count == 0) throw new Exception ("Error : Unit cards must specify a role");
                Role role = new();
                if (node.Range.Count == 1)
                {
                    if (node.Range[0] == "Melee") role = Role.Mele;
                    else if (node.Range[0] == "Ranged") role = Role.Range;
                    else if (node.Range[0] == "Siege") role = Role.Siege;
                    else throw new Exception ($"Error : Invalid range '{node.Range[0]}'");
                }
                if (node.Range.Count == 2)
                {
                    if (node.Range.Contains("Siege") || !(node.Range.Contains("Melee") && node.Range.Contains("Ranged"))) throw new Exception ("Error : Invalid Range. Multiple range cards only admit the combination 'Melee'+'Ranged'");
                    role = Role.Agile;
                }
                if (node.Range.Count > 2) throw new Exception ("Error : Cards cannot play 3 Ranges");
                if (node.type == "Oro") 
                {
                    Card newCard1 = new Unit(CardDatabase.Instance.id++, node.Power, node.Name, "This is a unit card", effects, node.Faction, "img 1", true, role);
                    return new CardResult(newCard1);
                }
                Card newCard2 = new Unit(CardDatabase.Instance.id++, node.Power, node.Name, "This is a unit card", effects, node.Faction, "img 1", false, role);
                return new CardResult(newCard2);

            case "Clima":
                if (node.Power != 0) throw new Exception ("Error: Weather card declaration must not contain the property 'Power'");
                if (node.Range.Count > 0) throw new Exception ("Error: Weather card declaration must not contain the property 'Range'");
                Card newCard3 = new Weather(CardDatabase.Instance.id++, node.Name, "This is a weather card", effects, node.Faction, "img 1");
                return new CardResult(newCard3);

            case "Despeje":
                if (node.Power != 0) throw new Exception ("Error: Clearing card declaration must not contain the property 'Power'");
                if (node.Range.Count > 0) throw new Exception ("Error: Clearing card declaration must not contain the property 'Range'");
                Card newCard4 = new Clearing(CardDatabase.Instance.id++, node.Name, "This is a clearing card", effects, node.Faction, "img 1");
                return new CardResult(newCard4);

            case "Aumento":
                if (node.Power != 0) throw new Exception ("Error: Booster card declaration must not contain the property 'Power'");
                if (node.Range.Count > 0) throw new Exception ("Error: Booster card declaration must not contain the property 'Range'");
                Card newCard5 = new Booster(CardDatabase.Instance.id++, node.Name, "This is a booster card", effects, node.Faction, "img 1");
                return new CardResult(newCard5);

            case "Senuelo":
                if (node.Power != 0) throw new Exception ("Error: Decoy card declaration must not contain the property 'Power'");
                if (node.Range.Count > 0) throw new Exception ("Error: Decoy card declaration must not contain the property 'Range'");
                Card newCard6 = new Decoy(CardDatabase.Instance.id++, node.Name, "This is a decoy card", effects, node.Faction, "img 1");
                return new CardResult(newCard6);
        }

        throw new Exception($"Error creating Card : There is no definition for type '{node.type}'");
    }

    public Result Visit(AST_CardDefinition node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("card def");
        return node.Body.Accept(this, globalScope, LocalScope);
    }

    public Result Visit(AST_ComparisonExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("comparison exp");

        AST l = node.Left.Type();
        if (l is AST_Identifier)
        {
            AST_Identifier l2 = (AST_Identifier) l;
            if(!LocalScope.ContainsKey(l2.ID)) throw new Exception ($"Error : Variable {l2.ID} does not exist in current context");
        }

        AST r = node.Left.Type();
        if (r is AST_Identifier)
        {
            AST_Identifier r2 = (AST_Identifier) l;
            if(!LocalScope.ContainsKey(r2.ID)) throw new Exception ($"Error : Variable {r2.ID} does not exist in current context");
        }

        ExpressionResult left = (ExpressionResult) node.Left.Accept(this, globalScope, LocalScope);
        if (node.Op != null)
        {
            ExpressionResult right = (ExpressionResult) node.Right.Accept(this, globalScope, LocalScope);

            if (left.type == "boolean" || right.type == "boolean") throw new Exception($"Error : Operator {node.Op} cannot be applied between objects of type Boolean");

            if ((left.type == "number" && right.type == "string") || (left.type == "string" && right.type == "number")) throw new Exception($"Error : Operator {node.Op} can only be applied between objects of the same type");

            return new ExpressionResult("boolean");
        }
        return left;
    }

    public Result Visit(AST_EffectActivation node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("effect activation");
        return node.Effect.Accept(this, globalScope, LocalScope);
    }

    public Result Visit(AST_EffectBody node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("effect body");
        if (globalScope.Effects.ContainsKey(node.Name)) throw new Exception ($"Error : Effect '{node.Name}' already exists");

        Dictionary<string, Result> newLocalScope = new Dictionary<string, Result> (LocalScope);
        
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        foreach (AST_ParamDeclaration param in node.Params)
        {
            Result res = param.Accept(this, globalScope, LocalScope);
            ParamDecResult paramDec = (ParamDecResult) res;
            if (parameters.ContainsKey(paramDec.name)) throw new Exception($"Effect '{node.Name}' already contains a argument named '{paramDec.name}'");
            parameters.Add(paramDec.name, paramDec.type);
            newLocalScope.Add(paramDec.name, paramDec);
        }

        Result r = node.Action.Accept(this, globalScope, newLocalScope);
        ActionResult a = (ActionResult) r;
        AST_Action actions = a.value;

        Effect eff = new Effect (node.Name, parameters, actions);
        globalScope.Effects.Add(node.Name, eff);

        return new EffectResult(eff);
    }

    public Result Visit(AST_EffectDefinition node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("effect def");
        return node.Body.Accept(this, globalScope, LocalScope);
    }

    public Result Visit(AST_EffectInvocation node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("effect invocation");
        if (!globalScope.Effects.ContainsKey(node.Name)) throw new Exception ($"Error : Effect '{node.Name}' does not exist in current context");

        UnityEngine.Debug.Log("a");
        Effect effect = globalScope.Effects[node.Name];
        
        UnityEngine.Debug.Log("b");
        List<string> paramNames = new List<string>();
        if (node.Params.Count != effect.Params.Count) throw new Exception($"Error : Effect '{effect.Name}' must receive {effect.Params.Count} parameters");
        
        Dictionary<string, Result> newLocalScope = new Dictionary<string, Result> (LocalScope);
        
        UnityEngine.Debug.Log("c");
        foreach (var param in node.Params)
        {
            UnityEngine.Debug.Log("d");
            ParamAssignResult par = (ParamAssignResult) param.Accept(this, globalScope, LocalScope);
            if (!effect.Params.ContainsKey(par.Name)) throw new Exception ($"Error : Effect '{effect.Name}' does not contain a parameter named '{par.Name}'");
            if (paramNames.Contains(par.Name)) throw new Exception($"Effect '{effect.Name}' already contains a argument named '{par.Name}'");
            if (par.Value != effect.Params[par.Name]) throw new Exception ($"Error : Invalid assignment of '{par.Value}' value to '{effect.Params[par.Name]}' parameter '{par.Name}'");
            paramNames.Add(par.Name);
            newLocalScope.Add(par.Name, par);
        }
        
        UnityEngine.Debug.Log("e");
        if (node.Selector != null)
        {
            if (node.Selector.Source == "parent") throw new Exception ("Error : Source 'parent' can only be used within a PostAction");
            node.Selector.Accept(this, globalScope, newLocalScope);
        }

        UnityEngine.Debug.Log("f");
        node.PostAction?.Accept(this, globalScope, newLocalScope);
        UnityEngine.Debug.Log("gs");

        return new EffectResult(effect);
    }

    public Result Visit(AST_Factor node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {   
        UnityEngine.Debug.Log("factor");
        UnityEngine.Debug.Log($"Factor type {node.Expression.GetType()}");
        if (node.Op == null)
        {
            return node.Expression.Accept(this, globalScope, LocalScope);
        }

        else
        {
            if (node.Expression.Type() is AST_Identifier)
            {
                AST_Identifier temp = (AST_Identifier) node.Expression.Type();
                if (!LocalScope.ContainsKey(temp.ID)) throw new Exception ($"Error : Variable '{temp.ID}' does not exist in current context");

                ExpressionResult r = (ExpressionResult) LocalScope[temp.ID];
                
                switch (node.Op)
                {
                    case "!":
                        if (r.type != "boolean") throw new Exception ($"Error : Operator '!' can only be applied to boolean expressions");
                        break;

                    case "++":
                    case "--":
                        if(r.type != "number") throw new Exception ($"Error : Operator '{node.Op}' can only be applied to arithmetic expressions");
                        break;
                }

                return r;
            }
            
            else 
            {
                ExpressionResult r = (ExpressionResult) node.Expression.Accept(this, globalScope, LocalScope);

                UnityEngine.Debug.Log($"{r.type} en while");
                switch (node.Op)
                {
                    case "!":
                        if (r.type != "boolean") throw new Exception ($"Error : Operator '!' can only be applied to boolean expressions");
                        break;

                    case "++":
                    case "--":
                        if(r.type != "number") throw new Exception ($"Error : Operator '{node.Op}' can only be applied to arithmetic expressions");
                        break;
                }

                return r;
            }
        }    
    }

    public Result Visit(AST_ForLoop node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("for loop");
        if (LocalScope.ContainsKey(node.Element.ID)) throw new Exception ($"Error : A target named '{node.Element.ID}' cannot be declared in this scope because that name is used in an enclosing local scope");
        var newLocalScope = new Dictionary<string, Result>(LocalScope)
        {
            { node.Element.ID, new ExpressionResult("card") }
        };

        if (node.Identifier != null)
        {
            if (!newLocalScope.ContainsKey(node.Identifier.ID)) throw new Exception ($"Error : Identifier '{node.Identifier.ID}' does not exist in current context");
            ExpressionResult r = (ExpressionResult) newLocalScope[node.Identifier.ID];
            if (r.type != "targets") throw new Exception ($"Error : Identifier '{node.Identifier.ID}' cannot be declared in 'for' loop because it is not a valid collection");
        }

        node.Collection?.Accept(this, globalScope, newLocalScope);

        node.Statements.Accept(this, globalScope, newLocalScope);

        return new Result();
    }

    public Result Visit(AST_FunctionCall node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("function call");
        ExpressionResult pr = (ExpressionResult)node.Prop.Accept(this, globalScope, LocalScope);
        if (pr.type != "target function" && pr.type != "context function") throw new Exception ($"Error : Invalid function '{node.Prop.Prop}'");

        if (pr.type == "target function")
        {
            string func = node.Prop.Prop.Last().ID;
            if (node.Exp != null)
            {
                
        UnityEngine.Debug.Log(node.Exp);
                if (globalScope.TargetFunctions[func].Item1 != 1) throw new Exception ($"Error : Method '{func}' must declare 1 argument");
                if (globalScope.TargetFunctions[func].Item2 != "card") throw new Exception ($"Error : Method '{func}' must receive a card as argument");
                
                return new ExpressionResult("card");
            }

            else if (node.Predicate != null) // case 'Find'
            {
        UnityEngine.Debug.Log("pred no null");
                if (globalScope.TargetFunctions[func].Item1 != 1) throw new Exception ($"Error : Method '{func}' must declare 1 argument");
                if (globalScope.TargetFunctions[func].Item2 != "predicate") throw new Exception ($"Error : Method '{func}' must receive a predicate as argument");
                node.Predicate.Accept(this, globalScope, LocalScope);

                return new ExpressionResult("collection");
            }

            else if (globalScope.TargetFunctions[func].Item1 != 0) throw new Exception ($"Error : Method '{func}' must declare 0 arguments");
            UnityEngine.Debug.Log("both null");
            return new ExpressionResult("void");
        }

        else // p.type is "context function"
        {
            string func = node.Prop.Prop.Last().ID;
            if (node.Predicate != null) throw new Exception($"Error : Function '{func}' cannot receive a 'predicate' as argument");
            
            if (node.Exp == null) throw new Exception($"Error : Function '{func}' must receive a 'card' argument");

            UnityEngine.Debug.Log("aqui?");
            node.Exp.Accept(this, globalScope, LocalScope);
            return new ExpressionResult("collection");
        }
    }

    public Result Visit(AST_Identifier node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("identifier");
        return new ExpressionResult("identifier");
    }

    public Result Visit(AST_Number node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("number");
        return new ExpressionResult("number");
    }

    public Result Visit(AST_ParamAssignment node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("param assign");
        ExpressionResult er = (ExpressionResult) node.Value.Accept(this, globalScope, LocalScope);
        return new ParamAssignResult(node.Name, er.type);
    }

    public Result Visit(AST_ParamDeclaration node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("param dec");
        return new ParamDecResult(node.Name, node.type);
    }

    public Result Visit(AST_PostAction node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("post acction");
        
        UnityEngine.Debug.Log(node.type);
        if (!globalScope.Effects.ContainsKey(node.type)) throw new Exception ($"Error : Effect '{node.type}' does not exist in current context");
        UnityEngine.Debug.Log("a");

        Effect effect = globalScope.Effects[node.type];
        UnityEngine.Debug.Log("b");
        
        List<string> paramNames = new List<string>();
        if (node.Params.Count != effect.Params.Count) throw new Exception($"Error : Effect '{effect.Name}' must receive {effect.Params.Count} parameters");
        foreach (var param in node.Params)
        {
        UnityEngine.Debug.Log("c");
            ParamAssignResult par = (ParamAssignResult) param.Accept(this, globalScope, LocalScope);
            if (!effect.Params.ContainsKey(par.Name)) throw new Exception ($"Error : Effect '{effect.Name}' does not contain a parameter named '{par.Name}'");
            if (paramNames.Contains(par.Name)) throw new Exception($"Effect '{effect.Name}' already contains a argument named '{par.Name}'");
            if (par.Value != effect.Params[par.Name]) throw new Exception ($"Error : Invalid assignment of '{par.Value}' value to '{effect.Params[par.Name]}' parameter '{par.Name}'");
            paramNames.Add(par.Name);
        }

        UnityEngine.Debug.Log("d");
        node.Selector?.Accept(this, globalScope, LocalScope);
        UnityEngine.Debug.Log("e");
        node.PostAction?.Accept(this, globalScope, LocalScope);
        return new Result();
    }

    public Result Visit(AST_Predicate node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("predicate");
        
        var newLocalScope = new Dictionary<string, Result> (LocalScope)
        {
            {node.ID.ID, new ExpressionResult("card")}
        };
        ExpressionResult er = (ExpressionResult) node.Condition.Accept(this, globalScope, newLocalScope);
        return new Result();
    }

    public Result Visit(AST_Program node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("program node");
        List<Card> c = new List<Card>();
        List<Effect> e = new List<Effect>();

        foreach (var item in node.Items)
        {
            UnityEngine.Debug.Log(item.Type());
            if (item.Type() is AST_EffectDefinition)
            {
                Result r = item.Accept(this, globalScope, LocalScope);
                EffectResult er = (EffectResult) r;
                e.Add(er.value);
            }
        }

        foreach (var item in node.Items)
        {
            if (item.Type() is AST_CardDefinition)
            {
                UnityEngine.Debug.Log("entro");
                Result r = item.Accept(this, globalScope, LocalScope);
                CardResult cr = (CardResult) r;
                c.Add(cr.value);
            }
        }

        foreach (var item in c)
        {
        UnityEngine.Debug.Log( "a" + item.CardName);
        } 
        return new ProgramResult(c, e);
    }

    public Result Visit(AST_ProgramVoid node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("visit void");
        return new Result();
    }

    public Result Visit(AST_PropertyAccess node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("prop access");
        
        string first = node.Prop[0].ID;
        if (!LocalScope.ContainsKey(first)) throw new Exception ($"Error : Identifier {first} does not exist in current context");
        
        ExpressionResult er = (ExpressionResult) LocalScope[first];
        
        if (er.type == "targets")
        {
            UnityEngine.Debug.Log("is targets");
            throw new Exception ($"Error : Invalid property {node.Prop[1].ID}");
        }
        else if (er.type == "context")
        {
            UnityEngine.Debug.Log("is context");
            string second = node.Prop[1].ID;
            if (!globalScope.ContextProperties.ContainsKey(second) && !globalScope.ContextFunctions.ContainsKey(second)) throw new Exception ($"Error : Invalid property {second}");
            if (globalScope.ContextProperties.ContainsKey(second))
            {
                if (globalScope.ContextProperties[second] == "final")
                {
                    if (node.Prop.Count > 2) throw new Exception ($"Error : Invalid property {node.Prop[2].ID}");
                    return new ExpressionResult("player");
                }

                string third = node.Prop[2].ID;
                if (!globalScope.TargetFunctions.ContainsKey(third)) throw new Exception ($"Error : Invalid function {third}");
                return new ExpressionResult("target function");
            }
            else
            {
                return new ExpressionResult("context function");
            }
        }
        else if (er.type == "card")
        {
            UnityEngine.Debug.Log("is card");
            string second = node.Prop[1].ID;
            if (!globalScope.CardProperties.ContainsKey(second)) throw new Exception ($"Error : Invalid property {second}");
            return new ExpressionResult(globalScope.CardProperties[second]);
        }

        else if (er.type == "collection")
        {
            UnityEngine.Debug.Log("is collection");
            string second = node.Prop[1].ID;
            if (!globalScope.TargetFunctions.ContainsKey(second)) throw new Exception ($"Error : Invalid property {second}");
            return new  ExpressionResult("target function");
        }

        else
        {
            ExpressionResult e = (ExpressionResult) LocalScope[first];
            throw new Exception ($"Error : Variable {first} does not have any '{e.type}' property");
        }
    }

    public Result Visit(AST_Selector node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("selector");
        
        if (!globalScope.SelectorSources.Contains(node.Source)) throw new Exception ($"Error : Invalid source '{node.Source}'");
        node.Predicate.Accept(this, globalScope, LocalScope);
        return new Result();
    }

    public Result Visit(AST_Statement node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("stmt");
        
        if (node is AST_ForLoop)
        {
            node.Accept(this, globalScope, LocalScope);
            return new StatementResult(node);
        }
        else if (node is AST_WhileLoop)
        {
            node.Accept(this, globalScope, LocalScope);
            return new StatementResult(node);
        }
        else if (node is AST_AssignStatement)
        {
            node.Accept(this, globalScope, LocalScope);
            return new StatementResult(node);
        }
        else //node is function call
        {
            node.Accept(this, globalScope, LocalScope);
            return new ExpressionResult("function call");
        }
    }

    public Result Visit(AST_StatementBlock node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("stmt block");
        
        foreach (var item in node.Statements)
        {
            item.Accept(this, globalScope, LocalScope);
        }
        return new Result();
    }

    public Result Visit(AST_String node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("string");
        return new ExpressionResult("string");
    }

    public Result Visit(AST_Term node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        AST l = node.Left.Type();
        if (l is AST_Identifier)
        {
            AST_Identifier l2 = (AST_Identifier) l;
            if(!LocalScope.ContainsKey(l2.ID)) throw new Exception ($"Error : Variable {l2.ID} does not exist in current context");
        }

        AST r = node.Left.Type();
        if (r is AST_Identifier)
        {
            AST_Identifier r2 = (AST_Identifier) l;
            if(!LocalScope.ContainsKey(r2.ID)) throw new Exception ($"Error : Variable {r2.ID} does not exist in current context");
        }
        
        ExpressionResult left = (ExpressionResult) node.Left.Accept(this, globalScope, LocalScope);
        if (node.Op != null)
        {
            ExpressionResult right = (ExpressionResult) node.Right.Accept(this, globalScope, LocalScope);

            switch (node.Op)
            {
                case "*":
                case "/":
                case "^":
                    if (!(left.type == "number" && right.type == "number")) throw new Exception($"Error : Operator {node.Op} can only be applied between number types");
                    break;

                case "@":
                case "@@":
                    if (!(left.type == "string" && right.type == "string")) throw new Exception($"Error : Operator {node.Op} can only be applied between string types");
                    break;
            }
        }
        return left;
    }

    public Result Visit(AST_WhileLoop node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        UnityEngine.Debug.Log("while loop");
        node.Condition.Accept(this, globalScope, LocalScope);
        node.Actions.Accept(this, globalScope, LocalScope);
        return new Result();
    }
}