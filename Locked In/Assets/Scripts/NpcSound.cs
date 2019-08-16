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

  public AudioClip keySlide;

  private float saidHelloAt;
  private float saidICanHearYouAt;
  private float saidPleaseAt;
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
    if (lastKnockAt <= saidHelloAt + 1) {
      audio.PlayOneShot(iCanHearYou);
      saidICanHearYouAt = Time.time;

      yield return new WaitForSeconds(4);
      if (lastKnockAt <= saidICanHearYouAt) {
        audio.PlayOneShot(please);
        saidPleaseAt = Time.time;

        yield return new WaitForSeconds(4);
        StartCoroutine(sayICantHearYou());
      }
    } else if (explainedSituationAt == 0) {
      StartCoroutine(explainSituation());
    }
  }

  private IEnumerator sayICantHearYou() {
    if (lastKnockAt <= saidPleaseAt) {
      currentQuestion = "";
      audio.PlayOneShot(iCantHearYou);

      yield return new WaitForSeconds(5);
      currentQuestion = "canYouHearMe";

      StartCoroutine(repeatInstructions());
    }
  }

  // If player doesn't respond for a bit, repeat the instructions.
  private IEnumerator repeatInstructions() {
    float waitStart = Time.time;

    yield return new WaitForSeconds(8);
    if (lastKnockAt <= waitStart) {
      audio.PlayOneShot(knockOnceOrTwice);
      StartCoroutine(repeatInstructions());
    }
  }

  private IEnumerator explainSituation(bool veryFunny = false) {
    audio.Stop();

    explainedSituationAt = Time.time;
    currentQuestion = "";
    knockCount = 0;

    audio.PlayOneShot(veryFunny ? veryFunnyLook : okayGreatLook);

    yield return new WaitForSeconds(3);
    audio.PlayOneShot(iFeelDumb);

    yield return new WaitForSeconds(16);
    currentQuestion = "willYouHelp";
    audio.PlayOneShot(knockOnceOrTwice);
  }

  private IEnumerator firstKey() {
    // TODO: Animate and add sound for first key sliding from under door and crab grabbing it and shuffling off.
    currentQuestion = "";
    audio.PlayOneShot(hereComes);

    // TODO: Let's actually have the key play this sound on its own audio source.
    yield return new WaitForSeconds(2);
    audio.PlayOneShot(keySlide, 0.5f);

    yield return new WaitForSeconds(5);
    audio.PlayOneShot(huhuhuh);
    saidHuhHuhHuhAt = Time.time;
    currentQuestion = "didYouLoseIt";

    yield return new WaitForSeconds(5);
    if (lastKnockAt < saidHuhHuhHuhAt) {
      audio.PlayOneShot(iSaidDidYouLoseIt);
    }
  }

  void secondKey() {
    currentQuestion = "";
    audio.PlayOneShot(howIsThatPossible);
    // TODO: Animate second key appearing (with sound).
    // Clicking key should destroy it, play a sound (maybe from bingo?) and allow player to open door by "knocking" again.
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
        secondKey();
      }
    } else if (knockCount == 2) {
      if (currentQuestion == "canYouHearMe") {
        bool veryFunny = true;
        StartCoroutine(explainSituation(veryFunny));
      } else if (currentQuestion == "willYouHelp") {
        audio.PlayOneShot(reallyComeOn);
        StartCoroutine(repeatInstructions());
      } else if (currentQuestion == "didYouLoseIt") {
        secondKey();
      }
    } else if (knockCount >= 15) {
      audio.PlayOneShot(aBunch);
    } else {
      audio.PlayOneShot(knockCounts[knockCount - 3]);
    }
  }

  // Communicate to the NPC that the player has just knocked.
  public void knock() {
    if (currentQuestion == "hello") {
      // If we already said hello, the player just knocked, and we haven't yet explained the situation, do that.
      StartCoroutine(explainSituation());
    } else if (currentQuestion != "") {
      // If we asked a question, start counting their knocks.
      knockCount++;
    } else if (knockCount > 0) {
      // If we're already counting, keep counting.
      knockCount++;
    }

    lastKnockAt = Time.time;
  }

  void Update() {
    // If we are counting knocks, wait for a delay and the respond.
    if (knockCount > 0 && (Time.time - lastKnockAt) > 1) {
      respondToKnocks(knockCount);
      knockCount = 0;
    }
  }
}
