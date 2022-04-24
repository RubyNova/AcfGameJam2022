using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventController : MonoBehaviour
{
    [Serializable]
    public class EventData
    {
        [SerializeField] public ACHNarrativeDriver.ScriptableObjects.NarrativeSequence narrativeSeq;
        [SerializeField] public MinigameSequence minigameSeq;
    }

    public List<EventData> eventDataList;
    public ACHNarrativeDriver.ScriptableObjects.NarrativeSequence finalNarrativeSeq;
    public ACHNarrativeDriver.NarrativeUIController narrativeController;
    public MinigameController minigameController;
    public GameObject minigameSystem;
    public GameObject narrativeSystem;

    private bool pointToMini;
    private int listPosition;
    private bool pointinToEnd;

    void Start()
    {
        listPosition = 0;
        pointToMini = false;
        pointinToEnd = false;
        minigameSystem.SetActive(false);
        narrativeController.ExecuteSequence(eventDataList[0].narrativeSeq);
    }

    public void listNext()
    {
        //if the function was previously loading the final narrative sequence, unload the sequence stuff and load the ending scene stuff
        if (pointinToEnd)
        {
            minigameSystem.SetActive(false);
            narrativeSystem.SetActive(false);
            //Load ending scene stuff
            return;
        }

        //if the event was pointing to a minigame, listPosition points to the next duo on the list
        if (pointToMini)
        {
            listPosition++;
        }
        //Swap between minigame and narrative
        pointToMini = !pointToMini;

        //if the eventDataList finished being iterated through, run the finalNarrativeSeq sequence and exit the function
        if (listPosition == eventDataList.Capacity)
        {
            minigameSystem.SetActive(false);
            narrativeSystem.SetActive(true);
            pointinToEnd = true;
            narrativeController.ExecuteSequence(finalNarrativeSeq);
            return;
        }

        //if the eventDataList points to a minigame, run the run the minigameSeq sequence and exit the function
        if (pointToMini)
        {
            narrativeSystem.SetActive(false);
            minigameSystem.SetActive(true);
            minigameController.executeSequence(eventDataList[listPosition].minigameSeq);
            return;
        }
        //if the eventDataList points to a narration, run the run the narrativeSeq sequence and exit the function
        else
        {
            minigameSystem.SetActive(false);
            narrativeSystem.SetActive(true);
            narrativeController.ExecuteSequence(eventDataList[listPosition].narrativeSeq);
            return;
        }
    }
}
