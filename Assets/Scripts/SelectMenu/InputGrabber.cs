using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGrabber : MonoBehaviour
{
    string inputString;

    public void GrabInput(string input)
    {
        inputString = input;
        Debug.Log(inputString);
    }
}
