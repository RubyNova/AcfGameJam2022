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
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Image characterRenderer;
    [SerializeField] private Image backgroundRenderer;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] private Slider slider;
    
    private bool isCurrentlyExecuting;
    private MinigameSequence currentGameSequence;
    private bool wasFound;
    private float correctTimer;
    private bool wasWrong;
    private float wrongTimer;

    //Initialization
    void Start()
    {
        currentGameSequence.userScore = 0;
        currentGameSequence.usedWords = new List<MinigameSequence.TextList.Word>();
        scoreDisplay.text = "0/" + currentGameSequence.wordListData.pointsNeeded.ToString();
        slider.value = 0;
        slider.maxValue = currentGameSequence.wordListData.pointsNeeded;
    }

    void Awake()
    {
        isCurrentlyExecuting = false;
        wasFound = false;
        correctTimer = 0.0f;
        wrongTimer = 0.0f;
    }

    //Updates every frame
    void Update()
    {
        if (!isCurrentlyExecuting)
        {
            Debug.Log("isCurrentlyExecuting: " + isCurrentlyExecuting);
            return;
        }

        if (wasFound)
        {
            if (correctTimer < 1.5f)
            {
                characterRenderer.GetComponent<Image>().color = new Color((correctTimer%0.5f) * 2.0f, 1.0f, (correctTimer%0.5f) * 2.0f, 1.0f);
                correctTimer += Time.deltaTime;
            }
            else
            {
                correctTimer = 0.0f;
                wasFound = false;
                characterRenderer.sprite = currentGameSequence.character.Poses[currentGameSequence.basePoseIndex];
                characterRenderer.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        if (wasWrong)
        {
            if (wrongTimer < 1.0f)
            {
                characterRenderer.GetComponent<Image>().color = new Color((0.75f + wrongTimer*0.25f), wrongTimer, wrongTimer, 1.0f);
                wrongTimer += Time.deltaTime;
            }
            else
            {
                wrongTimer = 0.0f;
                wasWrong = false;
                characterRenderer.sprite = currentGameSequence.character.Poses[currentGameSequence.basePoseIndex];
                characterRenderer.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("enter was pressed");
            compareInput();
        }*/
    }

    //Gets the user's input from the textbox then checks if the user's word was used before and if it's on the list
    //If it's on the list and wasn't used before, the score is calculated
    public void compareInput()
    {
        if (!inputField.text.Equals(""))
        {
            foreach (var listWord in currentGameSequence.wordListData.textList)
            {
                foreach (var oldWord in currentGameSequence.usedWords)
                {
                    if(inputField.text.Equals(oldWord.word, StringComparison.InvariantCultureIgnoreCase) )  //Word was already used
                    {
                        Debug.Log(inputField.text + " was already used");
                        wasWrong = true;
                        characterRenderer.sprite = currentGameSequence.character.Poses[currentGameSequence.wordFailPoseIndex];
                        return;
                    }
                }
                if (inputField.text.Equals(listWord.word, StringComparison.InvariantCultureIgnoreCase) ) //Word found and unused
                {
                    Debug.Log(inputField.text + " was FOUND");
                    wasFound = true;
                    correctTimer = 0;
                    characterRenderer.sprite = currentGameSequence.character.Poses[currentGameSequence.wordSuccessPoseIndex];
                    currentGameSequence.usedWords.Add(listWord);
                    calcScore(listWord);
                    return;
                }
            }
            Debug.Log(inputField.text + " was NOT found"); //Word isn't on the list of acceptable words
            wasWrong = true;
            characterRenderer.sprite = currentGameSequence.character.Poses[currentGameSequence.wordFailPoseIndex];
        }
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
        SliderValueChange();
        if (currentGameSequence.userScore >= currentGameSequence.wordListData.pointsNeeded)
        {
            currentGameSequence.userScore = currentGameSequence.wordListData.pointsNeeded;
            Debug.Log("Score met!!!");
            //Do success animations here
            listNextEvent.Invoke();
        }
    }

    public void executeSequence(MinigameSequence targetMinigame)
    {
        currentGameSequence = targetMinigame;
        backgroundRenderer.sprite = targetMinigame.backgroundSprite;
        characterRenderer.sprite = currentGameSequence.character.Poses[currentGameSequence.basePoseIndex];
        backgroundRenderer.enabled = true;
        characterRenderer.enabled = true;
        isCurrentlyExecuting = true;
        Debug.Log("isCurrentlyExecuting: " + isCurrentlyExecuting);
    }
    
    public void SliderValueChange()
    {
        scoreDisplay.text = currentGameSequence.userScore.ToString() + "/" + currentGameSequence.wordListData.pointsNeeded.ToString();
        slider.value = currentGameSequence.userScore;
        Debug.Log("slider.value: " + slider.value);
    }

}
