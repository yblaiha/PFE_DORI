using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using OctoMapSharp;

public class Lidar_ : MonoBehaviour
{
    public int numRays = 64;
    public float maxRange = 10f;
    public float angleRange = 360f;
    public float scanRate = 120f;
    public float verticalAngleRange = 180f;


    public bool draw = false;

    private float angleStep;
    private float lastScanTime;
    private float last_frontier;

    public GameObject emptyGO, fullGO, parent;

    private OccupancyMap map;

    private Vector3 objective;
    private Rigidbody rb;
    public float speed = 5f;
    public float stoppingDistance = 0.1f;
    public float ratio = 3f;

    void Start()
    {
        angleStep = angleRange / numRays;
        lastScanTime = Time.time;
        last_frontier = Time.time;
        map = new OccupancyMap();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        if (Time.time - last_frontier >= 2)
        {
            //Vector3 pos = (Vector3)map.GetNearestFrontier(transform.position/ratio)*ratio;
            //print(" Nearest Frontier = "+pos);
            //transform.position = pos;
            //objective = pos;
            last_frontier = Time.time;
            if(draw){DisplayMap();}
        }

        if (Time.time - lastScanTime >= 1f / scanRate)
        {
            lastScanTime = Time.time;
            CastRays();
            //print("Cells n° : " + map.GetCount());
        }
    }

    void FixedUpdate()
    {

        //Vector3 direction = objective - transform.position;

        // Apply force in direction of destination
        //Vector3 force = direction.normalized * speed;
        //rb.AddForce(force, ForceMode.Force);

    }


    void DisplayMap()
    {
        foreach (Transform child in parent.transform) {
             GameObject.Destroy(child.gameObject);
         }

        Vector3 side = new Vector3(200,0,0);

        /*List<List<Vector3Int>> frontiers = map.GetFrontiers();
        foreach( List<Vector3Int> l in frontiers){
            foreach(Vector3Int v in l){
                GameObject obj = Instantiate(emptyGO, (Vector3)v * ratio + side, Quaternion.identity,parent.transform);
                obj.transform.localScale = new Vector3(ratio,ratio,ratio);
            }
        } */
        foreach( Vector3Int v in map.GetAll()){
            if(map.GetValue(v) == OccupancyMap.Cell.Occupied){
                GameObject obj = Instantiate(fullGO, (Vector3)v * ratio + side, Quaternion.identity,parent.transform);
                obj.transform.localScale = new Vector3(ratio,ratio,ratio);
            }
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
                Ray ray = new Ray(transform.position, rayDirection);
                if (Physics.Raycast(ray, out hit, maxRange))
                {
                    Vector3 hitPoint = hit.point / ratio;
                    map.AddEmptyRay(transform.position/ratio, hitPoint);
                    map.UpdateValue(hitPoint, true);
                    float distance = hit.distance;
<<<<<<< Updated upstream
=======
                    Debug.DrawRay(transform.position, rayDirection*distance, Color.red);
                }
                else
                {
                    Debug.DrawRay(transform.position, rayDirection*maxRange, Color.green);
                    map.AddEmptyRay(transform.position/ratio, transform.position/ratio + rayDirection*maxRange/ratio);
>>>>>>> Stashed changes
                }
            }
        }
    }
}
