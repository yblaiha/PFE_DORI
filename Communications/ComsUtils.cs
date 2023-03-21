using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComsUtils
{

    public static Dictionary<string, float> material_absorption = new Dictionary<string, float>()
    {
        { "air", 1 },
        { "beton", 5 },
        { "bois", 2 },
        { "plomb", 1000}
    };

}
