using System.Collections;
using System.Collections.Generic;
using AllyEscort;
using UnityEngine;

namespace AllyEscort
{
    public class MoveToPointState : State
    {
        internal List<Vector3> path;

        /// <summary>
        /// Handle the initialization of the Move To Point State
        /// </summary>
        /// <returns>If successfully initialized return <code>True</code></returns>
        protected override bool HandleInitialize()
        {
            // the first argument must be a point
            if (Args[0] is Vector3 point)
            {
                CalculatePath(point);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Handle on enter. Nothing needed here
        /// </summary>
        protected override void HandleOnEnter() { }

        /// <summary>
        /// Handle the update
        /// </summary>
        protected override void HandleUpdate()
        {
            // check to see if the path is empty or null
            if (path == null)
            {
                HandleNullPath();
                return;
            }

            if (path.Count == 0)
            {
                HandleEmptyPath();
                return;
            }

            // calculate the distance to the target
            Vector3 target = path[0];
            float distanceToGoal = CalculatePathDistance();

            // move the owner towards the target, overriding the distance
            Vector3 displacement = Owner.MoveToPoint(target, distanceToGoal);

            // if the distance is very close to zer0, remove the first point in the path
            if (displacement.sqrMagnitude <= 0.005f)
            {
                RemoveFirstPointInPath();
            }
        }

        /// <summary>
        /// Handle on exit of the state
        /// </summary>
        protected override void HandleOnExit()
        { }

        public override void SetDebugCursorPosition()
        {
            // update the debug cursor to the end of the path
            if(path != null && path.Count > 0)
                Owner.cursorTransform.position = path[path.Count - 1];
        }

        private float CalculatePathDistance()
        {
            float distance = 0;
            for (int i = 0; i < path.Count; i++)
            {
                Vector3 a = (i == 0) ? Owner.transform.position : path[i - 1]; // use the Owner's position as the starting point when calculating distance
                Vector3 b = path[i];
                distance += (b - a).magnitude;
            }

            return distance;
        }

        /// <summary>
        /// Calculate a path to the target point.
        /// </summary>
        /// <param name="targetPoint">The end point of the path</param>
        protected virtual void CalculatePath(Vector3 targetPoint)
        {
            path = Owner.calculatePathComponent.GetPath(Owner.transform.position, targetPoint);
            if (path == null)
            {
                HandleNullPath();
            }
        }

        internal void RemoveFirstPointInPath()
        {
            path.RemoveAt(0);
            if (path.Count <= 0)
            {
                HandleEmptyPath();
            }
        }

        protected virtual void HandleNullPath()
        {
            // Double check if the path is null
            if (path == null)
            {
                Owner.TransitionToState("Idle");
            }
        }

        protected virtual void HandleEmptyPath()
        {
            // Double check if the path is empty
            if (path.Count == 0)
            {
                Owner.TransitionToState("Idle");
            }
        }
    }
}
