using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "MinigameSequence", menuName = "ScriptableObjects/MinigameSequence", order = 1)]
public class MinigameSequence : ScriptableObject
{
    [SerializeField] private TextList _wordListData;
    
    public TextList wordListData
    {
        get => _wordListData;
        set => _wordListData = value;
    }

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
