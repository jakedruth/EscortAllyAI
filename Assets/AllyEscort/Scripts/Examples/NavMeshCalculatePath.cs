using System.Collections;
using System.Collections.Generic;
using AllyEscort;
using UnityEngine;
using UnityEngine.AI;

namespace AllyEscort.Example
{
    /// <summary>
    /// This is an example class on how to implement the calculate path component with a nav mesh.
    /// </summary>
    public class NavMeshCalculatePath : CalculatePathComponent
    {
        /// <summary>
        /// Finds the closest point on the NavMesh in range of this value
        /// </summary>
        public float closestPointRange;

        /// <summary>
        /// Get a path between two points
        /// </summary>
        /// <param name="start">The start point of the path</param>
        /// <param name="end">The end point of the path</param>
        /// <returns>A list of points that connects start and end. Returns <code>null</code> if no path exists</returns>
        public override List<Vector3> GetPath(Vector3 start, Vector3 end)
        {
            // create a new NavMeshPath
            NavMeshPath navMeshPath = new NavMeshPath();

            // See if a point can be found at the start and end
            // And if a path exists between the two points
            if (NavMesh.SamplePosition(start, out NavMeshHit startHit, closestPointRange, NavMesh.AllAreas) &&
                NavMesh.SamplePosition(end, out NavMeshHit endHit, closestPointRange, NavMesh.AllAreas) &&
                NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, navMeshPath))
            {
                // extract the navMeshPath.corners and return the list
                List<Vector3> path = new List<Vector3>();
                path.AddRange(navMeshPath.corners);
                return path;
            }

            // Return null if no path exists
            return null;
        }
    }
}