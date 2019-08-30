using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour {
  public GameObject mainMenu;
  public GameObject settingsMenu;
  public AudioMixer audioMixer;
  public TMP_Dropdown resolutionDropdown;
  Resolution[] resolutions;
  
  void Start() {
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
  
  void Update() {
    // Return to main menu
    if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Return)) {
      mainMenu.SetActive(true);
      settingsMenu.SetActive(false);
    }
  }
  
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
