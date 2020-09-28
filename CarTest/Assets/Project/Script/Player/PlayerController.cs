﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private DriveTest carManager;

    private void Start()
    {
        carManager = GetComponent<DriveTest>();
    }

    private void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        float brake = Input.GetAxis("Jump");
        carManager.Move(vertical, horizontal, brake);
        carManager.CheckSkidSound();
        carManager.CalculateEngineSound();
    }
}
