using System.Collections;
using System.Collections.Generic;
using AudioManagement;
using UnityEngine;

public class TestAudioPlay : MonoBehaviour
{
    [SerializeField] private AudioClip _testMusic;
    [SerializeField] private AudioClip _testSfx;
    [SerializeField] private AudioController _controller;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller.PlayMusic(_testMusic);
        _controller.PlayEffect(_testSfx);
    }
}
