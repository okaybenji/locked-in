using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class PlayerSound : MonoBehaviour {
   public AudioClip stepLeft1;
   public AudioClip stepLeft2;
   public AudioClip stepRight1;
   public AudioClip stepRight2;
   public float footstepDelay;

   private AudioClip walkSound;
   private float nextFootstep = 0;
   private string nextFoot = "left";

   void FixedUpdate () {
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
         GetComponent<AudioSource>().PlayOneShot(walkSound, 0.7f);
         nextFootstep += footstepDelay;
       }
     }
   }
 }