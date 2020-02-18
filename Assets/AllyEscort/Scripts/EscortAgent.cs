using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public enum CommandType
    {
        MoveTo,
        Wait,
        SetMaxSpeed,
        SetMinSpeed,
    }

    public class EscortAgent : MonoBehaviour
    {
        public CalculatePathComponent calculatePathComponent;

        [Header("Variables")]
        public float maxSpeed;
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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void HandleCommand(CommandType commandType, object Arg = null)
        {
            switch (commandType)
            {
                case CommandType.MoveTo:
                    if (Arg is Vector3 point)
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
                        Debug.Log($"Error: command is not type Vector3. It is type: {Arg.GetType()}");
                    }
                    break;
                case CommandType.Wait:
                    StopAllCoroutines();
                    break;
                case CommandType.SetMaxSpeed:
                    if (Arg is float speed)
                    {
                        maxSpeed = speed;
                    }
                    else
                    {
                        Debug.Log($"Error: command is not type Float. It is type: {Arg.GetType()}");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
