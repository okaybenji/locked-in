using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class DoorController : MonoBehaviour {

  public bool isOpen = false;
  public float openSpeed = 10.0f;
  public AudioClip openDoor;

  private float openStart;

  public void open() {
    isOpen = true;
    openStart = Time.deltaTime;

    GetComponent<AudioSource>().PlayOneShot(openDoor);
  }

  void Update() {
    if (isOpen) {
      transform.Rotate(Vector3.up * (-openSpeed * Time.deltaTime));
      if (transform.rotation.y < -0.1f) {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        Destroy(GetComponentInChildren<MeshCollider>());
        Destroy(this);
      }
    }
  }
}
