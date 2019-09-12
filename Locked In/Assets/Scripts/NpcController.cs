using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour {
  public GameObject key1;
  public GameObject key2;
  public GameObject crab;

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
  private float saidHuhHuhHuhAt;
  private float explainedSituationAt;
  private float lastKnockAt;

  private string currentQuestion = "";

  private int knockCount;

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

  public void sayThanks() {
    audio.PlayOneShot(ahhThanks);
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
    StartCoroutine(AudioFadeOut.FadeOut(audio, 0.01f));
    yield return new WaitForSeconds(0.02f);

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
    currentQuestion = "";
    audio.PlayOneShot(hereComes);

    // Start the crab walking to intercept the key.
    crab.GetComponent<CrabController>().go();

    yield return new WaitForSeconds(2);
    key1.GetComponent<KeyController>().slide();

    yield return new WaitForSeconds(10);
    audio.PlayOneShot(huhuhuh);
    saidHuhHuhHuhAt = Time.time;
    currentQuestion = "didYouLoseIt";

    yield return new WaitForSeconds(15);
    if (lastKnockAt < saidHuhHuhHuhAt) {
      audio.PlayOneShot(iSaidDidYouLoseIt);
    }
  }

  private IEnumerator secondKey() {
    currentQuestion = "";
    audio.PlayOneShot(howIsThatPossible);

    yield return new WaitForSeconds(9);
    key2.GetComponent<KeyController>().slide();
  }

  private IEnumerator respondToKnocks(int knockCount) {
    StartCoroutine(AudioFadeOut.FadeOut(audio, 0.01f));
    yield return new WaitForSeconds(0.02f);

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
      } else if (currentQuestion == "isThisFunny") {
        currentQuestion = "willYouHelp";
        audio.PlayOneShot(reallyComeOn);
        StartCoroutine(repeatInstructions());
      }
    } else if (knockCount == 2) {
      if (currentQuestion == "canYouHearMe") {
        bool veryFunny = true;
        StartCoroutine(explainSituation(veryFunny));
      } else if (currentQuestion == "willYouHelp") {
        audio.PlayOneShot(reallyComeOn);
        StartCoroutine(repeatInstructions());
      } else if (currentQuestion == "didYouLoseIt") {
        StartCoroutine(secondKey());
      } else if (currentQuestion == "isThisFunny") {
        currentQuestion = "willYouHelp";
        audio.PlayOneShot(please);
        StartCoroutine(repeatInstructions());
      }
    } else if (knockCount >= 15) {
      audio.PlayOneShot(aBunch);
      currentQuestion = "isThisFunny";
    } else {
      audio.PlayOneShot(knockCounts[knockCount - 3]);
      StartCoroutine(repeatInstructions());
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
      StartCoroutine(respondToKnocks(knockCount));
      knockCount = 0;
    }
  }
}
