using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureManager : MonoBehaviour
{
    // Temperature difference with the environment
    public float temperature;
    public float diffusion_factor;

    // Cooling or not
    public bool self_generating;
    public float thermal_capacity;
    public float density;

    public Vector3 offset;

    public ThermalRenderer thermal_renderer;

    // internal parameters
    private float surface;
    private float volume;
    private float coef_k;
    private float sensitivity;
    private Mesh mesh;



    // Start is called before the first frame update
    void Start()
    {
        // Setting Mesh Parameter
        MeshFilter mf = gameObject.GetComponent<MeshFilter>();
        SkinnedMeshRenderer smr = gameObject.GetComponent<SkinnedMeshRenderer>();
        if ( mf != null)
            mesh = mf.mesh;
        else
        {
            if( smr != null)
                mesh = smr.sharedMesh;
            else
            {
                mesh = null;
                print("Error, Object without Mesh");
            }
        }

        // If cooling, compute surface and volume
        if (!self_generating){
            surface = CalculateSurfaceArea();
            volume = CalculateVolume();
            coef_k = (220 * surface) / (density * volume * thermal_capacity);
        }

        print("Analyze object "+name);
        print("Area : "+surface);
        print("Volume : "+volume);
        print("Coef k : "+coef_k);


        sensitivity = thermal_renderer.GetSensitivity();
        Shader temperature_shader = thermal_renderer.GetShader();
        Texture2D gradient_texture = thermal_renderer.GetGradientTexture();

        // Replace material shader with Thermal render one
        Material mat = gameObject.GetComponent<Renderer>().material;
        mat.shader = temperature_shader;
        mat.SetTexture("_ThermalGradient",gradient_texture);
        mat.SetFloat("_Sensitivity",sensitivity);
        mat.SetFloat("_DiffusionFactor",diffusion_factor);
        mat.SetVector("_Offset",offset);

    }

    // Update is called once per frame
    void Update()
    {
        if(! self_generating)
        {
            float dT = -1 * coef_k * temperature *  Time.deltaTime;
            temperature += dT;
        }

        gameObject.GetComponent<Renderer>().material.SetFloat("_Temperature",temperature);


        // Debug purpose DEBUG
        coef_k = (220 * surface) / (density * volume * thermal_capacity);
        sensitivity = thermal_renderer.GetSensitivity();
        gameObject.GetComponent<Renderer>().material.SetFloat("_Sensitivity",sensitivity);

    }

    private float CalculateSurfaceArea() {

        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        double sum = 0.0;

        for(int i = 0; i < triangles.Length; i += 3) {
            Vector3 corner = vertices[triangles[i]];
            Vector3 a = vertices[triangles[i + 1]] - corner;
            Vector3 b = vertices[triangles[i + 2]] - corner;

            sum += Vector3.Cross(a, b).magnitude;
        }
        return (float)(sum/2.0 * (transform.localScale.x * transform.localScale.x));
    }

    private float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    private float CalculateVolume()
    {
        float volume = 0;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume)* (transform.localScale.x * transform.localScale.x * transform.localScale.x);
    }
}
