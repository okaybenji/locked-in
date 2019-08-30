using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  public GameObject game;
  public GameObject menu;
  
  void Update() {
    // Resume game
    if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Return)) {
      game.SetActive(true);
      menu.SetActive(false);
    }
  }
    
  public void PlayGame() {
    SceneManager.LoadScene("Game");
  }
  
  public void QuitGame() {
    Application.Quit();
  }
}
