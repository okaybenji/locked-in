using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Player Sound ended up being too specific. This is just the player control code.
public class PlayerController : MonoBehaviour {
  public GameObject main;
  public MainController mainController;
  public GameObject npc;
  public GameObject invisibleWallA;
  public GameObject invisibleWallB;
  public GameObject door;
  public GameObject stairs;
  public GameObject blackBox; // Mesh which blocks player's view of skybox until end.
  public GameObject lightFrame; // Frame of light around the door.
  public GameObject insideClosetLight; // To activate when player enters closet.

  public bool hasKey = false;

  public TextMeshProUGUI subtitles;
  bool showedFootstepsCaption = false;

  // Footsteps
  public AudioClip stepLeft1;
  public AudioClip stepLeft2;
  public AudioClip stepRight1;
  public AudioClip stepRight2;
  public float footstepDelay;

  private AudioClip walkSound;
  private float nextFootstep = 0;
  private string nextFoot = "left";

  // Door knocking
  public bool inKnockZone = false;
  public AudioClip[] knocks;
  private int knock = 0;
  private int lastKnock = 0;

  // Control state for ending
  private float startingY;
  private bool gameIsEnding;

  void Start() {
    mainController = main.GetComponent<MainController>();
    startingY = transform.position.y;
  }

  void FixedUpdate () {
    if (mainController.state != "game") {
      return;
    }

    // Footsteps
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W)) {
     nextFootstep -= Time.deltaTime;
     if (nextFootstep <= 0) {
       if (nextFoot == "left") {
         if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5) {
           walkSound = stepLeft1;
         } else {
           walkSound = stepLeft2;
         }
         nextFoot = "right";
       } else {
          if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5) {
           walkSound = stepRight1;
         } else {
           walkSound = stepRight2;
         }
         nextFoot = "left";
       }
       GetComponent<AudioSource>().PlayOneShot(walkSound);
       if (!showedFootstepsCaption) {
        subtitles.text = "[Footsteps]";
         showedFootstepsCaption = true;
       }
       nextFootstep += footstepDelay;
     }
    }

    // Restart the game if the player falls off the world.
    if (transform.position.y < startingY) {
      endGame();
    }
  }

  void Update() {
    if (mainController.state != "game") {
      return;
    }

    // Door knocking/opening
    if (Input.GetMouseButtonDown(0) && inKnockZone) {
     if (door.GetComponent<DoorController>().isOpen) {
       return;
     }

     if (hasKey) {
       door.GetComponent<DoorController>().open();
       return;
     }

     // Make sure we don't play the same sound twice in a row.
     while (knock == lastKnock) {
       knock = UnityEngine.Random.Range(0, 4);
     }
     GetComponent<AudioSource>().PlayOneShot(knocks[knock]);
     npc.GetComponent<NpcController>().knock();
     lastKnock = knock;
    }
  }

  // Wait a few seconds, them fade out and end the game.
  // Doing it this way so I can set the gameIsEnding flag *after* the delay.
  private IEnumerator endGameAfterDelay() {
    yield return new WaitForSeconds(8);
    endGame();
  }

  // Fade to black, then restart the game.
  private void endGame() {
    if (gameIsEnding) {
      return;
    }

	Color fadeColor = Color.black;
    bool isFadeIn = false;
    float fadeTime = 5f;
    float fadeDelay = 0;
    Action onFadeFinish = () => SceneManager.LoadScene(Application.loadedLevel);

    CameraFade.StartAlphaFade(fadeColor, isFadeIn, fadeTime, fadeDelay, onFadeFinish);

    gameIsEnding = true;
  }

  void OnTriggerEnter(Collider other) {
   if (other.tag == "start") {
     StartCoroutine(npc.GetComponent<NpcController>().sayHello());
     // Block the player in so they don't wander off.
     invisibleWallA.GetComponent<MeshCollider>().enabled = true;
     // Destroy this collider so it doesn't fire again.
     Destroy(other);
   } else if (other.tag == "knockZone") {
     inKnockZone = true;
   } else if (other.tag == "end") {
     // Block the player in for visual reasons...
     invisibleWallB.GetComponent<MeshCollider>().enabled = true;
     // Make the NPC say thanks.
     StartCoroutine(npc.GetComponent<NpcController>().sayThanks());
     // Resize the black box for visual reasons...
     blackBox.transform.localScale = new Vector3(4, 4, 4);
     blackBox.transform.position -= new Vector3(32.26f, 0, 0);
     lightFrame.SetActive(false);
     insideClosetLight.SetActive(true);
     // Hide the stairs so you can't see them from in the "closet"
     stairs.GetComponent<MeshRenderer>().enabled = false;
     // Destroy this collider so it doesn't fire again.
     Destroy(other);
     // Fade to black after a few seconds and end the game.
     StartCoroutine(endGameAfterDelay());
   }
  }

  void OnTriggerExit(Collider other) {
   if (other.tag == "knockZone") {
     inKnockZone = false;
   }
  }
}