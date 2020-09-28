using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    private float antiRoll = 5000f;

    public Wheel whewheel_FL;
    public Wheel whewheel_FR;
    public Wheel whewheel_BL;
    public Wheel whewheel_BR;

    private Rigidbody rb;
    public Transform centerOfMass;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GroundWheels(whewheel_FL.wc, whewheel_FR.wc);
        GroundWheels(whewheel_BL.wc, whewheel_BR.wc);
    }

    void GroundWheels(WheelCollider _wheelLeft, WheelCollider _wheelRight)
    {
        WheelHit hit;
        float travelLeft = 1.0f;
        float travelRight = 1.0f;

        bool groundedLeft = _wheelLeft.GetGroundHit(out hit);
        if (groundedLeft)
        {
            travelLeft = (-_wheelLeft.transform.InverseTransformPoint(hit.point).y - _wheelLeft.radius)/_wheelLeft.suspensionDistance;
        }
        
        bool groundedRight = _wheelLeft.GetGroundHit(out hit);
        if (groundedLeft)
        {
            travelRight = (-_wheelRight.transform.InverseTransformPoint(hit.point).y - _wheelRight.radius)/_wheelRight.suspensionDistance;
        }

        float antiRollForce = (travelLeft - travelRight) * antiRoll;
        
        if(groundedLeft)
        {
            rb.AddForceAtPosition(_wheelLeft.transform.up * -antiRollForce, _wheelLeft.transform.position);
        }

        if (groundedRight)
        {
            rb.AddForceAtPosition(_wheelRight.transform.up * antiRollForce, _wheelRight.transform.position);
        }
    }
}
