using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AllyEscort
{
    public class EscortPathFinding : MonoBehaviour
    {
        public Vector3[] GetPathToPoint(Vector3 target)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit startHit, 1f, NavMesh.AllAreas) &&
                NavMesh.SamplePosition(target, out NavMeshHit endHit, 1f, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, path))
                    return path.corners;
            }

            return null;
        }
    }
}