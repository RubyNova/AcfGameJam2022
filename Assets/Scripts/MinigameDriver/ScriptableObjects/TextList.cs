using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextList", menuName = "ScriptableObjects/TextList", order = 1)]
public class TextList : ScriptableObject
{
    [Serializable]
    public class Word
    {
        public string word;
        public int rarity;
        public bool wasMentioned;
    }

    public string prefabName;
    public int prefabCount;
    public string prompt;
    public int pointsNeeded;
    public List<Word> textList;
}