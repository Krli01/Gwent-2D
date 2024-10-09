using System.Collections.Generic;

public interface IVisitor 
{
    Result Visit(AST node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_Action node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_ArithmeticExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_BinaryAssignStatement node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_Boolean node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_BooleanExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_CardBody node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_CardDefinition node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_ComparisonExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_EffectActivation node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_EffectBody node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_EffectDefinition node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_EffectInvocation node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_Factor node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_ForLoop node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_FunctionCall node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_Identifier node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_Number node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_ParamAssignment node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_ParamDeclaration node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_PostAction node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_Predicate node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_Program node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_ProgramVoid node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_PropertyAccess node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_Selector node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_Statement node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_StatementBlock node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_String node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_Term node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_UnaryAssignStatement node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_UnaryAssignFactor node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
    Result Visit(AST_WhileLoop node, GlobalScope globalScope, Dictionary<string, Result> LocalScope);
}
