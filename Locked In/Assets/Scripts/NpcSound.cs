using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSound : MonoBehaviour {
  public AudioClip hello;
  public AudioClip iCanHearYou;
  public AudioClip please;
  public AudioClip okayGreatLook;
  public AudioClip veryFunnyLook;
  public AudioClip reallyComeOn;
  public AudioClip iFeelDumb;
  public AudioClip iCantHearYou;
  public AudioClip knockOnceOrTwice;
  public AudioClip hereComes;
  public AudioClip huhuhuh;
  public AudioClip iSaidDidYouLoseIt;
  public AudioClip howIsThatPossible;
  public AudioClip ahhThanks;

  private float lastKnock;

  public void sayHello() {
    GetComponent<AudioSource>().PlayOneShot(hello, 1.0f);
  }

  // Communicate to the NPC that the player has just knocked.
  public void knock() {
    lastKnock = Time.time;
  }
}
