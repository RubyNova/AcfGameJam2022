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

            public Character Character => _character;
            public int? PoseIndex => _hasPoseIndex ?  _poseIndex : null;
            public string Text => _text;
        }

        [SerializeField] private List<CharacterDialogueInfo> _characterDialoguePairs;

        public IReadOnlyList<CharacterDialogueInfo> CharacterDialoguePairs => _characterDialoguePairs.AsReadOnly();
    }
}
