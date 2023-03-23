using UnityEngine;

public class Lidar_ : MonoBehaviour
{
    public int numRays = 64;
    public float maxRange = 10f;
    public float angleRange = 360f;
    public float scanRate = 120f;
    public float verticalAngleRange = 180f;

    private float angleStep;
    private float lastScanTime;

    void Start()
    {
        angleStep = angleRange / numRays;
        lastScanTime = Time.time;
    }

    void Update()
    {
        if (Time.time - lastScanTime >= 1f / scanRate)
        {
            lastScanTime = Time.time;
            CastRays();
        }
    }

    void CastRays()
    {
        for (int i = 0; i < numRays; i++)
        {
            float horizontalAngle = i * angleStep;
            Quaternion horizontalRotation = Quaternion.AngleAxis(horizontalAngle, Vector3.up);
            for (int j = 0; j < numRays; j++)
            {
                float verticalAngle = (j - numRays / 2) * verticalAngleRange / numRays;
                Quaternion verticalRotation = Quaternion.AngleAxis(verticalAngle, Vector3.right);
                Quaternion direction = horizontalRotation * verticalRotation;
                Vector3 rayDirection = direction * transform.forward;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, rayDirection, out hit, maxRange))
                {
                    Vector3 hitPoint = hit.point;
                    float distance = hit.distance;
                    // store hitPoint and distance
                }

                Debug.DrawRay(transform.position, rayDirection*maxRange, Color.green);
            }
        }
        // generate 3D point cloud based on hitPoints and distances
    }
}
