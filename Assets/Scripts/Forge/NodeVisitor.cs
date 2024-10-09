using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class NodeVisitor : IVisitor
{
    public Result Visit(AST node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        ////UnityEngine.Debug.Log("visit ast type : " + node.GetType());
        //UnityEngine.Debug.Log("visit ast");
        //UnityEngine.Debug.Log(node.GetType());
//        AST_Program a = (AST_Program) node;
        return node.Accept(this, globalScope, LocalScope);
    }

    public Result Visit(AST_Action node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("action");
        if (node.ParamNames.Count != 2) throw new Exception ($"Error : No definition of 'Action' takes {node.ParamNames.Count} arguments");
        if (node.ParamNames[0].ID == node.ParamNames[1].ID) throw new Exception($"Error : Both parameters of 'Action' cannot have the same name");

        var newLocalScope = new Dictionary<string, Result>(LocalScope)
        {
            { node.ParamNames[0].ID, new ExpressionResult("list") },
            { node.ParamNames[1].ID, new ExpressionResult("context") }
        };

        node.Actions.Accept(this, globalScope, newLocalScope);
        
        return new ActionResult(node);
    }

    public Result Visit(AST_ArithmeticExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("arithmetic exp");

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

    public Result Visit(AST_BinaryAssignStatement node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("assign stmt");
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
        //UnityEngine.Debug.Log("sale de assign");
        return new Result();
    }

    public Result Visit(AST_Boolean node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("bool");
        return new ExpressionResult("boolean");
    }

    public Result Visit(AST_BooleanExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("boolean exp");

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
        //UnityEngine.Debug.Log("card body");
        if (!CardDatabase.AvailableDecks.ContainsKey(node.Faction)) throw new Exception($"Error: There is no available faction named '{node.Faction}'");

        List<AST_EffectInvocation> effects = new List<AST_EffectInvocation>();
        for (int i = 0; i < node.Effect.Count; i++)
        {
            Result eff = node.Effect[i].Accept(this, globalScope, LocalScope);
            OnActivationResult effectResult = (OnActivationResult) eff;
            AST_EffectInvocation cardEffect = effectResult.value;
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
        //UnityEngine.Debug.Log("card def");
        return node.Body.Accept(this, globalScope, LocalScope);
    }

    public Result Visit(AST_ComparisonExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("comparison exp");

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
        //UnityEngine.Debug.Log("effect activation");
        return node.Effect.Accept(this, globalScope, LocalScope);
    }

    public Result Visit(AST_EffectBody node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("effect body");
        if (globalScope.Effects.ContainsKey(node.Name)) throw new Exception ($"Error : Effect '{node.Name}' already exists");

        Dictionary<string, Result> newLocalScope = new Dictionary<string, Result> (LocalScope);
        
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        foreach (AST_ParamDeclaration param in node.Params)
        {
            ExpressionResult res = (ExpressionResult) param.Accept(this, globalScope, LocalScope);
            if (parameters.ContainsKey(param.Name)) throw new Exception($"Effect '{node.Name}' already contains a argument named '{param.Name}'");
            parameters.Add(param.Name, param.type);
            newLocalScope.Add(param.Name, res);
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
        //UnityEngine.Debug.Log("effect def");
        return node.Body.Accept(this, globalScope, LocalScope);
    }

    public Result Visit(AST_EffectInvocation node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("effect invocation");
        if (!globalScope.Effects.ContainsKey(node.Name)) throw new Exception ($"Error : Effect '{node.Name}' does not exist in current context");

        Effect effect = globalScope.Effects[node.Name];
        
        List<string> paramNames = new List<string>();
        if (node.Params.Count != effect.Params.Count) throw new Exception($"Error : Effect '{effect.Name}' must receive {effect.Params.Count} parameters");
        
        Dictionary<string, Result> newLocalScope = new Dictionary<string, Result> (LocalScope);
        
        foreach (var param in node.Params)
        {
            ExpressionResult par = (ExpressionResult) param.Accept(this, globalScope, LocalScope);
            if (!effect.Params.ContainsKey(param.Name)) throw new Exception ($"Error : Effect '{effect.Name}' does not contain a parameter named '{param.Name}'");
            if (paramNames.Contains(param.Name)) throw new Exception($"Effect '{effect.Name}' already contains a argument named '{param.Name}'");
            if (par.type != effect.Params[param.Name]) throw new Exception ($"Error : Invalid assignment of '{param.Value}' value to '{effect.Params[param.Name]}' parameter '{param.Name}'");
            paramNames.Add(param.Name);
            newLocalScope.Add(param.Name, par);
        }
        
        if (node.Selector != null)
        {
            if (node.Selector.Source == "parent") throw new Exception ("Error : Source 'parent' can only be used within a PostAction");
            node.Selector.Accept(this, globalScope, newLocalScope);
        }

        node.PostAction?.Accept(this, globalScope, newLocalScope);

        return new OnActivationResult(node);
    }

    public Result Visit(AST_Factor node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {   
        //UnityEngine.Debug.Log("factor");
        ////UnityEngine.Debug.Log(node.Expression == null);
        ////UnityEngine.Debug.Log($"Factor type {node.Expression.GetType()}");
        return node.Expression.Accept(this, globalScope, LocalScope);
    }

    public Result Visit(AST_ForLoop node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("for loop");
        if (LocalScope.ContainsKey(node.Element.ID)) throw new Exception ($"Error : A collection named '{node.Element.ID}' cannot be declared in this scope because that name is used in an enclosing local scope");

        var newLocalScope = new Dictionary<string, Result>(LocalScope)
        {
            { node.Element.ID, new ExpressionResult("card") }
        };

        if (node.Identifier != null)
        {
            if (!newLocalScope.ContainsKey(node.Identifier.ID)) throw new Exception ($"Error : Identifier '{node.Identifier.ID}' does not exist in current context");
            ExpressionResult r = (ExpressionResult) newLocalScope[node.Identifier.ID];
            if (r.type != "list") throw new Exception ($"Error : Identifier '{node.Identifier.ID}' cannot be declared in 'for' loop because it is not a valid collection");
        }

        else if (node.Collection != null)
        {
            ExpressionResult res = (ExpressionResult) node.Collection.Accept(this, globalScope, newLocalScope);
            if(res.type != "list") throw new Exception ($"Error : Collection '{node.Identifier.ID}' cannot be declared in 'for' loop because it is not a valid collection");
        }

        else
        {
            ExpressionResult res = (ExpressionResult) node.Function.Accept(this, globalScope, LocalScope);
            if(res.type != "list") throw new Exception ($"Error : Collection '{node.Identifier.ID}' cannot be declared in 'for' loop because it is not a valid collection");
        }
        
        node.Statements.Accept(this, globalScope, newLocalScope);

        return new Result();
    }

    public Result Visit(AST_FunctionCall node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("function call");
        string func = node.Prop.Prop.Last().ID;

        List<AST_Identifier> aux = new List<AST_Identifier>(node.Prop.Prop);
        aux.RemoveAt(aux.Count-1);
        AST_PropertyAccess source = new AST_PropertyAccess(aux);

        ExpressionResult pr = (ExpressionResult) source.Accept(this, globalScope, LocalScope);

        if (pr.type == "context")
        {
            if (!globalScope.ContextFunctions.ContainsKey(func)) throw new Exception($"Error: Invalid function {func}");
            ExpressionResult param = (ExpressionResult) node.Param.Accept(this, globalScope, LocalScope);
            if (param.type != "player") throw new Exception($"Error: Function '{func}' does not receive a parameter of type '{param.type}'");
            return new ExpressionResult(globalScope.ContextFunctions[func]);
        }
        
        else if (pr.type == "list" || pr.type == "stack")
        {
            if (!globalScope.CollectionFunctions.ContainsKey(func)) throw new Exception($"Error: Invalid function {func}");
            if (pr.type == "list")
            {
                if(func == "Add" || func == "Remove" ||  func == "Count")
                {
                    if (node.Param == null && globalScope.CollectionFunctions[func].Item1 != 0)
                        throw new Exception($"Error: Function '{func}' must receive 1 parameter of type {globalScope.CollectionFunctions[func].Item2}");
                    if (node.Param != null && globalScope.CollectionFunctions[func].Item1 == 0)
                        throw new Exception($"Error: Function '{func}' must receive 0 parameters");
                }
                else if (func == "Find")
                {
                    if (node.Predicate == null)
                        throw new Exception($"Error: Function '{func}' must receive 1 parameter of type 'predicate'");
                }
                else throw new Exception($"Error: List '{node.Prop}' does not contain a property or method named '{func}'");
            }
            else
            {
                if(func == "Pop" || func == "Shuffle" ||  func == "Count")
                {
                    if (node.Param != null)
                        throw new Exception($"Error: Function '{func}' must receive 0 parameters");
                }
                else if (func == "Push" || func == "SendBottom")
                {
                    if (node.Param == null)
                        throw new Exception($"Error: Function '{func}' must receive 1 parameter of type {globalScope.CollectionFunctions[func].Item2}");
                }
                else throw new Exception($"Error: Stack '{node.Prop}' does not contain a property or method named '{func}'");
            }
            return new ExpressionResult(globalScope.CollectionFunctions[func].Item3);
        }
        
        else
        {
            throw new Exception($"Error: Invalid function '{func}'");
        }
    }

    public Result Visit(AST_Identifier node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("identifier");
        //UnityEngine.Debug.Log(node.ID + ": " + LocalScope[node.ID]);

        if (LocalScope.ContainsKey(node.ID)) return LocalScope[node.ID];
        return new ExpressionResult("identifier");
    }

    public Result Visit(AST_Number node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("number");
        return new ExpressionResult("number");
    }

    public Result Visit(AST_ParamAssignment node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("param assign");
        ExpressionResult er = (ExpressionResult) node.Value.Accept(this, globalScope, LocalScope);
        return er;
    }

    public Result Visit(AST_ParamDeclaration node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("param dec");
        return new ExpressionResult(node.type);
    }

    public Result Visit(AST_PostAction node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("post acction");
        
        ////UnityEngine.Debug.Log(node.type);
        if (!globalScope.Effects.ContainsKey(node.type)) throw new Exception ($"Error : Effect '{node.type}' does not exist in current context");

        Effect effect = globalScope.Effects[node.type];
        
        List<string> paramNames = new List<string>();
        if (node.Params.Count != effect.Params.Count) throw new Exception($"Error : Effect '{effect.Name}' must receive {effect.Params.Count} parameters");
        foreach (var param in node.Params)
        {
            ParamAssignResult par = (ParamAssignResult) param.Accept(this, globalScope, LocalScope);
            if (!effect.Params.ContainsKey(par.Name)) throw new Exception ($"Error : Effect '{effect.Name}' does not contain a parameter named '{par.Name}'");
            if (paramNames.Contains(par.Name)) throw new Exception($"Effect '{effect.Name}' already contains a argument named '{par.Name}'");
            if (par.Value != effect.Params[par.Name]) throw new Exception ($"Error : Invalid assignment of '{par.Value}' value to '{effect.Params[par.Name]}' parameter '{par.Name}'");
            paramNames.Add(par.Name);
        }

        node.Selector?.Accept(this, globalScope, LocalScope);
        node.PostAction?.Accept(this, globalScope, LocalScope);
        return new Result();
    }

    public Result Visit(AST_Predicate node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("predicate");
        
        var newLocalScope = new Dictionary<string, Result> (LocalScope)
        {
            {node.ID.ID, new ExpressionResult("card")}
        };
        ExpressionResult er = (ExpressionResult) node.Condition.Accept(this, globalScope, newLocalScope);
        return new Result();
    }

    public Result Visit(AST_Program node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("program node");
        List<Card> c = new List<Card>();
        List<Effect> e = new List<Effect>();

        foreach (var item in node.Items)
        {
            ////UnityEngine.Debug.Log(item.Type());
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
                ////UnityEngine.Debug.Log("entro");
                Result r = item.Accept(this, globalScope, LocalScope);
                CardResult cr = (CardResult) r;
                c.Add(cr.value);
            }
        }

        /*foreach (var item in c)
        {
        //UnityEngine.Debug.Log( "a" + item.CardName);
        }*/
        return new ProgramResult(c, e);
    }

    public Result Visit(AST_ProgramVoid node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("visit void");
        return new Result();
    }

    public Result Visit(AST_PropertyAccess node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("prop access");
        
        string first = node.Prop[0].ID;
        if (!LocalScope.ContainsKey(first)) throw new Exception ($"Error : Identifier '{first}' does not exist in current context");
        
        if(node.Prop.Count > 1)
        {
            ExpressionResult er = (ExpressionResult) LocalScope[first];
            string second = node.Prop[1].ID;

            if (er.type == "card")
            {
                if(!globalScope.CardProperties.ContainsKey(second)) throw new Exception ($"Error : Invalid card property '{second}'");
                if(node.Prop.Count > 2) throw new Exception($"Error: '{first}.{second}' does not contain any property or method named '{node.Prop[2].ID}'");
                return new ExpressionResult(globalScope.CardProperties[second]);
            }
            else if (er.type == "context")
            {
                if(!globalScope.ContextProperties.ContainsKey(second)) throw new Exception ($"Error : Invalid card property '{second}'");
                if(node.Prop.Count > 2) throw new Exception($"Error: '{first}.{second}' does not contain any property or method named '{node.Prop[2].ID}'");
                return new ExpressionResult(globalScope.ContextProperties[second]);
            }
            else
            {
                throw new Exception($"Error: '{first}' does not contain any property or method of type '{er.type}'");
            }
        }

        return LocalScope[first];
    }

    public Result Visit(AST_Selector node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("selector");
        
        if (!globalScope.SelectorSources.Contains(node.Source)) throw new Exception ($"Error : Invalid source '{node.Source}'");

        if (node.Predicate != null)
        {
            if (node.Source == "deck" || node.Source == "otherDeck") throw new Exception ($"Error : A filter predicate cannot be applied to source '{node.Source}'");
            node.Predicate.Accept(this, globalScope, LocalScope);
        }
        else if (node.Source != "deck" && node.Source != "otherDeck") throw new Exception ($"Error : Selector of source '{node.Source}' must declare a predicate");

        ExpressionResult Single = (ExpressionResult) node.Single.Accept(this, globalScope, LocalScope);
        if (Single.type != "boolean") throw new Exception ("Selector property 'Single' must have a 'boolean' type value");

        return new Result();
    }

    public Result Visit(AST_Statement node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("stmt");
        
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
        else if (node is AST_BinaryAssignStatement)
        {
            node.Accept(this, globalScope, LocalScope);
            return new StatementResult(node);
        }
        else if (node is AST_UnaryAssignStatement)
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
        //UnityEngine.Debug.Log("stmt block");
        
        foreach (var item in node.Statements)
        {
            item.Accept(this, globalScope, LocalScope);
        }
        return new Result();
    }

    public Result Visit(AST_String node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("string");
        return new ExpressionResult("string");
    }

    public Result Visit(AST_Term node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("term");
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

    public Result Visit(AST_UnaryAssignFactor node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("unary assign factor");
        if (node.ID != null)
            {
                if (!LocalScope.ContainsKey(node.ID.ID)) throw new Exception ($"Error : Variable '{node.ID.ID}' does not exist in current context");

                ExpressionResult r = (ExpressionResult) LocalScope[node.ID.ID];
                
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
                ExpressionResult r = (ExpressionResult) node.Prop.Accept(this, globalScope, LocalScope);

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

    public Result Visit(AST_UnaryAssignStatement node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("unary assign stmt");
            if (node.ID != null)
            {
                if (!LocalScope.ContainsKey(node.ID.ID)) throw new Exception ($"Error : Variable '{node.ID.ID}' does not exist in current context");

                ExpressionResult r = (ExpressionResult) LocalScope[node.ID.ID];
                
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
                ExpressionResult r = (ExpressionResult) node.Prop.Accept(this, globalScope, LocalScope);

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

    public Result Visit(AST_WhileLoop node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        //UnityEngine.Debug.Log("while loop");
        node.Condition.Accept(this, globalScope, LocalScope);
        node.Actions.Accept(this, globalScope, LocalScope);
        return new Result();
    }
}