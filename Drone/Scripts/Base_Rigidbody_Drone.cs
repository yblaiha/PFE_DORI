using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dori
{
    [RequireComponent(typeof(Rigidbody))]
    public class Base_Rigidbody_Drone : MonoBehaviour
    {

        #region Variables
        [Header("Rigidbody Properties")]
        public float weight = 1f;
        
        
        protected Rigidbody rb;
        protected float startDrag;
        protected float startAngularDrag;
        #endregion

        // Start is called before the first frame update
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            if(rb){ rb.mass = weight;}
            startDrag = rb.drag;
            startAngularDrag = rb.angularDrag;

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // If no rigidbody there is nothing to do
            if(!rb)
            {
                return;
            }

            // Handling physics
            HandlePhysics();
        }


        #region Custion Methods
            protected virtual void HandlePhysics()
            {

            } 

        #endregion
    }
}