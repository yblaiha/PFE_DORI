using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnAround : MonoBehaviour
{
    public float angularSpeed = 90f; // Degrees per second

    void Update()
    {
        // Calculate rotation based on angular speed and time
        float rotationAmount = angularSpeed * Time.deltaTime;

        // Apply rotation to object
        transform.Rotate(Vector3.up, rotationAmount);
    }
}
