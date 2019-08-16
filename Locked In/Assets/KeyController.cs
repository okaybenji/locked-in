using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {

  public AudioClip keySlide;
  public AudioClip keyCollect;

  public float slideSpeed = 0.8f;

  private bool isSliding = false;

  public void slide() {
    isSliding = true;

    GetComponent<AudioSource>().PlayOneShot(keySlide, 0.5f);
  }

  void Update() {
    if (isSliding) {
      Debug.Log("forward:" + Vector3.forward);
      Debug.Log("delta time:" + Time.deltaTime);
      Debug.Log("slide speed:" + slideSpeed);
      Debug.Log("moving object by " + Vector3.right * Time.deltaTime * slideSpeed);
      transform.Translate(Vector3.right * Time.deltaTime * slideSpeed);
      Debug.Log("position " + transform.position.x);

      if (transform.position.x > -11.5) {
        transform.position = new Vector3(-11.5f, transform.position.y, transform.position.z);
        isSliding = false;
      }
    }
  }
}
