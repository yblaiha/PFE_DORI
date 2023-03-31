using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAround : MonoBehaviour
{
    public Transform target; // the object to orbit around
    public Vector3 targetOffset; // the offset to apply to the target position
    public Vector3 centerOffset; // the offset to apply to the rotation center
    public float distance = 5.0f; // the distance from the target
    public float xSpeed = 120.0f; // the rotation speed around the x-axis
    public float ySpeed = 120.0f; // the rotation speed around the y-axis

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void Update()
    {
        x += Time.deltaTime * xSpeed;
        y -= Time.deltaTime * ySpeed;

        Quaternion rotation = Quaternion.Euler(y, x, 0);

        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position + targetOffset + centerOffset;

        transform.rotation = rotation;
        transform.position = position;
    }
}
