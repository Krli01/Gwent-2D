using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Interpreter
{
    public string Error = "";
    IVisitor visitor = new NodeVisitor();

    public Result Run(AST ast, Context context)  // kerraios hace el context
    {
        UnityEngine.Debug.Log(ast is AST_Program);
        
        Error = "";

        GlobalScope globalScope = new GlobalScope(context.AvailableEffects);

        Dictionary<string, Result> LocalScope = new Dictionary<string, Result>();

        try
        {
            Result result = ast.Accept(visitor, globalScope, LocalScope);      
            return result;
        }

        catch (Exception e)
        {
            Error = e.Message;
            return null;
        }
    }

}

public class GlobalScope
{
    public Dictionary<string, (int, string)> TargetFunctions {get; private set;}
    public Dictionary<string, int> ContextFunctions {get; private set;}
    public Dictionary<string, Effect> Effects {get; private set;}
    public Dictionary<string, string> ContextProperties {get; private set;}
    public Dictionary<string, string> CardProperties {get; private set;}
    public List<string> SelectorSources {get; private set;}

    public GlobalScope(Dictionary<string, Effect> effects)
    {
        Effects = new Dictionary<string, Effect>(effects);
        
        TargetFunctions = new Dictionary<string, (int, string)>
        {
            {"Find", (1, "predicate")},
            {"Push", (1, "card")},
            {"SendBottom", (1, "card")},
            {"Remove", (1, "card")},
            {"Add", (1, "card")},
            {"Pop", (0, "")},
            {"Shuffle", (0, "")},
        };

        ContextFunctions = new Dictionary<string, int>
        {
            {"HandOfPlayer", 1},
            {"FieldOfPlayer", 1},
            {"GraveyardOfPlayer", 1},
            {"DeckOfPlayer", 1},
        };

        ContextProperties = new Dictionary<string, string>
        {
            {"TriggerPlayer", "final"},
            {"Board", ""},
            {"Hand", ""},
            {"Deck", ""},
            {"Graveyard", ""},
            {"Field", ""}
        };

        CardProperties = new Dictionary<string, string>
        {
            {"Owner", "player"},
            {"Power", "number"},
            {"Name", "string"},
            {"Faction", "string"},
            {"Range", "list"},
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