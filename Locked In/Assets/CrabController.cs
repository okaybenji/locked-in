using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour {
  public GameObject key;
  public float walkSpeed = 0.2f;
  private bool isGrabbing = false;

  void Start() {
    var rb = gameObject.GetComponent<Rigidbody>();
    rb.isKinematic = true;
    rb.detectCollisions = false;
  }

  // Animate the crab walking toward where the key will land, picking it up, and walking off.
  public void go() {
    isGrabbing = true;

    StartCoroutine(grabKey());
  }

  private void Attach(GameObject to) {
    gameObject.transform.parent = to.transform;
//    gameObject.transform.position = to.transform.TransformPoint(new Vector3(0, 1f, 1f));
  }

  // Attaches the key to the crab.
  private IEnumerator grabKey() {
    yield return new WaitForSeconds(1.5f);
    Attach(key);
  }

  void Update() {
    if (isGrabbing) {
      transform.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
//      if (transform.position.x > -11.5) {
//        transform.position = new Vector3(-11.5f, transform.position.y, transform.position.z);
//        isSliding = false;
//        isCollectable = true;
//      }
    }
  }
}
