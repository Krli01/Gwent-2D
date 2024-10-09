using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ForgeManager : MonoBehaviour
{
    public static ForgeManager Instance { get; private set; }
    public TMP_InputField inputCode;
    public TextMeshProUGUI outputText;
    
    private Lexer lexer;
    private Parser parser;
    private Interpreter interpreter;

    void Awake()
    {
        if (Instance == null) Instance = this;
        lexer = new Lexer();
        parser = new Parser();
        interpreter = new Interpreter();
    }

    public void ButtonCompile()
    {
        Compile();
    }

    public void ButtonSaveItems()
    {
        Result res = Compile();
        if (res is ProgramResult)
        {
            ProgramResult p = (ProgramResult) res;

            foreach (var card in p.value.Item1)
            {
                List<Card> deck = CardDatabase.AvailableDecks[card.CardFaction];
                deck.Add(card);
            }

            foreach (var effect in p.value.Item2)
            {
                Dictionary<string, Effect> effects = CardDatabase.Effects;
                effects.Add(effect.Name, effect);
            }
        }
    }

    public Result Compile()
    {
        Result result = null;
        List<Token> tokens = lexer.Run(inputCode.text);
        if (tokens.Count == 0)
        {
            ConsolePrint(lexer.Error);
        }
        else
        {
            AST ast = parser.Run(tokens);
            if (ast == null)
            {
                Debug.Log(parser.Error);
                Debug.Log("Ast null");
                ConsolePrint(parser.Error);
            }
            else
            {
                result = interpreter.Run((AST_Program)ast, Context.Instance);
                if (result == null) ConsolePrint(interpreter.Error);
                else ConsolePrint("Success. Save items before leaving editor");
                Debug.Log("result null" + (result == null));
                Debug.Log(interpreter.Error);
            }

        }
        return result;
        //foreach (var token in tokens) Debug.Log($"Token {token.Lexeme}, {token.Type} : {token.Line}");
    }

    private void ConsolePrint(string output)
    {
        outputText.text = $"> {output}";
    }

    public void ClearScriptor()
    {
        inputCode.text = "";
    }

    
    public void AddCard()
    {
        string[] cardTemplate = new string[] {
        "card {",
        "    Type: \" \",", 
        "    Name: \" \",", 
        "    Faction: \" \",",
        "    Power:  ,",
        "    Range: [],",
        "    //Image:   // Escribe el directorio de la imagen. Recomendadas proporciones 1:1",
        "    OnActivation: [",
        "        {",
        "            Effect: ",
        "            Selector: {",
        "                Source: ",
        "                Single: false,",
        "                Predicate: () => ",
        "            },",
        "            PostAction: {",
        "                Type: ",
        "                Selector: {     // Opcional, si no se especifica se tomará el del efecto inicial",
        "                    Source: ",
        "                    Single: false,",
        "                    Predicate: () => ",
        "                }",
        "            }",
        "        }",
        "    ]",
        "}",
        };

        for (int i = 0; i < cardTemplate.Length; i++)
        {
            inputCode.text += cardTemplate[i];
            inputCode.text += '\n';
        }

        inputCode.caretPosition = inputCode.text.Length-1;
    }

    public void AddEffect()
    {
        string[] effectTemplate = new string[] {
        "effect {",
        "    Name: ",
        "    Params: {",
        "        ",
        "    },",
        "    Action: (targets, context) => {",
        "        for target in targets {",
        "           // Escribir aquí las acciones",
        "        };",
        "        while() {",
        "           // Escribir aquí las acciones",
        "        };",
        "    }",
        "}",
        };

        for (int i = 0; i < effectTemplate.Length; i++)
        {
            inputCode.text += effectTemplate[i];
            inputCode.text += '\n';
        }

        inputCode.caretPosition = inputCode.text.Length-1;
    }
}
