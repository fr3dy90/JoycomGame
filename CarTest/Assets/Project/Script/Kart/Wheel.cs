using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
   public WheelCollider wc;
   public GameObject wheelMesh;
   public bool canSteering;
   public Transform skidTrail;
   public ParticleSystem skidSmoke;

   private void Start()
   {
      wc = GetComponent<WheelCollider>();
      if (wc == null)
      {
         Debug.LogWarning("Missing wheel collider on:" + transform.name);
      }
   }
}
