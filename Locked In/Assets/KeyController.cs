using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class KeyController : MonoBehaviour {

  public AudioClip keySlide;
  public AudioClip keyCollect;

  public Camera camera;

  public float slideSpeed = 0.8f;

  private bool isSliding = false;
  private bool isCollectable = false;

  public void slide() {
    isSliding = true;

    GetComponent<AudioSource>().PlayOneShot(keySlide, 0.5f);
  }

  void Start() {
    GetComponentInChildren<Outline>().enabled = false;
  }

  void FixedUpdate() {
    // Determine if player is looking at me.
    RaycastHit hit;
    Ray ray = camera.ScreenPointToRay(Input.mousePosition);

    // If so, add a glow effect. If not, remove it.
    if (isCollectable && Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "key1") {
      Debug.Log("Enabling!!");
      GetComponentInChildren<Outline>().enabled = true;
    } else {
      GetComponentInChildren<Outline>().enabled = false;
    }
  }

  void Update() {
    if (isSliding) {
      transform.Translate(Vector3.right * Time.deltaTime * slideSpeed);
      if (transform.position.x > -11.5) {
        transform.position = new Vector3(-11.5f, transform.position.y, transform.position.z);
        isSliding = false;
        isCollectable = true;
      }
    }
  }
}
