using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordRenderer : MonoBehaviour
{
    [SerializeField] private List<Image> images;
    [SerializeField] private List<Sprite> sprites;

    private Dictionary<int, int> dictionary;

    void Awake()
    {
        dictionary = new();
        for (int i = 0; i < 26; i++)
        {
            dictionary.Add(i + 65, i);
        }
    }

    public void RenderWord(string word)
    {
        if (word.Length > 15)
        {
            throw new Exception("The word is too long");
        }
        
        word = word.ToUpper();

        for (int i = 0; i < word.Length; i++)
        {
            int value = (int) word[i];
            images[i].sprite = sprites[ dictionary[value] ];
        }

    }
}
