using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACHNarrativeDriver.ScriptableObjects;
using UnityEditor;

namespace ACHNarrativeDriver.Editor.Api
{
    public class Interpreter
    {
        public List<NarrativeSequence.CharacterDialogueInfo> Interpret(string sourceScript)
        {
            var characterPaths = AssetDatabase.FindAssets("t:Character").Select(AssetDatabase.GUIDToAssetPath);
            var characterAssets = characterPaths.Select(AssetDatabase.LoadAssetAtPath<Character>);
            List<NarrativeSequence.CharacterDialogueInfo> returnList = new();
            
            // remove any invalid new line strings

            if (sourceScript.Contains("\r"))
            {
                sourceScript = sourceScript.Replace("\r", "\n");
            }

            var sourceSplit = sourceScript.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            Character character = null;

            for (var index = 0; index < sourceSplit.Length; index++)
            {
                var line = sourceSplit[index];
                var splitLines = line.Split(": ", StringSplitOptions.RemoveEmptyEntries);

                if (splitLines.Length >= 4)
                {
                    throw new FormatException(
                        $"Invalid narrative script was provided to the interpreter. {splitLines.Length} arguments were provided when the maximum is 3. Invalid line number: {index + 1}");
                }

                var characterName = splitLines[0];
                
                if (characterName.Contains("\n"))
                {
                    characterName = characterName.Replace("\n", string.Empty);
                }
                
                
                characterName = characterName.Replace(Environment.NewLine, string.Empty);
                if (splitLines.Length > 1 && !string.IsNullOrWhiteSpace(characterName) &&
                    !characterName.All(char.IsNumber))
                {
                    character = characterAssets.FirstOrDefault(x =>
                        x.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase));
                }

                if (character == null)
                {
                    throw new FileNotFoundException(
                        $"The character {(string.IsNullOrWhiteSpace(characterName) ? "NO_CHARACTER_NAME" : characterName)} cannot be found in the asset database. Please ensure the character has been created and that the name has been spelt correctly. Line number: {index + 1}");
                }

                var poseIndexString = splitLines.FirstOrDefault(x => x.All(char.IsNumber));
                int? poseIndex = null;

                if (!string.IsNullOrWhiteSpace(poseIndexString))
                {
                    poseIndex = int.Parse(poseIndexString);
                }

                if (poseIndex.HasValue && poseIndex.Value >= character.Poses.Count)
                {
                    throw new IndexOutOfRangeException(
                        $"Character Pose Index was outside the bounds of the Poses collection. Length: {character.Poses.Count}, Index: {poseIndex.Value}. Line number: {index + 1}");
                }

                var text = splitLines.Last();

                NarrativeSequence.CharacterDialogueInfo info = new()
                {
                    Character = character,
                    PoseIndex = poseIndex,
                    Text = text
                };

                returnList.Add(info);
            }

            return returnList;
        }
    }
}