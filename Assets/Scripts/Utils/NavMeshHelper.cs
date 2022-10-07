using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshHelper
{
    public static Vector3 GetPosititionOnNavMeshInRange(Vector3 org, float range)
    {
        Vector3 searchLocation = org;
        searchLocation += UnityEngine.Random.Range(-range, range) * Vector3.forward;
        searchLocation += UnityEngine.Random.Range(-range, range) * Vector3.right;

        NavMeshHit hitResult;
        if (NavMesh.SamplePosition(searchLocation, out hitResult, range, NavMesh.AllAreas))
            return hitResult.position;

        return new Vector3(-1, -1, -1);
    }
}

