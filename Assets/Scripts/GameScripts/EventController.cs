using System;
using System.Collections.Generic;
using ACHNarrativeDriver;
using ACHNarrativeDriver.ScriptableObjects;
using AudioManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventController : MonoBehaviour
{
    [Serializable]
    public class EventData
    {
        [SerializeField] public NarrativeSequence narrativeSeq;
        [SerializeField] public MinigameSequence minigameSeq;
        [SerializeField] public NarrativeSequence next;
    }
    
    public List<EventData> narrativeMinigameLookupData;
    public NarrativeUIController narrativeController;
    public MinigameController minigameController;
    public GameObject minigameSystem;
    public GameObject narrativeSystem;

    private bool pointToMini;
    private bool pointingToEnd;

    private AudioController _audioController;
    private Dictionary<NarrativeSequence, MinigameSequence> _minigameDictionary;
    private Dictionary<MinigameSequence, NarrativeSequence> _narrativeDictionary;
    private NarrativeSequence _currentNarrativeSequence;
    private MinigameSequence _currentMinigame;

    void Start()
    {
        _minigameDictionary = new();
        _narrativeDictionary = new();
        _currentNarrativeSequence = narrativeMinigameLookupData[0].narrativeSeq;
        _currentMinigame = narrativeMinigameLookupData[0].minigameSeq;

        foreach (var data in narrativeMinigameLookupData)
        {
            _minigameDictionary.Add(data.narrativeSeq, data.minigameSeq);

            if (data.next is not null)
            {
                _narrativeDictionary.Add(data.minigameSeq, data.next);
            }
        }
        
        pointToMini = false;
        pointingToEnd = false;
        minigameSystem.SetActive(false);
        narrativeController.ExecuteSequence(_currentNarrativeSequence);
        _audioController = FindObjectOfType<AudioController>();
    }

    public void listNext()
    {
        //if the function was previously loading the final narrative sequence, unload the sequence stuff and load the ending scene stuff
        if (pointingToEnd)
        {
        }

        if (pointToMini)
        {
            if (!_narrativeDictionary.TryGetValue(_currentMinigame, out _currentNarrativeSequence))
            {
                GoToCredits();
                return;
            }
        }
        else
        {
            _currentNarrativeSequence = narrativeController.LastPlayedSequence;
            if (!_minigameDictionary.TryGetValue(_currentNarrativeSequence, out _currentMinigame))
            {
                GoToCredits();
                return;
            }
        }
        
        //Swap between minigame and narrative
        pointToMini = !pointToMini;
        
        if (pointToMini)
        {
            if (_currentMinigame is null)
            {
                return;
            }
            
            narrativeSystem.SetActive(false);
            minigameSystem.SetActive(true);
            minigameController.executeSequence(_currentMinigame);
            return;
        }
        else
        {
            minigameSystem.SetActive(false);
            narrativeSystem.SetActive(true);
            narrativeController.ExecuteSequence(_currentNarrativeSequence);
            return;
        }

        void GoToCredits()
        {
            minigameSystem.SetActive(false);
            narrativeSystem.SetActive(false);
            _audioController.StopMusic();
            SceneManager.LoadScene(2); // credits index
        }
    }
}
