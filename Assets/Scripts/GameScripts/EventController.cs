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
        [SerializeField] public MinigameTextInput minigameSeq;
        public bool pointToMini;
    }

    public List<EventData> eventDataList;
    public ACHNarrativeDriver.ScriptableObjects.NarrativeSequence finalNarrativeSeq;

    private int listPosition;

    void Start()
    {
        listPosition = 0;
    }

    public void listNext()
    {
        //if the event was pointing to a minigame, it goes to the next one on the list
        if ( eventDataList[listPosition].pointToMini )
        {
            listPosition++;
        }
        //Swap between minigame and narrative
        eventDataList[listPosition].pointToMini = !eventDataList[listPosition].pointToMini;
    }
}
