using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dori
{
    public interface I_Engine 
    {
        void InitEngine();
        void UpdateEngine(Rigidbody rb,Drone_movement move);
    }
}