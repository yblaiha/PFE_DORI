using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dori
{
    [RequireComponent(typeof(Drone_movement))]
    public class Controller : Base_Rigidbody_Drone
    {

        #region variables
        [Header("Control Properties")]
        public float minMaxPitch = 30f;
        public float minMaxRoll = 30f;
        public float yawMax = 7f;



        public float lerpSpeed = 2f;
        private Drone_movement move;
        private List<I_Engine> engines = new List<I_Engine>();

        private float finalPitch;
        private float finalRoll;
        private float finalYaw;
        private float yaw;

        #endregion

        #region Main methods

        void Start()
        {
            move = GetComponent<Drone_movement>();
            engines = GetComponentsInChildren<I_Engine>().ToList<I_Engine>();
            if(engines?.Any() != true)
                Debug.Log("Empty list");
        }

        void Update(){
            float x = 10.0f;
            float y = 1.0f;
            float z = 0.0f;
            Vector3 point = new Vector3(x, y, z);
           // GoTo_Drone(point);

        }
       
        #endregion

        #region Custom methods

        protected override void HandlePhysics()
        {
            HandleEngines();
            HandleControls();
        }

        protected virtual void HandleEngines()
        {
            // Loop through each engine 
            foreach(I_Engine engine in engines)
            {
                engine.UpdateEngine(rb , move);
            }
        }

        protected virtual void HandleControls()
        {
            float pitch = move.Cyclic.y * minMaxPitch;
            float roll =-move.Cyclic.x * minMaxRoll;
            yaw += move.Pedals * yawMax;

            finalPitch = Mathf.Lerp(finalPitch, pitch, Time.deltaTime * lerpSpeed);
            finalRoll = Mathf.Lerp(finalRoll, roll, Time.deltaTime * lerpSpeed);
            finalYaw = Mathf.Lerp(finalYaw, yaw, Time.deltaTime * lerpSpeed);

            // Rotations 
            Quaternion rot = Quaternion.Euler(finalPitch, finalYaw, finalRoll);
            rb.MoveRotation(rot);
        }

        public bool ReachedDestination(Vector3 waypoint, float threshold = 1f)
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, waypoint);
            return (distanceToWaypoint <= threshold);
        }

        // This function takes in a destination point and moves the drone towards it
        public void GoTo_Drone(Vector3 destinationPoint){

            GameObject drone = GameObject.FindGameObjectWithTag("Player");
            Vector3 currentPosition = drone.transform.position;

            // Calculate the direction and distance to the destination point
            Vector3 direction = destinationPoint - currentPosition;
            float verticalDifference = direction.y;
            float distanceToDestination = direction.magnitude;


            // Adjust the vertical difference to a maximum of 1 or -1 to prevent excessive climbing or diving
            float absVerticalDifference = Mathf.Abs(verticalDifference);
            if (absVerticalDifference > 1.0f)
            {
                verticalDifference /= absVerticalDifference;
            }

            direction.Normalize();
            if (distanceToDestination < 10f)
            {
                float ratio = distanceToDestination / 10.0f;
                direction *= ratio;
                verticalDifference /= 2f;
            }

            // Calculate the angle between the drone's current forward direction and the direction towards the destination point
            Vector2 droneToDestination = new Vector2(destinationPoint.x - currentPosition.x, destinationPoint.z - currentPosition.z);
            float angleToDestination = Vector2.SignedAngle(Vector2.right, droneToDestination);
            float currentRobotAngle = Vector2.SignedAngle(Vector2.right, new Vector2(transform.forward.x, transform.forward.z));
            float angleDifference = Mathf.DeltaAngle(currentRobotAngle, angleToDestination);

            // Set the yaw, roll, and pitch based on the angle difference and the distance to the destination point
            float yaw = (angleDifference != 0) ? -Mathf.Sign(angleDifference) * 0.4f : 0f;
            float roll = (angleDifference == 0 && ReachedDestination(destinationPoint)) ? 0f : 0f;
            float pitch = (angleDifference == 0 && !ReachedDestination(destinationPoint)) ? Mathf.Max(distanceToDestination / 10f + 0.01f, 1f) : 0f;

            Vector2 cyclic = new Vector2(roll,pitch);
            move.setCyclic(cyclic);
            move.setPedals(yaw);
            move.setThrottle(verticalDifference);
        }
        #endregion
    }
}