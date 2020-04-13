using System.Collections;
using System.Collections.Generic;
using AllyEscort;
using UnityEngine;

public class FollowTransformState : MoveToPointState
{
    [Header("Follow Variables")]
    public float targetDistance;
    public float reCalculateDistance;

    public Transform Target { get; private set; }

    protected override bool HandleInitialize()
    {
        // check to see if the first argument is a Transform 
        if (Args[0] is Transform transform)
        {
            Target = transform;
            CalculatePath(Target.position);
            return true;
        }

        return false;
    }
    /// <summary>
    /// In addition to the base movement, determine if the target has moved a substantial amount from the end point and calculate a new path
    /// </summary>
    protected override void HandleUpdate()
    {
        base.HandleUpdate();

        // set up variables
        Vector3 targetPos = Target.position;
        Vector3 pathEndPoint = path.Count > 0 ? path[path.Count - 1] : Position;
        Vector3 delta = targetPos - pathEndPoint;

        // Check to see if the target has moved too far
        if (delta.sqrMagnitude > reCalculateDistance * reCalculateDistance)
        {
            CalculatePath(targetPos);
        }
    }

    /// <summary>
    /// The end point of the path is the target. This adjusts the last point in the path
    /// so it is {targetDistance} units away form the calculated targetPoint
    /// </summary>
    /// <param name="targetPoint">The end point of the path</param>
    protected override void CalculatePath(Vector3 targetPoint)
    {
        base.CalculatePath(targetPoint);

        // Move the last point closer to the start of the path based on targetDistance
        ShortenPath(targetDistance);
    }



    /// <summary>
    /// Override the point to the target's position, not the last point in the path
    /// </summary>
    public override void HandleDebugCursorPosition()
    {
        Owner.cursorTransform.position = Target.position;
    }

    /// <summary>
    /// If the path is null, do nothing and wait until a new path is calculated
    /// </summary>
    protected override void HandleNullPath()
    {
        return;
    }

    /// <summary>
    /// If the path is empty, calculate a new path
    /// </summary>
    protected override void HandleEmptyPath()
    {
        CalculatePath(Target.position);
        return;
    }


}
