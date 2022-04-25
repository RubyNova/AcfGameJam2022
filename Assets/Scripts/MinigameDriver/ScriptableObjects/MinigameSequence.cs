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
    [Serializable]
    public class TextList
    {
        [Serializable]
        public class Word
        {
            public string word;
            public int rarity;
            public bool wasMentioned;
        }

        public string prompt;
        public int pointsNeeded;
        public List<Word> textList;
    }

    [SerializeField] private TextList _wordListData;

    [SerializeField] private ACHNarrativeDriver.ScriptableObjects.Character _character;
    [SerializeField] private bool _hasPoseIndex;
    [SerializeField] private int _poseIndex;
    
    public TextList wordListData
    {
        get => _wordListData;
        set => _wordListData = value;
    }

    public ACHNarrativeDriver.ScriptableObjects.Character Character
    {
        get => _character;
        set => _character = value;
    }

    public int? PoseIndex
    {
        get => _hasPoseIndex ? _poseIndex : null;
        set
        {
            if (value.HasValue)
            {
                _poseIndex = value.Value;
                _hasPoseIndex = true;
            }
            else
            {
                _hasPoseIndex = false;
            }
        }
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
