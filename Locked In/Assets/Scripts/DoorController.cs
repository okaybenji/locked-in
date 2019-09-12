using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class DoorController : MonoBehaviour {

  public bool isOpen = false;
  public float openSpeed = 10.0f;
  public AudioClip openDoor;

  // Outline effect.
  public Camera camera;
  public GameObject player;

  private float openStart;

  void Start() {
    GetComponentInChildren<Outline>().enabled = false;
  }

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
    } else if (player.GetComponent<PlayerController>().hasKey) {
      // Show an outline when player looks at door if they have the key.
      // Determine if player is looking at me.
      RaycastHit hit;
      Ray ray = camera.ScreenPointToRay(Input.mousePosition);

      // If so, add a glow effect. If not, remove it.
      if (Physics.Raycast(ray, out hit)) {
        GetComponentInChildren<Outline>().enabled = true;
       } else {
        GetComponentInChildren<Outline>().enabled = false;
      }
    }
  }
}
