using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public enum CommandType
    {
        MOVE_TO,
        WAIT,
        SET_MAX_SPEED,
        SET_MIN_SPEED,
    }

    public class EscortAgent : MonoBehaviour
    {
        public CalculatePathComponent calculatePathComponent;

        [Header("Variables")] public float maxSpeed;
        public float minSpeed;
        public float acceleration;
        public bool slowDownToTarget;
        public float slowDownRange;

        private void Awake()
        {
            if (calculatePathComponent == null)
            {
                Debug.LogError($"Error: CalculatePathComponent is null. This class must have one");
            }
        }

        /// <summary>
        /// Used to receive and handle commands.
        /// </summary>
        /// <param name="commandType">Used to determine what type of command is sent.</param>
        /// <param name="arg">The arguments that is needed to handle a command, like a point in world space or a speed parameter.</param>
        public void HandleCommand(CommandType commandType, object arg = null)
        {
            switch (commandType)
            {
                case CommandType.MOVE_TO:
                {
                    if (arg is Vector3 point)
                    {
                        List<Vector3> path = calculatePathComponent.GetPath(transform.position, point);
                        if (path != null)
                        {
                            StopAllCoroutines();
                            StartCoroutine(MoveAlongPath(path));
                        }
                    }
                    else
                    {
                        Debug.Log($"Error: command is not type Vector3. It is type: {arg.GetType()}");
                    }

                    break;
                }
                case CommandType.WAIT:
                {
                    StopAllCoroutines();
                    break;
                }
                case CommandType.SET_MAX_SPEED:
                {
                    if (arg is float speed)
                    {
                        maxSpeed = speed;
                    }
                    else
                    {
                        Debug.Log($"Error: command is not type Float. It is type: {arg.GetType()}");
                    }

                    break;
                }
                case CommandType.SET_MIN_SPEED:
                {
                    if (arg is float speed)
                    {
                        minSpeed = speed;
                    }
                    else
                    {
                        Debug.Log($"Error: command is not type Float. It is type: {arg.GetType()}");
                    }

                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        private IEnumerator MoveAlongPath(List<Vector3> path)
        {
            Vector3 pos = transform.position;
            float speed = 0;
            DrawPathDebug(path);

            while (path.Count > 0)
            {
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

                transform.position = pos;

                if (delta.sqrMagnitude <= 0.01f)
                {
                    path.RemoveAt(0);
                }

                yield return null;
            }
        }

        private void DrawPathDebug(IReadOnlyList<Vector3> path)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(path[i], path[i + 1], Color.blue, 5.0f, false);
            }
        }
    }
}
