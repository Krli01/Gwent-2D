using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;
using Unity.VisualScripting;

public class ExecutionVisitor : IVisitor
{
    public Result Visit(AST node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        throw new System.NotImplementedException();
    }

    public Result Visit(AST_Action node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("action");
        Dictionary<string, Result> newLocalScope = new Dictionary<string, Result>(LocalScope)
        {
            {node.ParamNames[0].ID, LocalScope["targets"]},
            {node.ParamNames[1].ID, new ContextResult()},
        };
        node.Actions.Accept(this, globalScope, newLocalScope);
        return new Result();
    }

    public Result Visit(AST_ArithmeticExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("arithmetic exp");
        Result aux = node.Left.Accept(this, globalScope, LocalScope);

        if (node.Op != null)
        {
            NumberResult Left = (NumberResult) aux;
            NumberResult Right = (NumberResult) node.Right.Accept(this, globalScope, LocalScope);

            float result;
            if (node.Op == "+")
            {
                result = Left.value + Right.value;
                return new NumberResult(result);
            }

            result = Left.value - Right.value;
            return new NumberResult(result);
        }

        return aux;
    }

    public Result Visit(AST_BinaryAssignStatement node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("assign");
        switch (node.Op)
        {
            case "=":
                if(node.ID != null)
                {
                    Result er;
                    if (node.Exp != null)
                    {
                        er = node.Exp.Accept(this, globalScope, LocalScope);
                    }
                    else
                    {
                        er = node.Func.Accept(this, globalScope, LocalScope);
                    }

                    if (LocalScope.ContainsKey(node.ID.ID))
                    {
                        LocalScope[node.ID.ID] = er;
                    }
                    else LocalScope.Add(node.ID.ID, er);
                }
                else // case 'card.Power'
                {
                    NumberResult value = (NumberResult) node.Exp.Accept(this, globalScope, LocalScope);
                    //globalScope.thisCard.SetPower(value.value);
                    GameCardResult c = (GameCardResult) LocalScope[node.Prop.Prop[0].ID];
                    c.value.SetPower(value.value);
                }
                break;

            case "+=":
                if(node.ID != null)
                {
                    NumberResult r = (NumberResult) node.Exp.Accept(this, globalScope, LocalScope);
                    LocalScope[node.ID.ID] = r;
                }
                else // case 'card.Power'
                {
                    NumberResult value =(NumberResult) node.Exp.Accept(this, globalScope, LocalScope);
                    //globalScope.thisCard.SetPower(globalScope.thisCard.Power + value.value);
                    GameCardResult c = (GameCardResult) LocalScope[node.Prop.Prop[0].ID];
                    c.value.SetPower(c.value.Power + 1);
                }
                break;

            case "-=":
                if(node.ID != null)
                {
                    NumberResult r = (NumberResult) node.Exp.Accept(this, globalScope, LocalScope);
                    LocalScope[node.ID.ID] = r;
                }
                else // case 'card.Power'
                {
                    NumberResult value =(NumberResult) node.Exp.Accept(this, globalScope, LocalScope);
                    //globalScope.thisCard.SetPower(globalScope.thisCard.Power - value.value);
                    GameCardResult c = (GameCardResult) LocalScope[node.Prop.Prop[0].ID];
                    c.value.SetPower(c.value.Power - 1);
                }
                break;
        }

        return new Result();
    }

    public Result Visit(AST_Boolean node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("boolean");
        return new BooleanResult(node.Value);
    }

    public Result Visit(AST_BooleanExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("boolean exp");
        Result aux = node.Left.Accept(this, globalScope, LocalScope);

        if (node.Op != null)
        {
            Debug.LogError(aux.GetType());
            BooleanResult Left = (BooleanResult) aux;
            BooleanResult Right = (BooleanResult) node.Right.Accept(this, globalScope, LocalScope);

            bool result;
            if (node.Op == "&&")
            {
                result = Left.value && Right.value;
                return new BooleanResult(result);
            }

            result = Left.value || Right.value;
            return new BooleanResult(result);
        }

        return aux;
    }

    public Result Visit(AST_CardBody node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        throw new System.NotImplementedException();
    }

    public Result Visit(AST_CardDefinition node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        throw new System.NotImplementedException();
    }

    public Result Visit(AST_ComparisonExpression node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("comparison");
        Result aux = node.Left.Accept(this, globalScope, LocalScope);

        if (node.Op != null)
        {
            bool result = false;

            if (node.Op == "==")
            {
                if (aux is NumberResult)
                {
                    NumberResult L = (NumberResult) aux;
                    NumberResult R = (NumberResult) node.Right.Accept(this, globalScope, LocalScope);
                    result = L.value == R.value;
                }
                if (aux is StringResult)
                {
                    StringResult L = (StringResult) aux;
                    StringResult R = (StringResult) node.Right.Accept(this, globalScope, LocalScope);
                    result = L.value == R.value;
                }
                if (aux is BooleanResult)
                {
                    BooleanResult L = (BooleanResult) aux;
                    BooleanResult R = (BooleanResult) node.Right.Accept(this, globalScope, LocalScope);
                    result = L.value == R.value;
                }
            }

            else
            {
                Debug.Log(aux.GetType());
                NumberResult L = (NumberResult) aux;
                // Result x = node.Right.Accept(this, globalScope, LocalScope);
                // Debug.LogWarning(x.GetType());
                NumberResult R = (NumberResult) node.Right.Accept(this, globalScope, LocalScope);

                if (node.Op == ">")
                {
                    result = L.value > R.value;
                }
                else if (node.Op == ">=")
                {
                    result = L.value >= R.value;
                }
                else if (node.Op == "<")
                {
                    result = L.value < R.value;
                }
                else if (node.Op == "<=")
                {
                    result = L.value <= R.value;
                }
            }
            Debug.Log("llega");
            return new BooleanResult(result);
        }

        return aux;
    }

    public Result Visit(AST_EffectActivation node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        throw new System.NotImplementedException();
    }

    public Result Visit(AST_EffectBody node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        throw new System.NotImplementedException();
    }

    public Result Visit(AST_EffectDefinition node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        throw new System.NotImplementedException();
    }

    public Result Visit(AST_EffectInvocation node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("effect invocation");
        Effect thisEffect = globalScope.Effects[node.Name];

        Dictionary<string, Result> newLocalScope = new Dictionary<string, Result> (LocalScope);
        
        foreach (var param in node.Params)
        {
            param.Accept(this, globalScope, newLocalScope);
        }

        if (node.Selector != null)
        {
            Debug.LogWarning($"{node.Name} selector");
            ListResult targets = (ListResult) node.Selector.Accept(this, globalScope, newLocalScope);
            /*foreach (GameCard card in targets.value)
            {
                if (card.BaseCard.CardName == )
            }*/
            newLocalScope.Add("targets", targets);
            //Debug.LogWarning(LocalScope["targets"]);
            if(targets.value.Count == 0) Debug.LogWarning("vacio");
            foreach(var target in targets.value) Debug.Log(target.BaseCard.CardName);
        }

        thisEffect.Action.Accept(this, globalScope, newLocalScope);

        node.PostAction?.Accept(this, globalScope, newLocalScope);

        return new Result();
    }

    public Result Visit(AST_Factor node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("factor");
        return node.Expression.Accept(this, globalScope, LocalScope);
    }

    public Result Visit(AST_ForLoop node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("forlu'");
        Debug.Log("entra");
        Dictionary<string, Result> newLocalScope = new Dictionary<string, Result>(LocalScope)
        {
            {node.Element.ID, new GameCardResult()},
        };
        
        if (node.Identifier != null)
        {
            ListResult targets = (ListResult) newLocalScope["targets"];
            foreach (GameCard target in targets.value)
            {
                Debug.Log(target.BaseCard.CardName);
            }
            Debug.Log(targets.value == null);
            foreach (GameCard target in targets.value) 
            {
                Dictionary<string, Result> targetLocalScope = new Dictionary<string, Result>(newLocalScope);
                targetLocalScope[node.Element.ID] = new GameCardResult(target);
                node.Statements.Accept(this, globalScope, targetLocalScope);
            }
            Debug.Log("era id");
        }

        else if (node.Collection != null)
        {
            ListResult t = (ListResult) LocalScope["targets"];
            ListResult targets = (ListResult) node.Collection.Accept(this, globalScope, newLocalScope);
            foreach (GameCard target in targets.value)
            {
                Debug.Log(target.BaseCard.CardName);
                Dictionary<string, Result> targetLocalScope = new Dictionary<string, Result>(newLocalScope);
                targetLocalScope[node.Element.ID] = new GameCardResult(target);
                node.Statements.Accept(this, globalScope, targetLocalScope);
            }
            Debug.Log("era prop");
        }

        else // (node.Function != null)
        {
            ListResult targets = (ListResult) node.Function.Accept(this, globalScope, newLocalScope);
            Debug.LogWarning("Galletica.Find");
            foreach (GameCard target in targets.value)
            {
                Debug.LogWarning(target.BaseCard.CardName);
                Dictionary<string, Result> targetLocalScope = new Dictionary<string, Result>(newLocalScope);
                targetLocalScope[node.Element.ID] = new GameCardResult(target);
                node.Statements.Accept(this, globalScope, targetLocalScope);
            }
        }
        
        return new Result();
    }

    public Result Visit(AST_FunctionCall node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("function call");
        string func = node.Prop.Prop.Last().ID;
        Debug.Log($"Last: {func}");
        List<AST_Identifier> newPA = new List<AST_Identifier>(node.Prop.Prop);
        newPA.RemoveAt(newPA.Count-1);
        AST_PropertyAccess temp = new AST_PropertyAccess(newPA);

        Result target = temp.Accept(this, globalScope, LocalScope);
        
        switch (func)
        {
            case "Find":
                Debug.Log("func Find");
                ListResult listtoFind = (ListResult) target;
                List<GameCard> found = new List<GameCard>();
                PredicateResult pred = (PredicateResult) node.Predicate.Accept(this, globalScope, LocalScope);
                Func<GameCard, bool> filter = pred.value;
                foreach(GameCard card6 in listtoFind.value)
                {
                    if (filter(card6))
                    {
                    found.Add(card6);
                        Debug.Log(card6.BaseCard.CardName);
                    } 
                }
                return new ListResult(found);

            case "Add":
            Debug.Log("func Add");
                ListResult listtoAdd = (ListResult) target;
                Player otherPlayer = TurnSystem.Instance.Active == PlayerManager.Instance.Player1 ? PlayerManager.Instance.Player2 : PlayerManager.Instance.Player1; 
                GameCardResult gameCard = (GameCardResult) node.Param.Accept(this, globalScope, LocalScope);

                if (listtoAdd.value.SequenceEqual(Context.Instance.Hand))
                {
                    Debug.Log("entra a hand add");
                    if (Context.Instance.Hand.Count < 10)
                    {
                        gameCard.value.transform.SetParent(TurnSystem.Instance.Active.thisHand.transform);
                        gameCard.value.transform.localPosition = Vector3.zero;
                        TurnSystem.Instance.Active.thisHand.ArrangeCards();
                    }
                    else
                    {
                        gameCard.value.transform.SetParent(TurnSystem.Instance.Active.overflow.transform);
                        TurnSystem.Instance.Active.graveyard.SendToGraveyard(gameCard.value);
                    }
                    Debug.Log(gameCard.value.transform.parent);
                    Debug.Log(gameCard.value.transform.parent);
                }

                if (listtoAdd.value.SequenceEqual(Context.Instance.HandOfPlayer(otherPlayer)))
                {
                    Debug.Log("entra a handOfP add");
                    gameCard.value.transform.SetParent(otherPlayer.thisHand.transform);
                    gameCard.value.transform.localPosition = Vector3.zero;
                    otherPlayer.thisHand.ArrangeCards();
                }

                if (listtoAdd.value.SequenceEqual(Context.Instance.Field))
                {
                    Debug.Log("entra a field add");
                    CardSlot[] slots = null;
                    switch (gameCard.value.BaseCard.thisRole)
                    {
                        case Role.Mele:
                            slots = TurnSystem.Instance.Active.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                        break;
                        
                        case Role.Range:
                            slots = TurnSystem.Instance.Active.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                        break;
                        
                        case Role.Siege:
                            slots = TurnSystem.Instance.Active.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                        break;
                        
                        case Role.Agile:
                            CardSlot[] meleRow = TurnSystem.Instance.Active.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                            CardSlot[] rangeRow = TurnSystem.Instance.Active.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                            int x = UnityEngine.Random.Range(0,1);
                            if (x==0) slots = meleRow;
                            else slots = rangeRow;
                        break;
                    }
                    
                    for (int i = 0; i < slots.Length; i++)
                     {
                        if (slots[i].transform.childCount == 0)
                        {
                            gameCard.value.transform.SetParent(slots[i].transform);
                            gameCard.value.transform.localPosition = Vector3.zero;
                            break;
                        }
                    }
                }

                if (listtoAdd.value.SequenceEqual(Context.Instance.FieldOfPlayer(otherPlayer)))
                {
                    Debug.Log("entra a fieldOfP add");
                    CardSlot[] slots = null;
                    switch (gameCard.value.BaseCard.thisRole)
                    {
                        case Role.Mele:
                            slots = otherPlayer.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                        break;
                        
                        case Role.Range:
                            slots = otherPlayer.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                        break;
                        
                        case Role.Siege:
                            slots = otherPlayer.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                        break;
                        
                        case Role.Agile:
                            CardSlot[] meleRow = otherPlayer.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                            CardSlot[] rangeRow = otherPlayer.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                            int x = UnityEngine.Random.Range(0,1);
                            if (x==0) slots = meleRow;
                            else slots = rangeRow;
                        break;
                    }
                    
                    for (int i = 0; i < slots.Length; i++)
                     {
                        if (slots[i].transform.childCount == 0)
                        {
                            gameCard.value.transform.SetParent(slots[i].transform);
                            gameCard.value.transform.localPosition = Vector3.zero;
                            break;
                        }
                    }
                }

                if (listtoAdd.value.SequenceEqual(Context.Instance.Board))
                {
                    Debug.Log("entra a board add");
                    CardSlot[] slots = null;
                    int x = UnityEngine.Random.Range(0,1);
                    switch (gameCard.value.BaseCard.thisRole)
                    {
                        case Role.Mele:
                            if (x == 0) slots = TurnSystem.Instance.Active.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                            else slots = otherPlayer.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                        break;
                        
                        case Role.Range:
                            if (x == 0) slots = TurnSystem.Instance.Active.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                            else slots = otherPlayer.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                        break;
                        
                        case Role.Siege:
                            if (x == 0) slots = TurnSystem.Instance.Active.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                            else slots = otherPlayer.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                        break;
                        
                        case Role.Agile:
                            CardSlot[] meleRow = TurnSystem.Instance.Active.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                            CardSlot[] rangeRow = TurnSystem.Instance.Active.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                            CardSlot[] meleRow2 = otherPlayer.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                            CardSlot[] rangeRow2 = otherPlayer.battlefield.Mele.GetComponentsInChildren<CardSlot>();
                            int y = UnityEngine.Random.Range(0,3);
                            if (y==0) slots = meleRow;
                            else if (y==1) slots = meleRow2;
                            else if (y==2) slots = rangeRow;
                            else slots = rangeRow2;
                        break;
                    }
                    
                    for (int i = 0; i < slots.Length; i++)
                     {
                        if (slots[i].transform.childCount == 0)
                        {
                            gameCard.value.transform.SetParent(slots[i].transform);
                            gameCard.value.transform.localPosition = Vector3.zero;
                            break;
                        }
                    }
                }
            return new Result();
            
            case "Remove":
                Debug.Log("func Remove");
                GameCardResult gameCard2 = (GameCardResult) node.Param.Accept(this, globalScope, LocalScope);
                gameCard2.value.transform.SetParent(null);
                GameObject.Destroy(gameCard2.value);
            return new Result();
            
            case "Push":
                Debug.Log("func Push");
                StackResult stacktoPush = (StackResult) target;

                GameCardResult gameCard7 = (GameCardResult) node.Param.Accept(this, globalScope, LocalScope);
                stacktoPush.value.Push(gameCard7.value.BaseCard);
            return new Result();
            
            case "Pop":
                Debug.Log("func Pop");
                Debug.Log("stack to pop: " + target.GetType());

                StackResult stacktoPop = (StackResult) target;
                GameCard popped = Game.GetNewCard();

                Card c = stacktoPop.value.Pop();
                popped.Assign(c);
            return new GameCardResult(popped);
            
            case "SendBottom":
                Debug.Log("func SendBottom");
                StackResult sendBottom = (StackResult) target;

                GameCardResult gameCard4 = (GameCardResult) node.Param.Accept(this, globalScope, LocalScope);
                Stack<Card> aux = new Stack<Card>();
                while (sendBottom.value.Count > 0)
                {
                    aux.Push(sendBottom.value.Pop());
                }
                sendBottom.value.Push(gameCard4.value.BaseCard);
                while (aux.Count > 0)
                {
                    sendBottom.value.Push(aux.Pop());
                }
            return new Result();
            
            case "Shuffle":
                Debug.Log("func Shuffle");
                if (target is StackResult)
                {
                    StackResult deck = (StackResult) target;
                    List<Card> auxx = new List<Card>();
                    int count = deck.value.Count;
                    while (deck.value.Count > 0) auxx.Add(deck.value.Pop());

                    int x;
                    while(deck.value.Count < count)
                    {
                        x = UnityEngine.Random.Range(0, auxx.Count);
                        Card nextCard = auxx[x];
                        if (nextCard.instancesLeft > 0) 
                        {
                            deck.value.Push(auxx[x]);
                            auxx.RemoveAt(x);
                        }
                    }
                }
            return new Result();
            
            case "HandOfPlayer":
                Debug.Log("func HandOfPlayer");
                Result r = node.Param.Accept(this, globalScope, LocalScope);
                if (r is IdentifierResult)
                {
                    IdentifierResult id = (IdentifierResult) r;
                    PlayerResult res = (PlayerResult) LocalScope[id.value];
                    Player player = res.value;
                    List<GameCard> hand = Context.Instance.HandOfPlayer(player);
                    return new ListResult(hand);
                }
                else
                {
                    PlayerResult pr = (PlayerResult) r;
                    Player player = pr.value;
                    List<GameCard> hand = Context.Instance.HandOfPlayer(player);
                    return new ListResult(hand);
                }
            
            case "FieldOfPlayer":
                Debug.Log("func FieldOfPlayer");
                Result r2 = node.Param.Accept(this, globalScope, LocalScope);
                if (r2 is IdentifierResult)
                {
                    IdentifierResult id = (IdentifierResult) r2;
                    PlayerResult res = (PlayerResult) LocalScope[id.value];
                    Player player = res.value;
                    List<GameCard> field = Context.Instance.FieldOfPlayer(player);
                    return new ListResult(field);
                }
                else
                {
                    PlayerResult pr = (PlayerResult) r2;
                    Player player = pr.value;
                    List<GameCard> field = Context.Instance.FieldOfPlayer(player);
                    return new ListResult(field);
                }
            
            case "GraveyardOfPlayer":
                Debug.Log("func GraveyardOfPlayer");
                Result r3 = node.Param.Accept(this, globalScope, LocalScope);
                if (r3 is IdentifierResult)
                {
                    IdentifierResult id = (IdentifierResult) r3;
                    PlayerResult res = (PlayerResult) LocalScope[id.value];
                    Player player = res.value;
                    Stack<Card> graveyard = Context.Instance.GraveyardOfPlayer(player);
                    return new StackResult(graveyard);
                }
                else
                {
                    PlayerResult pr = (PlayerResult) r3;
                    Player player = pr.value;
                    Stack<Card> graveyard = Context.Instance.GraveyardOfPlayer(player);
                    return new StackResult(graveyard);
                }
            
            case "DeckOfPlayer":
                Debug.Log("func DeckOfPlayer");
                Result r4 = node.Param.Accept(this, globalScope, LocalScope);
                if (r4 is IdentifierResult)
                {
                    IdentifierResult id = (IdentifierResult) r4;
                    PlayerResult res = (PlayerResult) LocalScope[id.value];
                    Player player = res.value;
                    Stack<Card> deck = Context.Instance.DeckOfPlayer(player);
                    return new StackResult(deck);
                }
                else
                {
                    PlayerResult pr = (PlayerResult) r4;
                    Player player = pr.value;
                    Stack<Card> deck = Context.Instance.DeckOfPlayer(player);
                    return new StackResult(deck);
                }
        }

        Debug.LogWarning($"Unhandled function '{func}'. Check FunctionCall");
        return new Result();
    }

    public Result Visit(AST_Identifier node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("identifier");
        if(!LocalScope.ContainsKey(node.ID)) return new IdentifierResult(node.ID);
        return LocalScope[node.ID];
    }

    public Result Visit(AST_Number node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("number");
        return new NumberResult(node.Value);
    }

    public Result Visit(AST_ParamAssignment node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("param assign");
        Result r = node.Value.Accept(this, globalScope, LocalScope);
        LocalScope.Add(node.Name, r);
        
        return new Result();
    }

    public Result Visit(AST_ParamDeclaration node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        throw new System.NotImplementedException();
    }

    public Result Visit(AST_PostAction node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("posaction");
        Effect thisEffect = globalScope.Effects[node.type];

        Dictionary<string, Result> newLocalScope = new Dictionary<string, Result> (LocalScope);
        
        foreach (var param in node.Params)
        {
            param.Accept(this, globalScope, LocalScope);
        }

        if (node.Selector != null)
        {
            ListResult targets = (ListResult) node.Selector.Accept(this, globalScope, newLocalScope);
            newLocalScope["targets"] = targets;
        }

        thisEffect.Action.Accept(this, globalScope, newLocalScope);

        node.PostAction?.Accept(this, globalScope, newLocalScope);

        return new Result();
    }

    public Result Visit(AST_Predicate node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("predicate");
        Func<GameCard, bool> predicate = new Func<GameCard, bool>(param =>
        {
            var newLocalScope = new Dictionary<string, Result>(LocalScope)
            {
                [node.ID.ID] = new GameCardResult(param)
            };
            
            Debug.LogWarning(node.Condition);
            BooleanResult result = (BooleanResult) node.Condition.Accept(this, globalScope, newLocalScope);
            return result.value;
            //return true;
        });

        return new PredicateResult(predicate);
    }

    public Result Visit(AST_Program node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        throw new System.NotImplementedException();
    }

    public Result Visit(AST_ProgramVoid node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        throw new System.NotImplementedException();
    }

    public Result Visit(AST_PropertyAccess node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("prop access");
        string first = node.Prop[0].ID;

        if (LocalScope[first] is GameCardResult)
        {
            Debug.Log("prop access cardProp");
            GameCardResult cr = (GameCardResult) LocalScope[first];
            string second = node.Prop[1].ID;
            switch (second)
            {
                case "Owner":
                    return new PlayerResult(cr.value.Owner);

                case "Power":
                    return new NumberResult(cr.value.Power);

                case "Faction":
                    return new StringResult(cr.value.BaseCard.CardFaction);

                case "Name":
                    return new StringResult(cr.value.BaseCard.CardName);

                case "Range":
                    return new StringResult(cr.value.BaseCard.thisRole.ToString());
            }
        }

        else if (LocalScope[first] is ListResult)
        {
            Debug.Log("prop access list");
            ListResult r = (ListResult) LocalScope[first];
            return r;
        }
        
        else if (LocalScope[first] is StackResult)
        {
            Debug.Log("prop access stack");
            StackResult r = (StackResult) LocalScope[first];
            return r;
        }

        else if (LocalScope[first] is ContextResult)
        {
            Debug.Log("prop access context prop?");
            if (node.Prop.Count > 1)
            {
                Debug.Log("prop access context func");
                string second = node.Prop[1].ID;

                switch (second)
                {
                    case "TriggerPlayer":
                        return new PlayerResult (Context.Instance.TriggerPlayer);
                    case "Board":
                        return new ListResult (Context.Instance.Board);
                    case "Hand":
                        return new ListResult (Context.Instance.Hand);
                    case "Deck":
                        return new StackResult (Context.Instance.Deck);
                    case "Graveyard":
                        return new StackResult (Context.Instance.Graveyard);
                    case "Field":
                        return new ListResult (Context.Instance.Field);
                }
            }

            else //(globalScope.ContextFunctions.ContainsKey(second))
                return new ContextResult();
        }

        Debug.LogWarning($"PropertyAccess '{node.Prop}' has an unhandled value. Check why");
        return null;
    }

    public Result Visit(AST_Selector node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("selector");
        PredicateResult Predicate = (PredicateResult) node.Predicate.Accept(this, globalScope, LocalScope);
        Func<GameCard, bool> filter = Predicate.value;

        List<GameCard> targets = new List<GameCard>();
        Stack<Card> deck = new Stack<Card>();
        switch (node.Source)
        {
            case "board":
                List<GameCard> source = Context.Instance.Board;
                Debug.LogWarning("case board");
                foreach (GameCard card in source)
                {
                    if(filter(card)) targets.Add(card);
                    Debug.LogWarning(filter(card));
                }
                Debug.LogWarning(targets.Count);
                break;

            case "hand":
                List<GameCard> source2 = Context.Instance.Hand;
                foreach (GameCard card in source2)
                {
                    if(filter(card)) targets.Add(card);
                }
                break;

            case "otherHand":
                List<GameCard> source3 = Context.Instance.otherHand;
                foreach (GameCard card in source3)
                {
                    if(filter(card)) targets.Add(card);
                }
                break;

            case "deck":
                deck = Context.Instance.Deck;
                break;

            case "otherDeck":
                deck = Context.Instance.otherDeck;
                break;

            case "field":
                List<GameCard> source4 = Context.Instance.Field;
                foreach (GameCard card in source4)
                {
                    if(filter(card)) targets.Add(card);
                }
                break;

            case "otherField":
                List<GameCard> source5 = Context.Instance.otherField;
                foreach (GameCard card in source5)
                {
                    if(filter(card)) targets.Add(card);
                }
                break;

            default :
                return LocalScope["targets"];
        }
        Debug.LogError($"targets: {targets.Count}");
        if (targets.Count != 0)
        {
            BooleanResult Single = (BooleanResult) node.Single.Accept(this, globalScope, LocalScope);
            if (Single.value) targets = new List<GameCard> { targets[0] };
            return new ListResult(targets);
        }

        return new StackResult(deck);
    }

    public Result Visit(AST_Statement node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("stmt");
        if (node is AST_ForLoop)
        {
            node.Accept(this, globalScope, LocalScope);
            return new Result();
        }
        else if (node is AST_WhileLoop)
        {
            node.Accept(this, globalScope, LocalScope);
            return new Result();
        }
        else if (node is AST_BinaryAssignStatement)
        {
            node.Accept(this, globalScope, LocalScope);
            return new Result();
        }
        else //node is function call
        {
            node.Accept(this, globalScope, LocalScope);
            return new Result();
        }
    }

    public Result Visit(AST_StatementBlock node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("stmt block");
        foreach (GameCard card in Context.Instance.Board) Debug.LogError(card.BaseCard.CardName + ": " + card.Power);
        //Dictionary<string, Result> newLocalScope = new Dictionary<string, Result>(LocalScope);
        foreach (AST_Statement stmt in node.Statements) stmt.Accept(this, globalScope, LocalScope);
        PlayerManager.Instance.Player1.battlefield.UpdatePoints();
        PlayerManager.Instance.Player2.battlefield.UpdatePoints();
        foreach (GameCard card in Context.Instance.Board) Debug.LogError(card.BaseCard.CardName + ": " + card.Power);
        return new Result();
    }

    public Result Visit(AST_String node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("string");
        return new StringResult(node.Value);
    }

    public Result Visit(AST_Term node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("term");
        Result Left = node.Left.Accept(this, globalScope, LocalScope);

        if (node.Op != null)
        {
            Result Right = node.Right.Accept(this, globalScope, LocalScope);

            switch(node.Op)
            {
                case "*":
                    NumberResult L1 = (NumberResult) Left;
                    NumberResult R1 = (NumberResult) Right;
                    return new NumberResult(L1.value * R1.value);
                    
                case "/":
                    NumberResult L2 = (NumberResult) Left;
                    NumberResult R2 = (NumberResult) Right;
                    return new NumberResult(L2.value / R2.value);

                case "^":
                    NumberResult L3 = (NumberResult) Left;
                    NumberResult R3 = (NumberResult) Right;
                    double power = Math.Pow(L3.value, R3.value);
                    float pow = (float) power;
                    return new NumberResult(pow);

                case "@":
                    StringResult L4 = (StringResult) Left;
                    StringResult R4 = (StringResult) Right;
                    return new StringResult(L4.value + R4.value);

                case "@@":
                    Debug.Log(Left.GetType());
                    Debug.Log(Right.GetType());
                    StringResult L5 = (StringResult) Left;
                    StringResult R5 = (StringResult) Right;
                    return new StringResult(L5.value + " " + R5.value);
            }
        }

        return Left;
    }

    public Result Visit(AST_UnaryAssignFactor node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        if (node.ID != null)
        {
            Result r = LocalScope[node.ID.ID];
                
            switch (node.Op)
            {
                case "!":
                    BooleanResult br = (BooleanResult) r;
                    BooleanResult result = new BooleanResult(!br.value);
                    LocalScope[node.ID.ID] = result;
                    return result;

                case "++":
                    NumberResult nr = (NumberResult) r;
                    Debug.LogWarning("Value: " + nr.value);
                    NumberResult result2 = new NumberResult(nr.value + 1);
                    LocalScope[node.ID.ID] = result2;
                    return result2;

                case "--":
                    NumberResult nr2 = (NumberResult) r;
                    NumberResult result3 = new NumberResult(nr2.value - 1);
                    LocalScope[node.ID.ID] = result3;
                    return result3;
            }

            return new Result();
        }
            
        else // cases card.Power++, card.Power--, ++card.Power, --card.Power
        {
            NumberResult value = (NumberResult) node.Prop.Accept(this, globalScope, LocalScope);

            switch (node.Op)
            {

                case "++":
                    //globalScope.thisCard.SetPower(value.value + 1);
                    GameCardResult c = (GameCardResult) LocalScope[node.Prop.Prop[0].ID];
                    Debug.LogWarning("Value: " + value.value);
                    c.value.SetPower(c.value.Power + 1);
                    return new NumberResult(c.value.Power + 1);

                case "--":
                    //globalScope.thisCard.SetPower(value.value - 1);
                    GameCardResult cr = (GameCardResult) LocalScope[node.Prop.Prop[0].ID];
                    cr.value.SetPower(cr.value.Power - 1);
                    return new NumberResult(cr.value.Power - 1);
            }

            return new Result();
        }
    }
    
    public Result Visit(AST_UnaryAssignStatement node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        if (node.ID != null)
        {
            Result r = LocalScope[node.ID.ID];
                
            switch (node.Op)
            {
                case "!":
                    BooleanResult br = (BooleanResult) r;
                    BooleanResult result = new BooleanResult(!br.value);
                    LocalScope[node.ID.ID] = result;
                break;

                case "++":
                    NumberResult nr = (NumberResult) r;
                    Debug.LogWarning("Value: " + nr.value);
                    NumberResult result2 = new NumberResult(nr.value + 1);
                    LocalScope[node.ID.ID] = result2;
                break;

                case "--":
                    NumberResult nr2 = (NumberResult) r;
                    NumberResult result3 = new NumberResult(nr2.value - 1);
                    LocalScope[node.ID.ID] = result3;
                break;
            }

            return new Result();
        }
            
        else // cases card.Power++, card.Power--, ++card.Power, --card.Power
        {
            NumberResult value = (NumberResult) node.Prop.Accept(this, globalScope, LocalScope);

            switch (node.Op)
            {

                case "++":
                    //globalScope.thisCard.SetPower(value.value + 1);
                    GameCardResult c = (GameCardResult) LocalScope[node.Prop.Prop[0].ID];
                    c.value.SetPower(c.value.Power + 1);
                break;

                case "--":
                    //globalScope.thisCard.SetPower(value.value - 1);
                    GameCardResult cr = (GameCardResult) LocalScope[node.Prop.Prop[0].ID];
                    cr.value.SetPower(cr.value.Power - 1);
                break;
            }

            return new Result();
        }
    }

    public Result Visit(AST_WhileLoop node, GlobalScope globalScope, Dictionary<string, Result> LocalScope)
    {
        Debug.Log("while");
        BooleanResult condition = (BooleanResult) node.Condition.Accept(this, globalScope, LocalScope);
        while (condition.value)
        {
            if(LocalScope.ContainsKey("Amount"))
            {
                NumberResult nr = (NumberResult) LocalScope["i"];
                float i = nr.value;
                Debug.LogError("i = " + i);
                NumberResult r = (NumberResult) LocalScope["Amount"];
                Debug.LogError(r.value);
            }
            node.Actions.Accept(this, globalScope, LocalScope);
            condition = (BooleanResult) node.Condition.Accept(this, globalScope, LocalScope);
        }
        return new Result();
    }
}