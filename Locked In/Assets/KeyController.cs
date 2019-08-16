using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class KeyController : MonoBehaviour {

  public AudioClip keySlide;
  public AudioClip keyCollect;

  public float slideSpeed = 0.8f;

  private bool isSliding = false;

  public void slide() {
    isSliding = true;

    GetComponent<AudioSource>().PlayOneShot(keySlide, 0.5f);

    GetComponentInChildren<Outline>().enabled = true;
  }

  void Start() {
    GetComponentInChildren<Outline>().enabled = false;
  }

  void Update() {
    if (isSliding) {
      transform.Translate(Vector3.right * Time.deltaTime * slideSpeed);
      if (transform.position.x > -11.5) {
        transform.position = new Vector3(-11.5f, transform.position.y, transform.position.z);
        isSliding = false;
      }
    }
  }
}
