using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationManager : MonoBehaviour
{

    public float emmission_power;
    public float reception_power;

    public bool broadcasting;

    public CommunicationSimulator coms;

    // Start is called before the first frame update
    void Start()
    {
        coms.addCommunication(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (broadcasting)
            broadcast("test");
    }

    public void recieve(string s)
    {
        print(s);
    }

    public bool send(CommunicationManager target, string message)
    {
        return coms.send(this,target,message);
    }

    public void broadcast(string message)
    {
        coms.broadcast(this,message);
    }

}
