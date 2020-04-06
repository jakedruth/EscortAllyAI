using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCalculatePath : AllyEscort.CalculatePathComponent
{
    public float closestPointRange;

    public override List<Vector3> GetPath(Vector3 start, Vector3 end)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        if (NavMesh.SamplePosition(start, out NavMeshHit startHit, closestPointRange, NavMesh.AllAreas) &&
            NavMesh.SamplePosition(end, out NavMeshHit endHit, closestPointRange, NavMesh.AllAreas) &&
            NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, navMeshPath))
        {
            List<Vector3> path = new List<Vector3>();
            path.AddRange(navMeshPath.corners);
            return path;
        }

        return null;
    }
}
