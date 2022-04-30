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
    [SerializeField] private Image promptDisplayBG;
    [SerializeField] private TextMeshProUGUI promptDisplay;
    [SerializeField] private Transform usedWordsView;
    [SerializeField] private GameObject usedWordsPrefab;
    [SerializeField] private Sprite floraBG;
    [SerializeField] private Sprite emBG;
    
    private bool isCurrentlyExecuting;
    private MinigameSequence currentGameSequence;
    private bool wasFound;
    private float correctTimer;
    private bool wasWrong;
    private float wrongTimer;

    //Initialization
    void Start()
    {
        inputField.ActivateInputField();
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

        if(currentGameSequence.userScore >= currentGameSequence.wordListData.pointsNeeded)
        {
            inputField.DeactivateInputField();
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
                if (currentGameSequence.userScore >= currentGameSequence.wordListData.pointsNeeded)
                {
                    RunSuccess();
                }
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
            string temp = inputField.text;
            inputField.text = "";
            foreach (var listWord in currentGameSequence.wordListData.textList)
            {
                foreach (var oldWord in currentGameSequence.usedWords)
                {
                    if(temp.Equals(oldWord.word, StringComparison.InvariantCultureIgnoreCase) )  //Word was already used
                    {
                        Debug.Log(temp + " was already used");
                        wasWrong = true;
                        characterRenderer.sprite = currentGameSequence.character.Poses[currentGameSequence.wordFailPoseIndex];
                        return;
                    }
                }
                if (temp.Equals(listWord.word, StringComparison.InvariantCultureIgnoreCase) ) //Word found and unused
                {
                    Debug.Log(temp + " was FOUND");
                    wasFound = true;
                    correctTimer = 0;
                    characterRenderer.sprite = currentGameSequence.character.Poses[currentGameSequence.wordSuccessPoseIndex];
                    currentGameSequence.usedWords.Add(listWord);
                    SpawnPrefab(listWord);
                    calcScore(listWord);
                    return;
                }
            }
            Debug.Log(temp + " was NOT found"); //Word isn't on the list of acceptable words
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
        if (currentGameSequence.userScore >= currentGameSequence.wordListData.pointsNeeded)
        {
            currentGameSequence.userScore = currentGameSequence.wordListData.pointsNeeded;
        }
        SliderValueChange();
    }

    public void executeSequence(MinigameSequence targetMinigame)
    {
        Awake();
        currentGameSequence = targetMinigame;
        backgroundRenderer.sprite = targetMinigame.backgroundSprite;
        characterRenderer.sprite = currentGameSequence.character.Poses[currentGameSequence.basePoseIndex];
        backgroundRenderer.enabled = true;
        characterRenderer.enabled = true;
        isCurrentlyExecuting = true;
        promptDisplay.text = targetMinigame.wordListData.prompt;
        if (currentGameSequence.character.Name.Equals("Flora", StringComparison.InvariantCultureIgnoreCase))
        {
            promptDisplayBG.sprite = floraBG;
            promptDisplay.color = new Color(0.05681734f, 0.5283019f, 0.4487417f, 1.0f);
        }
        else
        {
            promptDisplayBG.sprite = emBG;
            promptDisplay.color = new Color(0.7169812f, 0.1281938f, 0.06358131f, 1.0f);
        }
        Debug.Log("isCurrentlyExecuting: " + isCurrentlyExecuting);
        currentGameSequence.userScore = 0;
        currentGameSequence.usedWords = new List<MinigameSequence.TextList.Word>();
        scoreDisplay.text = "0/" + currentGameSequence.wordListData.pointsNeeded.ToString(); //Mackie bimbo moment #28
        slider.value = 0;
        slider.maxValue = currentGameSequence.wordListData.pointsNeeded;
    }
    
    public void SliderValueChange()
    {
        scoreDisplay.text = currentGameSequence.userScore.ToString() + "/" + currentGameSequence.wordListData.pointsNeeded.ToString();
        slider.value = currentGameSequence.userScore;
        Debug.Log("slider.value: " + slider.value);
    }

    public void SpawnPrefab(MinigameSequence.TextList.Word oldWord)
    {
        var yes = Instantiate(usedWordsPrefab, usedWordsView);
        yes.GetComponent<WordRenderer>().RenderWord(oldWord.word);
        
        if (usedWordsView.childCount > 9)
        {
            Destroy(usedWordsView.GetChild(0).gameObject);
        }
    }

    public void DeletePrefabs()
    {
        foreach (Transform child in usedWordsView)
        {
            Destroy(child.gameObject);
        }
    }

    public void RunSuccess()
    {
        Debug.Log("Score met!!!");
        //Do success animations here
        DeletePrefabs();
        listNextEvent.Invoke();
    }

}
