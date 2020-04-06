using System.Collections;
using System.Collections.Generic;
using AllyEscort;
using UnityEngine;

namespace AllyEscort
{
    public class MoveToPointState : State
    {
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

        internal override void HandleOnEnter() { }

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

            Vector3 target = path[0];
            float distanceToGoal = CalculatePathDistance();
            Vector3 displacement = Owner.MoveToPoint(target, distanceToGoal);

            if (displacement.sqrMagnitude <= 0.005f)
            {
                RemoveFirstPointInPath();
            }

            //// Calculate Move Direction
            //Vector3 pos = Owner.transform.position;
            //Vector3 target = path[0];
            //Vector3 delta = target - pos;
            //MoveDirection = delta.normalized;

            //// Check to see if should slow down
            //if (slowDownToTarget)
            //{
            //    // get the displacement from the Owner to the end point
            //    Vector3 displacement = path[path.Count - 1] - Owner.transform.position;

            //    if (displacement.sqrMagnitude < slowDownRange * slowDownRange)
            //    {
            //        // Calculate the actual distance of the path
            //        float distance = CalculatePathDistance();
            //        if (distance <= slowDownRange)
            //        {
            //            // update the speed based on the range
            //            //SpeedMultiplier = displacement.magnitude / slowDownRange;
            //        }
            //    }
            //    else
            //    {
            //        //SpeedMultiplier = 1;
            //    }
            //}
            //else
            //{
            //    //SpeedMultiplier = 1;
            //}


            //// Check to see if the Owner is at the target, or very close
            //if (/*SpeedMultiplier <= 0.001f ||*/ delta.sqrMagnitude <= 0.0015f)
            //{
            //    RemoveFirstPointInPath();
            //}


            ////// Update the speed
            ////currentSpeed = GetSpeed();

            ////// Calculate the velocity
            ////Vector3 velocity = delta.normalized * currentSpeed;

            ////// Calculate the step the Owner will take this frame
            ////Vector3 step = velocity * Time.deltaTime;

            ////// If the step is greater than the distance
            ////if (step.sqrMagnitude > delta.sqrMagnitude)
            ////{
            ////    // set the step equal to the distance, so it does not over step
            ////    step = delta;
            ////}

            ////// Move the Owner
            //////pos += step;
            //////Owner.transform.position = pos;
            ////Owner.CharacterController.Move(step);


        }

        //internal float GetSpeed()
        //{
        //    // Calculate the new speed
        //    float speed =  Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);

        //    if (useSmoothStop)
        //    {
        //        // get the displacement from the Owner to the end point
        //        Vector3 delta = path[path.Count - 1] - Owner.transform.position;

        //        if (delta.sqrMagnitude < slowDownRange * slowDownRange)
        //        {
        //            // Calculate the actual distance of the path
        //            float distance = CalculatePathDistance();
        //            if (distance <= slowDownRange)
        //            {
        //                // update the speed based on the range
        //                float fractionWithinRange = delta.magnitude / slowDownRange;
        //                speed = fractionWithinRange * maxSpeed;
        //            }
        //        }
        //    }

        //    // Make sure the speed is never lower than the minimum speed
        //    if (speed < minSpeed)
        //        speed = minSpeed;

        //    return speed;
        //}

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
