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