using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player Sound ended up being too specific. This is just the player control code.
public class PlayerController : MonoBehaviour {
  public GameObject npc;
  public GameObject invisibleWall;
  public GameObject door;

  public bool hasKey = false;

  // Footsteps
  public AudioClip stepLeft1;
  public AudioClip stepLeft2;
  public AudioClip stepRight1;
  public AudioClip stepRight2;
  public float footstepDelay;

  private AudioClip walkSound;
  private float nextFootstep = 0;
  private string nextFoot = "left";

  // Door knocking
  public bool inKnockZone = false;
  public AudioClip[] knocks;
  private int knock = 0;
  private int lastKnock = 0;

  void FixedUpdate () {
   // Footsteps
   if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W)) {
     nextFootstep -= Time.deltaTime;
     if (nextFootstep <= 0) {
       if (nextFoot == "left") {
         if (Random.Range(0.0f, 1.0f) > 0.5) {
           walkSound = stepLeft1;
         } else {
           walkSound = stepLeft2;
         }
         nextFoot = "right";
       } else {
          if (Random.Range(0.0f, 1.0f) > 0.5) {
           walkSound = stepRight1;
         } else {
           walkSound = stepRight2;
         }
         nextFoot = "left";
       }
       GetComponent<AudioSource>().PlayOneShot(walkSound);
       nextFootstep += footstepDelay;
     }
   }
  }

  void Update() {
   // Door knocking/opening
   if (Input.GetMouseButtonDown(0) && inKnockZone) {
     if (door.GetComponent<DoorController>().isOpen) {
       return;
     }

     if (hasKey) {
       door.GetComponent<DoorController>().open();
       return;
     }

     // Make sure we don't play the same sound twice in a row.
     while (knock == lastKnock) {
       knock = Random.Range(0, 4);
     }
     GetComponent<AudioSource>().PlayOneShot(knocks[knock]);
     npc.GetComponent<NpcController>().knock();
     lastKnock = knock;
   }
  }

  void OnTriggerEnter(Collider other) {
   if (other.tag == "start") {
     StartCoroutine(npc.GetComponent<NpcController>().sayHello());
     // Block the player in so they don't wander off.
     invisibleWall.GetComponent<MeshCollider>().enabled = true;
     // Destroy this collider so it doesn't fire again.
     Destroy(other);
   }

   if (other.tag == "knockZone") {
     inKnockZone = true;
   }
  }

  void OnTriggerExit(Collider other) {
   if (other.tag == "knockZone") {
     inKnockZone = false;
   }
  }
}