using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dori
{
    [RequireComponent(typeof(BoxCollider))]
    public class Engine : MonoBehaviour, I_Engine
    {
        
        #region variables
        [Header("Engine Properties")]
        public float maxPower = 4f;

        [Header("Propeller Properties")]
        public Transform propeller; 
        public float propellerRot = 100f;
        private float speed = 0f;
        #endregion

        #region Interface methods

            public void InitEngine()
            {
                throw new System.NotImplementedException();
            }

            public void UpdateEngine(Rigidbody rb, Drone_movement move)
            {
                
                Vector3 upVec = transform.up;
                upVec.x = 0f;
                upVec.z = 0f;

                float diff = 1 - upVec.magnitude;
                float finalDiff = Physics.gravity.magnitude * diff;

                Vector3 engineForce = Vector3.zero;

                // Calculate the engine force by adding the weight of the drone (mass * gravity)
                // to the final difference and multiplying it by the throttle and max power,
                // then dividing by 4 engines
                engineForce = transform.up * ((rb.mass * Physics.gravity.magnitude + finalDiff) + (move.Throttle * maxPower)) / 4f;
                rb.AddForce(engineForce, ForceMode.Force);
                HandlePropellers(move.Throttle);
            }

            void HandlePropellers(float throttle)
            {
                if(!propeller)
                {
                    return;
                }
                
                if(throttle > 0)
                    speed += throttle * propellerRot;
                else if ( speed > propellerRot)
                    speed -= propellerRot; 
                    
                propeller.Rotate(Vector3.up, speed);
            }
        #endregion
    }
}