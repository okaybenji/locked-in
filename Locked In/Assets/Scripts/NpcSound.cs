using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSound : MonoBehaviour {
  public AudioSource audio;
  public AudioClip[] knockCounts;
  public AudioClip aBunch;
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

  private float saidHelloAt;
  private float saidICanHearYouAt;
  private float saidPleaseAt;
  private float saidICantHearYouAt;
  private float explainedSituationAt;
  private float finishedExplanationAt;
  private float lastKnockAt;
  private int knockCount;
  private bool countingKnocks = false;

  public IEnumerator sayHello() {
    audio.PlayOneShot(hello);
    saidHelloAt = Time.time;

    yield return new WaitForSeconds(4);
    StartCoroutine(sayICanHearYou());
  }

  private IEnumerator sayICanHearYou() {
    if (lastKnockAt <= saidHelloAt) {
      audio.PlayOneShot(iCanHearYou);
      saidICanHearYouAt = Time.time;
      yield return new WaitForSeconds(4);
      StartCoroutine(sayPlease());
    } else if (explainedSituationAt == 0) {
      StartCoroutine(explainSituation());
    }
  }

  private IEnumerator sayPlease() {

    if (lastKnockAt <= saidICanHearYouAt) {
      audio.PlayOneShot(please);
      saidPleaseAt = Time.time;

      yield return new WaitForSeconds(4);
      sayICantHearYou();
    }
  }

  private void sayICantHearYou() {
    if (lastKnockAt <= saidPleaseAt) {
      audio.PlayOneShot(iCantHearYou);
      saidICantHearYouAt = Time.time;
    }
  }

  private IEnumerator explainSituation() {
    explainedSituationAt = Time.time;
    audio.Stop();
    audio.PlayOneShot(okayGreatLook);
    yield return new WaitForSeconds(2);
    audio.PlayOneShot(iFeelDumb);
    yield return new WaitForSeconds(16);
    audio.PlayOneShot(knockOnceOrTwice);
    yield return new WaitForSeconds(2);
    finishedExplanationAt = Time.time;
  }

  private void respondToKnocks(int knockCount) {
    audio.Stop();

    if (knockCount == 1) {
      Debug.Log("player said yes");
    } else if (knockCount == 2) {
      Debug.Log("player said no");
    } else if (knockCount >= 15) {
      audio.PlayOneShot(aBunch);
    } else {
      audio.PlayOneShot(knockCounts[knockCount - 3]);
    }
  }

  // Communicate to the NPC that the player has just knocked.
  public void knock() {
    // If we already said hello, the player just knocked, and we haven't yet explained the situation, do that.
    if ((saidHelloAt + 3) < Time.time && explainedSituationAt == 0) {
      StartCoroutine(explainSituation());
    }

    // If we said we can't hear the player talking, start counting their knocks.
    if (saidICantHearYouAt > 0 && (saidICantHearYouAt + 2) < Time.time) {
      countingKnocks = true;
    }

    if (countingKnocks) {
      knockCount++;
    }

    lastKnockAt = Time.time;
  }

  void Update() {
    // If we are counting knocks, wait for a delay and the respond.
    if (countingKnocks && (Time.time - lastKnockAt) > 1) {
      respondToKnocks(knockCount);
      knockCount = 0;
      countingKnocks = false;
    }
  }
}
