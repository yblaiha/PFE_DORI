using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dori
{
    [RequireComponent(typeof(PlayerInput))]
    public class Drone_movement : MonoBehaviour
    {

        #region Variables
            private Vector2 cyclic;
            private float pedals;
            private float throttle;

            public Vector2 Cyclic { get => cyclic; }
            public float Pedals { get => pedals; }
            public float Throttle { get => throttle;  }
        #endregion

       

        #region Input
        private void setCyclic(Vector2 value)
        {
            cyclic = value;
        }
        private void setPedals(float value)
        {
            pedals = value;
        }
        private void setThrottle(float value)
        {
            throttle = value;
        }


        private void OnCyclic(InputValue value)
        {
            cyclic = value.Get<Vector2>();
        }
        private void OnPedals(InputValue value)
        {
            pedals = value.Get<float>();
        }
        private void OnThrottle(InputValue value)
        {
            throttle = value.Get<float>();
        }


        #endregion
    }
}