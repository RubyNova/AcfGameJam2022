using System;
using System.Collections.Generic;
using UnityEngine;

namespace ACHNarrativeDriver.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewNarrativeSequence", menuName = "ACH Narrative Driver / NarrativeSequence")]
    public class NarrativeSequence : ScriptableObject
    {
        [Serializable]
        public class CharacterDialogueInfo
        {
            [SerializeField] private Character _character;
            [SerializeField] private bool _hasPoseIndex;
            [SerializeField] private int _poseIndex;
            [SerializeField] private string _text;

            public Character Character
            {
                get => _character;
                set => _character = value;
            }

            public int? PoseIndex
            {
                get => _hasPoseIndex ? _poseIndex : null;
                set
                {
                    if (value.HasValue)
                    {
                        _poseIndex = value.Value;
                        _hasPoseIndex = true;
                    }
                    else
                    {
                        _hasPoseIndex = false;
                    }
                }
            }

            public string Text
            {
                get => _text;
                set => _text = value;
            }

            public override string ToString()
            {
                return $"Character: {Character.Name}, HasPoseIndex: {_hasPoseIndex}, {(_hasPoseIndex ? "PoseIndex: " + PoseIndex + ", " : string.Empty)}Text: {Text}";
            }
        }
        
        [SerializeField] private NarrativeSequence _nextSequence;
        [SerializeField] private List<CharacterDialogueInfo> _characterDialoguePairs;

        public NarrativeSequence NextSequence
        {
            get => _nextSequence;
            set => _nextSequence = value;
        }

        public List<CharacterDialogueInfo> CharacterDialoguePairs
        {
            get => _characterDialoguePairs;
            set => _characterDialoguePairs = value;
        }

        public string SourceScript { get; set; }
    }
}
