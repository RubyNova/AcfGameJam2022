using System;
using System.Collections.Generic;
using ACHNarrativeDriver.ScriptableObjects;
using UnityEngine;

namespace ACHNarrativeDriver
{
    public class RuntimeVariablesController : MonoBehaviour
    {
        [SerializeField] private PredefinedVariables _predefinedVariables;

        private Dictionary<string, string> _variables;

        private void Awake()
        {
            if (_predefinedVariables == null)
            {
                return;
            }
            
            foreach (var variable in _predefinedVariables.Variables)
            {
                _variables.Add(variable.Key, variable.Value);
            }
        }

        public void AddVariable(string key, string value) => _variables.Add(key, value);
        public string GetVariableValue(string key) => _variables[key];
    }
}
