using System;
using UnityEditor;
using UnityEngine;

namespace ACHNarrativeDriver.Editor
{
    public class NarrativeSequenceEditor : EditorWindow
    {
        private void OnGUI()
        {
            GUILayout.Label("Hello world", EditorStyles.boldLabel);

            if (GUILayout.Button("Test", GUILayout.MinWidth(500), GUILayout.MinHeight(500)))
            {
                
            }
        }

        [MenuItem("Window / ACH Narrative Driver / Narrative Sequence Editor")]
        public static void ShowEditor()
        {
            var window = EditorWindow.GetWindow<NarrativeSequenceEditor>();
            window.minSize = new Vector2(500, 500);
        }
    }
}
