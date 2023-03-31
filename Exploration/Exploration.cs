using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OctoMapSharp;

public class Exploration : MonoBehaviour
{

    private OctoMap map;

    public float startSize;
    public float minSize;

    public void Start(){

        map = new OctoMap(new Vector3(0,0,0), startSize, minSize);

    }

    private List<List<Vector3>>? GetFrontiers()
    {
        List<List<Vector3>> frontiers;
        List<Vector3> frontier_points;


        OctoMapRaw nodes = map.GetOctoMapNodes();

        for( int i = 0; i < nodes.Positions.Length; i++)
        {

        }
        return null;

    }





}
