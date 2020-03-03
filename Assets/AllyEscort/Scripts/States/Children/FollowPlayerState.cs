using System.Collections;
using System.Collections.Generic;
using AllyEscort;
using UnityEngine;

public class FollowPlayerState : GoToPointState
{
    [Header("Follow Variables")]
    public float minDistance;
    public Transform Target { get; private set; }

    internal override bool HandleInitialize()
    {
        if (Args[0] is Transform transform)
        {
            Target = transform;
            CalculatePath(Target.position);
            return true;
        }

        return false;
    }

    internal override void HandleUpdate()
    {
        if (path == null)
            return;

        // see if the path needs to be recalculated
        Vector3 targetDelta = Target.position - (path.Count == 0 ? Owner.transform.position : path[path.Count - 1]);
        float sqrDistance = targetDelta.sqrMagnitude;
        float totalRange = slowDownRange + minDistance;

        if (sqrDistance > (minDistance + 0.1f) * (minDistance + 0.1f))
        {
            CalculatePath(Target.position);
        }

        if (path.Count == 0)
            return;

        Vector3 pos = Owner.transform.position;
        Vector3 delta = path[0] - pos;
        Vector3 dir = delta.normalized;

        speed = Mathf.MoveTowards(speed, maxSpeed, acceleration * Time.deltaTime);

       

        if (speed < minSpeed)
            speed = minSpeed;

        // Check to see if should slow down to the target
        if (slowDownToTarget && path.Count >= 1 && delta.sqrMagnitude < totalRange * totalRange)
        {
            float fractionWithinRange = delta.magnitude - minDistance / slowDownRange;
            speed = Mathf.Max(fractionWithinRange * maxSpeed, 0);
        }

        Vector3 velocity = dir * speed;
        pos += velocity * Time.deltaTime;

        Owner.transform.position = pos;

        if (delta.sqrMagnitude <= 0.01f)
        {
            path.RemoveAt(0);
        }
    }

    public new void CalculatePath(Vector3 targetPoint)
    {
        base.CalculatePath(targetPoint);

        //int last = path.Count - 1;
        //Vector3 lastPoint = path[last];
        //Vector3 prev = path.Count == 1 ? Owner.transform.position : path[last - 1];
        //Vector3 delta = lastPoint - prev;
        //Vector3 dir = delta.normalized;

        //float distance = delta.magnitude;
        //float newDistance = Mathf.Max(distance - minDistance, 0);

        //path[last] = prev + dir * newDistance;
    }
}
