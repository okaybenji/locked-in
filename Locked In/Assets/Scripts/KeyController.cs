using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class KeyController : MonoBehaviour {

  public GameObject player;
  public GameObject crab;

  public AudioClip keySlide;
  public AudioClip keyCollect;

  public Camera camera;

  public float slideSpeed = 0.8f;

  public bool collectable = false;  // Whether designer intends this key to ever be collectable.
  public bool isAttachedToCrab = false;

  private bool isSliding = false;
  private bool isCollectable = false; // Whether the key is collectable at this moment.

  private Vector3 attachStartKey; // Where the key was when it got attached to the crab.
  private Vector3 attachStartCrab; // Where the crab was when it got the key.

  public void slide() {
    isSliding = true;

    GetComponent<AudioSource>().PlayOneShot(keySlide, 0.5f);
  }

  void Start() {
    GetComponentInChildren<Outline>().enabled = false;
  }

  // Delays giving key so door doesn't immediately open and key has time to play its SFX.
  private IEnumerator giveKey() {
    yield return new WaitForSeconds(0.5f);

    player.GetComponent<PlayerController>().hasKey = true;
    gameObject.SetActive(false);

    // TODO: Fade out key before destroying it.
    Destroy(this);
  }

  void Update() {
    if (isCollectable) {
      // Determine if player is looking at me.
      RaycastHit hit;
      Ray ray = camera.ScreenPointToRay(Input.mousePosition);

      // If so, add a glow effect. If not, remove it.
      if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "key1") {
        GetComponentInChildren<Outline>().enabled = true;

        // If player clicks me, play the collect sound.
        if (Input.GetMouseButtonDown(0)) {
          GetComponent<AudioSource>().PlayOneShot(keyCollect);
          StartCoroutine(giveKey());
        }
       } else {
        GetComponentInChildren<Outline>().enabled = false;
      }
    }

    if (isSliding) {
      transform.Translate(Vector3.right * Time.deltaTime * slideSpeed);
      if (transform.position.x > -11.5) {
        transform.position = new Vector3(-11.5f, transform.position.y, transform.position.z);
        isSliding = false;
        if (collectable) {
          isCollectable = true;
        }
      }
    } else if (isAttachedToCrab) {
      // Keep the key attached to the crab.
      if (attachStartKey.x == 0 && attachStartKey.y == 0 && attachStartKey.z == 0) {
        attachStartKey = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        attachStartCrab = new Vector3(crab.transform.position.x, crab.transform.position.y, crab.transform.position.z);
      }
      var relativeCrabPos = new Vector3(
        crab.transform.position.x - attachStartCrab.x,
        crab.transform.position.y - attachStartCrab.y,
        crab.transform.position.z - attachStartCrab.z
      );
      transform.position = new Vector3(
        attachStartKey.x + relativeCrabPos.x,
        attachStartKey.y + relativeCrabPos.y,
        attachStartKey.z + relativeCrabPos.z
      );
    }
  }
}
