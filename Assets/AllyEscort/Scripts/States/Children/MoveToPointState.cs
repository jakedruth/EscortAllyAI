using System.Collections;
using System.Collections.Generic;
using AllyEscort;
using UnityEngine;

namespace AllyEscort
{
    public class MoveToPointState : State
    {
        [Header("Variables")]
        public float maxSpeed;
        public float minSpeed;
        internal float currentSpeed;
        public float acceleration;
        public bool slowDownToTarget;
        public float slowDownRange;

        internal List<Vector3> path;

        internal override bool HandleInitialize()
        {
            if (Args[0] is Vector3 point)
            {
                CalculatePath(point);
                return true;
            }

            return false;
        }

        internal override void HandleOnEnter()
        {
            currentSpeed = minSpeed;
        }

        internal override void HandleUpdate()
        {
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

            // Set up variables
            Vector3 pos = Owner.transform.position;
            Vector3 targetPos = path[0];
            Vector3 delta = targetPos - pos;

            // Update the speed
            currentSpeed = GetSpeed();

            // Calculate the velocity
            Vector3 velocity = delta.normalized * currentSpeed;

            // Calculate the step the Owner will take this frame
            Vector3 step = velocity * Time.deltaTime;

            // If the step is greater than the distance
            if (step.sqrMagnitude > delta.sqrMagnitude)
            {
                // set the step equal to the distance, so it does not over step
                step = delta;
            }

            // Move the Owner
            //pos += step;
            //Owner.transform.position = pos;
            Owner.CharacterController.Move(step);

            // Check to see if the Owner is at the target, or very close
            if (delta.sqrMagnitude <= 0.01f)
            {
                RemoveFirstPointInPath();
            }
        }

        internal float GetSpeed()
        {
            // Calculate the new speed
            float speed =  Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

            if (slowDownToTarget)
            {
                // get the displacement from the Owner to the end point
                Vector3 delta = path[path.Count - 1] - Owner.transform.position;

                if (delta.sqrMagnitude < slowDownRange * slowDownRange)
                {
                    // Calculate the actual distance of the path
                    float distance = CalculatePathDistance();
                    if (distance <= slowDownRange)
                    {
                        // update the speed based on the range
                        float fractionWithinRange = delta.magnitude / slowDownRange;
                        speed = fractionWithinRange * maxSpeed;
                    }
                }
            }

            // Make sure the speed is never lower than the minimum speed
            if (speed < minSpeed)
                speed = minSpeed;

            return speed;
        }

        internal override void HandleOnExit()
        { }

        public override void SetDebugCursorPosition()
        {
            if(path.Count > 0)
                Owner.cursorTransform.position = path[path.Count - 1];
        }

        private float CalculatePathDistance()
        {
            float distance = 0;
            for (int i = 0; i < path.Count; i++)
            {
                Vector3 a = (i == 0) ? Owner.transform.position : path[i - 1];
                Vector3 b = path[i];
                distance += (b - a).magnitude;
            }

            return distance;
        }

        internal virtual void CalculatePath(Vector3 targetPoint)
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

        internal virtual void HandleNullPath()
        {
            // Double check if the path is null
            if (path == null)
            {
                Owner.TransitionToState("Idle");
            }
        }

        internal virtual void HandleEmptyPath()
        {
            Owner.TransitionToState("Idle");
        }
    }
}
