using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainController : MonoBehaviour {
  public GameObject mainMenu;
  public GameObject settingsMenu;
  public GameObject canvas;
  public GameObject game;
  public GameObject npc;
  public AudioMixer audioMixer;
  public TextMeshProUGUI playButton;
  public TMP_Dropdown resolutionDropdown;
  Resolution[] resolutions;
  AudioSource npcAudio;

  public string state = "mainMenu";

  void Start() {
    npcAudio = npc.GetComponent<AudioSource>();
    updateResolutionList();
    // Pause the game
    Time.timeScale = 0f;
    npcAudio.Pause();
  }

  void ActivateMainMenu() {
    state = "mainMenu";

    // Pause the game
    Time.timeScale = 0f;
    npcAudio.Pause();

    // Show the UI canvas
    canvas.GetComponent<Canvas>().enabled = true;

    // Show the main menu
    mainMenu.SetActive(true);

    // Hide the settings menu
    settingsMenu.SetActive(false);
  }

  public void ActivateSettingsMenu() {
    state = "settingsMenu";

    // Show the settings menu
    settingsMenu.SetActive(true);
    // Hide the main menu
    mainMenu.SetActive(false);
  }

  void ActivateGame() {
    state = "game";

    // Hide the main menu
    mainMenu.SetActive(false);

    // Hide the UI canvas
    canvas.GetComponent<Canvas>().enabled = false;

    // Unpause the game
    Time.timeScale = 1.0f;
    npcAudio.UnPause();

    // In case this is the first time activating the game, change menu text.
    playButton.text = "Resume";
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      BackOut();
    }
  }

  // Back out of whatever you're in
  public void BackOut() {
    if (state == "mainMenu") {
      ActivateGame();
    } else {
      // Backing out of either the game or the settings menu takes you to the main menu.
      ActivateMainMenu();
    }
  }

  void updateResolutionList() {
    resolutions = Screen.resolutions;
    resolutionDropdown.ClearOptions();

    List<string> options = new List<string>();

    int currentResolutionIndex = 0;
    for (int i = 0; i < resolutions.Length; i++) {
      string option = resolutions[i].width + " x " + resolutions[i].height;
      options.Add(option);

      // Default to screen resolution.
      if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
        currentResolutionIndex = i;
      }
    }

    resolutionDropdown.AddOptions(options);
    resolutionDropdown.value = currentResolutionIndex;
    resolutionDropdown.RefreshShownValue();
  }

  public void Quit() {
    Application.Quit();
  }

  // Settings methods
  public void SetVolume(float volume) {
    audioMixer.SetFloat("Volume", volume);
  }

  public void SetQuality(int index) {
    QualitySettings.SetQualityLevel(index);
  }

  public void SetFullscreen(bool isFullscreen) {
    Screen.fullScreen = isFullscreen;
  }

  public void SetResolution(int index) {
    Resolution resolution = resolutions[index];
    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
  }
}
