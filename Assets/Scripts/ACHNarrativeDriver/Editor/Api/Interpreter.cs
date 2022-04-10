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

            if (!sourceScript.Contains(Environment.NewLine))
            {
                var newLineChar = Environment.NewLine;

                if (sourceScript.Contains("\n"))
                {
                    newLineChar = "\n";
                }
                else if (sourceScript.Contains("\r"))
                {
                    newLineChar = "\r";
                }
                else if (sourceScript.Contains("\r\n"))
                {
                    newLineChar = "\r\n";
                }
                
                sourceScript = sourceScript.Replace(newLineChar, Environment.NewLine);
            }
            
            var sourceSplit = sourceScript.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            
            for (var index = 0; index < sourceSplit.Length; index++)
            {
                var line = sourceSplit[index];
                var splitLines = line.Split(": ", StringSplitOptions.RemoveEmptyEntries);

                if (splitLines.Length <= 1)
                {
                    throw new FormatException($"Invalid narrative script was provided to the interpreter. Invalid line number: {index + 1}");
                }
                
               var characterName = splitLines[0];
               var character = characterAssets.FirstOrDefault(x => x.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase));

               if (character == null)
               {
                   throw new FileNotFoundException(
                       $"The character {characterName} cannot be found in the asset database. Please ensure the character has been created and that the name has been spelt correctly. Invalid line number: {index + 1}");
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
                       $"Character Pose Index was outside the bounds of the Poses collection. Length: {character.Poses.Count}, Index: {poseIndex.Value}");
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
