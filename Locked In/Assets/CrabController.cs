using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour {
  public GameObject key;
  public float walkSpeed = 0.2f;
  private bool isGrabbing = false;

  // Animate the crab walking toward where the key will land, picking it up, and walking off.
  public void go() {
    isGrabbing = true;

    StartCoroutine(grabKey());
  }

  // Attaches the key to the crab.
  private IEnumerator grabKey() {
    yield return new WaitForSeconds(3);
    key.GetComponent<KeyController>().isAttachedToCrab = true;
  }

  void Update() {
    if (isGrabbing) {
      transform.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
      if (transform.position.z > 20) {
        Destroy(this);
      }
    }
  }
}
