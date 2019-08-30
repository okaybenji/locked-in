using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainController : MonoBehaviour {
  public bool GameIsPaused = true;
  public GameObject menu;
  public GameObject mainMenu;
  public GameObject settingsMenu;
  public GameObject game;
  public GameObject npc;
  public AudioMixer audioMixer;
  public TMP_Dropdown resolutionDropdown;
  Resolution[] resolutions;
  AudioSource npcAudio;
  
  void Start() {
    npcAudio = npc.GetComponent<AudioSource>();
    updateResolutionList();
  }
  
  void Update() {
    // TODO: Should be able to remove this to allow pressing escape to get back to the main menu.
    // That works, except the menu freezes.
    // This is similar to, but the opposite of, the problem I had before where pressing the
    // Play button froze the menu, but pressing escape worked fine??
    // (Fixed this by removing the button altogether!)
    // Another option would be to just let escape toggle the menu, whether Settings or Main
    // instead of always returning to Main before hiding the menu.
    // The way it is now sucks, because if you DO press escape in the menu, your cursor disappears.
    if (settingsMenu.activeInHierarchy) {
      return;
    }
    
    if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return)) {
      BackOut();
    }
  }
  
  // Back out of whatever you're in
  public void BackOut() {
    // If you're in the menu...
    if (GameIsPaused) {
      if (mainMenu.activeInHierarchy) {
        // Resume the game.
        Resume();
      } else {
        // Return to the main menu.
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
      }
    } else {
      // If you're in the game...
      Pause();
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

  void Resume() {
    if (!game.activeInHierarchy) {
      game.SetActive(true);
    }
    GameIsPaused = false;
    Time.timeScale = 1f;
    npcAudio.UnPause();
    menu.SetActive(false);
  }
  
  // TODO: I would love to start with the game active but paused!
  // But apparently Unity fades in the game and UI when you activate the game?
  // And setting timeScale to 0 at the start would cause it to never fade in.
  // It just stays black forever. WTF is that about?
  // Also if you do pause the game while it's fading in, the UI will get stuck
  // only partly faded in. WTF is that about???
  void Pause() {
    menu.SetActive(true);
    GameIsPaused = true;
    mainMenu.SetActive(true);
    settingsMenu.SetActive(false);
    npcAudio.Pause();
    Time.timeScale = 0f;
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
