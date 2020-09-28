using System;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class DriveTest : MonoBehaviour
{
    public Wheel[] wheels;
    public float torque = 200;
    public float maxSteringAngle = 30;
    public float maxBrakeTorque = 500;

    public AudioSource skidSound;
    public float skidTrigger = 0.4f;
    public AudioSource engine;
    
    public Transform skidTrailPrefab;

    public Rigidbody rb;
    public float gearLength = 3;
    public float currentSpeed
    {
        get { return rb.velocity.magnitude * gearLength; }
    }

    public float lowPitch = 1;
    public float highPitch = 6;
    public int numGears = 5;
    private float rpm;
    private int currentGear = 1;
    private float currentGearPercent;
    public float maxSpeed = 80;
    

    private void Start()
    {
        foreach (Wheel _wheel  in wheels)
        {
            _wheel.skidSmoke.Stop();
        }
    }

    /*
    private void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        float brake = Input.GetAxis("Jump");
        Move(vertical, horizontal, brake);
        CheckSkidSound();
        CalculateEngineSound();
    }
    */

    public void Move(float _accel, float _steer, float _brake)
    {
        float accel = Mathf.Clamp(_accel, -1, 1);
        float steer = Mathf.Clamp(_steer, -1, 1) * maxSteringAngle;
        float brake = Mathf.Clamp(_brake,0,1) *maxBrakeTorque;
        float thrustTorque = 0;
        if (currentSpeed < maxSpeed)
        {
            thrustTorque = accel * torque;
        }
        
        foreach (Wheel _wheel in wheels)
        {
            _wheel.wc.motorTorque = thrustTorque;
            if (_wheel.canSteering)
            {
                _wheel.wc.steerAngle = steer;
            }
            else
            {
                _wheel.wc.brakeTorque = brake;
            }
            //The desire position and rotation of Wheel Mesh
            Quaternion wheelMeshRotation;
            Vector3 wheelMeshPosition;
            _wheel.wc.GetWorldPose(out wheelMeshPosition, out wheelMeshRotation);
            _wheel.wheelMesh.transform.SetPositionAndRotation(wheelMeshPosition,wheelMeshRotation);
            
        }
    }

    public void CheckSkidSound()
    {
        int numSkiddin = 0;
        foreach (Wheel wheel in wheels)
        {
            WheelHit hit;
            wheel.wc.GetGroundHit(out hit);

            if (Mathf.Abs(hit.forwardSlip) >= skidTrigger || Mathf.Abs(hit.sidewaysSlip) >= skidTrigger)
            {
                numSkiddin++;
                if (!skidSound.isPlaying)
                {
                    skidSound.Play();
                }
                StartSkidTrail(wheel);
                wheel.skidSmoke.Emit(1);
            }
            else
            {
                EndSkilTrail(wheel);
                wheel.skidSmoke.Stop();
            }
        }

        if (numSkiddin == 0 && skidSound.isPlaying)
        {
            skidSound.Stop();
        }
    }

    void StartSkidTrail(Wheel _wheel)
    {
        if (_wheel.skidTrail == null)
        {
            _wheel.skidTrail = Instantiate(skidTrailPrefab);
        }
        _wheel.skidTrail.parent = _wheel.wc.transform;
        _wheel.skidTrail.localRotation = Quaternion.Euler(90, 0, 0);
        _wheel.skidTrail.localPosition = -Vector3.up * _wheel.wc.radius;
    }

    void EndSkilTrail(Wheel _wheel)
    {
        if (_wheel.skidTrail == null)
        {
            return;
        }

        Transform holder = _wheel.skidTrail;
        _wheel.skidTrail = null;
        holder.parent = null;
        holder.rotation = Quaternion.Euler(90,0,0);
        Destroy(holder.gameObject, 30); 
    }

    public void CalculateEngineSound()
    {
        float gearPercentage = (1 / (float) numGears);
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1), Mathf.Abs(currentSpeed / maxSpeed));
        currentGearPercent = Mathf.Lerp(currentGearPercent, targetGearFactor, Time.deltaTime * 5f);
        float gearNumFactor = currentGear / (float) numGears;
        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearPercent);
        float speedPercentage = Mathf.Abs(currentSpeed / maxSpeed);
        float upperGearMax = (1 / (float) numGears * (currentGear + 1));
        float downGearMax = (1 / (float) numGears * currentGear);

        if (currentGear > 0 && speedPercentage < downGearMax)
        {
            currentGear--;
        }

        if (speedPercentage > upperGearMax && (currentGear < (numGears - 1)))
        {
            currentGear++;
        }

        float pitch = (Mathf.Lerp(lowPitch, highPitch, rpm));
        engine.pitch = Mathf.Min(highPitch, pitch) * 0.25f;
    }
}