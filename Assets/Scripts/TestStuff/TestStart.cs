using ACHNarrativeDriver;
using ACHNarrativeDriver.ScriptableObjects;
using UnityEngine;

public class TestStart : MonoBehaviour
{
    [SerializeField] private NarrativeUIController _target;
    [SerializeField] private NarrativeSequence _sequence;
    
    // Start is called before the first frame update
    void Start() => _target.ExecuteSequence(_sequence);
}
