using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dori
{
    // This class inherits from Base_Rigidbody_Drone and
    // adds control functionality to the drone
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
            // Get a list of all the I_Engine components that are children
            engines = GetComponentsInChildren<I_Engine>().ToList<I_Engine>();
            if(engines?.Any() != true)
                Debug.Log("Empty list");
        }


        void Update(){
            //test_GoTo();
        }

        // Fonction de test GoToDrone
        /*void test_GoTo(){
            if(reach1 == 0){
                GoTo_Drone(point1);
                if(ReachedDestination(point1))
                    {   Debug.Log("Drone Reached point 1" + point1);
                        reach1 = 1;}
               } else if(reach2 == 0 && reach1 == 1){
                GoTo_Drone(point2);
                if(ReachedDestination(point2))
                    {   Debug.Log("Drone Reached point 2" + point2);
                        reach2 = 1;}
            } else if(reach3 == 0 && reach2 == 1 && reach1 == 1){
                GoTo_Drone(point3);
                if(ReachedDestination(point3))
                    {   Debug.Log("Drone Reached point 3" + point3);
                        reach3 = 1;}
            }
        }*/

        #endregion

        #region Custom methods

        protected override void HandlePhysics()
        {
            HandleEngines();
            HandleControls();
        }

         // This method updates each engine component in the list of engines
        protected virtual void HandleEngines()
        {
            // Loop through each engine
            foreach(I_Engine engine in engines)
            {
                engine.UpdateEngine(rb , move);
            }
        }

        // This method handles the movement controls of the drone
        protected virtual void HandleControls()
        {
            // Calculate the pitch, roll, and yaw values based on the input
            float pitch = move.Cyclic.y * minMaxPitch;
            float roll =-move.Cyclic.x * minMaxRoll;
            yaw += move.Pedals * yawMax;

            // Smoothly interpolate the pitch, roll, and yaw values
            finalPitch = Mathf.Lerp(finalPitch, pitch, Time.deltaTime * lerpSpeed);
            finalRoll = Mathf.Lerp(finalRoll, roll, Time.deltaTime * lerpSpeed);
            finalYaw = Mathf.Lerp(finalYaw, yaw, Time.deltaTime * lerpSpeed);

            // Calculate drone rotation based on the pitch, yaw, and roll
            Quaternion rot = Quaternion.Euler(finalPitch, finalYaw, finalRoll);
            rb.MoveRotation(rot);
        }

        // This method verifies if the drone have reached destination
        public bool ReachedDestination(Vector3 waypoint, float threshold = 0.1f)
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

            Debug.Log("Distance : " + distanceToDestination);

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

            float yaw = 0f;
            float roll = 0f;
            float pitch = 0f;

            if (Mathf.Abs(angleDifference) > 5)
            {
                yaw = -Mathf.Sign(angleDifference) * 0.4f;
                pitch = 0;
                roll = 0;
            }
            else if (!ReachedDestination(destinationPoint))
            {
                yaw = 0;
                roll = 0;
                pitch = 0.1f;
            }
            else
            {
                roll = 0f;
                pitch = 0f;
                yaw = 0f;
            }

            Vector2 cyclic = new Vector2(roll,pitch);
            move.setCyclic(cyclic);
            move.setPedals(yaw);
            move.setThrottle(verticalDifference);
        }
        #endregion
    }
}
