using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MinigameSequence : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TextList _wordListData;
    //[SerializeField] private ACHNarrativeDriver.ScriptableObjects.Character _convoPartner;

    public TMP_InputField inputField
    {
        get => _inputField;
        set => _inputField = value;
    }
    public TextList wordListData
    {
        get => _wordListData;
        set => _wordListData = value;
    }
    /*public ACHNarrativeDriver.ScriptableObjects.Character convoPartner
    {
        get => _convoPartner;
        set => _convoPartner = value;
    }*/

    private List<TextList.Word> _usedWords;
    private int _userScore;

    public List<TextList.Word> usedWords
    {
        get => _usedWords;
        set => _usedWords = value;
    }
    public int userScore
    {
        get => _userScore;
        set => _userScore = value;
    }
}
