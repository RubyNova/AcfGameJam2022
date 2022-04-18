using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordListDisplay : MonoBehaviour
{

    public TextList wordList;
    public TextMeshProUGUI listDisplay;
    protected string text;

    void Update()
    {
        text = "Words on list:";

        foreach (var word in wordList.textList)
        {
            text += "\n";
            text += word.word;
        }
        listDisplay.text = text;
    }
}
