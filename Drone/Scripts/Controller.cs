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
        public float yawMax = 15f;

        public float lerpSpeed = 2f;
        private Drone_movement move;
        private List<I_Engine> engines = new List<I_Engine>();

        private float finalPitch;
        private float finalRoll;
        private float finalYaw;
        private float yaw;

        #endregion

        #region Main methods
        // Start is called before the first frame update
        void Start()
        {
            move = GetComponent<Drone_movement>();
            engines = GetComponentsInChildren<I_Engine>().ToList<I_Engine>();
            if(engines?.Any() != true)
                Debug.Log("Empty list");
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
            // Auto Hover 
            //rb.AddForce(Vector3.up * (rb.mass * Physics.gravity.magnitude));
            // Loop throug heach engine 
            foreach(I_Engine engine in engines)
            {
                engine.UpdateEngine(rb , move);
            }
        }

        protected virtual void HandleControls()
        {
            float pitch = move.Cyclic.y * minMaxPitch;
            float roll = -move.Cyclic.x * minMaxRoll;
            yaw += move.Pedals * yawMax;

            finalPitch = Mathf.Lerp(finalPitch, pitch, Time.deltaTime * lerpSpeed);
            finalRoll = Mathf.Lerp(finalPitch, roll, Time.deltaTime * lerpSpeed);
            finalYaw = Mathf.Lerp(finalPitch, yaw, Time.deltaTime * lerpSpeed);

            // Rotations 
            Quaternion rot = Quaternion.Euler(finalPitch, finalYaw, finalRoll);
            rb.MoveRotation(rot);
        }

        #endregion
    }
}