using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NpcController : MonoBehaviour {
  public GameObject key1;
  public GameObject key2;
  public GameObject crab;

  public TextMeshProUGUI subtitles;

  public AudioSource audio;
  public AudioClip[] knockCounts; // Audio responses to various numbers of knocks.
  public string[] knockCountsSub; // Subtitle response to various numbers of knocks.
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
    subtitles.text = "Hello? Is someone there?";
    saidHelloAt = Time.time;

    yield return new WaitForSeconds(1);
    currentQuestion = "hello";

    yield return new WaitForSeconds(3);
    if (lastKnockAt <= saidHelloAt + 1) {
      audio.PlayOneShot(iCanHearYou);
      subtitles.text = "I can hear your footsteps.";
      saidICanHearYouAt = Time.time;

      yield return new WaitForSeconds(4);
      if (lastKnockAt <= saidICanHearYouAt) {
        audio.PlayOneShot(please);
        subtitles.text = "Please, I need your help.";
        saidPleaseAt = Time.time;

        yield return new WaitForSeconds(4);
        StartCoroutine(sayICantHearYou());
      }
    } else if (explainedSituationAt == 0) {
      StartCoroutine(explainSituation());
    }
  }

  public IEnumerator sayThanks() {
    audio.PlayOneShot(ahhThanks);
    subtitles.text = "Ahh, thanks.";
    yield return new WaitForSeconds(3);
    subtitles.text = "...What?";
  }

  private IEnumerator sayICantHearYou() {
    if (lastKnockAt <= saidPleaseAt) {
      currentQuestion = "";
      audio.PlayOneShot(iCantHearYou);
      subtitles.text = "Look, I can’t hear you if you’re talking. Just knock once for yes and twice for no.";
      yield return new WaitForSeconds(5);

      subtitles.text = "Can you hear me?";
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
      subtitles.text = "Knock once for yes, twice for no.";
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
    subtitles.text = veryFunny ? "[Chuckles] Very funny. Look..." : "Okay, great, look...";

    yield return new WaitForSeconds(3);
    audio.PlayOneShot(iFeelDumb);
    subtitles.text = "I feel really dumb, but I need your help.";

    yield return new WaitForSeconds(3);
    subtitles.text = "This door locks and unlocks with a key from the outside.";

    yield return new WaitForSeconds(4);
    subtitles.text = "I locked the door but forgot something inside, so I went back to grab it, and now I’m locked in.";

    yield return new WaitForSeconds(6);
    subtitles.text = "If I slide you the key, will you unlock the door for me?";

    yield return new WaitForSeconds(3);
    currentQuestion = "willYouHelp";
    audio.PlayOneShot(knockOnceOrTwice);
    subtitles.text = "Knock once for yes, twice for no.";
  }

  private IEnumerator firstKey() {
    currentQuestion = "";
    audio.PlayOneShot(hereComes);
    subtitles.text = "Okay, great, here comes.";

    // Start the crab walking to intercept the key.
    crab.GetComponent<CrabController>().go();

    yield return new WaitForSeconds(2);
    StartCoroutine(key1.GetComponent<KeyController>().slide());

    yield return new WaitForSeconds(10);
    audio.PlayOneShot(huhuhuh);
    subtitles.text = "Hellooo? Did you lose the key? Huhuhuh.";
    saidHuhHuhHuhAt = Time.time;
    yield return new WaitForSeconds(2);
    currentQuestion = "didYouLoseIt";

    yield return new WaitForSeconds(15);
    if (lastKnockAt < saidHuhHuhHuhAt) {
      audio.PlayOneShot(iSaidDidYouLoseIt);
      subtitles.text = "I said, “Did you lose the key?”";
    }
  }

  private IEnumerator secondKey() {
    currentQuestion = "";
    audio.PlayOneShot(howIsThatPossible);
    subtitles.text = "What, seriously? I was being sarcastic. I have no idea how that’s even possible.";

    yield return new WaitForSeconds(5);
    subtitles.text = "Okay, well, I have one more copy... here, just open the door this time.";

    yield return new WaitForSeconds(4);
    StartCoroutine(key2.GetComponent<KeyController>().slide());
  }

  private IEnumerator respondToKnocks(int knockCount) {
    if (currentQuestion == "") {
      knockCount = 0;
      yield break;
    }
    
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
        subtitles.text = "Really? Come on. Just unlock it for me.";
        StartCoroutine(repeatInstructions());
      }
    } else if (knockCount == 2) {
      if (currentQuestion == "canYouHearMe") {
        bool veryFunny = true;
        StartCoroutine(explainSituation(veryFunny));
      } else if (currentQuestion == "willYouHelp") {
        audio.PlayOneShot(reallyComeOn);
        subtitles.text = "Really? Come on. Just unlock it for me.";
        StartCoroutine(repeatInstructions());
      } else if (currentQuestion == "didYouLoseIt") {
        StartCoroutine(secondKey());
      } else if (currentQuestion == "isThisFunny") {
        currentQuestion = "willYouHelp";
        audio.PlayOneShot(please);
        subtitles.text = "Please, I need your help.";
        StartCoroutine(repeatInstructions());
      }
    } else if (knockCount >= 15) {
      currentQuestion = "";
      audio.PlayOneShot(aBunch);
      subtitles.text = "Okay, you just knocked, like, a bunch of times. Is this funny to you?";
      yield return new WaitForSeconds(4);
      currentQuestion = "isThisFunny";
    } else {
      // Make sure player has a chance to hear at least some of the response before
      // responding to their next knocks (in case they accidentally interrupt)
      string lastQuestion = currentQuestion;
      currentQuestion = "";
      audio.PlayOneShot(knockCounts[knockCount - 3]);
      subtitles.text = knockCountsSub[knockCount - 3];
      yield return new WaitForSeconds(3);
      currentQuestion = lastQuestion;
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

    if (knockCount == 1) {
      subtitles.text = "[Knock]";
    } else if (knockCount == 2) {
      subtitles.text = "[Knock, knock]";
    } else if (knockCount == 3) {
      subtitles.text = "[Knock, knock, knock]";
    } else if (knockCount == 4) {
      subtitles.text = "[Knock, knock, knock...]";
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
