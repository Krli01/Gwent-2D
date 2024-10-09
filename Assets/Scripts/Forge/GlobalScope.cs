using System;
using System.Collections.Generic;
using System.Diagnostics;

public class GlobalScope
{
    public Dictionary<string, (int, string, string)> CollectionFunctions {get; private set;}
    public Dictionary<string, string> ContextFunctions {get; private set;}
    public Dictionary<string, Effect> Effects {get; private set;}
    public Dictionary<string, string> ContextProperties {get; private set;}
    public Dictionary<string, string> CardProperties {get; private set;}
    public List<string> SelectorSources {get; private set;}
    public GameCard thisCard {get; set;}

    public GlobalScope(Dictionary<string, Effect> effects)
    {
        Effects = new Dictionary<string, Effect>(effects);
        
        CardProperties = new Dictionary<string, string>
        {
            {"Owner", "player"},
            {"Power", "number"},
            {"Name", "string"},
            {"Faction", "string"},
            {"Range", "list"},
        };
        
        CollectionFunctions = new Dictionary<string, (int, string, string)>
        {
            // item1: param amount
            // item2: param type
            // item3: return type

            {"Find", (1, "predicate", "list")},
            {"Add", (1, "card", "")},
            {"Remove", (1, "card", "")},
            {"Count", (0, "", "number")},
            {"Push", (1, "card", "")},
            {"SendBottom", (1, "card", "")},
            {"Pop", (0, "", "card")},
            {"Shuffle", (0, "", "")},
        };

        ContextFunctions = new Dictionary<string, string>
        {
            // value: return type
            {"HandOfPlayer", "list"},
            {"FieldOfPlayer", "list"},
            {"GraveyardOfPlayer", "stack"},
            {"DeckOfPlayer", "stack"},
        };

        ContextProperties = new Dictionary<string, string>
        {
            // value: return type
            {"TriggerPlayer", ""},
            {"Board", "list"},
            {"Hand", "list"},
            {"Deck", "stack"},
            {"Graveyard", "stack"},
            {"Field", "list"}
        };

        SelectorSources = new List<string>
        {
            "board",
            "hand",
            "otherHand",
            "deck",
            "otherDeck",
            "Field",
            "otherField",
            "parent",
        };
    }

}

// if (LocalScope.ContainsKey("context"))       
        // {
        //     IdentifierResult r = (IdentifierResult) LocalScope["context"]; 
        //     if (LocalScope["context"] == r) // first is context
        //     {
        //         string second = node.Prop[1].ID;
        //         if (globalScope.ContextProperties.ContainsKey(second))
        //         {
        //             if (second == "TriggerPlayer")
        //             {
        //                 return new PlayerResult(Context.Instance.TriggerPlayer);
        //             }

        //             if (node.Prop.Count > 2)
        //             {
        //                 return new FunctionAccessResult(node.Prop);
        //             }
                    
        //             switch (second)
        //             {
        //                 case "Hand":
        //                     return new ListResult(Context.Instance.Hand);
                    
        //                 case "Board":
        //                     return new ListResult(Context.Instance.Board);
                    
        //                 case "Field":
        //                     return new ListResult(Context.Instance.Field);
                    
        //                 case "Graveyard":
        //                     return new StackResult(Context.Instance.Graveyard);

        //                 case "Deck":
        //                     return new StackResult(Context.Instance.Deck);
        //             }
        //         }
        //         else
        //         {
        //             return new FunctionAccessResult(node.Prop);
        //         }
        //     }
        // }
        // else if (LocalScope[first] is GameCardResult)
        // {
        //     GameCardResult cr = (GameCardResult) LocalScope[first];
        //     string second = node.Prop[1].ID;
        //     switch (second)
        //     {
        //         case "Owner":
        //             return new PlayerResult(cr.value.Owner);

        //         case "Power":
        //             return new NumberResult(cr.value.Power);

        //         case "Faction":
        //             return new StringResult(cr.value.BaseCard.CardFaction);

        //         case "Name":
        //             return new StringResult(cr.value.BaseCard.CardName);

        //         case "Range":
        //             return new StringResult(cr.value.BaseCard.thisRole.ToString());
        //     }

        // }

        // else if (LocalScope[first] is ListResult || LocalScope[first] is StackResult)
        // {
        //     return new FunctionAccessResult(node.Prop);
        // }

    // FUNCTION CALL NODE.VISITOR

    // if (pr.type == "collection function")
    //     {
    //         string func = node.Prop.Prop.Last().ID;
    //         if (node.Exp != null)
    //         {
    //             if (globalScope.CollectionFunctions[func].Item1 != 1) throw new Exception ($"Error : Method '{func}' must declare 1 argument");
    //             if (globalScope.CollectionFunctions[func].Item2 != "card") throw new Exception ($"Error : Method '{func}' must receive a card as argument");
                
    //             return new ExpressionResult("card");
    //         }

    //         else if (node.Predicate != null) // case 'Find'
    //         {
    //             if (globalScope.CollectionFunctions[func].Item1 != 1) throw new Exception ($"Error : Method '{func}' must declare 1 argument");
    //             if (globalScope.CollectionFunctions[func].Item2 != "predicate") throw new Exception ($"Error : Method '{func}' must receive a predicate as argument");
    //             node.Predicate.Accept(this, globalScope, LocalScope);

    //             return new ExpressionResult("list");
    //         }

    //         else if (globalScope.CollectionFunctions[func].Item1 != 0) throw new Exception ($"Error : Method '{func}' must declare 0 arguments");
    //         return new ExpressionResult("void");
    //     }

    //     else // p.type is "context function"
    //     {
    //         string func = node.Prop.Prop.Last().ID;
    //         if (node.Predicate != null) throw new Exception($"Error : Function '{func}' cannot receive a 'predicate' as argument");
            
    //         if (node.Exp == null) throw new Exception($"Error : Function '{func}' must receive a 'card' argument");

    //         node.Exp.Accept(this, globalScope, LocalScope);
    //         return new ExpressionResult(globalScope.ContextFunctions[func]);
    //     } 

    //PROP ACCESS
        // if (er.type == "card")
        // {
        //     string second = node.Prop[1].ID;
        //     if(!globalScope.CardProperties.ContainsKey(second)) throw new Exception ($"Error : Invalid card property '{second}'");
        //     if(node.Prop.Count > 2) throw new Exception($"Error: '{first}.{second}' does not contain any property or method named '{node.Prop[2].ID}'");
        //     return new ExpressionResult(globalScope.CardProperties[second]);
        // }

        // else if (er.type == "list" || er.type == "stack")
        // {
        //     string second = node.Prop[1].ID;
        //     if(node.Prop.Count > 2) 
        //         throw new Exception($"Error: '{first}.{second}' does not contain any property or method named '{node.Prop[2].ID}'");

        //     return new ExpressionResult(er.type);
        // }

        // else if (er.type == "context")
        // {
        //     string second = node.Prop[1].ID;
        //     if (!globalScope.ContextFunctions.ContainsKey(second) && !globalScope.ContextProperties.ContainsKey(second)) 
        //         throw new Exception ($"Error : Context does not contain any property or method named '{second}'");

        //     if (globalScope.ContextProperties.ContainsKey(second))
        //     {
        //         if (globalScope.ContextProperties[second] == "") // case 'TriggerPlayer'
        //         {
        //             if (node.Prop.Count > 2) throw new Exception ($"Error : {first}.{second} does not contain any property or method named '{node.Prop[2].ID}'");
        //             return new ExpressionResult("player");
        //         }

        //         if (node.Prop.Count > 2)
        //         {
        //             string third = node.Prop[2].ID;
        //             if (node.Prop.Count > 3) 
        //                 throw new Exception ($"Error : {first}.{second}.{third} does not contain any property or method named '{node.Prop[2].ID}'");
        //             return new ExpressionResult ("collection function");
        //         }

        //         return new ExpressionResult (globalScope.ContextProperties[second]);
        //     }

        //     else //(globalScope.ContextFunctions.ContainsKey(second))
        //     {
        //         if (node.Prop.Count > 2)
        //         {
        //             throw new Exception($"Error: Invalid function '{node.Prop[2].ID}'");
        //         }

        //         return new ExpressionResult ("context function");
        //     }
        // }


    