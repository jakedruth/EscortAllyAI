using System.Collections;
using System.Collections.Generic;
using AllyEscort;
using UnityEngine;

namespace AllyEscort
{
    public class GoToPointState : State
    {
        [Header("Variables")]
        public float maxSpeed;
        public float minSpeed;
        internal float speed;
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
            speed = minSpeed;
        }

        internal override void HandleUpdate()
        {

            if (path == null || path.Count <= 0)
            {
                Owner.TransitionToState("ToIdle");
                return;
            }

            Vector3 pos = Owner.transform.position;
            Vector3 delta = path[0] - pos;
            Vector3 dir = delta.normalized;

            speed = Mathf.MoveTowards(speed, maxSpeed, acceleration * Time.deltaTime);

            // Check to see if should slow down to the target
            if (slowDownToTarget && path.Count == 1 && delta.sqrMagnitude < slowDownRange * slowDownRange)
            {
                float fractionWithinRange = delta.magnitude / slowDownRange;
                speed = fractionWithinRange * maxSpeed;
            }

            if (speed < minSpeed)
                speed = minSpeed;

            Vector3 velocity = dir * speed;
            pos += velocity * Time.deltaTime;

            Owner.transform.position = pos;

            if (delta.sqrMagnitude <= 0.01f)
            {
                path.RemoveAt(0);
            }
        }

        internal override void HandleOnExit()
        { }

        public void UpdatePath(List<Vector3> path)
        {
            this.path = path;
        }

        public void CalculatePath(Vector3 targetPoint)
        {
            path = Owner.calculatePathComponent.GetPath(Owner.transform.position, targetPoint);
        }
    }
}
