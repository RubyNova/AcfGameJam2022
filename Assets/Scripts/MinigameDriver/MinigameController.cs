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
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Image characterRenderer;
    [SerializeField] private Image backgroundRenderer;
    
    private bool isCurrentlyExecuting;

    //Initialization
    void Start()
    {
        currentGameSequence.userScore = 0;
        currentGameSequence.usedWords = new List<MinigameSequence.TextList.Word>();
        isCurrentlyExecuting = false;
    }

    //Updates every frame
    void Update()
    {
        if (!isCurrentlyExecuting)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("enter was pressed");
            compareInput();
        }
    }

    //Gets the user's input from the textbox then checks if the user's word was used before and if it's on the list
    //If it's on the list and wasn't used before, the score is calculated
    public void compareInput()
    {
        bool wasUsed = false;
        foreach (var listWord in currentGameSequence.wordListData.textList)
        {
            foreach (var oldWord in currentGameSequence.usedWords)
            {
                if(inputField.text.Equals(oldWord.word, StringComparison.InvariantCultureIgnoreCase) )  //Word was already used
                {
                    Debug.Log(inputField.text + " was already used");
                    wasUsed = true;
                    return;
                }
            }
            if (!wasUsed && inputField.text.Equals(listWord.word, StringComparison.InvariantCultureIgnoreCase) ) //Word found and unused
            {
                Debug.Log(inputField.text + " was FOUND");
                calcScore(listWord);
                currentGameSequence.usedWords.Add(listWord);
                return;
            }
        }
        Debug.Log(inputField.text + " was NOT found"); //Word isn't on the list of acceptable words
    }

    //Calculate the value of the word input and add it to the current userScore
    public void calcScore(MinigameSequence.TextList.Word userWord)
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
        Debug.Log("Current Score: " + currentGameSequence.userScore + "/" + currentGameSequence.wordListData.pointsNeeded);
        if (currentGameSequence.userScore >= currentGameSequence.wordListData.pointsNeeded)
        {
            currentGameSequence.userScore = currentGameSequence.wordListData.pointsNeeded;
            Debug.Log("Score met!!!");
            listNextEvent.Invoke();
        }
    }

    public void executeSequence(MinigameSequence targetMinigame)
    {
        currentGameSequence = targetMinigame;
    }
}
