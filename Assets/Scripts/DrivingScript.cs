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


    public AudioSource engineSound;
    float rpm;
    public int currentGear = 1;
    public float currentGearPerc;
    public int numGears = 5;
    public float gearLength = 5f;




    public void EngineSound()
    {
        float gearPercentage = (1 / (float)numGears);
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage *
        (currentGear + 1), Mathf.Abs(currentSpeed / maxSpeed));
        //określamy ile mocy naszego biegu mamy, jak dojdzie do 100 lub 0 będziemy zmieniać bieg
        currentGearPerc = Mathf.Lerp(currentGearPerc, targetGearFactor, Time.deltaTime * 5f);
        // wyliczamy obroty silnika oraz na jakiej prędkości zmieniamy bieg
        var gearNumFactor = currentGear / (float)numGears;
        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearPerc);
        float speedPercentage = Mathf.Abs(currentSpeed / maxSpeed);
        float upperGearMax = (1 / (float)numGears) * (currentGear + 1);
        float downGearMax = (1 / (float)numGears) * currentGear;
        //zmiana biegów
        if (currentGear > 0 && speedPercentage < downGearMax) currentGear--;
        if (speedPercentage > upperGearMax && (currentGear < (numGears - 1))) currentGear++;
        //zmiana dźwięku
        float pitch = Mathf.Lerp(1, 6, rpm);
        engineSound.pitch = Mathf.Min(6, pitch) * 0.15f;
    }


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

        currentSpeed = rb.velocity.magnitude * 5;

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
