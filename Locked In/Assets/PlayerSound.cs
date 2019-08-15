using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class PlayerSound : MonoBehaviour {
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
             Debug.Log("left1");
             walkSound = stepLeft1;
           } else {
             Debug.Log("left2");
             walkSound = stepLeft2;
           }
           nextFoot = "right";
         } else {
            if (Random.Range(0.0f, 1.0f) > 0.5) {
             walkSound = stepRight1;
              Debug.Log("right1");
           } else {
             walkSound = stepRight2;
              Debug.Log("right2");
           }
           nextFoot = "left";
         }
         GetComponent<AudioSource>().PlayOneShot(walkSound, 1.0f);
         nextFootstep += footstepDelay;
       }
     }
   }

   void Update() {
     // Door knocking
     if (Input.GetMouseButtonDown(0) && inKnockZone) {
       // Make sure we don't play the same sound twice in a row.
       while (knock == lastKnock) {
         knock = Random.Range(0, 4);
       }
       GetComponent<AudioSource>().PlayOneShot(knocks[knock], 1.0f);
       lastKnock = knock;
     }
   }

   void OnTriggerEnter(Collider other) {
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