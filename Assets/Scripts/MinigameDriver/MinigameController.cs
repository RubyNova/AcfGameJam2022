using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MinigameController : MonoBehaviour
{
    [SerializeField] private UnityEvent listNextEvent;
    [SerializeField] private MinigameSequence currentGameSequence;

    private bool isCurrentlyExecuting;

    //Initialization
    void Start()
    {
        currentGameSequence.userScore = 0;
        currentGameSequence.usedWords = new List<TextList.Word>();
        isCurrentlyExecuting = false;
    }

    //Updates every frame
    void Update()
    {
        if (!isCurrentlyExecuting)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("enter was pressed");
            compareInput();
        }

        if (currentGameSequence.userScore >= currentGameSequence.wordListData.pointsNeeded)
        {
            listNextEvent.Invoke();
        }
    }

    //Gets the user's input from the textbox then checks if the user's word was used before and if it's on the list
    //If it's on the list and wasn't used before, the score is calculated
    public void compareInput()
    {
        bool wasFound = false;
        foreach (var userWord in currentGameSequence.wordListData.textList)
        {
            foreach (var oldWord in currentGameSequence.usedWords)
            {
                if(currentGameSequence.inputField.text.Equals(oldWord.word, StringComparison.InvariantCultureIgnoreCase) )  //Word was already used
                {
                    wasFound = true;
                    break;
                }
            }
            if (!wasFound && currentGameSequence.inputField.text.Equals(userWord.word, StringComparison.InvariantCultureIgnoreCase) ) //Word found and unused
            {
                calcScore(userWord);
                currentGameSequence.usedWords.Add(userWord);
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

        currentGameSequence.userScore += temp;
    }

    public void executeSequence(MinigameSequence targetMinigame)
    {
        currentGameSequence = targetMinigame;
    }
}
