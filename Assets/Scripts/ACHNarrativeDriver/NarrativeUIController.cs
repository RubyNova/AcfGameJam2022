using System;
using System.Collections;
using System.Text;
using ACHNarrativeDriver.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ACHNarrativeDriver
{
    public class NarrativeUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _narrativeTextBox;
        [SerializeField] private TMP_Text _characterNameTextBox;

        private Coroutine _rollingTextRoutine;
        private readonly WaitForSeconds _rollingCharacterTime = new(0.04f);
        private NarrativeSequence _currentNarrativeSequence;
        private bool _isCurrentlyExecuting;
        private bool _nextDialogueLineRequested;
        private int _currentDialogueIndex;

        private void Awake()
        {
            _isCurrentlyExecuting = false;
            _currentDialogueIndex = 0;
        }

        private void Update()
        {
            if (!_isCurrentlyExecuting || (/*_rollingTextRoutine is not null ||*/ !_nextDialogueLineRequested))
            {
                return;
            }

            if (_currentDialogueIndex >= _currentNarrativeSequence.CharacterDialoguePairs.Count)
            {
                _currentDialogueIndex = 0;
                
                if (_currentNarrativeSequence.NextSequence is null)
                {
                    _isCurrentlyExecuting = false;
                    _currentNarrativeSequence = null;
                    return;
                }
                else
                {
                    _currentNarrativeSequence = _currentNarrativeSequence.NextSequence;
                }
            }
            
            _nextDialogueLineRequested = false;

            if (_rollingTextRoutine is not null)
            {
                ResetRollingTextRoutine();
                _narrativeTextBox.text =
                    _currentNarrativeSequence.CharacterDialoguePairs[_currentDialogueIndex - 1].Text;
                return;
            }
            
            _rollingTextRoutine =
                StartCoroutine(
                    PerformRollingText(_currentNarrativeSequence.CharacterDialoguePairs[_currentDialogueIndex]));
            _currentDialogueIndex++;
        }

        private void ResetRollingTextRoutine()
        {
            StopCoroutine(_rollingTextRoutine);
            _rollingTextRoutine = null;
        }

        private IEnumerator PerformRollingText(NarrativeSequence.CharacterDialogueInfo targetDialogueInfo)
        {
            StringBuilder sb = new();
            _characterNameTextBox.text = targetDialogueInfo.Character.Name;

            foreach (var character in targetDialogueInfo.Text)
            {
                sb.Append(character);
                _narrativeTextBox.text = sb.ToString();
                yield return _rollingCharacterTime;
            }

            _rollingTextRoutine = null;
        }

        public void ExecuteSequence(NarrativeSequence targetSequence)
        {
            if (_rollingTextRoutine is not null)
            {
                ResetRollingTextRoutine();
            }

            _narrativeTextBox.text = string.Empty;
            _characterNameTextBox.text = string.Empty;
            _currentNarrativeSequence = targetSequence;
            _nextDialogueLineRequested = true;
            _isCurrentlyExecuting = true;
        }

        public void ExecuteNextDialogueLine()
        {
            if (_currentNarrativeSequence is null)
            {
                return;
            }

            _nextDialogueLineRequested = true;
        }
    }
}