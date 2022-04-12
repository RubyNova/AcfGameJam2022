using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextInputTest : MonoBehaviour
{
    public TextMeshProUGUI userInputDisplay;
    public TextMeshProUGUI isFoundDisplay;
    public TMP_InputField inputField;
    public TextList wordList;

    void Start()
    {
        isFoundDisplay.text = "";
    }

    public void setUserInput()
    {
        userInputDisplay.text = "User Input: " + inputField.text;
    }


    public void compareInput()
    {
        if( wordList.textList.Contains(inputField.text) )
            isFoundDisplay.text = "Word IS found";
        else
            isFoundDisplay.text = "Word is NOT found";
    }
}
