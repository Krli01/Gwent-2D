using System.Collections.Generic;

public interface IVisitable
{
    Result Accept(IVisitor v, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
}
