using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccupancyMap
{

    public enum Cell : sbyte
    {
        Unknown = -1,
        Empty = 0,
        Occupied = 1
    }

    private Dictionary<Vector3Int,Cell> occ_map;

    public OccupancyMap()
    {
        occ_map = new Dictionary<Vector3Int,Cell>();
    }

    public void UpdateValue(Vector3 coords, bool value)
    {
        Vector3Int int_coords = Vector3Int.RoundToInt(coords);
        Cell c_value = value ? Cell.Occupied : Cell.Empty;
        if( !occ_map.ContainsKey(int_coords) || occ_map[int_coords] == Cell.Empty)
        {
            occ_map[int_coords] = c_value;
        }
    }

    public void AddEmptyRay(Vector3 origin, Vector3 end_point, float resolution = 10)
    {
        Vector3 dir = Vector3.Normalize(end_point - origin) / resolution;
        for (int i = 0; i < resolution * Mathf.Round((origin - end_point).magnitude - 1); i++)
        {
            UpdateValue(origin + dir * i, false);
        }
    }

    public List<Vector3Int> GetAll(){
        return new List<Vector3Int>(occ_map.Keys);
    }

    public Cell GetValue(Vector3Int p){return occ_map[p];}

    public Vector3Int GetNearestFrontier( Vector3 pos)
    {
        List<List<Vector3Int>> frontiers = GetFrontiers();
        float minDist = Mathf.Infinity;
        Vector3Int curr = Vector3Int.zero;
        foreach(List<Vector3Int> l in frontiers)
        {
            foreach(Vector3Int v in l)
            {
                float dist = (pos - v).magnitude;
                if( dist < minDist)
                {
                    minDist = dist;
                    curr = v;
                }
            }
        }
        return curr;
    }


    public List<List<Vector3Int>> GetFrontiers()
    {
        List<List<Vector3Int>> frontier_groups = new List<List<Vector3Int>>();
        foreach(KeyValuePair<Vector3Int,Cell> pair in occ_map)
        {
            if(isFrontier(pair.Key) && pair.Value == Cell.Empty)
            {
                List<Vector3Int> neighbours = GetNeighbours(pair.Key);
                bool found = false;
                foreach(List<Vector3Int> frontier in frontier_groups)
                {
                    foreach(Vector3Int cell in frontier)
                    {
                        if (neighbours.Contains(cell))
                        {
                            frontier.Add(pair.Key);
                            found = true;
                            break;
                        }
                    }
                }
                if( !found)
                {
                    List<Vector3Int> frontier = new List<Vector3Int>();
                    frontier.Add(pair.Key);
                    frontier_groups.Add(frontier);
                }
            }
        }
        return frontier_groups;
    }

    public int GetCount()
    {
        return occ_map.Count;
    }

    // ValidNeighboors
    private List<Vector3Int> GetNeighbours(Vector3Int position)
    {
        List<Vector3Int> ret = new List<Vector3Int>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                for(int z = -1; z <= 1; z++)
                {
                    if(x==0 || y==0 || z==0){continue;}
                    Vector3Int new_position = position + new Vector3Int(x,y,z);
                    ret.Add(new_position);
                }
            }
        }
        return ret;
    }

    private bool isFrontier(Vector3Int position)
    {
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                for(int z = -1; z <= 1; z++)
                {
                    if(x==0 || y==0 || z==0){continue;}
                    Vector3Int new_position = position + new Vector3Int(x,y,z);
                    if(!occ_map.ContainsKey(new_position))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // return valid neighboors for A*
    private List<Vector3Int> GetValidNeighbours(Vector3Int position)
    {
        List<Vector3Int> ret = GetNeighbours(position);

        bool IsNotFree(Vector3Int pos)
        {
            Cell type = occ_map.GetValueOrDefault(pos,Cell.Unknown);
            return (sbyte)type < 1;
        }

        ret.RemoveAll(IsNotFree);
        return ret;
    }

    private List<Vector3Int> AStar(Vector3 start, Vector3 end)
    {
        Vector3Int start_i = Vector3Int.RoundToInt(start);
        Vector3Int end_i = Vector3Int.RoundToInt(end);

        // Data storage structures
        Dictionary<Vector3Int, float> cost_list = new Dictionary<Vector3Int, float>();
        Dictionary<Vector3Int, Vector3Int> previous_list = new Dictionary<Vector3Int, Vector3Int>();
        List<Vector3Int> ongoing_list = new List<Vector3Int>();
        PriorityQueue<Vector3Int, float> explore_list = new PriorityQueue<Vector3Int, float>();

        // Setting up initial Node
        cost_list[start_i] = 0;
        previous_list[start_i] = start_i;
        ongoing_list.Add(start_i);
        explore_list.Enqueue(start_i, (end_i - start_i).magnitude);

        // Path Retrieval Function
        List<Vector3Int> ReconstructPath(Vector3Int node)
        {
            float total_cost = cost_list[end_i];
            List<Vector3Int> nodes = new List<Vector3Int>();
            nodes.Add(end_i);
            Vector3Int curr = node;
            while (curr != start_i)
            {
                curr = previous_list[curr];
                nodes.Add(curr);
            }
            nodes.Reverse();
            return nodes;
        }

        while (explore_list.Count > 0)
        {
            Vector3Int node = explore_list.Dequeue();
            ongoing_list.Remove(node);

            if (node.Equals(end_i))
            {
                return ReconstructPath(node);
            }

            List<Vector3Int> neighbours = GetValidNeighbours(node);

            foreach (Vector3Int neighbour in neighbours)
            {
                // Cost is: Distance To Previous Node + Cost of previous Node
                float cost = (neighbour - node).magnitude + cost_list[node];

                // If we found a better than current path to neighbour, we record it
                if (cost < cost_list.GetValueOrDefault(neighbour, Mathf.Infinity))
                {
                    cost_list[neighbour] = cost;
                    previous_list[neighbour] = node;
                    float heuristic = cost + (end_i - neighbour).magnitude;
                    if (!ongoing_list.Contains(neighbour))
                    {
                        ongoing_list.Add(neighbour);
                        explore_list.Enqueue(neighbour,heuristic);
                    }
                }
            }
        }

        return null;
    }

}
