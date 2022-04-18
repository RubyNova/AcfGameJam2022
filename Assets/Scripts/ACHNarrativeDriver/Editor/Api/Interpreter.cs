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
        public List<NarrativeSequence.CharacterDialogueInfo> Interpret(string sourceScript,
            PredefinedVariables predefinedVariables)
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
                characterName = ResolvePredefinedVariables(characterName, predefinedVariables);

                if (splitLines.Length > 1 && !string.IsNullOrWhiteSpace(characterName) &&
                    !characterName.All(char.IsNumber))
                {
                    character = characterAssets.FirstOrDefault(x =>
                        x.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase));
                }

                if (character is null)
                {
                    throw new FileNotFoundException(
                        $"The character {(string.IsNullOrWhiteSpace(characterName) ? "NO_CHARACTER_NAME" : characterName)} cannot be found in the asset database. Please ensure the character has been created and that the name has been spelt correctly. Line number: {index + 1}");
                }

                var poseIndexString = splitLines.FirstOrDefault(x => x.All(char.IsNumber));
                int? poseIndex = null;

                if (!string.IsNullOrWhiteSpace(poseIndexString))
                {
                    poseIndexString = ResolvePredefinedVariables(poseIndexString, predefinedVariables);
                    poseIndex = int.Parse(poseIndexString);
                }

                if (poseIndex.HasValue && poseIndex.Value >= character.Poses.Count)
                {
                    throw new IndexOutOfRangeException(
                        $"Character Pose Index was outside the bounds of the Poses collection. Length: {character.Poses.Count}, Index: {poseIndex.Value}. Line number: {index + 1}");
                }

                var text = splitLines.Last();
                text = ResolvePredefinedVariables(text, predefinedVariables);

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

        public string ResolvePredefinedVariables(string targetString, PredefinedVariables variables)
        {
            if (variables is null)
            {
                return targetString;
            }

            var unresolvedVariables = targetString.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Where(x => x[0] == '$').Select(x => x.Replace("$", string.Empty));

            foreach (var variable in unresolvedVariables)
            {
                var variableValue = variables.Variables.FirstOrDefault(x => x.Key == variable);
                
                if (variableValue is null)
                {
                    continue;
                }

                targetString = targetString.Replace($"${variable}", variableValue.Value);
            }

            return targetString;
        }
    }
}