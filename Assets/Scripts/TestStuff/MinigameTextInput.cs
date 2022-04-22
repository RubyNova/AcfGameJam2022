using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MinigameTextInput : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextList wordListData;
    //public ACHNarrativeDriver.ScriptableObjects.Character convoPartner;
    private List<TextList.Word> usedWords;
    private int userScore;
    public UnityEvent listNextEvent;

    //Initialization
    void Start()
    {
        userScore = 0;
        usedWords = new List<TextList.Word>();
    }

    //Updates every frame
    void Update()
    {
        if(userScore >= wordListData.pointsNeeded)
        {
            listNextEvent.Invoke();
        }
    }

    //Gets the user's input from the textbox then checks if the user's word was used before and if it's on the list
    //If it's on the list and wasn't used before, the score is calculated
    public void compareInput()
    {
        bool wasFound = false;
        foreach (var userWord in wordListData.textList)
        {
            foreach (var oldWord in usedWords)
            {
                if(inputField.text.Equals(oldWord.word, StringComparison.InvariantCultureIgnoreCase) )  //Word was already used
                {
                    wasFound = true;
                    break;
                }
            }
            if (!wasFound && inputField.text.Equals(userWord.word, StringComparison.InvariantCultureIgnoreCase) ) //Word found and unused
            {
                calcScore(userWord);
                usedWords.Add(userWord);
                break;
            }
        }
    }

    //Calculate the value of the word input and add it to the current userScore
    public void calcScore(TextList.Word userWord)
    {
        if (userWord.rarity > 4)
            userWord.rarity = 4;
        if (userWord.rarity < 1)
            userWord.rarity = 1;

        int wasMentionedInt = userWord.wasMentioned ? 1 : 0;

        int temp = (int) Math.Floor(1000 + 100*Math.Sqrt(userWord.rarity)*userWord.word.Length + 200*wasMentionedInt);
        
        if ( (temp % 50) >= 25)
        {
            temp += 50;
        }
        temp -= (temp % 50);

        userScore += temp;
    }

    //userScore Getter
    public int getUserScore()
    {
        return userScore;
    }

    /*public void scoreMet()
    {
        
    }
    */
}
