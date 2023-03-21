using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalRenderer : MonoBehaviour
{

    public Shader ThermalShader;
    public Shader NormalShader;
    public float Sensitivity;
    public Texture2D ColorGradient;

    // Start is called before the first frame update
    void OnEnable()
    {
        GetComponent<Camera>().SetReplacementShader(NormalShader, "RenderType");
    }

    public Shader GetShader(){ return ThermalShader;}
    public float GetSensitivity(){ return Sensitivity;}
    public Texture2D GetGradientTexture(){return ColorGradient;}
}
