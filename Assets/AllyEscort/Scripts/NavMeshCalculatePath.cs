using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCalculatePath : CalculatePathComponent
{
    public float maxDistance;

    public override List<Vector3> GetPath(Vector3 start, Vector3 end)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        if (NavMesh.SamplePosition(start, out NavMeshHit startHit, maxDistance, NavMesh.AllAreas) &&
            NavMesh.SamplePosition(end, out NavMeshHit endHit, maxDistance, NavMesh.AllAreas) &&
            NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, navMeshPath))
        {
            List<Vector3> path = new List<Vector3>();
            path.AddRange(navMeshPath.corners);
            return path;
        }

        return null;
    }
}
