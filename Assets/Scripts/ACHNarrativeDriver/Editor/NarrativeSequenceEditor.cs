using ACHNarrativeDriver.Editor.Api;
using ACHNarrativeDriver.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace ACHNarrativeDriver.Editor
{
    public class NarrativeSequenceEditor : EditorWindow
    {
        private NarrativeSequence _currentNarrativeSequence;
        private Interpreter _interpreter = new();
        
        private void OnGUI()
        {
            GUILayout.Label("Narrative Sequence Editor", EditorStyles.boldLabel);
            _currentNarrativeSequence = (NarrativeSequence)EditorGUILayout.ObjectField("Target", _currentNarrativeSequence, typeof(NarrativeSequence), false);

            if (_currentNarrativeSequence == null)
            {
                return;
            }
            
            GUILayout.Label("Source Script", EditorStyles.label);
            var previousSourceScript = _currentNarrativeSequence.SourceScript;
            _currentNarrativeSequence.SourceScript = GUILayout.TextArea(_currentNarrativeSequence.SourceScript,
                GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            var previousSequence = _currentNarrativeSequence.NextSequence;
            _currentNarrativeSequence.NextSequence = (NarrativeSequence)EditorGUILayout.ObjectField("Next Narrative Sequence", _currentNarrativeSequence.NextSequence, typeof(NarrativeSequence), false);

            bool compiledScriptChanged = false;
            if (GUILayout.Button("Save Source Script"))
            {
                compiledScriptChanged = true;
                var listOfStuff = _interpreter.Interpret(_currentNarrativeSequence.SourceScript);
                _currentNarrativeSequence.CharacterDialoguePairs = listOfStuff;
            }

            if (_currentNarrativeSequence.SourceScript != previousSourceScript || _currentNarrativeSequence.NextSequence != previousSequence || compiledScriptChanged)
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
