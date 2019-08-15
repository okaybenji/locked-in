using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSound : MonoBehaviour {
  public AudioClip hello;

  public void sayHello() {
    Debug.Log("here we go!");
    GetComponent<AudioSource>().PlayOneShot(hello, 1.0f);
  }
}
