using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrivingScript : MonoBehaviour
{
    public WheelScript[] wheels; // nasze 4 kola
    public float torque = 200; // moment obrotowy
    public float maxSteerAngle = 30;
    public float maxBrakeTorque = 500;
    public float maxSpeed = 200;
    public Rigidbody rb;
    public float currentSpeed;
    public GameObject backLights;


    public void Drive(float accel, float brake, float steer)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;

        if (brake != 0)
            backLights.SetActive(true);
        else
            backLights.SetActive(false);

        float thrustTorque = 0;
        if (currentSpeed < maxSpeed)
        {
            thrustTorque = accel * torque;
        }

        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = thrustTorque;

            if (wheel.frontWheel)
                wheel.wheelCollider.steerAngle = steer;
            else
                wheel.wheelCollider.brakeTorque = brake;


            Quaternion quat;
            Vector3 position;

            wheel.wheelCollider.GetWorldPose(out position, out quat);
            wheel.wheel.transform.position = position;
            wheel.wheel.transform.rotation = quat;
        }
    }

}
