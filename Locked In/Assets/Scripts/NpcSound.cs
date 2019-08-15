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
  public AudioClip huhuhuh; // lolz did you lose the key or something
  public AudioClip iSaidDidYouLoseIt;
  public AudioClip howIsThatPossible;
  public AudioClip ahhThanks;

  private float saidHelloAt;
  private float saidICanHearYouAt;
  private float saidPleaseAt;
  private float saidICantHearYouAt;
  private float saidHuhHuhHuhAt;
  private float explainedSituationAt;
  private float lastKnockAt;

  private string currentQuestion = "";

  private int knockCount;
  private bool countingKnocks = false;

  public IEnumerator sayHello() {
    audio.PlayOneShot(hello);
    saidHelloAt = Time.time;

    yield return new WaitForSeconds(1);
    currentQuestion = "hello";
    yield return new WaitForSeconds(3);
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
      StartCoroutine(sayICantHearYou());
    }
  }

  private IEnumerator sayICantHearYou() {
    if (lastKnockAt <= saidPleaseAt) {
      currentQuestion = "";
      audio.PlayOneShot(iCantHearYou);
      saidICantHearYouAt = Time.time;
      yield return new WaitForSeconds(5);
      currentQuestion = "canYouHearMe";
      yield return new WaitForSeconds(8);
      repeatInstructions();
    }
  }

  private void repeatInstructions() {
    if (lastKnockAt <= saidICantHearYouAt) {
      audio.PlayOneShot(knockOnceOrTwice);
    }
  }

  private IEnumerator explainSituation(bool veryFunny = false) {
    audio.Stop();

    explainedSituationAt = Time.time;
    currentQuestion = "";
    countingKnocks = false;

    if (veryFunny) {
      audio.PlayOneShot(veryFunnyLook);
    } else {
      audio.PlayOneShot(okayGreatLook);
    }
    yield return new WaitForSeconds(3);
    audio.PlayOneShot(iFeelDumb);
    yield return new WaitForSeconds(16);
    currentQuestion = "willYouHelp";
    audio.PlayOneShot(knockOnceOrTwice);
  }

  private IEnumerator firstKey() {
    // TODO: ...
    currentQuestion = "";
    audio.PlayOneShot(hereComes);
    yield return new WaitForSeconds(5);
    audio.PlayOneShot(huhuhuh);
    saidHuhHuhHuhAt = Time.time;
    currentQuestion = "didYouLoseIt";
    yield return new WaitForSeconds(5);
    sayISaidDidYouLoseIt();
  }

  private void sayISaidDidYouLoseIt() {
    if (lastKnockAt < saidHuhHuhHuhAt) {
      audio.PlayOneShot(iSaidDidYouLoseIt);
    }
  }

  private IEnumerator secondKey() {
    // TODO: ...
    audio.PlayOneShot(howIsThatPossible);
    yield return new WaitForSeconds(10);
    audio.PlayOneShot(ahhThanks);
  }

  private void respondToKnocks(int knockCount) {
    audio.Stop();

    Debug.Log("respondToKnocks: " + knockCount);
    Debug.Log("current Question: " + currentQuestion);

    // Player knocks once for yes, twice for no.
    if (knockCount == 1) {
      if (currentQuestion == "canYouHearMe") {
        StartCoroutine(explainSituation());
      } else if (currentQuestion == "willYouHelp") {
        StartCoroutine(firstKey());
      } else if (currentQuestion == "didYouLoseIt") {
        StartCoroutine(secondKey());
      }
    } else if (knockCount == 2) {
      if (currentQuestion == "canYouHearMe") {
        bool veryFunny = true;
        StartCoroutine(explainSituation(veryFunny));
      } else if (currentQuestion == "willYouHelp") {
        audio.PlayOneShot(reallyComeOn);
      } else if (currentQuestion == "didYouLoseIt") {
        StartCoroutine(secondKey());
      }
    } else if (knockCount >= 15) {
      audio.PlayOneShot(aBunch);
    } else {
      audio.PlayOneShot(knockCounts[knockCount - 3]);
    }
  }

  // Communicate to the NPC that the player has just knocked.
  public void knock() {
    // If we asked if they can hear us, start counting their knocks.
    if (currentQuestion == "hello") {
      // If we already said hello, the player just knocked, and we haven't yet explained the situation, do that.
      StartCoroutine(explainSituation());
    } else if (currentQuestion == "canYouHearMe" || currentQuestion == "willYouHelp") {
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
      if (currentQuestion == "") {
        knockCount = 0;
        countingKnocks = false;
        return;
      }

      respondToKnocks(knockCount);
      knockCount = 0;
      countingKnocks = false;
    }
  }
}
