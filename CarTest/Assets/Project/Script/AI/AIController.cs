using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public WayPointSystem waypoints;
    private DriveTest carManager;
    public float steeringSensitive = 0.01f;
    private Vector3 target;
    private int currentWaypoint = 0;
    public float minDistance;

    private void Start()
    {
        carManager = GetComponent<DriveTest>();
        target = waypoints.points[currentWaypoint];
    }

    private void Update()
    {
        Vector3 localTarget = carManager.rb.transform.InverseTransformPoint(target);
        float distanceToTarget = Vector3.Distance(target, carManager.rb.transform.position);
        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        float steer = Mathf.Clamp(targetAngle * steeringSensitive, -1, 1) * Mathf.Sign(carManager.currentSpeed);
        float accel = .5f;
        float brake = 0f;

        if (distanceToTarget < 10)
        {
            brake = 1;
            accel = 0;
        }
        
        carManager.Move(accel, steer, brake);

        if (distanceToTarget < minDistance)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.points.Length)
            {
                currentWaypoint = 0;
            }

            target = waypoints.points[currentWaypoint];
        }
        
        carManager.CheckSkidSound();
        carManager.CalculateEngineSound();
    }
}
