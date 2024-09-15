using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForgeManager : MonoBehaviour
{
    public static ForgeManager Instance { get; private set; }
    public TMP_InputField inputCode;
    //public InputField inputField
    public Lexer lexer;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Compile()
    {

        List<Token> tokens = lexer.Run(inputCode.text);
        if (lexer.Success)
        {
            foreach (var t in tokens) Debug.Log($"Token {t.Type}, {t.Lexeme} in line {t.Line}");
        }
        else Debug.Log(lexer.Error);
    }
}
