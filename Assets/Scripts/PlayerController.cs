using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    DrivingScript driveScript;


    void Start()
    {
        driveScript = GetComponent<DrivingScript>();
    }

    void Update()
    {
        float accel = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");
        float brake = Input.GetAxis("Jump");
        driveScript.Drive(accel,brake, steer);
        driveScript.EngineSound();
    }
}
