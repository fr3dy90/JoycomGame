using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipKart : MonoBehaviour
{
    private Rigidbody rb;
    private float lastTimeChecked;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.up.y > 0.5f || rb.velocity.magnitude > 1)
        {
            lastTimeChecked = Time.time;
        }

        if (Time.time > lastTimeChecked + 3)
        {
            RightCar();
        }
    }

    void RightCar()
    {
        transform.position += Vector3.up;
        transform.rotation = Quaternion.LookRotation(transform.forward);
    }
}
