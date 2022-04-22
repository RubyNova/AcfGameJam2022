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
    
    private bool pointToMini;
    private int listPosition;

    void Start()
    {
        listPosition = 0;
        pointToMini = false;
        narrativeController.ExecuteSequence(eventDataList[0].narrativeSeq);
    }

    public void listNext()
    {
        //if the event was pointing to a minigame, it goes to the next one on the list
        if (pointToMini)
        {
            listPosition++;
        }
        //Swap between minigame and narrative
        pointToMini = !pointToMini;

        if (pointToMini)
        {
            minigameController.executeSequence(eventDataList[listPosition].minigameSeq);
            return;
        }
        else
        {
            narrativeController.ExecuteSequence(eventDataList[listPosition].narrativeSeq);
            return;
        }
    }
}
