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
    [SerializeField] private Slider volumeSlider;

    private int volume;

    void Start()
    {
        currentBG.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        currentBG.sprite = mainMenuBG;
        volume = 5;
        optionsMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);
    }

    void Update()
    {
        volume = (int) volumeSlider.value;
        volumeDisplay.text = volume.ToString();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OptionsMenu()
    {
        currentBG.color = new Color(0.2399788f, 0.1243681f, 0.4811321f, 1.0f);
        currentBG.sprite = optionsMenuBG;
        optionsMenuObject.SetActive(true);
        mainMenuObject.SetActive(false);
    }

    public void BackToMainMenu()
    {
        currentBG.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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
