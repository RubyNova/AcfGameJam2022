using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private GameObject optionsMenuObject;
    [SerializeField] private TextMeshProUGUI volumeDisplay;

    private int volume;

    void Start()
    {
        volume = 5;
        optionsMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);
    }

    void Update()
    {
        volumeDisplay.text = volume.ToString();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OptionsMenu()
    {
        optionsMenuObject.SetActive(true);
        mainMenuObject.SetActive(false);
    }

    public void RaiseVolume()
    {
        if (volume < 10)
            volume++;
    }
    public void LowerVolume()
    {
        if (volume > 0)
            volume--;
    }

    public void BackToMainMenu()
    {
        optionsMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
