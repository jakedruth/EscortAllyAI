using System.Collections;
using System.Collections.Generic;
using AllyEscort;
using UnityEngine;

public class FollowPlayerState : MoveToPointState
{
    [Header("Follow Variables")]
    public float targetDistance;
    public float reCalculateDistance;

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
        base.HandleUpdate();

        // set up variables
        Vector3 targetPos = Target.position;
        Vector3 pathEndPoint = path.Count > 0 ? path[path.Count - 1] : Owner.transform.position;
        Vector3 delta = targetPos - pathEndPoint;

        // Check to see if the target has moved too far
        if (delta.sqrMagnitude > reCalculateDistance * reCalculateDistance)
        {
            CalculatePath(targetPos);
        }
    }

    internal override void CalculatePath(Vector3 targetPoint)
    {
        base.CalculatePath(targetPoint);

        // Move the last point closer to the start of the path based on targetDistance
        float remainingDistance = targetDistance;
        for (int i = path.Count - 1; i >= 0; i--)
        {
            Vector3 a = (i == 0) ? Owner.transform.position : path[i - 1];
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

    public override void SetDebugCursorPosition()
    {
        Owner.cursorTransform.position = Target.position;
    }

    internal override void HandleNullPath()
    {
        return;
    }

    internal override void HandleEmptyPath()
    {
        CalculatePath(Target.position);
        return;
    }


}
