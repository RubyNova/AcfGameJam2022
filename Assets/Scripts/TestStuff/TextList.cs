using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextList", menuName = "ScriptableObjects/TextList", order = 1)]
public class TextList : ScriptableObject
{
    public string prefabName;
    public int prefabCount;
    public List<string> textList;
}