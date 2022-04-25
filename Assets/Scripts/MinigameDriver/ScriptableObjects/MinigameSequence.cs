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
    [SerializeField] private int _basePoseIndex;
    [SerializeField] private int _wordSuccessPoseIndex;
    [SerializeField] private int _wordFailPoseIndex;
    [SerializeField] private Sprite _backgroundSprite;

    private List<TextList.Word> _usedWords;
    private int _userScore;

    public TextList wordListData
    {
        get => _wordListData;
        set => _wordListData = value;
    }

    public ACHNarrativeDriver.ScriptableObjects.Character character
    {
        get => _character;
        set => _character = value;
    }

    public int basePoseIndex
    {
        get => _basePoseIndex;
        set => _basePoseIndex = value;
    }

    public int wordSuccessPoseIndex
    {
        get => _wordSuccessPoseIndex;
        set => _wordSuccessPoseIndex = value;
    }

    public int wordFailPoseIndex
    {
        get => _wordFailPoseIndex;
        set => _wordFailPoseIndex = value;
    }

    public Sprite backgroundSprite
    {
        get => _backgroundSprite;
        set => _backgroundSprite = value;
    }

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
