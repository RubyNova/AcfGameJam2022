using System.Collections.Generic;
using ACHNarrativeDriver.Editor.Api;
using ACHNarrativeDriver.ScriptableObjects;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ACHNarrativeDriver.Editor
{
    public class NarrativeSequenceEditor : EditorWindow
    {
        private NarrativeSequence _currentNarrativeSequence;
        private Interpreter _interpreter = new();
        private bool _currentChoicesValue = false;
        private bool _firstRead = true;

        private void OnGUI()
        {
            GUILayout.Label("Narrative Sequence Editor", EditorStyles.boldLabel);
            _currentNarrativeSequence = (NarrativeSequence)EditorGUILayout.ObjectField("Target",
                _currentNarrativeSequence, typeof(NarrativeSequence), false);

            if (_currentNarrativeSequence is null)
            {
                _currentChoicesValue = false;
                _firstRead = true;
                return;
            }

            if (_firstRead)
            {
                _firstRead = false;
                if (_currentNarrativeSequence.Choices.Count > 0)
                {
                    _currentChoicesValue = true;
                }
            }

            GUILayout.Label("Source Script", EditorStyles.label);
            var previousSourceScript = _currentNarrativeSequence.SourceScript;
            _currentNarrativeSequence.SourceScript = GUILayout.TextArea(_currentNarrativeSequence.SourceScript,
                GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));


            _currentChoicesValue = GUILayout.Toggle(_currentChoicesValue, "Has Choices");

            bool nextNarrativeSequenceModified = false;
            if (_currentChoicesValue)
            {
                for (var index = 0; index < _currentNarrativeSequence.Choices.Count; index++)
                {
                    var choice = _currentNarrativeSequence.Choices[index];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Choice text");
                    var previousText = choice.ChoiceText;
                    choice.ChoiceText = GUILayout.TextField(choice.ChoiceText);
                    GUILayout.EndHorizontal();
                    var previousResponse = choice.NarrativeResponse;
                    choice.NarrativeResponse = (NarrativeSequence)EditorGUILayout.ObjectField(
                                                                   "Narrative Response", choice.NarrativeResponse, typeof(NarrativeSequence),
                                                                   false);
                    if (previousText != choice.ChoiceText || previousResponse != choice.NarrativeResponse)
                    {
                        nextNarrativeSequenceModified = true;
                    }
                }

                if (GUILayout.Button("Add new"))
                {
                    _currentNarrativeSequence.Choices.Add(new());
                    nextNarrativeSequenceModified = true;
                }

                if (GUILayout.Button("Remove last"))
                {
                    _currentNarrativeSequence.Choices.RemoveAt(_currentNarrativeSequence.Choices.Count - 1);
                    nextNarrativeSequenceModified = true;
                }
            }
            else
            {
                _currentNarrativeSequence.Choices.Clear();
                var previousSequence = _currentNarrativeSequence.NextSequence;
                _currentNarrativeSequence.NextSequence = (NarrativeSequence)EditorGUILayout.ObjectField(
                    "Next Narrative Sequence", _currentNarrativeSequence.NextSequence, typeof(NarrativeSequence),
                    false);

                if (previousSequence != _currentNarrativeSequence.NextSequence)
                {
                    nextNarrativeSequenceModified = true;
                }
            }

            bool compiledScriptChanged = false;
            if (GUILayout.Button("Save Source Script"))
            {
                compiledScriptChanged = true;
                var listOfStuff = _interpreter.Interpret(_currentNarrativeSequence.SourceScript);
                _currentNarrativeSequence.CharacterDialoguePairs = listOfStuff;
            }

            if (_currentNarrativeSequence.SourceScript != previousSourceScript || nextNarrativeSequenceModified || compiledScriptChanged)
            {
                EditorUtility.SetDirty(_currentNarrativeSequence);
            }
        }

        [MenuItem("Window / ACH Narrative Driver / Narrative Sequence Editor")]
        public static void ShowEditor()
        {
            var window = EditorWindow.GetWindow<NarrativeSequenceEditor>(title: "Narrative Sequence Editor");
            window.minSize = new Vector2(500, 500);
        }
    }
}