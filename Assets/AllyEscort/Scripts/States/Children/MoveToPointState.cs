using System.Collections;
using System.Collections.Generic;
using AllyEscort;
using UnityEngine;

namespace AllyEscort
{
    public class MoveToPointState : State
    {
        internal List<Vector3> path;
        protected Vector3 previousPathPoint;

        /// <summary>
        /// Handle the initialization of the Move To Point State
        /// </summary>
        /// <returns>If successfully initialized return <code>True</code></returns>
        protected override bool HandleInitialize()
        {
            // the first argument must be a Vector3 point
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
        protected override void HandleOnEnter()
        { }

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

            Plane plane = new Plane(path[0] - previousPathPoint, Position);
            if (!plane.GetSide(path[0]))
            {
                RemoveFirstPointInPath();
                if (path.Count == 0)
                {
                    HandleEmptyPath();
                    return;
                }
            }

            // get the target point
            Vector3 target = path[0];
            Vector3 delta = target - Position;
            
            // Set the input
            delta.y = 0;
            Vector3 input = delta.normalized;

            // dampen input if using smooth stopping
            if (Owner.useSmoothStopping)
            {
                float distanceToGoal = CalculatePathDistance();
                if (distanceToGoal <= Owner.useSmoothStopping.value)
                {
                    float percentage = distanceToGoal / Owner.useSmoothStopping.value;
                    input *= percentage;

                    if (distanceToGoal <= 0.005f)
                    {
                        RemoveFirstPointInPath();
                        Position = target;
                        Input = Vector3.zero;
                        return;
                    }
                }
            }

            // set the input
            Input = input;

            if (delta.sqrMagnitude <= 0.005f)
            {
                RemoveFirstPointInPath();
                Position = target;
            }
        }

        /// <summary>
        /// Handle on exit of the state
        /// </summary>
        protected override void HandleOnExit()
        { }

        public override void HandleDebugCursorPosition()
        {
            // update the debug cursor to the end of the path
            if (path != null && path.Count > 0)
            {
                Owner.cursorTransform.position = path[path.Count - 1];
                //for (int i = 0; i < path.Count; i++)
                //{
                //    Vector3 a = (i == 0) ? Owner.transform.position : path[i - 1]; // use the Owner's position as the starting point when calculating distance
                //    Vector3 b = path[i];
                //    Debug.DrawLine(a, b, Color.yellow, 0.01f, false);
                //}
            }
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
            path = Owner.calculatePathComponent.GetPath(Position, targetPoint);
            if (path == null)
            {
                HandleNullPath();
                return;
            }

            previousPathPoint = Position;
        }

        /// <summary>
        /// Shorten the path length
        /// </summary>
        /// <param name="length">The amount distance to be removed</param>
        protected void ShortenPath(float length)
        {
            float remainingDistance = length;
            for (int i = path.Count - 1; i >= 0; i--)
            {
                Vector3 a = (i == 0) ? Position : path[i - 1];
                Vector3 b = path[i];
                Vector3 delta = b - a;
                float distance = delta.magnitude;
                if (distance > remainingDistance)
                {
                    path[i] = Vector3.MoveTowards(b, a, remainingDistance);
                    break;
                }

                remainingDistance -= distance;
                path.RemoveAt(i);
            }
        }

        internal void RemoveFirstPointInPath()
        {
            previousPathPoint = path[0];
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
