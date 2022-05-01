using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using AudioManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private GameObject optionsMenuObject;
    [SerializeField] private GameObject namingMenuObject;
    [SerializeField] private ACHNarrativeDriver.Api.RuntimeVariables narrativeRuntimeVariableObject;
    [SerializeField] private TextMeshProUGUI volumeDisplay;
    [SerializeField] private Image currentBG;
    [SerializeField] private Sprite mainMenuBG;
    [SerializeField] private Sprite sideMenuBG;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioController audioController; // I'd normally use underscore prefix but this is not my file
    [SerializeField] private TMP_InputField inputField;
    
    void Start()
    {
        inputField.text = "";
        currentBG.color = new Color(0.9150943f, 0.6681915f, 0.6707927f, 1.0f);
        currentBG.sprite = mainMenuBG;
        optionsMenuObject.SetActive(false);
        namingMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);
        volumeSlider.value = audioController.MusicVolume;
        OnVolumeSliderValueChanged(); // This is needed because UNITY UBER S U C C C C C C C
    }

    public void UpdateNameAndStartGame()
    {
        narrativeRuntimeVariableObject.UpdateVariable("player", inputField.text);
        SceneManager.LoadScene("GameScene");
    }

    public void StartGame()
    {
        currentBG.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        currentBG.sprite = sideMenuBG;
        optionsMenuObject.SetActive(false);
        namingMenuObject.SetActive(true);
        mainMenuObject.SetActive(false);
    }

    public void OptionsMenu()
    {
        currentBG.color = new Color(0.2399788f, 0.1243681f, 0.4811321f, 1.0f);
        currentBG.sprite = sideMenuBG;
        optionsMenuObject.SetActive(true);
        namingMenuObject.SetActive(false);
        mainMenuObject.SetActive(false);
    }

    public void BackToMainMenu()
    {
        currentBG.color = new Color(0.9150943f, 0.6681915f, 0.6707927f, 1.0f);
        currentBG.sprite = mainMenuBG;
        optionsMenuObject.SetActive(false);
        namingMenuObject.SetActive(false);
        mainMenuObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnVolumeSliderValueChanged()
    {
        volumeDisplay.text = ((int)(volumeSlider.value * 100)).ToString(CultureInfo.InvariantCulture);
    }
}
