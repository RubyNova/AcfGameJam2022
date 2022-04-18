using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextInputTest : MonoBehaviour
{
    public TextMeshProUGUI userInputDisplay;
    public TextMeshProUGUI isFoundDisplay;
    public TextMeshProUGUI scoreTrackerDisplay;
    public TextMeshProUGUI promptDisplay;
    public TMP_InputField inputField;
    public TextList wordListData;
    private List<TextList.Word> usedWords;
    private int userScore;

    //Initialization

    void Start()
    {
        promptDisplay.text = wordListData.prompt;
        isFoundDisplay.text = "";
        userScore = 0;
        usedWords = new List<TextList.Word>();
    }

    //Updates every frame

    void Update()
    {
        scoreTrackerDisplay.text = "User Score: " + userScore + "\nNeeded Score: " + wordListData.pointsNeeded;
        if(userScore >= wordListData.pointsNeeded)
        {
            scoreTrackerDisplay.text = "Success!!!";
            //scoreMet();
        }
    }

    //Gets the user's input from the textbox

    public void setUserInput()
    {
        userInputDisplay.text = "User Input: " + inputField.text.ToLower();
    }

    //Checks if the user's word was used before and if it's on the list
    //If it's on the list and wasn't used before, the score is calculated

    public void compareInput()
    {
        bool wasFound = false;
        isFoundDisplay.text = "Word is NOT found";
        foreach (var userWord in wordListData.textList)
        {
            foreach (var oldWord in usedWords)
            {
                if(inputField.text.Equals(oldWord.word, StringComparison.InvariantCultureIgnoreCase) )
                {
                    isFoundDisplay.text = "Word was already used";
                    wasFound = true;
                    break;
                }
            }
            if (!wasFound && inputField.text.Equals(userWord.word, StringComparison.InvariantCultureIgnoreCase) )
            {
                isFoundDisplay.text = "Word IS found\nWord value is " + calcScore(userWord);
                usedWords.Add(userWord);
                break;
            }
        }
    }

    //Calculate the value of the word input and add it to the current userScore
    public int calcScore(TextList.Word userWord)
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
        return temp;
    }

    /*public void scoreMet()
    {
        
    }
    */
}
