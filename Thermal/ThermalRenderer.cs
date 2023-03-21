using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalRenderer : MonoBehaviour
{

    public Shader ThermalShader;
    public Shader NormalShader;
    public float Sensitivity;
    public Texture2D ColorGradient;
    public Camera ThermalCamera;
    public Camera ClassicCamera;

    // Start is called before the first frame update
    void Start()
    {
        ClassicCamera.SetReplacementShader(NormalShader, "RenderType");
        ThermalCamera.backgroundColor = ColorGradient.GetPixel(0,0);
    }

    public Shader GetShader(){ return ThermalShader;}
    public float GetSensitivity(){ return Sensitivity;}
    public Texture2D GetGradientTexture(){return ColorGradient;}
}
