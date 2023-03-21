using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationSimulator : MonoBehaviour
{
    private List<CommunicationManager> coms;

    public bool DEBUG;
    // Start is called before the first frame update
    void Start()
    {
        coms = new List<CommunicationManager>();
        Physics.queriesHitBackfaces = true;
    }

    // Adding and removing drones communication
    public void addCommunication(CommunicationManager c)
    {
        this.coms.Add(c);
    }

    public void removeCommunication(CommunicationManager c)
    {
        this.coms.Remove(c);
    }

    // Communication Options

    public void broadcast( CommunicationManager origin, string s)
    {
        foreach(CommunicationManager c in coms)
        {
            if(c != origin )
                send(origin,c,s);
        }
    }

    // Communication checks

    public bool send(CommunicationManager origin, CommunicationManager destination, string message)
    {
        // Defining usefull variables
        Vector3 pos_o = origin.gameObject.transform.position;
        Vector3 pos_d = destination.gameObject.transform.position;
        float distance = (pos_d - pos_o).magnitude;

        // Debug line draw
        if(this.DEBUG){Debug.DrawLine(pos_o, pos_d, Color.red);}

        // Create hits arrays
        RaycastHit[] hits_forwards, hits_backwards;
        hits_forwards  = Physics.RaycastAll(pos_o, pos_d - pos_o, distance);
        hits_backwards = Physics.RaycastAll(pos_d, pos_o - pos_d, distance);

        //Adding all hits to a List
        List<(bool, RaycastHit)> hits = new List<(bool, RaycastHit)>();
        foreach(RaycastHit h in hits_forwards){ hits.Add((true,h));}
        foreach(RaycastHit h in hits_backwards){ hits.Add((false,h));}

        // Defining distance to origin function
        int Compare((bool,RaycastHit) h1,(bool,RaycastHit) h2)
        {
            float dist1 = (h1.Item2.point - pos_o).magnitude;
            float dist2 = (h2.Item2.point - pos_o).magnitude;
            return dist1.CompareTo(dist2);
        }

        // Sorting list to have front and back hits
        hits.Sort((a,b)=>Compare(a,b));

        // Sorting and getting materials passed through
        // WARNING Algorithm false when interpenetration occurs
        int current_open = 0;
        Vector3 last_pos = pos_o;
        List<(float,string)> ret = new List<(float,string)>();
        Stack<int> waiting = new Stack<int>();

        for( int i = 0; i < hits.Count; i++)
        {
            if(hits[i].Item1)
            {
                current_open += 1;
                waiting.Push(i);
                //Air between materials
                if (current_open == 1)
                {
                    Vector3 pos = hits[i].Item2.point;
                    ret.Add(((pos - last_pos).magnitude,"air"));
                }
            }
            else
            {
                if(current_open == 0){print("ERROR, Non Matching faces"); return false;}
                else
                {
                    current_open -= 1;
                    Vector3 pos1 = hits[waiting.Pop()].Item2.point;
                    Vector3 pos2 = hits[i].Item2.point;
                    string name = hits[i].Item2.collider.gameObject.name;
                    ret.Add(((pos2 - pos1).magnitude,name));
                    if(current_open == 0){last_pos = pos2;}
                }
            }
        }
        if(waiting.Count > 0){print("ERROR, DRONE INSIDE OBJECT");}
        ret.Add(((pos_d - last_pos).magnitude,"air"));

        // Debug drawing of impacts and material print
        if(this.DEBUG){
            print(origin.gameObject.name + "  - hits : " + hits.Count);
            foreach ((bool,RaycastHit) h in hits){
                Debug.DrawLine(transform.position, h.Item2.point, Color.blue);
            }

            print("Went through :");
            foreach ((float,string) d in ret){
                print(d.Item1 * 100.0f + " cm of " + d.Item2);
            }
        }

        // Signal Force calculation
        float weighted_distance = 0;
        foreach((float,string) material in ret)
        {
            float factor = ComsUtils.material_absorption.GetValueOrDefault(material.Item2,5.0f);
            weighted_distance += (material.Item1 * factor );
            if(this.DEBUG){
                print("intensity after " + material.Item1 + " m of " + material.Item2 + " = " + (origin.emmission_power/(weighted_distance*weighted_distance)));
            }
        }
        float final_strength = origin.emmission_power / (weighted_distance*weighted_distance);

        // Debug print signal strength
        if(this.DEBUG){ print("Endpoint intensity : " + final_strength);}

        // Decide if com is valid
        if(final_strength > destination.reception_power){
            destination.recieve(message);
            if(this.DEBUG){Debug.DrawLine(pos_o, pos_d, Color.green);}
            return true;
        }
        return false;

    }


}
