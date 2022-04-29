using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private GameObject optionsMenuObject;
    [SerializeField] private TextMeshProUGUI volumeDisplay;
    [SerializeField] private Image currentBG;
    [SerializeField] private Sprite mainMenuBG;
    [SerializeField] private Sprite optionsMenuBG;

    private int volume;

    void Start()
    {
        currentBG.sprite = mainMenuBG;
        currentBG.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
        currentBG.sprite = optionsMenuBG;
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
        currentBG.sprite = mainMenuBG;
        optionsMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public int GetVolume()
    {
        return volume;
    }
}
